using ConsoleApp.Lib;
using ConsoleApp.Models;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        var document = await appSettings.GetOpenApiDocumentAsync();
        var fileInfo = await document.GenerateCSharpClient(appSettings?.ClassName, appSettings?.Namespace);

    }
}