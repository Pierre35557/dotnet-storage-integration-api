using SharePointIntegrationApi.Api.Extensions;
using SharePointIntegrationApi.Api.Middleware;
using SharePointIntegrationApi.Application.ServiceRegistration;
using SharePointIntegrationApi.Infrastructure.ServiceRegistration;

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

