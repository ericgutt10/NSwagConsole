namespace CHR_API_Utilities.Models;

public record class CHRBearerTokenRequest(
        string? client_id, string? client_secret
    )
{
    public string? audience { get; set; } = "https://inavisphere.chrobinson.com";
    public string? grant_type { get; set; } = "client_credentials";
};