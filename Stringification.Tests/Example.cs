using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Stringification;

namespace Stringifier.Test
{
    public class StringifyExample
    {
        public enum Country { Macau, Macedonia, Malawi }

        public class Person
        {
            public string Name { get; set; }
            public int? Age { get; set; }
        }

        public class Location
        {
            public string Address { get; set; }
            public Country Country { get; set; }
            public DateTime LastUpdate { get; set; }
        }

        public class Company
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public bool Active { get; set; }
            public decimal Sales { get; set; }
            public Location Location { get; set; }
            public IEnumerable<Person> People { get; set; }
        }

        protected readonly Action<string> Write;
        public StringifyExample(ITestOutputHelper output) => Write = output.WriteLine;

        [Fact]
        public void T09_Example()
        {
            var company = new Company
            {
                Name = "Oracle",
                Id = 42,
                Active = true,
                Sales = 100,
                Location = new Location
                {
                    Address = "31 Oak",
                    Country = Country.Malawi,
                    LastUpdate = DateTime.Now
                },
                People = new List<Person>
                {
                    new Person
                    {
                        Name = "Bill",
                        Age = 18
                    },
                    new Person
                    {
                        Name = "Nester",
                        Age = 81
                    }
                }
            };

            Write(company.Stringify());
        }
    }
}

