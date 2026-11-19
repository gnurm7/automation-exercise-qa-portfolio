using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise;

public sealed class LoginPage(IWebDriver driver) : Page(driver)
{
    public string LoginError => Text(By.CssSelector(".login-form form p"));
    public string SignupError => Text(By.CssSelector(".signup-form form p"));
    public bool LoginFormVisible => Visible(Data("login-email")).Displayed;
    public void Login(string email, string password)
    {
        Fill(Data("login-email"), email);
        Fill(Data("login-password"), password);
        Click(Data("login-button"));
    }
    public void BeginSignup(string email)
    {
        Fill(Data("signup-name"), Account.Name);
        Fill(Data("signup-email"), email);
        Click(Data("signup-button"));
    }
}

public sealed class RegistrationPage(IWebDriver driver) : Page(driver)
{
    public string Confirmation => Text(Data("account-created"));
    public void Register(Account account)
    {
        var fields = account.Fields();
        Click(By.Id("id_gender1"));
        Fill(By.Id("password"), fields["password"]);
        new SelectElement(Visible(By.Id("days"))).SelectByValue(fields["birth_date"]);
        new SelectElement(Visible(By.Id("months"))).SelectByValue(fields["birth_month"]);
        new SelectElement(Visible(By.Id("years"))).SelectByValue(fields["birth_year"]);
        new SelectElement(Visible(By.Id("country"))).SelectByText(fields["country"]);
        foreach (var (id, key) in new Dictionary<string, string>
        {
            ["first_name"] = "firstname", ["last_name"] = "lastname", ["company"] = "company",
            ["address1"] = "address1", ["address2"] = "address2", ["state"] = "state",
            ["city"] = "city", ["zipcode"] = "zipcode", ["mobile_number"] = "mobile_number"
        }) Fill(By.Id(id), fields[key]);
        Click(Data("create-account"));
    }
    public void Continue() => Click(Data("continue-button"));
}
