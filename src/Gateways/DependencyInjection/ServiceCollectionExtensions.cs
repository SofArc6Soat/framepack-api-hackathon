using Amazon.SQS;
using Core.Infra.MessageBroker;
using Core.Infra.MessageBroker.DependencyInjection;
using Gateways.Dtos.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Gateways.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayDependencyServices(this IServiceCollection services, string connectionString, Queues queues)
        {
            //services.AddScoped<IClienteGateway, ClienteGateway>();
            //services.AddScoped<IProdutoGateway, ProdutoGateway>();
            //services.AddScoped<IFuncionarioGateway, FuncionarioGateway>();

            //services.AddInfraDependencyServices(connectionString);

            // AWS SQS
            services.AddAwsSqsMessageBroker();

            services.AddSingleton<ISqsService<ProdutoCriadoEvent>>(provider => new SqsService<ProdutoCriadoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueProdutoCriadoEvent));
            services.AddSingleton<ISqsService<ProdutoAtualizadoEvent>>(provider => new SqsService<ProdutoAtualizadoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueProdutoAtualizadoEvent));
            services.AddSingleton<ISqsService<ProdutoExcluidoEvent>>(provider => new SqsService<ProdutoExcluidoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueueProdutoExcluidoEvent));
        }
    }

    [ExcludeFromCodeCoverage]
    public record Queues
    {
        public string QueueProdutoCriadoEvent { get; set; } = string.Empty;
        public string QueueProdutoAtualizadoEvent { get; set; } = string.Empty;
        public string QueueProdutoExcluidoEvent { get; set; } = string.Empty;
    }
}