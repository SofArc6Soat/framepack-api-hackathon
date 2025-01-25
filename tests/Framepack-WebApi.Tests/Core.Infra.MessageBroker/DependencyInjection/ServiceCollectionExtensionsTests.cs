using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using Core.Infra.MessageBroker.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Framepack_WebApi.Tests.Core.Infra.MessageBroker.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddAwsSqsMessageBroker_ShouldAddAwsOptionsAndSqsService()
        {
            // Arrange
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            services.AddAwsSqsMessageBroker();
            serviceProvider = services.BuildServiceProvider();

            // Assert
            var awsOptions = serviceProvider.GetService<AWSOptions>();
            Assert.NotNull(awsOptions);
            Assert.Equal("default", awsOptions.Profile);
            Assert.Equal(Amazon.RegionEndpoint.USEast1, awsOptions.Region);

            var sqsService = serviceProvider.GetService<IAmazonSQS>();
            Assert.NotNull(sqsService);
        }
    }
}