using OpenQA.Selenium;

namespace AutomationExercise;

public sealed class Header(IWebDriver driver) : Page(driver)
{
    public string AccountName => Text(By.CssSelector("#header .shop-menu a > b"));
    public bool IsSignedIn => Driver.FindElements(By.CssSelector("a[href='/logout']")).Any(e => e.Displayed);
    public void LogOut() => Click(By.CssSelector("a[href='/logout']"));
}
