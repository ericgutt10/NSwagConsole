using System.Text.Json.Serialization;

namespace CHR_API_Utilities.Models;

public interface IRatingAPIBase
{
    public string? GlobalClientId { get; set; }
    public string? GlobalClientSecret { get; set; }
}

public record RatingAPI_TEST : IRatingAPIBase
{
    public string? GlobalClientId { get; set; }
    public string? GlobalClientSecret { get; set; }
};

public record RatingAPI_PROD : IRatingAPIBase
{
    public string? GlobalClientId { get; set; }
    public string? GlobalClientSecret { get; set; }
};