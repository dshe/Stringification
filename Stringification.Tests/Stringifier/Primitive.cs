namespace Stringification.Tests;

public class PrimitiveTests(ITestOutputHelper output) : TestBase(output)
{
    [Fact]
    public void T01_String()
    {
        Assert.Equal("\"\"", Stringifier.Stringify("", includeTypeName: false));
        Assert.Equal("String: \"\"", Stringifier.Stringify("", includeTypeName: true));

        const string str = "somestring";
        Assert.Equal("\"" + str + "\"", Stringifier.Stringify(str, includeTypeName: false));
    }

    [Fact]
    public void T02_Int()
    {
        Assert.Equal("42", Stringifier.Stringify(42, includeTypeName: false));
    }

    [Fact]
    public void T03_Date()
    {
        Assert.Equal(DateTime.Now.ToString(), Stringifier.Stringify(DateTime.Now, includeTypeName: false));
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
