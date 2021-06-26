using System;
using Xunit;
using Xunit.Abstractions;
using Stringification;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Stringification.Tests
{
    public class NoConstructor
    {
        public string P1 { get; }
        private NoConstructor(Object o) => P1 = "data";
    }

    internal class BaseClass10
    {
        /*
        public int PropertyInBasei;
        public string PropertyInBase;
        public BaseClass10(int x, string s)
        {
            PropertyInBasei = x;
            PropertyInBase = "init";
        }
        //public BaseClass10() 
        //{
        //    PropertyInBase = "init";
        //}
        public BaseClass10(long x = 1)
        {
            PropertyInBase = "init";
        }

        public BaseClass10(int x)
        {
            PropertyInBasei = x;
            PropertyInBase = "init";
        }
        */

    }

    internal class TestClass10 //: BaseClass10
    {
        public int Property { get; init; }
        //public TestClass10() { }
    }


    public class CreateInstanceTests : TestBase
    {
        public CreateInstanceTests(ITestOutputHelper output) : base(output) { }

    [Fact]
        public void T01_Create_Instance()
        {
            var test = new TestClass1()
            {
                //PropertyInBase = 1,
                Property = 2
            };

            var result = test.Stringify();

            //Assert.Equal("TestClass1: {PropertyInBase:1, Property:2}", result);

           // if (result != null)
            //    WriteLine(result);

            var xxx = new BaseClass10();

            //var xx = Utilities.CreateInstance<BaseClass10>();
            var xx = Stringifier.CreateInstance<NoConstructor>();
            Write(xx.Stringify());
        }
    }
}