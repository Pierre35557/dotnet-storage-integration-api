using StorageIntegrationApi.Api.Extensions;
using StorageIntegrationApi.Api.Middleware;
using StorageIntegrationApi.Application.ServiceRegistration;
using StorageIntegrationApi.Infrastructure.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.ConfigureApiControllers();
builder.Services.AddSwagger();

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

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

    app.MapControllers();
}

