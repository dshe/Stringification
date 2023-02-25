using Microsoft.Extensions.Logging;

namespace Stringification.Tests;

public abstract class TestBase
{
    protected readonly Action<string> Write;
    protected readonly ILogger Logger;
    protected readonly Stringifier Stringifier;

    protected TestBase(ITestOutputHelper output)
    {
        Write = output.WriteLine;

        Logger = LoggerFactory
            .Create(builder =>
                builder.AddMXLogger(Write))
            .CreateLogger("Test");

        Stringifier = new Stringifier(Logger);
    }
}
