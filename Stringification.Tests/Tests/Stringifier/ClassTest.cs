using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Stringification;

namespace TestStringifier
{
    public class ClassTests
    {
        protected readonly Action<string> Write;
        public ClassTests(ITestOutputHelper output) => Write = output.WriteLine;

        [Fact]
        public void T03_Primitives()
        {
            const string str = "somestring";
            Assert.Equal(str, str.Stringify(includeTypeName: false));
            Assert.Equal("A", "A".Stringify(includeTypeName: false));
            Assert.Equal("42", 42.Stringify(includeTypeName: false));
            Assert.Equal(DateTime.Now.ToString(), DateTime.Now.Stringify(includeTypeName: false));
        }

        public class Company
        {
            public string Name { get; set; } = "";
        }

        [Fact]
        public void T02_Empty_Class()
        {
            var c = new Company();
            Assert.Null(c.Stringify(includeTypeName: false));
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
}
