using Microsoft.EntityFrameworkCore;
using DemoAPI.Data;
using DemoAPI.Filters.OperationFilter;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Seup Database connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShirtStoreManagement"));
});

// Add services to the container.
builder.Services.AddControllers();

//Used swashbuckle v6.5.0 and OpenApi v1.6.18, those are the only versions that allowed me to follow through the tutorials without throwing error messages.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AuthorizationHeaderOperationFilter>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure Controllers pipeline
app.MapControllers();

// Run the application
app.Run();



































// Using OpenApi instead of Swagger
//builder.Services.AddOpenApi(options =>
//{
//    options.AddDocumentTransformer((document, context, cancellationToken) =>
//    {
//        document.Components ??= new OpenApiComponents();
//        document.Components.SecuritySchemes.TryAdd("Bearer", new OpenApiSecurityScheme
//        {
//            Scheme = "Bearer",
//            Type = SecuritySchemeType.Http,
//            BearerFormat = "JWT",
//            In = ParameterLocation.Header,
//        });


//        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
//        {
//            [new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//            {
//                Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                {
//                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            }] = new string[] { }
//        });

//        return Task.CompletedTask;
//    });
//});
