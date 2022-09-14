using UpdateFlow.Products;

namespace UpdateFlow.Transformation.Specials;

[Product("B")]
public class Scope : SpecialCreator
{
    public Task<string> ApplyAsync() => Task.FromResult("Scope");
}