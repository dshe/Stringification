using System.Collections.Generic;
using Stringifier.Test;
using Xunit;
using Xunit.Abstractions;
using Stringification;
using System;
using StringEnums;
using NodaTime;

namespace TestAutoData
{
    public enum TestEnum { EOne, ETwo, EThree }

    public sealed class TestStringEnum : StringEnum<TestStringEnum>
    {
        public static readonly TestStringEnum Undefined = Create("");
        public static readonly TestStringEnum Cash = Create("C");
        public static readonly TestStringEnum Stock = Create("STK");
    }

    public class WithPrivateConstructor
    {
        public int PublicProperty { get; set; }
        public int PrivateProperty { get; private set; }
        public int ReadOnlyProperty { get; }
        private WithPrivateConstructor(Object o)
        {
            ReadOnlyProperty = -1;
        }
    }

    public class TestClass
    {
        public int GetterOnly { get; }
        public int Second { get; set; }
        //public IList<InnerClass> Legs { get; } = new List<InnerClass>();
        public TestStringEnum SecurityType { get; } = TestStringEnum.Cash;
        private TestClass(int val)
        {
            GetterOnly = val;
        }

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
        public void Test0_Prinitives()
        {
            TestCreate<char>();
            TestCreate<string>();
            TestCreate<bool>();
            TestCreate<int>();
            TestCreate<long>();
            TestCreate<float>();
            TestCreate<double>();
            TestCreate<decimal>();
            TestCreate<DateTime>();
            TestCreate<ZonedDateTime>();
            TestCreate<LocalDateTime>();
            TestCreate<List<int>>();
            TestCreate<TestEnum>();
            TestCreate<TestStringEnum>();
        }

        [Fact]
        public void Test1_Class()
        {
            TestCreate<WithPrivateConstructor>();
        }


        [Fact]
        public void Test_1()
        {
            var c1 = AutoData.Create<TestClass>();
            var c2 = AutoData.Create<TestClass>();

            WriteLine(c1.Stringify());
            WriteLine("");
            WriteLine(c2.Stringify());

            Assert.NotEqual(c1.Stringify(), c2.Stringify());
        }
    }
}
