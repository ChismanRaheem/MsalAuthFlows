using Microsoft.Identity.Client;
using mysampleApp;
using Newtonsoft.Json.Linq;

internal class Program
{
    private static async System.Threading.Tasks.Task Runasync()
    {

        AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.json");

        IPublicClientApplication app;

        app = PublicClientApplicationBuilder.Create(config.ClientId)

        //.WithClientSecret(config.ClientSecret)
        .WithAuthority(new Uri(config.Authority))
        .WithDefaultRedirectUri()
        .Build();


        //acquire token
        string[] scopes = new string[] { $"{config.ApiUrl}.default" };

        AuthenticationResult? result = null;
        IAccount firstAccounts;
        var accounts = await app.GetAccountsAsync();
        firstAccounts = accounts.FirstOrDefault();

        try

        {

            result = await app.AcquireTokenSilent(scopes, firstAccounts).ExecuteAsync();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Token acquired");

            Console.ResetColor();

        }

        catch (MsalUiRequiredException ex)
        {
            result = app.AcquireTokenInteractive(scopes)
               .WithAccount(firstAccounts)
               .WithPrompt(Prompt.Consent)
               .ExecuteAsync().Result;


            // Invalid scope. The scope has to be of the form "https://resourceurl/.default"

            // Mitigation: change the scope to be as expected

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Scope provided is not supported");

            Console.ResetColor();

        }

        //Graph api call read all users.
        if (result != null)

        {

            var httpClient = new HttpClient();

            var apiCaller = new ProtectedApiCallHelper(httpClient);

            apiCaller.CallWebApiAndProcessResultASync($"{config.ApiUrl}v1.0/users", result.AccessToken, Display).Wait();

        }
    }
    private static void Main(string[] args)
    {
        Runasync().GetAwaiter().GetResult();
    }

    private static void Display(JObject result)
    {

        foreach (JProperty child in result.Properties().Where(p => !p.Name.StartsWith("@")))

        {

            Console.WriteLine($"{child.Name} = {child.Value}");

        }

    }
}