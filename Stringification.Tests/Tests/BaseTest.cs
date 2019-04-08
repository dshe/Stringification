using System;
using Xunit;
using Xunit.Abstractions;

namespace Stringification.Tests
{
    public abstract class BaseTest
    {
        protected readonly Action<string> WriteLine;

        public BaseTest(ITestOutputHelper output)
        {
            WriteLine = output.WriteLine;
        }
    }
}

