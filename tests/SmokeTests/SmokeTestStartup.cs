using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api;
using Controllers;
using Infra.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace SmokeTests;

public class SmokeTestStartup : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureServices(
        services =>
        {
            // Remove o cliente e contexto DynamoDB reais
            var dynamoDbClientDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAmazonDynamoDB));
            if (dynamoDbClientDescriptor != null)
            {
                services.Remove(dynamoDbClientDescriptor);
            }

            var dynamoDbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDynamoDBContext));
            if (dynamoDbContextDescriptor != null)
            {
                services.Remove(dynamoDbContextDescriptor);
            }

            // Adiciona o DynamoDB in-memory para testes
            DynamoDbConfig.Configure(services, "http://localhost:8000", "fakeAccessKey", "fakeSecretKey");

            services.AddScoped<IUsuarioController, UsuarioController>();
            services.AddScoped<IConversaoController, ConversaoController>();
        });
}