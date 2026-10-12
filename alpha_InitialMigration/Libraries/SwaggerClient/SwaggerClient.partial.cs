using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Client;

public partial class SwaggerClient
{
    public static void UpdateJsonSerializerSettings(ref JsonSerializerOptions settings)
    {
        settings = JsonOpt;
    }

    public static JsonSerializerOptions JsonOpt => new(JsonSerializerDefaults.Web)
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        PropertyNameCaseInsensitive = true,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        WriteIndented = true,
        RespectNullableAnnotations = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
}

public partial class TokenSuccessResponse
{
    public int FetchTime;

    public static TokenSuccessResponse Create(string? accessToken, int fetchTime = 0)
    {
        return new TokenSuccessResponse() //("Bearer", 86400, accessToken, "read:status")
        {
            Access_token = accessToken ?? "",
            Expires_in = 86400,
            Token_type = "Bearer",
            Scope = "read:status",
            FetchTime = fetchTime
        };
    }
}

public partial class DateTime
{
    public Object? Value { get; set; }
}

public static class DateTimeExtensions
{
    public static string? ToString(this
        Object? Value,
        string? format,
        System.IFormatProvider? formatProvider)
    {
        if (Value is System.DateTime dt)
        {
            return dt.ToString(format, formatProvider);
        }
        else
        {
            return Value?.ToString();
        }
    }
}

public partial class ApiException : System.Exception
{
    public int StatusCode { get; private set; }

    public string? Response { get; private set; }

    public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

    public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
        : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
    {
        StatusCode = statusCode;
        Response = response;
        Headers = headers;
    }

    public override string ToString()
    {
        return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
    }
}

[System.CodeDom.Compiler.GeneratedCode("NSwag", "14.7.1.0 (NJsonSchema v11.6.1.0 (Newtonsoft.Json v13.0.0.0))")]
public partial class ApiException<TResult> : ApiException
{
    public TResult Result { get; private set; }

    public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
        : base(message, statusCode, response, headers, innerException)
    {
        Result = result;
    }
}