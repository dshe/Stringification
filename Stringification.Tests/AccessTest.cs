using System;
using Xunit;
using Xunit.Abstractions;
using Stringification;

namespace Stringifier.Test
{
    public class AccessTests
    {
        internal class TestClass
        {
#pragma warning disable 414
            public readonly string PublicReadonlyField;
            internal readonly string InternalReadonlyField;
            private readonly string privateReadonlyField;

            public string PublicField;
            internal string InternalField;
            private string privateField;

            public string PublicProperty { get; }
            internal string InternalProperty { get; }
            private string PrivateProperty { get; }
#pragma warning restore 0414

            public TestClass() { }
            public TestClass(bool set)
            {
                if (!set)
                    return;
                PublicReadonlyField = "";
                InternalReadonlyField = "";
                privateReadonlyField = "";

                PublicField = "";
                InternalField = "";
                privateField = "";

                PublicProperty = "";
                InternalProperty = "";
                PrivateProperty = "";
            }
        }

        protected readonly Action<string> Write;
        public AccessTests(ITestOutputHelper output) => Write = output.WriteLine;

        [Fact]
        public void T01_Access_Default()
        {

            var test = new TestClass(set: false);

            var result = test.Stringify();
            Write(result);

            Assert.Equal("TestClass: {}", result);
        }

        [Fact]
        public void T02_Access()
        {
            var test = new TestClass(set: true);

            var result = test.Stringify();
            Write(result);

            Assert.Equal("TestClass: {PublicProperty:\"\"}", result);
        }

    }
}