using Microsoft.OpenApi.Models;

namespace GymMangamentSystem.Apis.Extention
{
    public static class AddSwaggerDocumentation
    {
        public static IServiceCollection AddSwaggerDocumentationService(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreAPI", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }
                };

                options.AddSecurityDefinition("bearer", securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, new[] { "bearer" } }
        });
            });

            return services;
        }

    }
}
