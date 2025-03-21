using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Services;
using Microsoft.EntityFrameworkCore;

using ShoppingBasket.Data.Repositories;

using ShoppingBasket.Data.Database;
using ShoppingBasket.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add SQLite Database
builder.Services.AddDbContext<ShoppingBasketDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Initialize log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

// Add services to the container
builder.Services.AddControllers();

// Add Swagger services
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Register your BasketService and DiscountService
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

// Register ILog as a singleton
builder.Services.AddSingleton<ILog>(provider =>
{
    return LogManager.GetLogger(typeof(Program));
});

var allowedOrigins = builder.Configuration.GetValue<string>("allowedOrigins")?.Split(",") ?? new string[] { };

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});
var app = builder.Build();

app.UseCors();

//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger(); // Enable Swagger UI
//    app.UseSwaggerUI(); // Enable Swagger UI in the browser
//}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();