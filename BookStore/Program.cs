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

    /// <summary>
    /// Работа с логгама
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
    /// Работа с middleware
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
    /// Работа с сервисами
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Подключение к базе данных
        services.AddDbContext<BookStoreContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Добавление сервисов
        services.AddScoped<BookService>();
        services.AddScoped<UsersService>();

        // Добавление контроллеров и поддержку API
        services.AddControllers();

        // Добавление Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
}