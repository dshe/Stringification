using StringEnums;

namespace Stringification.Tests;

public sealed class Country : StringEnum<Country>
{
    public static Country Undefined { get; } = Create("");
    public static Country Zambia { get; } = Create("Z");
    public static Country Tunisia { get; } = Create("Tu");
    public static Country Maroc { get; } = Create("M");
}

public class StringEnumTests : TestBase
{
    public StringEnumTests(ITestOutputHelper output) : base(output) { }

    public enum City { Lima, Osaka, Lukasa }

    public class Company
    {
        //public int Age { get; init; } = 42;
        //public string Name { get; init; } = "";
        //public City MyCity { get; init; }
        public Country MyCountry { get; init; } = Country.Undefined;
    }


    [Fact]
    public void T01_TypeTest()
    {
        Company company = new()
        { MyCountry = Country.Tunisia };
        //const string str = "somestring";
        //Assert.Equal(DateTime.Now.ToString(), DateTime.Now.Stringify(includeTypeName: false));
        Write(company.Stringify());
        //Write(company.MyCountry.ToString());
        //var xxx = Utilities.GetNonDefaultProperties(company);
        ;
    }

}
