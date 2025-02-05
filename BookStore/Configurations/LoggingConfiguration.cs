using Serilog;
public static class LoggingConfiguration
{
    public static void ConfigureLogging(IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Information()
       .WriteTo.Console()
       .WriteTo.File("logs/log.txt")
       .CreateLogger();

        hostBuilder.UseSerilog();
    }
}
