using Api.Configuration;
using Controllers.DependencyInjection;
using Core.WebApi.Configurations;
using Core.WebApi.DependencyInjection;
using Gateways.Cognito.DependencyInjection;
using Gateways.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics.CodeAnalysis;

namespace Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 500 * 1024 * 1024; // 500 MB
            });

            var settings = EnvironmentConfig.ConfigureEnvironment(services, _configuration);

            var jwtBearerConfigureOptions = new JwtBearerConfigureOptions
            {
                Authority = settings.CognitoSettings.Authority,
                MetadataAddress = settings.CognitoSettings.MetadataAddress
            };

            services.AddApiDefautConfig(jwtBearerConfigureOptions);

            services.AddHealthCheckConfig();

            services.AddControllerDependencyServices();

            var sqsQueues = new Queues
            {
                QueueConversaoSolicitadaEvent = settings.AwsSqsSettings.QueueConversaoSolicitadaEvent
            };

            services.AddGatewayDependencyServices(settings.AwsDynamoDbSettings.ServiceUrl, settings.AwsDynamoDbSettings.AccessKey, settings.AwsDynamoDbSettings.SecretKey, sqsQueues);

            services.AddGatewayCognitoDependencyServices(settings.CognitoSettings.ClientId, settings.CognitoSettings.ClientSecret, settings.CognitoSettings.UserPoolId);
        }

        public static void Configure(IApplicationBuilder app) =>
            app.UseApiDefautConfig();
    }
}