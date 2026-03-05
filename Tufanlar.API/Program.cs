using Microsoft.EntityFrameworkCore;
using Tufanlar.API.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný Servisini Ekle (HATAYI ÇÖZEN KISIM BURASI)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Controller'larý (API uçlarýný) ekle
builder.Services.AddControllers();

// 3. Swagger (API Test Ekraný) ayarlarý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. HTTP Ýstek Hattý (Pipeline)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();