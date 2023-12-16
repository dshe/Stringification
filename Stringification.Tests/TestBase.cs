using Microsoft.Extensions.Logging;

namespace Stringification.Tests;

public abstract class TestBase
{
    protected readonly Action<string> Write;
    protected readonly Stringifier Stringifier;
    protected readonly ILoggerFactory LogFactory;
    protected readonly ILogger Logger;

    protected TestBase(ITestOutputHelper output)
    {
        Write = output.WriteLine;

        LogFactory = LoggerFactory
            .Create(builder => builder.AddMXLogger(Write));

        Logger = LogFactory.CreateLogger("Test");

        Stringifier = new Stringifier(LogFactory);
    }
}
