using System.Collections;
using System.Linq.Expressions;
using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace GymMangamentSystem.UnitTests.TestDoubles;

internal sealed class StubUserStore : IUserStore<AppUser>
{
    public void Dispose() { }
    public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id);
    public Task<string?> GetUserNameAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);
    public Task SetUserNameAsync(AppUser user, string? userName, CancellationToken cancellationToken) { user.UserName = userName; return Task.CompletedTask; }
    public Task<string?> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(user.NormalizedUserName);
    public Task SetNormalizedUserNameAsync(AppUser user, string? normalizedName, CancellationToken cancellationToken) { user.NormalizedUserName = normalizedName; return Task.CompletedTask; }
    public Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
    public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
    public Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
    public Task<AppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken) => Task.FromResult<AppUser?>(null);
    public Task<AppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) => Task.FromResult<AppUser?>(null);
}

internal sealed class FakeUserManager : UserManager<AppUser>
{
    public FakeUserManager()
        : base(new StubUserStore(), Microsoft.Extensions.Options.Options.Create(new IdentityOptions()), new PasswordHasher<AppUser>(),
            Array.Empty<IUserValidator<AppUser>>(), Array.Empty<IPasswordValidator<AppUser>>(),
            new UpperInvariantLookupNormalizer(), new IdentityErrorDescriber(), null!,
            NullLogger<UserManager<AppUser>>.Instance) { }

    public List<AppUser> UsersValue { get; } = [];
    public Func<string, Task<AppUser?>> FindByEmailHandler { get; set; } = _ => Task.FromResult<AppUser?>(null);
    public Func<string, Task<AppUser?>> FindByIdHandler { get; set; } = _ => Task.FromResult<AppUser?>(null);
    public Func<AppUser, string, Task<bool>> CheckPasswordHandler { get; set; } = (_, _) => Task.FromResult(false);
    public Func<AppUser, string, Task<IdentityResult>> CreateHandler { get; set; } = (_, _) => Task.FromResult(IdentityResult.Success);
    public Func<AppUser, string, Task<IdentityResult>> AddToRoleHandler { get; set; } = (_, _) => Task.FromResult(IdentityResult.Success);
    public Func<AppUser, Task<string>> EmailTokenHandler { get; set; } = _ => Task.FromResult("email-token");
    public Func<AppUser, Task<string>> ResetTokenHandler { get; set; } = _ => Task.FromResult("reset-token");
    public Func<AppUser, string, string, Task<IdentityResult>> ResetPasswordHandler { get; set; } = (_, _, _) => Task.FromResult(IdentityResult.Success);
    public Func<AppUser, string, string, Task<IdentityResult>> ChangePasswordHandler { get; set; } = (_, _, _) => Task.FromResult(IdentityResult.Success);
    public Func<AppUser, string, Task<IdentityResult>> ConfirmEmailHandler { get; set; } = (_, _) => Task.FromResult(IdentityResult.Success);
    public Func<AppUser, Task<IList<string>>> GetRolesHandler { get; set; } = _ => Task.FromResult<IList<string>>(["Member"]);
    public Func<AppUser, Task<IdentityResult>> UpdateHandler { get; set; } = _ => Task.FromResult(IdentityResult.Success);

    public override IQueryable<AppUser> Users => new TestAsyncEnumerable<AppUser>(UsersValue);
    public override Task<AppUser?> FindByEmailAsync(string email) => FindByEmailHandler(email);
    public override Task<AppUser?> FindByIdAsync(string userId) => FindByIdHandler(userId);
    public override Task<bool> CheckPasswordAsync(AppUser user, string password) => CheckPasswordHandler(user, password);
    public override Task<IdentityResult> CreateAsync(AppUser user, string password) => CreateHandler(user, password);
    public override Task<IdentityResult> AddToRoleAsync(AppUser user, string role) => AddToRoleHandler(user, role);
    public override Task<string> GenerateEmailConfirmationTokenAsync(AppUser user) => EmailTokenHandler(user);
    public override Task<string> GeneratePasswordResetTokenAsync(AppUser user) => ResetTokenHandler(user);
    public override Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword) => ResetPasswordHandler(user, token, newPassword);
    public override Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword) => ChangePasswordHandler(user, currentPassword, newPassword);
    public override Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token) => ConfirmEmailHandler(user, token);
    public override Task<IList<string>> GetRolesAsync(AppUser user) => GetRolesHandler(user);
    public override Task<IdentityResult> UpdateAsync(AppUser user) => UpdateHandler(user);
}

internal sealed class FakeSignInManager : SignInManager<AppUser>
{
    public FakeSignInManager(UserManager<AppUser> userManager)
        : base(userManager, new HttpContextAccessor(),
            new UserClaimsPrincipalFactory<AppUser>(userManager, Microsoft.Extensions.Options.Options.Create(new IdentityOptions())),
            Microsoft.Extensions.Options.Options.Create(new IdentityOptions()), NullLogger<SignInManager<AppUser>>.Instance,
            new AuthenticationSchemeProvider(Microsoft.Extensions.Options.Options.Create(new AuthenticationOptions())),
            new DefaultUserConfirmation<AppUser>()) { }

    public int SignOutCalls { get; private set; }
    public override Task SignOutAsync() { SignOutCalls++; return Task.CompletedTask; }
}

internal sealed class FakeTokenService : ITokenService
{
    public Func<AppUser, Task<(string, RefreshToken)>> CreateHandler { get; set; } =
        _ => Task.FromResult(("jwt", new RefreshToken { Token = "refresh" }));
    public Func<string, Task<(string, RefreshToken)>> RefreshHandler { get; set; } =
        _ => Task.FromResult(("jwt", new RefreshToken { Token = "refresh" }));
    public Func<string, Task<bool>> RevokeHandler { get; set; } = _ => Task.FromResult(true);
    public Task<(string, RefreshToken)> CreateTokenAsync(AppUser user) => CreateHandler(user);
    public Task<(string, RefreshToken)> RefreshTokenAsync(string refreshToken) => RefreshHandler(refreshToken);
    public Task<bool> RevokeTokenAsync(string refreshToken) => RevokeHandler(refreshToken);
}

internal sealed class FakeOtpService : IOtpService
{
    public string GeneratedOtp { get; set; } = "123456";
    public bool IsValid { get; set; }
    public string GenerateOtp(string email) => GeneratedOtp;
    public bool IsValidOtp(string email, string otp) => IsValid;
}

internal sealed class FakeEmailService : IEmailService
{
    public List<(string To, string Subject, string Body)> Sent { get; } = [];
    public Exception? ExceptionToThrow { get; set; }
    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        if (ExceptionToThrow is not null) throw ExceptionToThrow;
        Sent.Add((to, subject, body));
        return Task.CompletedTask;
    }
}

internal sealed class TestAsyncQueryProvider<TEntity>(IQueryProvider inner) : IAsyncQueryProvider
{
    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);
    public object? Execute(Expression expression) => inner.Execute(expression);
    public TResult Execute<TResult>(Expression expression) => inner.Execute<TResult>(expression);
    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var resultType = typeof(TResult).GetGenericArguments()[0];
        var executeMethod = typeof(IQueryProvider).GetMethods()
            .Single(m => m.Name == nameof(IQueryProvider.Execute) && m.IsGenericMethod)
            .MakeGenericMethod(resultType);
        var result = executeMethod.Invoke(inner, [expression]);
        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(resultType).Invoke(null, [result])!;
    }
}

internal sealed class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
    public TestAsyncEnumerable(Expression expression) : base(expression) { }
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
        new TestAsyncEnumerator<T>(((IEnumerable<T>)this).GetEnumerator());
    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

internal sealed class TestAsyncEnumerator<T>(IEnumerator<T> inner) : IAsyncEnumerator<T>
{
    public T Current => inner.Current;
    public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(inner.MoveNext());
    public ValueTask DisposeAsync() { inner.Dispose(); return ValueTask.CompletedTask; }
}
