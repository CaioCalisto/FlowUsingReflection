using Amazon.DynamoDBv2.DataModel;

namespace UpdateFlow.Outputs.AWSDatabase;

[DynamoDBTable("Security")]
public class Security
{
    [DynamoDBHashKey]
    public string Id { get; set; }

    public string Name { get; set; }
}