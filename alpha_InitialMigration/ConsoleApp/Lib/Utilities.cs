using ConsoleApp.Models;
using NSwag;
using YamlDotNet.Core;

namespace ConsoleApp.Lib;

public static class Utilities
{
    public static async Task<OpenApiDocument>
        GetOpenApiDocumentAsync(this
            AppSettings? settings)
    {
        try
        {
            var doc = settings switch
            {
                { SwaggerYamlUrl: not null } => await OpenApiYamlDocument.FromUrlAsync(settings.SwaggerYamlUrl),
                { SwaggerYamlFile: not null } => await OpenApiYamlDocument.FromFileAsync(settings.SwaggerYamlFile),
                { SwaggerJsonUrl: not null } => await OpenApiDocument.FromUrlAsync(settings.SwaggerJsonUrl),
                { SwaggerJsonFile: not null } => await OpenApiDocument.FromFileAsync(settings.SwaggerJsonFile),
                _ => throw new ArgumentException("Invalid settings object. All Swagger URL and file options are null.")
            };

            return doc;
        }
        catch(SyntaxErrorException ex)
        {
            throw new InvalidOperationException($"{ex.Message}.{Environment.NewLine}Error start: {ex.Start} - End: {ex.End}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load OpenAPI document.", ex);
        }
    }
}