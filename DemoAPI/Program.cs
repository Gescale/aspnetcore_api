using DemoAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using DemoAPI.Data;
{
    
}

var builder = WebApplication.CreateBuilder(args);

//Seup Database connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShirtStoreManagement"));
});

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Configure Controllers pipeline
app.MapControllers();

// Run the application
app.Run();
