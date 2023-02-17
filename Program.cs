using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddAzureAppConfiguration(options =>
                {
                    options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
                            // Load all keys that start with `TestApp:` and have no label
                            .Select("*")
                            // Configure to reload configuration if the registered sentinel key is modified
                            .ConfigureRefresh(refreshOptions =>
                                refreshOptions.Register("Sentinel", refreshAll: true));
                });
            })
            .ConfigureServices(services =>
            {
                // Make Azure App Configuration services available through dependency injection.
                services.AddAzureAppConfiguration();
            })
            .ConfigureFunctionsWorkerDefaults(app =>
            {
                // Use Azure App Configuration middleware for data refresh.
                app.UseAzureAppConfiguration();
            })
            .Build();

        host.Run();
    }
}