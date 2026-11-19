using OpenQA.Selenium;

namespace AutomationExercise;

public sealed class CheckoutPage(IWebDriver driver) : Page(driver)
{
    public string Delivery => Text(By.Id("address_delivery"));
    public string Billing => Text(By.Id("address_invoice"));
    public decimal Total => Money(Text(By.CssSelector("#cart_info tr:last-child .cart_total_price")));
    public void PlaceOrder()
    {
        Fill(By.Name("message"), "Synthetic QA order");
        Click(By.CssSelector("a[href='/payment']"));
    }
    public void Pay()
    {
        Fill(Data("name-on-card"), Account.Name);
        Fill(Data("card-number"), "4111111111111111");
        Fill(Data("cvc"), "123");
        Fill(Data("expiry-month"), "12");
        Fill(Data("expiry-year"), (DateTime.UtcNow.Year + 2).ToString());
        Click(Data("pay-button"));
    }
    public string Confirmation => Text(Data("order-placed"));
    public void DownloadInvoice() => Click(By.CssSelector("a[href^='/download_invoice/']"));
}
