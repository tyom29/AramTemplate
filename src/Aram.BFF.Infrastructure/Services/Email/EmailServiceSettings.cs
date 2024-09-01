namespace Aram.BFF.Infrastructure.Services.Email;

public class EmailServiceSettings
{
    public const string Section = "EmailServiceSettings";

    public string ClientId { get; set; } = null!;
    public string TenantId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}