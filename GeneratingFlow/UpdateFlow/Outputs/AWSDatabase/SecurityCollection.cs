using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using UpdateFlow.Products;

namespace UpdateFlow.Outputs.AWSDatabase;

[Product("A", "B")]
public class SecurityCollection : IOutput
{
    private readonly IDynamoDBContext _context;
    
    public SecurityCollection()
    {
        var dynamoDb = new AmazonDynamoDBClient(new AmazonDynamoDBConfig()
            {ServiceURL = "http://localhost:7566"});
        _context = new DynamoDBContext(dynamoDb);
    }
    
    public async Task Save(string name)
    {
        var batch = _context.CreateBatchWrite<Security>();
        batch.AddPutItem(new Security()
        {
            Id = Guid.NewGuid().ToString(),
            Name = name
        });

        await batch.ExecuteAsync();
    }
}