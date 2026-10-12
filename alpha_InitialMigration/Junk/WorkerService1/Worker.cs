using CHR_API_Utilities;
using Common.Client;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;

using static CHR_API_Utilities.ApiUtilsExtensions;

namespace WorkerService1
{
    public class Worker(
        IConfiguration configuration,
        IHostEnvironment env,
        IHttpClientFactory HttpClientFactory,
        ILogger<Worker> logger
        ) : BackgroundService
    {
        private const string CHR_TOKEN_ROUTE = "v1/oauth/token";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var client = HttpClientFactory.CreateClient("chr");

            TokenSuccessResponse? tokenResponse = configuration.GetAccessToken();

            if (tokenResponse.TokenIsExpired())
            {
                var tokReq = configuration.GetTokenRequest(env);

                try
                {
                    var chrClient = new SwaggerClient(client);

                    tokenResponse = await chrClient.Generate_TokenAsync(
                        new TokenRequest
                        {
                            Client_id = tokReq?.client_id ?? "",
                            Client_secret = tokReq?.client_secret ?? ""
                        }
                        , stoppingToken);

                    tokenResponse!.FetchTime = System.DateTime.UtcNow.GetTimeInSeconds();
                    tokenResponse.SaveConfig();
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }

            stoppingToken = new CancellationToken(true);
            return;
        }
    }
}