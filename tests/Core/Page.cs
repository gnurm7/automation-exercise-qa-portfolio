using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AutomationExercise;

public abstract class Page(IWebDriver driver)
{
    protected IWebDriver Driver { get; } = driver;
    protected WebDriverWait Wait => new(Driver, TimeSpan.FromSeconds(15));

    public void Open(string path)
    {
        Driver.Navigate().GoToUrl(new Uri(new Uri("https://automationexercise.com"), path));
        ReportExternalBlock();
    }

    protected IWebElement Visible(By by)
    {
        var wait = Wait;
        wait.Message = $"Expected visible element {by}. Current URL: {Driver.Url}";
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        try
        {
            return wait.Until(d => d.FindElements(by).FirstOrDefault(element => element.Displayed))!;
        }
        catch (WebDriverTimeoutException)
        {
            ReportExternalBlock();
            throw;
        }
    }

    private void ReportExternalBlock()
    {
        var heading = Driver.FindElements(By.CssSelector("body > h1")).FirstOrDefault()?.Text ?? "";
        foreach (var indicator in new[] { "under heavy load", "too many requests", "access denied", "just a moment" })
        {
            if (heading.Contains(indicator, StringComparison.OrdinalIgnoreCase) ||
                Driver.Title.Contains(indicator, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"External environment blocked navigation at {new Uri(Driver.Url).AbsolutePath}: {indicator}.");
        }
    }

    protected void Click(By by) => Visible(by).Click();
    protected string Text(By by) => Normalize(Visible(by).Text);
    protected void Fill(By by, string value)
    {
        var element = Visible(by);
        element.Clear();
        element.SendKeys(value);
    }

    protected static string Normalize(string value) => Regex.Replace(value.Trim(), @"\s+", " ");

    protected static By Data(string name) => By.CssSelector($"[data-qa='{name}']");
    protected static decimal Money(string text) =>
        decimal.Parse(Regex.Match(text, @"[0-9]+(?:\.[0-9]+)?").Value, CultureInfo.InvariantCulture);
}
