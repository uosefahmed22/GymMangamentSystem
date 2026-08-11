using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Identity;
using GymMangamentSystem.Reposatory.Services.Auth;
using GymMangamentSystem.UnitTests.TestDoubles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GymMangamentSystem.UnitTests.Services.Auth;

public class TokenServiceRefreshTests
{
    [Fact]
    public async Task RefreshTokenAsync_WhenTokenDoesNotExist_ThrowsUnauthorized()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => Sut(new FakeUserManager()).RefreshTokenAsync("missing"));
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenIsExpired_ThrowsUnauthorized()
    {
        var manager = new FakeUserManager();
        manager.UsersValue.Add(UserWithRefresh("expired", DateTime.UtcNow.AddMinutes(-1)));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => Sut(manager).RefreshTokenAsync("expired"));
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenRevocationCannotBeSaved_Throws()
    {
        var manager = new FakeUserManager
        {
            UpdateHandler = _ => Task.FromResult(IdentityResult.Failed(new IdentityError()))
        };
        manager.UsersValue.Add(UserWithRefresh("active", DateTime.UtcNow.AddMinutes(10)));
        await Assert.ThrowsAsync<InvalidOperationException>(() => Sut(manager).RefreshTokenAsync("active"));
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenActive_RevokesOldAndCreatesNewToken()
    {
        var manager = new FakeUserManager();
        var user = UserWithRefresh("active", DateTime.UtcNow.AddMinutes(10));
        var oldToken = Assert.Single(user.RefreshTokens);
        manager.UsersValue.Add(user);
        var (_, newToken) = await Sut(manager).RefreshTokenAsync("active");
        Assert.NotNull(oldToken.Revoked);
        Assert.NotEqual(oldToken.Token, newToken.Token);
        Assert.Equal(2, user.RefreshTokens.Count);
    }

    [Fact]
    public async Task RevokeTokenAsync_WhenTokenDoesNotExist_ReturnsFalse()
    {
        Assert.False(await Sut(new FakeUserManager()).RevokeTokenAsync("missing"));
    }

    [Fact]
    public async Task RevokeTokenAsync_WhenTokenIsExpired_ReturnsFalse()
    {
        var manager = new FakeUserManager();
        manager.UsersValue.Add(UserWithRefresh("expired", DateTime.UtcNow.AddMinutes(-1)));
        Assert.False(await Sut(manager).RevokeTokenAsync("expired"));
    }

    [Fact]
    public async Task RevokeTokenAsync_WhenSaveFails_Throws()
    {
        var manager = new FakeUserManager
        {
            UpdateHandler = _ => Task.FromResult(IdentityResult.Failed(new IdentityError()))
        };
        manager.UsersValue.Add(UserWithRefresh("active", DateTime.UtcNow.AddMinutes(10)));
        await Assert.ThrowsAsync<InvalidOperationException>(() => Sut(manager).RevokeTokenAsync("active"));
    }

    [Fact]
    public async Task RevokeTokenAsync_WhenActive_RevokesAndReturnsTrue()
    {
        var manager = new FakeUserManager();
        var user = UserWithRefresh("active", DateTime.UtcNow.AddMinutes(10));
        manager.UsersValue.Add(user);
        Assert.True(await Sut(manager).RevokeTokenAsync("active"));
        Assert.NotNull(Assert.Single(user.RefreshTokens).Revoked);
    }

    private static TokenService Sut(FakeUserManager manager)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["JWT:Key"] = "this-is-a-test-key-that-is-long-enough-123456789",
            ["JWT:DurationInDays"] = "1",
            ["JWT:ValidIssuer"] = "issuer",
            ["JWT:ValidAudience"] = "audience"
        }).Build();
        return new TokenService(configuration, manager);
    }

    private static AppUser UserWithRefresh(string token, DateTime expires)
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(), Email = "user@test.com", UserName = "user",
            DisplayName = "User", UserCode = "G1"
        };
        user.RefreshTokens.Add(new RefreshToken
            { Token = token, Created = DateTime.UtcNow, Expires = expires });
        return user;
    }
}
