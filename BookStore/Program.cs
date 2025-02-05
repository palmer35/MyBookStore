using Serilog;

internal class Program
{
    private static void Main(string[]args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ������������ �����������
        LoggingConfiguration.ConfigureLogging(builder.Host);

        // ������������ ��������
        ServiceConfiguration.ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // ������������ middleware
        MiddlewareConfiguration.ConfigureMiddleware(app);

        try
        {
            Log.Information("������ ����������...");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "���������� ����������� � �������.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}