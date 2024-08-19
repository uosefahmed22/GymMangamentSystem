using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration configuration,UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<(string, RefreshToken)> CreateTokenAsync(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            // Create claims for the JWT
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? throw new InvalidOperationException("User email cannot be null")),
            new Claim(ClaimTypes.GivenName, user.DisplayName ?? string.Empty),
            new Claim("TrainerId", user.Id.ToString()),
            new Claim("UserId", user.Id.ToString())
        };

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
            {
                throw new InvalidOperationException("User roles cannot be null or empty");
            }

            // Add user roles to claims
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Create JWT security key
            var jwtKey = _configuration["JWT:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key cannot be null or empty");
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            // Create JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Create Refresh Token
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(30),
                Created = DateTime.UtcNow
            };

            // Save the refresh token for the user (make sure to persist this to the database)
            user.RefreshTokens.Add(refreshToken);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to update user with refresh token");
            }

            // Return JWT token and the new refresh token
            return (jwtToken, refreshToken);
        }


        public async Task<(string, RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null || !user.RefreshTokens.Any(t => t.Token == refreshToken && t.IsActive))
                throw new UnauthorizedAccessException("Invalid refresh token");

            var refreshTokenEntity = user.RefreshTokens.Single(t => t.Token == refreshToken);

            if (!refreshTokenEntity.IsActive)
                throw new UnauthorizedAccessException("Refresh token has expired");

            refreshTokenEntity.Revoked = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return await CreateTokenAsync(user);
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null) return false;

            var refreshTokenEntity = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (refreshTokenEntity == null || !refreshTokenEntity.IsActive)
                return false;

            refreshTokenEntity.Revoked = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return true;
        }
        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(7), 
                    Created = DateTime.UtcNow
                };
            }
        }


    }
}
