using System;
using Xunit;
using Xunit.Abstractions;
using Stringification;

namespace Stringification.Tests
{
    public class AccessTests :  TestBase
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

            public string PublicProperty { get; set; } = "";
            internal string InternalProperty { get; } = "";
            private string PrivateProperty { get; } = "";
#pragma warning restore 0414

            public TestClass() { }
        }

        public AccessTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void T01_Access_Default()
        {
            TestClass test = new();

            Assert.Equal("TestClass: {}", test.Stringify());

            test.PublicProperty = "x";
            Assert.Equal("TestClass: {PublicProperty:\"x\"}", test.Stringify());
        }
    }
}