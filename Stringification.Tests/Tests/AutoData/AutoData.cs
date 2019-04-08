using Stringification;
using Xunit;
using Xunit.Abstractions;

namespace Stringification.Tests
{
    public sealed class Tag
    {
        public string P1 { get; }
        public string P2 { get; set; } = "";
        public string P3 { get; private set; }
        public Tag()
        {
            P1 = "1";
            //P1 = p1;
            //P2 = p2;
            P3 = "3";
        }
    }

    public class TestProperty : BaseTest
    {
        public TestProperty(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Test1()
        {
            var instance = AutoData.Create<Tag>();
            Assert.False(string.IsNullOrEmpty(instance.P1));
            Assert.False(string.IsNullOrEmpty(instance.P2));
            Assert.False(string.IsNullOrEmpty(instance.P3));
            WriteLine(instance.Stringify());
        }

    }
}
