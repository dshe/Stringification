using System;
using Xunit;
using Xunit.Abstractions;
using Stringification;

namespace Stringification.Tests
{
    public class AccessTests : BaseTest
    {
        internal class TestClass
        {
#pragma warning disable 414
            public readonly string PublicReadonlyField = "";
            internal readonly string InternalReadonlyField = "";
            private readonly string privateReadonlyField = "";

            public string PublicField = "";
            internal string InternalField = "";
            private string privateField = "";

            public string PublicProperty { get; } = "";
            internal string InternalProperty { get; } = "";
            private string PrivateProperty { get; } = "";
#pragma warning restore 0414

            public TestClass() { }
        }

        public AccessTests(ITestOutputHelper output): base(output) { }

        [Fact]
        public void T01_Access_Default()
        {
            var test = new TestClass();

            var result = test.Stringify();
            if (result != null)
                WriteLine(result);

            Assert.Equal("TestClass: {}", result);
        }

    }
}