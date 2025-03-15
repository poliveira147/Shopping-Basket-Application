using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your BasketService
builder.Services.AddScoped<IBasketService, BasketService>();
// Register DiscountService
builder.Services.AddScoped<IDiscountService, DiscountService>(); 

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