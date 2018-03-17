# Kodiak.Azure.WebHostExtensions

This extension simplifies how to use the Azure Key Vault in a .NET Core web application at startup time.

This is available as a NuGet package called [Kodiak.Azure.WebHostExtension](https://www.nuget.org/packages/Kodiak.Azure.WebHostExtension/0.0.1-alpha). 

Your Program.cs file could look like the one below.

```cs
using Kodiak.Azure.WebHostExtension;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MyDotNetApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .AddAzureKeyVaultSecretsToConfiguration("https://myvaultname.vault.azure.net")
                .UseStartup<Startup>()
                .Build();
    }
}
```

Now you can access secrets in your code as shown in the code below - make sure that IConfiguration is injected into your class's constructor.

```cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MyDotNetApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            ViewBag.Secret = _configuration["MySecretName"];

            return View();
        }
    }
}
```
