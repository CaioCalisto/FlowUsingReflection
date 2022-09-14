using UpdateFlow.Products;

namespace UpdateFlow.Transformation.Bullets;

[Product("B")]
public class Granade : BulletCreator
{
    public Task<string> ApplyAsync() => Task.FromResult("granade bullet launcher");
}