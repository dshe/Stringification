namespace Stringification.Tests;

public class StringifyExamples(ITestOutputHelper output)
{
    public enum Country { Macau, Macedonia, Malawi }

    public class Person(string name, int age)
    {
        public string Name { get; } = name;
        public int Age { get; } = age;
    }

    public class Location(string? address, StringifyExamples.Country country, DateTime updated)
    {
        public string? Address { get; set; } = address;
        public Country Country { get; set; } = country;
        public DateTime Updated { get; set; } = updated;
    }

    public class Company
    {
        public string Name { get; set; } = "";
        public int Id { get; set; }
        public bool Active { get; set; }
        public decimal Sales { get; set; }
        public Location? Location { get; set; } = null;
        public IEnumerable<Person>? People { get; set; }
    }

    protected readonly Action<string> Write = output.WriteLine;

    [Fact]
    public void T01_Example()
    {
        Company company = new()
        {
            Name = "Aco",
            Id = 0,
            Active = true,
            Location = new Location("3 Ruey", Country.Macedonia, DateTime.Now),
            People = [new Person("Natalia", 24), new Person("Natasha", 42)]
        };

        Write(company.Stringify());
    }

    [Fact]
    public void T02_Example()
    {
        Company company = new()
        {
            Name = "Aco",
            Id = 9,
            Active = false,
            Location = new Location("3 Ruey", Country.Macedonia, DateTime.Now),
            People = [new Person("Natalia", 24), new Person("Natasha", 42)]
        };

        var str = company.Stringify();
        Write(str);
    }

    public class TestClass
    {
        public string SomeString { get; } = "";
        internal TestClass()
        {
        }
        public TestClass(string ss)
        {
            //SomeString = ss;
        }
    }

    [Fact]
    public void T03_Example()
    {
        var s = new TestClass("abc");
        var str = s.Stringify();
        Assert.Equal("TestClass: {}", str);
        Write(str);
    }
}
