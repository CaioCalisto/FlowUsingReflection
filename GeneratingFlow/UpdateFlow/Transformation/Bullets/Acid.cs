using UpdateFlow.Products;

namespace UpdateFlow.Transformation.Bullets;

[Product("A")]
public class Acid : BulletCreator
{
    public Task<string> ApplyAsync() => Task.FromResult("acid bullet launcher");
}