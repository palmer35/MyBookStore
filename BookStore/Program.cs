using Microsoft.EntityFrameworkCore;
using Serilog;

public class Program
{
    public static void Main(string[]args)
    {
        ConfigureLogging();

        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        ConfigureMiddleware(app);

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

    /// <summary>
    /// ������ � �������
    /// </summary>
    private static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt")
            .CreateLogger();
    }

    /// <summary>
    /// ������ � middleware
    /// </summary>
    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
    }

    /// <summary>
    /// ������ � ���������
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // ����������� � ���� ������
        services.AddDbContext<BookStoreContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // ���������� ��������
        services.AddScoped<BookService>();
        services.AddScoped<UsersService>();

        // ���������� ������������ � ��������� API
        services.AddControllers();

        // ���������� Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
}