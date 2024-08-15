using GymMangamentSystem.Apis.Helpers;
using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Identity;
using GymMangamentSystem.Reposatory.Data.Context;
using GymMangamentSystem.Reposatory.Services;
using GymMangamentSystem.Reposatory.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json;

namespace GymMangamentSystem.Apis.Extention
{
    public static class IdentityServicesExtentions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1; 
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Message = "You are not authorized to access this resource."
                        });

                        return context.Response.WriteAsync(result);
                    }
                };
            });
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.AddSingleton<SoftDeleteInterceptor>();

            // Add DbContext with respect to the SoftDeleteInterceptor
            services.AddDbContext<AppDBContext>(
                (serviceProvider, options) => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnections"))
                .AddInterceptors(serviceProvider.GetRequiredService<SoftDeleteInterceptor>()));


            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySetting"));
            
            services.AddSingleton(cloudinary =>
            {
                var config = configuration.GetSection("CloudinarySetting").Get<CloudinarySettings>();
                var account = new CloudinaryDotNet.Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new CloudinaryDotNet.Cloudinary(account);
            });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
