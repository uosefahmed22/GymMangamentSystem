using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Services.Auth;
using GymMangamentSystem.UnitTests.TestDoubles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GymMangamentSystem.UnitTests.Services.Auth;

public class TokenServiceTests
{
    [Fact]
    public void GenerateRefreshToken_ReturnsUniqueCryptographicallySizedActiveTokens()
    {
        var sut = CreateSut(new FakeUserManager());
        var first = sut.GenerateRefreshToken();
        var second = sut.GenerateRefreshToken();
        Assert.NotEqual(first.Token, second.Token);
        Assert.Equal(32, Convert.FromBase64String(first.Token).Length);
        Assert.True(first.IsActive);
        Assert.InRange(first.Expires, DateTime.UtcNow.AddDays(6.99), DateTime.UtcNow.AddDays(7.01));
    }

    [Fact]
    public async Task CreateTokenAsync_WhenUserIsNull_Throws()
    {
        var sut = CreateSut(new FakeUserManager());
        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.CreateTokenAsync(null!));
    }

    [Fact]
    public async Task CreateTokenAsync_WhenEmailIsNull_Throws()
    {
        var sut = CreateSut(new FakeUserManager());
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateTokenAsync(new AppUser()));
    }

    [Fact]
    public async Task CreateTokenAsync_WhenUserHasNoRole_Throws()
    {
        var manager = new FakeUserManager
        {
            GetRolesHandler = _ => Task.FromResult<IList<string>>([])
        };
        var sut = CreateSut(manager);
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateTokenAsync(User()));
    }

    [Fact]
    public async Task CreateTokenAsync_WhenJwtKeyIsMissing_Throws()
    {
        var sut = CreateSut(new FakeUserManager(), new Dictionary<string, string?>
        {
            ["JWT:DurationInDays"] = "1"
        });
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateTokenAsync(User()));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("not-a-number")]
    [InlineData("0")]
    [InlineData("-1")]
    public async Task CreateTokenAsync_WhenDurationIsInvalid_Throws(string? duration)
    {
        var settings = ValidSettings();
        settings["JWT:DurationInDays"] = duration;
        var sut = CreateSut(new FakeUserManager(), settings);
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateTokenAsync(User()));
        Assert.Contains("positive", exception.Message);
    }

    [Fact]
    public async Task CreateTokenAsync_WhenUpdateFails_Throws()
    {
        var manager = new FakeUserManager
        {
            UpdateHandler = _ => Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "db" }))
        };
        var sut = CreateSut(manager);
        await Assert.ThrowsAsync<Exception>(() => sut.CreateTokenAsync(User()));
    }

    [Fact]
    public async Task CreateTokenAsync_WhenValid_ReturnsJwtAndPersistsRefreshToken()
    {
        var updateCalls = 0;
        var manager = new FakeUserManager
        {
            GetRolesHandler = _ => Task.FromResult<IList<string>>(["Member", "Trainer"]),
            UpdateHandler = _ => { updateCalls++; return Task.FromResult(IdentityResult.Success); }
        };
        var user = User();
        var sut = CreateSut(manager);

        var (jwt, refresh) = await sut.CreateTokenAsync(user);
        var parsed = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        Assert.Equal("issuer", parsed.Issuer);
        Assert.Contains("audience", parsed.Audiences);
        Assert.Equal(user.Id, parsed.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal(user.Email, parsed.Claims.Single(c => c.Type == ClaimTypes.Email).Value);
        Assert.Equal(["Member", "Trainer"], parsed.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));
        Assert.Same(refresh, Assert.Single(user.RefreshTokens));
        Assert.Equal(32, Convert.FromBase64String(refresh.Token).Length);
        Assert.Equal(1, updateCalls);
    }

    private static TokenService CreateSut(FakeUserManager manager, Dictionary<string, string?>? settings = null)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings ?? ValidSettings())
            .Build();
        return new TokenService(configuration, manager);
    }

    private static Dictionary<string, string?> ValidSettings() => new()
    {
        ["JWT:Key"] = "this-is-a-test-key-that-is-long-enough-123456789",
        ["JWT:DurationInDays"] = "1",
        ["JWT:ValidIssuer"] = "issuer",
        ["JWT:ValidAudience"] = "audience"
    };

    private static AppUser User() => new()
    {
        Id = Guid.NewGuid().ToString(),
        Email = "user@test.com",
        UserName = "user",
        DisplayName = "User",
        UserCode = "G1"
    };
}
