using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Initialize log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

// Add services to the container
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your BasketService and DiscountService
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

// Register ILog as a singleton
builder.Services.AddSingleton<ILog>(provider =>
{
    return LogManager.GetLogger(typeof(Program));
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger UI
    app.UseSwaggerUI(); // Enable Swagger UI in the browser
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();