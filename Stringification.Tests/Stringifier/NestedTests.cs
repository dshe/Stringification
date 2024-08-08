namespace Stringification.Tests;

public sealed class Nested1
{
    //public int NestedProperty1 { get; init; }
    public Nested1()
    {
    }
}

#pragma warning disable CS9113
public sealed class ClassX(string ss)
{
    public Nested1 Nested1 { get; private set; } = new Nested1();
}
#pragma warning restore CS9113

public class EmptyTests(ITestOutputHelper output) : TestBase(output)
{
    [Fact]
    public void T01_Empty_Default()
    {
        var test = new ClassX("xx");
        var result = test.Stringify();
        Assert.Equal("ClassX: {}", result);
    }
}
