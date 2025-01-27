using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Infra.Configurations;
using Moq;

namespace Framepack_WebApi.Tests.External.Infra.Configurations;

public class DynamoDbConfigTests
{
    [Fact]
    public void ConfigDynamoDb_ValidParameters_ShouldReturnClient()
    {
        // Arrange
        var serviceUrl = "http://localhost:8000";
        var accessKey = "accessKey";
        var secretKey = "secretKey";

        // Act
        var client = typeof(DynamoDbConfig)
            .GetMethod("ConfigDynamoDb", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .Invoke(null, new object[] { serviceUrl, accessKey, secretKey }) as AmazonDynamoDBClient;

        // Assert
        Assert.NotNull(client);
    }

    [Fact]
    public async Task CreateTableIfNotExists_TableAlreadyExists_ShouldNotThrowException()
    {
        // Arrange
        var mockClient = new Mock<IAmazonDynamoDB>();
        mockClient
            .Setup(x => x.DescribeTableAsync("Conversoes", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DescribeTableResponse
            {
                Table = new TableDescription { TableStatus = "ACTIVE" }
            });

        // Act & Assert
        await DynamoDbConfig.CreateTableIfNotExists(mockClient.Object);
        mockClient.Verify(x => x.DescribeTableAsync("Conversoes", It.IsAny<CancellationToken>()), Times.Once);
        mockClient.Verify(x => x.CreateTableAsync(It.IsAny<CreateTableRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateTableIfNotExists_TableDoesNotExist_ShouldCreateTable()
    {
        // Arrange
        var mockClient = new Mock<IAmazonDynamoDB>();

        // Simula o lançamento de ResourceNotFoundException ao chamar DescribeTableAsync
        mockClient
            .SetupSequence(x => x.DescribeTableAsync("Conversoes", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ResourceNotFoundException("Table not found")) // Primeiro lançamento da exceção
            .ReturnsAsync(new DescribeTableResponse // Depois, retorna a tabela criada
            {
                Table = new TableDescription { TableStatus = "ACTIVE" }
            });

        mockClient
            .Setup(x => x.CreateTableAsync(It.IsAny<CreateTableRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateTableResponse()); // Mock para criar tabela

        // Act
        await DynamoDbConfig.CreateTableIfNotExists(mockClient.Object);

        // Assert
        mockClient.Verify(x => x.DescribeTableAsync("Conversoes", It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        mockClient.Verify(x => x.CreateTableAsync(It.IsAny<CreateTableRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task CreateTableIfNotExists_TableStatusCreating_ShouldWaitUntilActive()
    {
        // Arrange
        var mockClient = new Mock<IAmazonDynamoDB>();
        var describeTableResponseQueue = new Queue<DescribeTableResponse>();

        // Simula o lançamento de ResourceNotFoundException ao chamar DescribeTableAsync
        mockClient
            .SetupSequence(x => x.DescribeTableAsync("Conversoes", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ResourceNotFoundException("Table not found")) // Tabela não encontrada
            .ReturnsAsync(new DescribeTableResponse // Estado CREATING
            {
                Table = new TableDescription { TableStatus = "CREATING" }
            })
            .ReturnsAsync(new DescribeTableResponse // Estado ACTIVE
            {
                Table = new TableDescription { TableStatus = "ACTIVE" }
            });

        mockClient
            .Setup(x => x.CreateTableAsync(It.IsAny<CreateTableRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateTableResponse()); // Mock para criação da tabela

        // Act
        await DynamoDbConfig.CreateTableIfNotExists(mockClient.Object);

        // Assert
        mockClient.Verify(x => x.CreateTableAsync(It.IsAny<CreateTableRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        mockClient.Verify(x => x.DescribeTableAsync("Conversoes", It.IsAny<CancellationToken>()), Times.AtLeast(2));
    }

}