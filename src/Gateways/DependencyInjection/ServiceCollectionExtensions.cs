using Infra.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Gateways.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayDependencyServices(this IServiceCollection services, string dynamoDbServiceUrl, string dynamoDbAccessKey, string dynamoDbSecretKey, Queues queues)
        {
            services.AddScoped<IConversaoGateway, ConversaoGateway>();

            services.AddInfraDependencyServices(dynamoDbServiceUrl, dynamoDbAccessKey, dynamoDbSecretKey);

            // AWS SQS
            //services.AddAwsSqsMessageBroker();

            //services.AddSingleton<ISqsService<ConversaoCriada>>(provider => new SqsService<ConversaoCriada>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueConversaoCriadaEvent));
        }
    }

    [ExcludeFromCodeCoverage]
    public record Queues
    {
        public string QueueConversaoCriadaEvent { get; set; } = string.Empty;
    }
}