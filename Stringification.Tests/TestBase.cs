using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
namespace Stringification.Tests;

public abstract class TestBase
{
    protected readonly Action<string> Write;
    protected readonly ILogger Logger;
    protected readonly Stringifier Stringifier;

    protected TestBase(ITestOutputHelper output)
    {
        Write = output.WriteLine;

        Logger = new LoggerFactory()
            .AddMXLogger(Write)
            .CreateLogger("Test");

        Stringifier = new Stringifier(Logger);
    }
}
