using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Google.Protobuf.WellKnownTypes;

namespace v4net6funcconfigstart563
{
    public class Function1
    {
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        public Function1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _configuration = configuration;
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            // Read configuration data
            string keyName = "TestApp:Settings:Message";
            string message = _configuration[keyName];

            string keyName2 = "TestApp:Settings:Message2";
            string message2 = _configuration[keyName2];

            string sentinelKey = "Sentinel";
            string sentinelMessage = _configuration[keyName2];



            response.WriteString(message + "__" ?? $"Please create a key-value with the key '{keyName}' in Azure App Configuration.____{System.Environment.NewLine}");
            response.WriteString(message2 + "__" ?? $"Please create a key-value with the key '{keyName2}' in Azure App Configuration.____{System.Environment.NewLine}");
            response.WriteString(sentinelKey + "__" ?? $"Please create a key-value with the key '{sentinelMessage}' in Azure App Configuration.____{System.Environment.NewLine}");

            return response;
        }
    }
}
