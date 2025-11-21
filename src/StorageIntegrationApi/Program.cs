using StorageIntegrationApi.Api.Extensions;
using StorageIntegrationApi.Api.Helpers;
using StorageIntegrationApi.Api.Middleware;
using StorageIntegrationApi.Application.ServiceRegistration;
using StorageIntegrationApi.Infrastructure.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

LoadEnvironmentVariables();
ConfigureAppSettings(builder);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.ConfigureApiControllers();
builder.Services.AddSwagger();
builder.Services.AddApiRateLimiting();

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

static void LoadEnvironmentVariables()
{
    var root = Directory.GetCurrentDirectory();
    var dotenv = Path.Combine(root, "dev.env");
    EnvironmentConfigLoader.Load(dotenv);
}

void ConfigureAppSettings(WebApplicationBuilder builder)
{
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddEnvironmentVariables();
}

static void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<ApiResponseMiddleware>();

    app.UseRateLimiter();

    app.MapControllers();
}

