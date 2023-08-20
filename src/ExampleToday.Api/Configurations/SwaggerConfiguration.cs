using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExampleToday.Api.Configurations;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Frases Famosas", Version = "v1" });

            c.AddRequiredAuthorizationUser();
            c.AddSecurityRequirementApplication();
        });
    }

    private static void AddSecurityRequirementApplication(this SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Application", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "Autorização apenas para aplicações externas",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Application"
                    }
                },
                Array.Empty<string>()
            }
        });
    }

    private static void AddRequiredAuthorizationUser(this SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("User", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "Autorização para usuários"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "User"
                    },
                },
                Array.Empty<string>()
            }
        });
    }
}