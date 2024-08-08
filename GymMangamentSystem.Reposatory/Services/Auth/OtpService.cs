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
            var key = KeyGeneration.GenerateRandomKey(32);
            StoreKeyInCache(email, key);
            var totp = new Totp(key, step: 3600);
            return totp.ComputeTotp();
        }
        public bool IsValidOtp(string email, string otp)
        {
            var key = RetrieveKeyFromCache(email);
            if (key is null)
                return false;

            var totp = new Totp(key, step: 3600);
            var isValiddOtp = totp.VerifyTotp(otp, out _, new VerificationWindow(1, 1));
            if (!isValiddOtp)
                return false;

            _cache.Remove(email);
            _cache.Set(email, true, TimeSpan.FromMinutes(10));

            return isValiddOtp;
        }
        private void StoreKeyInCache(string email, byte[] key)
            =>
            _cache.Set(email, key, TimeSpan.FromMinutes(60));
        private byte[]? RetrieveKeyFromCache(string email)
        {
            if (_cache.TryGetValue(email, out byte[]? key))
                return key;

            return null;
        }
    }
}
