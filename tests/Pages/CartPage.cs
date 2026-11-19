using OpenQA.Selenium;

namespace AutomationExercise;

public sealed record CartLine(string Name, decimal Price, int Quantity, decimal Total);

public sealed class CartPage(IWebDriver driver) : Page(driver)
{
    public CartLine Line(int id)
    {
        var row = $"#product-{id}";
        return new(Text(By.CssSelector($"{row} .cart_description h4")),
            Money(Text(By.CssSelector($"{row} .cart_price"))),
            int.Parse(Text(By.CssSelector($"{row} .cart_quantity"))),
            Money(Text(By.CssSelector($"{row} .cart_total"))));
    }

    public bool Has(int id) => Driver.FindElements(By.Id($"product-{id}")).Any(e => e.Displayed);
    public string EmptyMessage => Text(By.Id("empty_cart"));
    public void Remove(int id)
    {
        Click(By.CssSelector($"#product-{id} .cart_quantity_delete"));
        Wait.Until(_ => !Has(id));
    }
    public void Checkout() => Click(By.CssSelector(".check_out"));
    public string GuestPrompt => Text(By.Id("checkoutModal"));
}
