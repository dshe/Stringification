namespace Stringification.Tests;

public class StringifyExamples
{
    public enum Country { Macau, Macedonia, Malawi }

    public class Person
    {
        public string Name { get; }
        public int Age { get; }
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    public class Location
    {
        public string? Address { get; set; }
        public Country Country { get; set; }
        public DateTime Updated { get; set; }
        public Location(string? address, Country country, DateTime updated)
        {
            Address = address;
            Country = country;
            Updated = updated;
        }
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

    protected readonly Action<string> Write;
    public StringifyExamples(ITestOutputHelper output) => Write = output.WriteLine;

    [Fact]
    public void T01_Example()
    {
        Company company = new()
        {
            Name = "Aco",
            Id = 0,
            Active = true,
            Location = new Location("3 Ruey", Country.Macedonia, DateTime.Now),
            People = new List<Person>() { new Person("Natalia", 24), new Person("Natasha", 42) }
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
            People = new List<Person>() { new Person("Natalia", 24), new Person("Natasha", 42) }
        };

        var str = company.Stringify();
        Write(str);
    }

}
