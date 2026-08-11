using GymMangamentSystem.Apis.Controllers;
using GymMangamentSystem.Core.Models.Identity;
using GymMangamentSystem.UnitTests.TestDoubles;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.UnitTests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task RefreshToken_WhenServiceSucceeds_ReturnsBothTokens()
    {
        var tokens = new FakeTokenService
        {
            RefreshHandler = _ => Task.FromResult(("new-jwt", new RefreshToken { Token = "new-refresh" }))
        };
        var result = await new AuthController(tokens).RefreshToken(new TokenRequest { RefreshToken = "old" });
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("new-jwt", ok.Value!.ToString());
        Assert.Contains("new-refresh", ok.Value!.ToString());
    }

    [Fact]
    public async Task RefreshToken_WhenServiceRejects_ReturnsUnauthorized()
    {
        var tokens = new FakeTokenService
        {
            RefreshHandler = _ => throw new UnauthorizedAccessException("invalid")
        };
        var result = await new AuthController(tokens).RefreshToken(new TokenRequest { RefreshToken = "bad" });
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task RevokeToken_WhenTokenIsInvalid_ReturnsBadRequest()
    {
        var tokens = new FakeTokenService { RevokeHandler = _ => Task.FromResult(false) };
        var result = await new AuthController(tokens).RevokeToken(new RevokeTokenRequest { RefreshToken = "bad" });
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RevokeToken_WhenTokenIsActive_ReturnsOk()
    {
        var tokens = new FakeTokenService { RevokeHandler = _ => Task.FromResult(true) };
        var result = await new AuthController(tokens).RevokeToken(new RevokeTokenRequest { RefreshToken = "valid" });
        Assert.IsType<OkObjectResult>(result);
    }
}
