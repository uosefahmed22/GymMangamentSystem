using GymMangamentSystem.Core.Models.Identity;

namespace GymMangamentSystem.UnitTests.Models.Identity;

public class RefreshTokenTests
{
    [Fact]
    public void IsActive_WhenFutureAndNotRevoked_ReturnsTrue()
    {
        var token = new RefreshToken { Expires = DateTime.UtcNow.AddMinutes(1) };
        Assert.True(token.IsActive);
        Assert.False(token.IsExpired);
    }

    [Fact]
    public void IsActive_WhenExpired_ReturnsFalse()
    {
        var token = new RefreshToken { Expires = DateTime.UtcNow.AddSeconds(-1) };
        Assert.False(token.IsActive);
        Assert.True(token.IsExpired);
    }

    [Fact]
    public void IsActive_WhenRevoked_ReturnsFalse()
    {
        var token = new RefreshToken
        {
            Expires = DateTime.UtcNow.AddMinutes(1),
            Revoked = DateTime.UtcNow
        };
        Assert.False(token.IsActive);
    }
}
