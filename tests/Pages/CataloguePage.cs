using OpenQA.Selenium;

namespace AutomationExercise;

public sealed class CataloguePage(IWebDriver driver) : Page(driver)
{
    private static readonly By Heading = By.CssSelector(".features_items h2.title");
    private const string Cards = ".features_items .productinfo";

    public string Title => Text(Heading);
    public IReadOnlyList<string> Products
    {
        get
        {
            Visible(Heading);
            return Driver.FindElements(By.CssSelector($"{Cards} p")).Select(e => Normalize(e.Text)).ToArray();
        }
    }
    public void Search(string term)
    {
        Fill(By.Id("search_product"), term);
        Click(By.Id("submit_search"));
        Wait.Until(_ => Title.Contains("Searched Products", StringComparison.OrdinalIgnoreCase));
    }
    private static By ProductLink(int id) => By.CssSelector($".features_items a[href='/product_details/{id}']");
    public void View(int id) => Click(ProductLink(id));
    public bool HasProduct(int id) => Driver.FindElements(ProductLink(id)).Any(e => e.Displayed);
    public void Category(string group, int id)
    {
        Click(By.CssSelector($"a[href='#{group}']"));
        Click(By.CssSelector($"a[href='/category_products/{id}']"));
    }
    public void Brand(string name) => Click(By.CssSelector($"a[href='/brand_products/{name}']"));
}

public sealed class ProductPage(IWebDriver driver) : Page(driver)
{
    public string Name => Text(By.CssSelector(".product-information h2"));
    public decimal Price => Money(Text(By.CssSelector(".product-information span span")));
    public string Category
    {
        get
        {
            var label = Visible(By.CssSelector(".product-information h2 + p"));
            // Read the label's own text. Browser-inserted links can add unrelated descendant copy.
            var text = (string)((IJavaScriptExecutor)Driver).ExecuteScript(
                "return Array.from(arguments[0].childNodes).filter(n => n.nodeType === Node.TEXT_NODE).map(n => n.data).join('');",
                label)!;
            return Normalize(text.Split(':', 2)[1]);
        }
    }
    public string Quantity => Visible(By.Id("quantity")).GetDomProperty("value")!;
    public void SetQuantity(string quantity) => Fill(By.Id("quantity"), quantity);
    public bool QuantityIsValid => (bool)((IJavaScriptExecutor)Driver)
        .ExecuteScript("return arguments[0].validity.valid;", Visible(By.Id("quantity")))!;
    public void AddToCart()
    {
        Click(By.CssSelector(".product-information button.cart"));
        Visible(By.Id("cartModal"));
    }
    public void ViewCart() => Click(By.CssSelector("#cartModal a[href='/view_cart']"));
}
