using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace AutomationExercise;

internal static class WebDriverFactory
{
    public static IWebDriver Create(string downloads)
    {
        var headed = Environment.GetEnvironmentVariable("QA_HEADED") == "1";
        var browser = Environment.GetEnvironmentVariable("QA_BROWSER") ?? "chrome";
        IWebDriver driver = browser.ToLowerInvariant() switch
        {
            "chrome" => new ChromeDriver(Chrome(headed, downloads)),
            "firefox" => new FirefoxDriver(Firefox(headed, downloads)),
            _ => throw new ArgumentException($"Unsupported QA_BROWSER: {browser}")
        };
        try
        {
            driver.Manage().Window.Size = new(1440, 1000);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(45);
            return driver;
        }
        catch
        {
            driver.Dispose();
            throw;
        }
    }

    private static ChromeOptions Chrome(bool headed, string downloads)
    {
        var options = new ChromeOptions { PageLoadStrategy = PageLoadStrategy.Eager };
        if (!headed) options.AddArgument("--headless");
        options.AddUserProfilePreference("download.default_directory", downloads);
        options.AddUserProfilePreference("download.prompt_for_download", false);
        return options;
    }

    private static FirefoxOptions Firefox(bool headed, string downloads)
    {
        var options = new FirefoxOptions { PageLoadStrategy = PageLoadStrategy.Eager };
        if (!headed) options.AddArgument("-headless");
        options.SetPreference("browser.download.folderList", 2);
        options.SetPreference("browser.download.dir", downloads);
        options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "text/plain,application/octet-stream");
        return options;
    }
}
