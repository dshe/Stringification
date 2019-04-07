using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Stringification;

namespace Examples
{
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
        public void T09_Example()
        {
            var company = new Company
            {
                Name = "Aco",
                Id = 9,
                Active = true,
                Location = new Location("3 Ruey", Country.Macedonia, DateTime.Now),
                People = new List<Person>() { new Person("Natalia", 18), new Person("Natasha", 42) }
            };

            Write(company.Stringify());
        }
    }
}

