using System.Text.Json.Serialization;

namespace CHR_API_Utilities.Models;

//[JsonDerivedType(typeof(RatingAPI_TEST),nameof(RatingAPI_TEST))]
//[JsonDerivedType(typeof(RatingAPI_PROD),nameof(RatingAPI_PROD))]
//[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
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