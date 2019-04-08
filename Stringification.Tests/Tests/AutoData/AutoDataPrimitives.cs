using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Stringification;
using System;
using StringEnums;
using NodaTime;

namespace Stringification.Tests
{
    public enum TestEnum { One, Two, Three }

    public sealed class TestStringEnum : StringEnum<TestStringEnum>
    {
        public static readonly TestStringEnum Undefined = Create("");
        public static readonly TestStringEnum Cash = Create("C");
        public static readonly TestStringEnum Stock = Create("STK");
    }

    public class AutoDataPrimitvesTest : BaseTest
    {
        public AutoDataPrimitvesTest(ITestOutputHelper output) : base(output) { }

        private void TestCreate<T>()
        {
            var value = AutoData.Create<T>();
            if (value == null)
                throw new ArgumentNullException();
            WriteLine(value.Stringify());
        }

        [Fact]
        public void Test_Others()
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
            TestCreate<Instant>();
            TestCreate<LocalTime>();
            TestCreate<LocalDateTime>();
            TestCreate<ZonedDateTime>();
            TestCreate<TestEnum>();
            TestCreate<TestStringEnum>();
        }

        [Fact]
        public void Test_List()
        {
            TestCreate<List<int>>();
        }

        [Fact]
        public void Test_Dictionary()
        {
            TestCreate<Dictionary<int, string>>();
        }

        [Fact]
        public void Test_KeyValuePair()
        {
            TestCreate<KeyValuePair<int, string>>();
        }

    }
}
