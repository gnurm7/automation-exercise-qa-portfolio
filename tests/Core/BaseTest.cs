using System.Text.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace AutomationExercise;

[NonParallelizable]
public abstract class BaseTest
{
    private static readonly HttpClient Api = new()
    {
        BaseAddress = new("https://automationexercise.com/api/"),
        Timeout = TimeSpan.FromSeconds(25)
    };
    private readonly List<Account> accounts = [];
    protected IWebDriver Driver { get; private set; } = null!;
    protected string Downloads { get; private set; } = "";
    private string artifacts = "";

    [SetUp]
    public void StartBrowser()
    {
        accounts.Clear();
        artifacts = Path.GetFullPath(Environment.GetEnvironmentVariable("QA_ARTIFACTS") ?? "artifacts/ui");
        Directory.CreateDirectory(artifacts);
        Downloads = Directory.CreateTempSubdirectory("ae-downloads-").FullName;
        Driver = WebDriverFactory.Create(Downloads);
    }

    protected Account NewAccount()
    {
        var account = Account.Create();
        accounts.Add(account);
        return account;
    }

    protected async Task<Account> SeedAccount()
    {
        var account = NewAccount();
        using var result = await Request(HttpMethod.Post, "createAccount", account.Fields());
        Assert.That(result.RootElement.GetProperty("responseCode").GetInt32(), Is.EqualTo(201),
            "Synthetic account fixture could not be created.");
        return account;
    }

    protected static async Task<JsonDocument> Request(HttpMethod method, string path,
        Dictionary<string, string>? fields = null)
    {
        using var request = new HttpRequestMessage(method, path);
        if (fields != null) request.Content = new FormUrlEncodedContent(fields);
        using var response = await Api.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Fixture/catalogue endpoint {path} returned HTTP {(int)response.StatusCode}.");
        return JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    }

    [TearDown]
    public async Task Finish()
    {
        try
        {
            if (Driver is ITakesScreenshot camera &&
                TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                try
                {
                    var file = Path.Combine(artifacts, $"{TestContext.CurrentContext.Test.ID}-{Guid.NewGuid():N}.png");
                    camera.GetScreenshot().SaveAsFile(file);
                    TestContext.AddTestAttachment(file, "Browser state at failure");
                    TestContext.Progress.WriteLine($"Failure state: {new Uri(Driver.Url).AbsolutePath}; title: {Driver.Title}");
                }
                catch (Exception error)
                {
                    TestContext.Progress.WriteLine($"Screenshot unavailable: {error.Message}");
                }
            }
        }
        finally
        {
            try { Driver?.Dispose(); }
            finally
            {
                Driver = null!;
                foreach (var account in accounts)
                {
                    try
                    {
                        using var result = await Request(HttpMethod.Delete, "deleteAccount",
                            new() { ["email"] = account.Email, ["password"] = account.Password });
                        var code = result.RootElement.GetProperty("responseCode").GetInt32();
                        if (code is not (200 or 404))
                            TestContext.Progress.WriteLine($"Fixture cleanup returned application code {code}.");
                    }
                    catch (Exception error)
                    {
                        TestContext.Progress.WriteLine($"Fixture cleanup unavailable: {error.GetType().Name}");
                    }
                }
                if (Directory.Exists(Downloads)) Directory.Delete(Downloads, true);
            }
        }
    }
}
