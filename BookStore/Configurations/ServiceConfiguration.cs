using Microsoft.EntityFrameworkCore;
public static class ServiceConfiguration
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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