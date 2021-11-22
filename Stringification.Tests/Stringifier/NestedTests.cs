using Xunit;
using Xunit.Abstractions;
namespace Stringification.Tests;

public sealed class Nested1
{
    //public int NestedProperty1 { get; init; }
    public Nested1()
    {
    }
}

public sealed class ClassX
{
    public Nested1 Nested1 { get; private set; }
    //public int Property2 { get; init; }
    public ClassX(string ss)
    {
        Nested1 = new Nested1();
    }
}

public class EmptyTests : TestBase
{
    public EmptyTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void T01_Empty_Default()
    {
        var test = new ClassX("xx");
        var result = test.Stringify();
        Assert.Equal("ClassX: {}", result);
    }
}
