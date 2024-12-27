using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookStore.Models;

var builder = WebApplication.CreateBuilder(args);

// ��������� ����������� �������
// 1. ����������� � ���� ������
builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. ��������� ���� ������� BookFinder � BookStoreManager
builder.Services.AddScoped<BookFinder>();
builder.Services.AddScoped<BookStoreManager>();

// 3. ��������� ����������� � ��������� API
builder.Services.AddControllers();

// 4. ��������� Swagger (������������ ��� API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ��������� middleware
if (app.Environment.IsDevelopment())
{
    // �������� Swagger ������ � ������ ����������
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �������� ������������� � ����������� ������������
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // ������������ ������� � �������������

// ������ ����������
app.Run();