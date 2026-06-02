using Microsoft.EntityFrameworkCore;
using SmartVisitorManagement.Core.Interfaces.Repositories;
using SmartVisitorManagement.Core.Interfaces.Services;
using SmartVisitorManagement.Infrastructure.Data;
using SmartVisitorManagement.Infrastructure.Repositories;
using SmartVisitorManagement.Services;

namespace SmartVisitorManagement.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("SmartVisitorManagement.Infrastructure")
            ));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVisitorRepository, VisitorRepository>();
        services.AddScoped<IVisitRepository, VisitRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IVisitorService, VisitorService>();
        services.AddScoped<IVisitService, VisitService>();
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Smart Visitor Management API",
                Version = "v1",
                Description = "A professional Visitor Management System built with ASP.NET Core 8"
            });

            // Add API Key input to Swagger UI
            options.AddSecurityDefinition("ApiKey", new()
            {
                Description = "Enter your API Key in the field below. Header: X-Api-Key",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Name = "X-Api-Key",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new()
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "ApiKey" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}