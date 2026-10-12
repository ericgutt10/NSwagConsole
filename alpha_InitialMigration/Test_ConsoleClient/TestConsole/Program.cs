using Common.Client;

internal class Program
{
    private static void Main(string[] args)
    {
        using var client = HttpClientFactory.Create();

        client.BaseAddress = new UriBuilder()
        {
            Scheme = "https",
            // Host = environmentURL!
        }.Uri;

        //get a token
    }
}