using GymMangamentSystem.Reposatory.Services.Auth;
using Microsoft.Extensions.Caching.Memory;

namespace GymMangamentSystem.UnitTests.Services.Auth;

public sealed class OtpServiceTests : IDisposable
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private readonly OtpService _sut;

    public OtpServiceTests() => _sut = new OtpService(_cache);

    [Fact]
    public void GenerateOtp_ReturnsSixNumericCharacters()
    {
        var otp = _sut.GenerateOtp("user@test.com");
        Assert.Equal(6, otp.Length);
        Assert.True(otp.All(char.IsDigit));
    }

    [Fact]
    public void IsValidOtp_WhenEmailWasNotRequested_ReturnsFalse()
    {
        Assert.False(_sut.IsValidOtp("missing@test.com", "123456"));
    }

    [Fact]
    public void IsValidOtp_WhenOtpIsWrong_ReturnsFalse()
    {
        _sut.GenerateOtp("user@test.com");
        Assert.False(_sut.IsValidOtp("user@test.com", "wrong"));
    }

    [Fact]
    public void IsValidOtp_WhenOtpIsCorrect_VerifiesEmail()
    {
        var otp = _sut.GenerateOtp("user@test.com");
        var result = _sut.IsValidOtp("user@test.com", otp);
        Assert.True(result);
        Assert.True(_cache.TryGetValue("user@test.com", out bool verified));
        Assert.True(verified);
    }

    [Fact]
    public void IsValidOtp_WhenSameOtpIsUsedTwice_ReturnsFalseSecondTime()
    {
        var otp = _sut.GenerateOtp("user@test.com");
        Assert.True(_sut.IsValidOtp("user@test.com", otp));
        Assert.False(_sut.IsValidOtp("user@test.com", otp));
    }

    [Fact]
    public void GenerateOtp_AfterPreviousVerification_ClearsVerifiedFlag()
    {
        var otp = _sut.GenerateOtp("user@test.com");
        Assert.True(_sut.IsValidOtp("user@test.com", otp));
        _sut.GenerateOtp("user@test.com");
        Assert.False(_cache.TryGetValue("user@test.com", out bool _));
    }

    public void Dispose() => _cache.Dispose();
}
