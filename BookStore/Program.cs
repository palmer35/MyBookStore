using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Добавляем необходимые сервисы
// 1. Подключение к базе данных
builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Добавляем ваши сервисы BookFinder и BookStoreManager
builder.Services.AddScoped<BookFinder>();
builder.Services.AddScoped<BookStoreManager>();

// 3. Добавляем контроллеры и поддержку API
builder.Services.AddControllers();

// 4. Добавляем Swagger (документация для API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включаем маршрутизацию и подключение контроллеров
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Сопоставляем запросы с контроллерами

// Запуск приложения
app.Run();