namespace UpdateFlow.Products;

public class ProductAttribute : Attribute
{
    private string[] types;

    public ProductAttribute(string type)
    {
        this.types = new[] {type};
    }

    public ProductAttribute(params string[] types)
    {
        this.types = types;
    }
    
    public string[] GetProductTypes() => types;
}