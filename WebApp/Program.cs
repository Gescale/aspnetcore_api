using System.Globalization;
using WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("ShirtsApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7056/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IWebApiExecuter, WebApiExecuter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
