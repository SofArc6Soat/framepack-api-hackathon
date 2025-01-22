﻿using Amazon.SQS;
using Core.Infra.MessageBroker;
using Core.Infra.MessageBroker.DependencyInjection;
using Core.Infra.S3.DependencyInjection;
using Gateways.Dtos.Events;
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

            // AWS S3
            services.AddAwsS3();

            // AWS SQS
            services.AddAwsSqsMessageBroker();

            services.AddSingleton<ISqsService<ConversaoSolicitada>>(provider => new SqsService<ConversaoSolicitada>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueConversaoSolicitadaEvent));
        }
    }

    [ExcludeFromCodeCoverage]
    public record Queues
    {
        public string QueueConversaoSolicitadaEvent { get; set; } = string.Empty;
    }
}