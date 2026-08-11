namespace GymMangamentSystem.Apis.Helpers;

public sealed class ClientSettings
{
    public const string SectionName = "ClientSettings";

    public string? EmailConfirmationRedirectUrl { get; set; }
    public string[] AllowedOrigins { get; set; } = [];
}
