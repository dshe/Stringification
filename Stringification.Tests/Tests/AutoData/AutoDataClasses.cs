using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Stringification;
using System;
using StringEnums;
using NodaTime;

namespace Stringification.Tests
{
    public class WithNoConstructor
    {
        public string P1 { get; }
        private WithNoConstructor(Object o) => P1 = "data";
    }

    public class WithPrivateConstructor
    {
        public string P1 { get; }
        private WithPrivateConstructor() => P1 = "data";
    }

    public class AutoDataTests : BaseTest
    {
        public AutoDataTests(ITestOutputHelper output) : base(output) { }

        private void TestCreate<T>()
        {
            var value = AutoData.Create<T>();
            if (value == null)
                throw new ArgumentNullException();
            WriteLine(value.Stringify());
        }

        [Fact]
        public void Test1_Class()
        {
            TestCreate<WithNoConstructor>();
        }


        [Fact]
        public void Test_1()
        {
            var c1 = AutoData.Create<WithNoConstructor>();
            var c2 = AutoData.Create<WithNoConstructor>();

            WriteLine(c1.Stringify());
            WriteLine("");
            WriteLine(c2.Stringify());

            Assert.NotEqual(c1.Stringify(), c2.Stringify());
        }
    }
}
