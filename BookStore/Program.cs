using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .CreateLogger();

builder.Host.UseSerilog();

// ��������� ����������� �������
// 1. ����������� � ���� ������
builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. ��������� ���� ������� BookFinder � BookStoreManager
builder.Services.AddScoped<BookFinder>();
builder.Services.AddScoped<UserFinder>();

// 3. ��������� ����������� � ��������� API
builder.Services.AddControllers();

// 4. ��������� Swagger (������������ ��� API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ��������� middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �������� ������������� � ����������� ������������
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // ������������ ������� � �������������

try
{
    Log.Information("������ ����������...");
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "���������� ����������� � �������.");
}
finally
{
    Log.CloseAndFlush();
}
