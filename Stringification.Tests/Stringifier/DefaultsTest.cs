namespace Stringification.Tests;

public class DefaultsTest
{
    public enum SomeEnum
    {
        Undefined = -1,
        Yes = 0,
        No = 1
    }

    public class Somethang
    {
        public SomeEnum Some { get; set; } = SomeEnum.Undefined;
    }

    [Fact]
    public void T0_Defaults()
    {
        var somethang = new Somethang();

        Assert.Equal("", somethang.Stringify(includeTypeName: false));

        somethang.Some = SomeEnum.Yes;

        Assert.Equal("{Some:Yes}", somethang.Stringify(includeTypeName: false));
    }
}

