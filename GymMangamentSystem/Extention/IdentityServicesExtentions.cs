using GymMangamentSystem.Apis.Helpers;
using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Identity;
using GymMangamentSystem.Reposatory.Data.Context;
using GymMangamentSystem.Reposatory.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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
            });
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnections"));
            });

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
