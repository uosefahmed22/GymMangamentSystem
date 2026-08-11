using GymMangamentSystem.Core.IServices.Auth;
using Microsoft.Extensions.Caching.Memory;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Services.Auth
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;

        public OtpService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public string GenerateOtp(string email)
        {
            _cache.Remove(email);
            var key = KeyGeneration.GenerateRandomKey(32);
            StoreKeyInCache(email, key);
            var totp = new Totp(key, step: 300);
            return totp.ComputeTotp();
        }
        public bool IsValidOtp(string email, string otp)
        {
            var key = RetrieveKeyFromCache(email);
            if (key is null)
                return false;

            var totp = new Totp(key, step: 300);
            var isValidOtp = totp.VerifyTotp(otp, out _, new VerificationWindow(0, 0));
            if (!isValidOtp)
                return false;

            _cache.Remove(GetOtpCacheKey(email));
            _cache.Set(email, true, TimeSpan.FromMinutes(10));

            return true;
        }
        private void StoreKeyInCache(string email, byte[] key)
            =>
            _cache.Set(GetOtpCacheKey(email), key, TimeSpan.FromMinutes(5));
        private byte[]? RetrieveKeyFromCache(string email)
        {
            if (_cache.TryGetValue(GetOtpCacheKey(email), out byte[]? key))
                return key;

            return null;
        }
        private static string GetOtpCacheKey(string email) => $"otp:key:{email}";
    }
}
