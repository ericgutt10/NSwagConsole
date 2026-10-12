//using Newtonsoft.Json.Converters;
//using Newtonsoft.Json;
using CHR_API_Utilities.Models;
using Common.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CHR_API_Utilities;

public static class ApiUtilsExtensions
{
    public const string APPSETTINGS_FILE = "appsettings.json";

    private static JsonSerializerOptions _jsonOpt = default!;

    public static JsonSerializerOptions JsonOpt => _jsonOpt ??= new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        WriteIndented = true,
    };

    public static int GetTimeInSeconds(this System.DateTime now)
    {
        TimeSpan ts = now - new System.DateTime(1970, 1, 1);
        int totalSeconds = (int)ts.TotalSeconds;
        return totalSeconds;
    }

    public static TokenSuccessResponse GetAccessToken(this IConfiguration configuration)
    {
        var chrSettings = configuration.GetSection("CHRAccessToken").Get<CHRAccessToken>();

        TokenSuccessResponse? tokenResponse = TokenSuccessResponse.Create(
            chrSettings?.Access_token,
            chrSettings?.FetchTime ?? 0
        );

        return tokenResponse;
    }

    public static CHRBearerTokenRequest? GetTokenRequest(
        this IConfiguration configuration, IHostEnvironment env
    )
    {
        IRatingAPIBase? clientIdSecrets = env.IsDevelopment() ?
            configuration.GetSection("RatingAPI_TEST").Get<RatingAPI_TEST>() :
            configuration.GetSection("RatingAPI_PROD").Get<RatingAPI_PROD>();

        var tokReq = new CHRBearerTokenRequest(
                    clientIdSecrets?.GlobalClientId,
                    clientIdSecrets?.GlobalClientSecret);

        return tokReq;
    }

    public static bool TokenIsExpired(this TokenSuccessResponse? tokenResponse)
    {
        if (tokenResponse is null) { return true; }

        var tokenExpired =
            (tokenResponse.Expires_in + tokenResponse.FetchTime) <= System.DateTime.UtcNow.GetTimeInSeconds();

        return tokenExpired;
    }

    public static bool SaveConfig(this
        TokenSuccessResponse? tokenResponse)
    {
        try
        {
            var configFile = APPSETTINGS_FILE.ConfigFileInfo();
            var configObj = configFile.GetConfigObjectFromFile();

            if (configObj is null)
            {
                Debug.WriteLine("Failed to deserialize configuration file.");
                return false;
            }
            configObj["CHRAccessToken"] = new JsonObject
            {
                ["access_token"] = tokenResponse!.Access_token,
                ["fetchTime"] = System.DateTime.UtcNow.GetTimeInSeconds()
            };
            var updatedConfigJson = JsonSerializer.Serialize(configObj, JsonOpt);
            File.WriteAllText(configFile.FullName, updatedConfigJson);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }

        return true;
    }

    public static FileInfo ConfigFileInfo(this
    string? configFileName
    //   ILogger? logger
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configFileName);
        var configFile = new FileInfo(Path.Combine(
            AppContext.BaseDirectory,
            configFileName));
        if (!configFile.Exists)
        {
            // logger?.Warning("Configuration file not found: {f}", configFile);
            throw new FileNotFoundException(configFile.FullName);
        }
        return configFile;
    }

    public static JsonObject GetConfigObjectFromFile(this
        FileInfo configFile
        //ILogger? logger
        )
    {
        try
        {
            var configJson = File.ReadAllText(configFile.FullName);
            var configObj = JsonSerializer.Deserialize<JsonObject>(configJson, JsonOpt);
            if (configObj is null)
            {
                var msg = $"Failed to deserialize configuration file: {configFile}";
                //logger?.Warning(msg);
                throw new JsonException(msg);
            }

            return configObj;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            //logger?.Warning(ex, ex.Message);
            throw;
        }
    }
}