namespace AutomationExercise;

public sealed record Account(string Email, string Password)
{
    public const string Name = "Demo Tester";
    public const string Address = "123 Demo Avenue";

    public static Account Create()
    {
        var id = Guid.NewGuid().ToString("N");
        return new($"ae-{id}@example.com", $"Demo-{id}!");
    }

    public Dictionary<string, string> Fields() => new()
    {
        ["name"] = Name, ["email"] = Email, ["password"] = Password,
        ["title"] = "Mr", ["birth_date"] = "15", ["birth_month"] = "6", ["birth_year"] = "1992",
        ["firstname"] = "Demo", ["lastname"] = "Tester", ["company"] = "Example QA",
        ["address1"] = Address, ["address2"] = "Suite 2", ["country"] = "Canada",
        ["zipcode"] = "A1A 1A1", ["state"] = "Ontario", ["city"] = "Toronto",
        ["mobile_number"] = "4165550100"
    };
}
