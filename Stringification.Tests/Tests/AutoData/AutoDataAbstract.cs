using Stringification;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Stringification.Tests
{
    public abstract class Abs // input + output
    {
        public string P1 { get; } = "";
    }

    public class Class0: Abs
    {
        public string P3 { get; } = "";
    }

    public class Class1 // input + output
    {
        public Abs AbsInstance { get; set; } = new Class0();
        public string P2 { get; } = "";
    }

    public class TestAbstract : BaseTest
    {
        public TestAbstract(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Test1()
        {
            Assert.Throws<InvalidDataException>(() => AutoData.Create<Abs>());
            var instance0 = AutoData.Create<Class0>();
            var instance1 = AutoData.Create<Class1>();

            var list = AutoData.Create<List<Abs>>();
            Assert.Empty(list);
        }
    }
}
