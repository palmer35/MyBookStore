using Serilog;

internal class Program
{
    private static void Main(string[]args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Конфигурация логирования
        LoggingConfiguration.ConfigureLogging(builder.Host);

        // Конфигурация сервисов
        ServiceConfiguration.ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Конфигурация middleware
        MiddlewareConfiguration.ConfigureMiddleware(app);

        try
        {
            Log.Information("Запуск приложения...");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Приложение завершилось с ошибкой.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}