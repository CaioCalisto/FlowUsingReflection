using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using FluentAssertions;
using UpdateFlow.Outputs.AWSDatabase;
using Xunit;

namespace UpdateFlow.Test;

public class SecurityFlowTests
{
    private const string DynamoDbCollection = "Security";
    private const string LocalStackUrl = "http://localhost:7566";
    private readonly IAmazonDynamoDB _dynamoDb;
    private IDynamoDBContext _context;
    private SecurityFlow flow;
    
    public SecurityFlowTests()
    {
        _dynamoDb = new AmazonDynamoDBClient(new AmazonDynamoDBConfig()
            {ServiceURL = LocalStackUrl});
        _context = new DynamoDBContext(_dynamoDb);
        flow = new SecurityFlow();
    }
    
    [Fact]
    public async void SituationA()
    {
        // Assert
        await DeleteTableAsync();
        await CreateTableAsync();
        var productName = "A";
        var product = new Product(productName);
        var expectedProductSaved = "A acid bullet launcher";
        
        // Act
        await flow.Execute(product);
        
        // Assert
        var allData = await GetAllDataAsync();
        allData.Should().Contain(data => data.Name == expectedProductSaved);
    }
    
    [Fact]
    public async void SituationB()
    {
        // Assert
        await DeleteTableAsync();
        await CreateTableAsync();
        var productName = "B";
        var product = new Product(productName);
        var expectedProductSaved = "B granade bullet launcher - Scope";

        // Act
        await flow.Execute(product);
        
        // Assert
        var allData = await GetAllDataAsync();
        allData.Should().Contain(data => data.Name == expectedProductSaved);
    }

    private async Task<IEnumerable<Security>> GetAllDataAsync()
    {
        var table = _context.GetTargetTable<Security>();
        var scanOps = new ScanOperationConfig();
        scanOps.Limit = 10;
        var results = table.Scan(scanOps);
        List<Document> data = await results.GetNextSetAsync();
        return _context.FromDocuments<Security>(data);
    }

    // Of course it needs to run alone
    private async Task DeleteTableAsync()
    {
        var request = new DeleteTableRequest
        {
            TableName = "Security",
        };

        var response = await _dynamoDb.DeleteTableAsync(request);
    }
    
    private async Task CreateTableAsync()
    {
        var tables = await _dynamoDb.ListTablesAsync();
        if (!tables.TableNames.Contains(DynamoDbCollection))
        {
            var request = new CreateTableRequest()
            {
                TableName = "Security",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        // "S" = string, "N" = number, and so on.
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        // "HASH" = hash key, "RANGE" = range key.
                        KeyType = "HASH"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 10,
                    WriteCapacityUnits = 5
                },
            };

            var response = await _dynamoDb.CreateTableAsync(request);
        }
    }
}