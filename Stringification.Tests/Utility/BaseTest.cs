﻿using System;
using Xunit;
using Xunit.Abstractions;
using Stringification;

namespace Stringifier.Test
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

