namespace Stringification.Tests;

public class ClassTests(ITestOutputHelper output) : TestBase(output)
{
    public class Company
    {
        public string Name { get; set; } = "";
    }

    [Fact]
    public void T03_Empty_Class()
    {
        var c = new Company();
        //Assert.Empty(c.Stringify(includeTypeName: false));
        Assert.Equal("Company: {}", c.Stringify());
    }

    [Fact]
    public void T07_Classes()
    {
        var company = new Company { Name = "Microsoft" };
        Assert.Equal("Company: {Name:\"Microsoft\"}", company.Stringify());
    }

    public class CompanyWithCtor
    {
        public string? Name { get; set; }
        public CompanyWithCtor()
        {
            Name = "some name";
        }
    }

    [Fact]
    public void T07_ClassesEmpty()
    {
        var company = new CompanyWithCtor();
        company.Name = null;
        Write(company.Stringify());
        Assert.Equal("CompanyWithCtor: {Name:}", company.Stringify());
    }

    [Fact]
    public void T04_Enumerable()
    {
        var list = new List<string> { "item2", "item1" }; // sorted
        var result1 = list.Stringify();
        Write(result1);
        Assert.Equal("List`1: [\"item2\", \"item1\"]", result1);

        var result2 = list.ToArray().Stringify(); // not sorted
        Write(result2);
        Assert.Equal("String[]: [\"item2\", \"item1\"]", result2);
    }

    [Fact]
    public void T06_Exception()
    {
        var ex1 = Assert.Throws<FormatException>(() => int.Parse("invalid"));
        Write(ex1.Stringify(true));

        var ex2 = new InvalidTimeZoneException("message");
        Write(ex2.Stringify());
    }
}
