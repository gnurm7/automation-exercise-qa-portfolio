using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise;

[TestFixture]
public sealed class AccountCheckoutTests : BaseTest
{
    private LoginPage Login => new(Driver);
    private Header Header => new(Driver);
    private CartPage Cart => new(Driver);
    private CheckoutPage Checkout => new(Driver);

    [Test, Category("Regression"), Property("Scenario", "EC-07")]
    public void RegistersSyntheticCustomer()
    {
        var account = NewAccount();
        Login.Open("/login");
        Login.BeginSignup(account.Email);
        var registration = new RegistrationPage(Driver);
        registration.Register(account);
        Assert.That(registration.Confirmation, Is.EqualTo("Account Created!").IgnoreCase, "Registration confirmation.");
        registration.Continue();
        AssertSignedIn();
    }

    [Test, Category("Regression"), Property("Scenario", "EC-08")]
    public async Task DuplicateEmailIsRejected()
    {
        var account = await SeedAccount();
        Login.Open("/login");
        Login.BeginSignup(account.Email);
        Assert.That(Login.SignupError, Does.Contain("Email Address already exist"), "Duplicate signup should explain the conflict.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-09")]
    public async Task WrongPasswordCanBeCorrected()
    {
        var account = await SeedAccount();
        Login.Open("/login");
        Login.Login(account.Email, $"{account.Password}-wrong");
        Assert.That(Login.LoginError, Does.Contain("email or password is incorrect"), "Incorrect password should show a login error.");
        Login.Login(account.Email, account.Password);
        AssertSignedIn();
    }

    [Test, Category("Regression"), Property("Scenario", "EC-09")]
    public void UnknownEmailCannotSignIn()
    {
        var unknown = Account.Create();
        Login.Open("/login");
        Login.Login(unknown.Email, unknown.Password);
        Assert.That(Login.LoginError, Does.Contain("email or password is incorrect"), "Unknown account should be rejected.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-10")]
    public async Task LogoutClearsAuthenticatedNavigationAfterRefresh()
    {
        await SignIn();
        Header.LogOut();
        Assert.That(Login.LoginFormVisible, Is.True, "Logout should return to the login form.");
        Driver.Navigate().Refresh();
        Assert.That(Login.LoginFormVisible, Is.True, "Login form should remain visible after refresh.");
        Assert.That(Header.IsSignedIn, Is.False, "Session should remain signed out on refresh.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-06")]
    public async Task GuestCartSurvivesLogin()
    {
        var item = AddCartItem();
        await SignIn();
        Cart.Open("/view_cart");
        Assert.That(Cart.Line(1), Is.EqualTo(item), "Signing in should preserve the guest cart line.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-11")]
    public async Task CheckoutUsesRegistrationAddresses()
    {
        var account = await SignIn();
        PrepareCheckout();
        var fields = account.Fields();
        var expected = new[] { Account.Name, Account.Address, fields["address2"],
            $"{fields["city"]} {fields["state"]} {fields["zipcode"]}", fields["country"], fields["mobile_number"] };
        var delivery = Checkout.Delivery;
        var billing = Checkout.Billing;
        Assert.Multiple(() =>
        {
            foreach (var value in expected)
            {
                Assert.That(delivery, Does.Contain(value), $"Delivery address should include '{value}'.");
                Assert.That(billing, Does.Contain(value), $"Billing address should include '{value}'.");
            }
        });
    }

    [Test, Category("Regression"), Property("Scenario", "EC-12")]
    public async Task CheckoutPreservesCartLineAndOrderTotal()
    {
        await SignIn();
        var expected = PrepareCheckout();
        Assert.That(Cart.Line(1), Is.EqualTo(expected), "Checkout should preserve the cart summary.");
        Assert.That(Checkout.Total, Is.EqualTo(expected.Total), "Order total should equal the line total.");
    }

    [Test, Category("E2E"), Category("Regression"), Property("Scenario", "EC-13/EC-14")]
    public async Task OrderConfirmationOffersDownloadableInvoice()
    {
        await SignIn();
        var item = PrepareCheckout();
        Checkout.PlaceOrder();
        Checkout.Pay();
        Assert.That(Checkout.Confirmation, Is.EqualTo("Order Placed!").IgnoreCase, "Order acceptance should be visible.");
        Checkout.DownloadInvoice();
        var invoice = new WebDriverWait(Driver, TimeSpan.FromSeconds(20)).Until(_ =>
            Directory.EnumerateFiles(Downloads, "invoice*.txt").FirstOrDefault());
        Assert.That(invoice, Is.Not.Null, "Browser should save the invoice to the test download directory.");
        var content = await File.ReadAllTextAsync(invoice!);
        Assert.That(content, Does.Contain(Account.Name), "Invoice should identify the customer.");
        Assert.That(content, Does.Match($@"\b{item.Total}\b"), "Invoice should include the order amount.");
    }

    private CartLine PrepareCheckout()
    {
        var item = AddCartItem();
        Cart.Checkout();
        return item;
    }

    private CartLine AddCartItem()
    {
        var product = new ProductPage(Driver);
        product.Open("/product_details/1");
        var item = new CartLine(product.Name, product.Price, 2, product.Price * 2);
        product.SetQuantity("2");
        product.AddToCart();
        product.ViewCart();
        return item;
    }

    private async Task<Account> SignIn()
    {
        var account = await SeedAccount();
        Login.Open("/login");
        Login.Login(account.Email, account.Password);
        AssertSignedIn();
        return account;
    }

    private void AssertSignedIn()
    {
        Assert.That(Header.AccountName, Is.EqualTo(Account.Name), "Header should show the signed-in synthetic customer.");
        Assert.That(Header.IsSignedIn, Is.True, "Authenticated navigation should offer logout.");
    }
}
