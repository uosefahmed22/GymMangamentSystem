namespace GymMangamentSystem.Core.IServices.Auth;

public interface IEmailService
{
    Task SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken = default);
}
