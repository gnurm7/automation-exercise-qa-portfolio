using System.Text.Json;
using NUnit.Framework;

namespace AutomationExercise;

[TestFixture]
public sealed class CatalogueCartTests : BaseTest
{
    private CataloguePage Catalogue => new(Driver);
    private ProductPage Product => new(Driver);
    private CartPage Cart => new(Driver);

    [Test, Category("Smoke"), Category("Regression"), Property("Scenario", "EC-01")]
    public void SearchAndOpenMatchingProduct()
    {
        Catalogue.Open("/products");
        Catalogue.Search("blue top");
        Assert.That(Catalogue.Products, Does.Contain("Blue Top"), "Product search should include Blue Top.");
        Catalogue.View(1);
        Assert.That(Product.Name, Is.EqualTo("Blue Top"), "Detail page must identify the selected product.");
    }

    [TestCase("men tshirt", 2)]
    [TestCase("sleeveless dress", 3)]
    [Category("Regression"), Property("Scenario", "EC-01")]
    public void SearchFindsKnownProduct(string term, int productId)
    {
        Catalogue.Open("/products");
        Catalogue.Search(term);
        Assert.That(Catalogue.HasProduct(productId), Is.True, $"Search term '{term}' should show the link for product {productId}.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-16")]
    public void EmptySearchResultsAllowRecovery()
    {
        Catalogue.Open("/products");
        Catalogue.Search($"no-product-{Guid.NewGuid():N}");
        Assert.That(Catalogue.Products, Is.Empty, "An unmatched term should produce no product cards.");
        Catalogue.Search("Blue Top");
        Assert.That(Catalogue.Products, Does.Contain("Blue Top"), "A valid follow-up search should recover.");
    }

    [TestCase("Women", 1, "Women - Dress Products")]
    [TestCase("Men", 3, "Men - Tshirts Products")]
    [Category("Regression"), Property("Scenario", "EC-15")]
    public void CategoryShowsMatchingHeading(string group, int category, string expected)
    {
        Catalogue.Open("/products");
        Catalogue.Category(group, category);
        Assert.That(Catalogue.Title, Is.EqualTo(expected).Using(StringComparer.OrdinalIgnoreCase), "Category heading should identify the selected category.");
        Assert.That(Catalogue.Products, Is.Not.Empty, "Known category should contain products.");
    }

    [TestCase("Polo", "Blue Top")]
    [TestCase("H&M", "Summer White Top")]
    [Category("Regression"), Property("Scenario", "EC-15")]
    public void BrandShowsKnownProduct(string brand, string product)
    {
        Catalogue.Open("/products");
        Catalogue.Brand(brand);
        Assert.That(Catalogue.Title, Does.Contain(brand).IgnoreCase, "Brand heading should identify the selected brand.");
        Assert.That(Catalogue.Products, Does.Contain(product), "Branded catalogue should contain its known product.");
    }

    [TestCase(1), TestCase(2)]
    [Category("Regression"), Property("Scenario", "EC-20")]
    public async Task ProductDetailsAgreeWithPublicApi(int id)
    {
        using var api = await Request(HttpMethod.Get, "productsList");
        Assert.That(api.RootElement.GetProperty("responseCode").GetInt32(), Is.EqualTo(200), "Catalogue application response.");
        JsonElement expected = api.RootElement.GetProperty("products").EnumerateArray()
            .Single(row => row.GetProperty("id").GetInt32() == id);
        Product.Open($"/product_details/{id}");
        var category = expected.GetProperty("category");
        Assert.Multiple(() =>
        {
            Assert.That(Product.Name, Is.EqualTo(expected.GetProperty("name").GetString()), "API/UI product name.");
            Assert.That($"Rs. {Product.Price}", Is.EqualTo(expected.GetProperty("price").GetString()), "API/UI unit price.");
            Assert.That(Product.Category, Is.EqualTo($"{category.GetProperty("usertype").GetProperty("usertype").GetString()} > {category.GetProperty("category").GetString()}"), "API/UI category.");
        });
    }

    [Test, Category("Regression"), Property("Scenario", "EC-21")]
    public void ProductDefaultsToOneItem()
    {
        Product.Open("/product_details/1");
        Assert.That(Product.Quantity, Is.EqualTo("1"), "An untouched quantity should default to one.");
    }

    [TestCase("-1", false)]
    [TestCase("0", false)]
    [TestCase("1", true)]
    [TestCase("1.5", false)]
    [Category("Regression"), Property("Scenario", "EC-17")]
    public void QuantityFieldReportsNumberConstraints(string quantity, bool valid)
    {
        Product.Open("/product_details/1");
        Product.SetQuantity(quantity);
        Assert.That(Product.Quantity, Is.EqualTo(quantity), "The input must contain the intended boundary value.");
        Assert.That(Product.QuantityIsValid, Is.EqualTo(valid), "HTML number constraints should accept positive integers.");
    }

    [TestCase(1), TestCase(3), TestCase(5)]
    [Category("Smoke"), Category("Regression"), Property("Scenario", "EC-02")]
    public void CartPreservesQuantityAndComputesTotal(int quantity)
    {
        Product.Open("/product_details/1");
        var expected = new CartLine(Product.Name, Product.Price, quantity, Product.Price * quantity);
        Product.SetQuantity(quantity.ToString());
        Product.AddToCart();
        Product.ViewCart();
        Assert.That(Cart.Line(1), Is.EqualTo(expected), "Cart should preserve product, price, quantity and computed total.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-03")]
    public void TwoProductsRemainDistinct()
    {
        var first = Add(1);
        var second = Add(2);
        Cart.Open("/view_cart");
        Assert.Multiple(() =>
        {
            Assert.That(Cart.Line(1), Is.EqualTo(first), "First item should retain its own data.");
            Assert.That(Cart.Line(2), Is.EqualTo(second), "Second item should retain its own data.");
        });
    }

    [Test, Category("Regression"), Property("Scenario", "EC-04")]
    public void RemovalSurvivesRefresh()
    {
        Add(1);
        var kept = Add(2);
        Cart.Open("/view_cart");
        Cart.Remove(1);
        Driver.Navigate().Refresh();
        Assert.That(Cart.Line(2), Is.EqualTo(kept), "Unremoved row should be unchanged.");
        Assert.That(Cart.Has(1), Is.False, "Removed row should remain absent after refresh.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-22")]
    public void RemovingFinalItemShowsEmptyCart()
    {
        Add(1);
        Cart.Open("/view_cart");
        Cart.Remove(1);
        Assert.That(Cart.EmptyMessage, Does.Contain("Cart is empty"), "Removing the last item should show the empty state.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-05")]
    public void GuestCheckoutPromptsForAuthentication()
    {
        Add(1);
        Cart.Open("/view_cart");
        Cart.Checkout();
        Assert.That(Cart.GuestPrompt, Does.Contain("Register / Login"), "Guest must sign in before checkout.");
    }

    [Test, Category("Regression"), Property("Scenario", "EC-18")]
    public void IndependentBrowserDoesNotShareCart()
    {
        Add(1);
        Cart.Open("/view_cart");
        Assert.That(Cart.Has(1), Is.True, "The original browser should contain the test item.");
        using var other = WebDriverFactory.Create(Downloads);
        var isolated = new CartPage(other);
        isolated.Open("/view_cart");
        Assert.That(isolated.EmptyMessage, Does.Contain("Cart is empty"), "Independent browser should start with an empty cart.");
    }

    private CartLine Add(int id)
    {
        Product.Open($"/product_details/{id}");
        var expected = new CartLine(Product.Name, Product.Price, 1, Product.Price);
        Product.AddToCart();
        Product.ViewCart();
        return expected;
    }
}
