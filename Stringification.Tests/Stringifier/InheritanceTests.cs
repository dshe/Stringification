using System.Reflection;

namespace Stringification.Tests;

internal class BaseClass1
{
    public int PropertyInBase { get; init; }
    public BaseClass1() { }
}

internal class TestClass1 : BaseClass1
{
    public int Property { get; init; }
    public TestClass1()
    {
    }
}


public class InheritanceTests : TestBase
{
    public InheritanceTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void T01_Access_Default()
    {
        var test = new TestClass1()
        {
            PropertyInBase = 1,
            Property = 2
        };

        var result = test.Stringify();

        Assert.Equal("TestClass1: {PropertyInBase:1, Property:2}", result);

        if (result != null)
            Write(result);

        var xx = Stringifier.CreateInstance(typeof(BaseClass1).GetTypeInfo());
        ;


    }
}
