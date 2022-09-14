namespace UpdateFlow.Products;

public class ProductAttribute : Attribute
{
    private string type;

    public ProductAttribute(string type)
    {
        this.type = type;
    }

    public string GetProductType() => type;
}