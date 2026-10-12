using CHR_API_Utilities.Models;
using WorkerService1.Models;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = Host.CreateApplicationBuilder(args);
                builder.Services.AddHostedService<Worker>();

                var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
                var chrSettings = builder.Configuration.GetSection("CHRAccessToken").Get<CHRAccessToken>();

                Console.WriteLine($"AppSettings: {chrSettings?.Access_token}");

                builder.Services.AddHttpClient("chr", client =>
                {
                    client.BaseAddress = new UriBuilder
                    { 
                        Scheme = Uri.UriSchemeHttps,
                        Host = appSettings?.CHREnvironmentURL ?? throw new InvalidOperationException("CHREnvironmentURL is not configured.") 
                    }.Uri;
                });

                if (builder.Environment.IsDevelopment())
                {
                    builder.Configuration.AddUserSecrets<Program>();
                    //clientSecret = builder.Configuration[clientSecretKey!];
                    //connStrBuilder.UserID = builder.Configuration["KineticUser:un"];
                    //connStrBuilder.Password = builder.Configuration["KineticUser:pw"];
                }




                var host = builder.Build();
                host.Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
