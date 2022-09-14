
namespace UpdateFlow.Transformation.Bullets;

public interface BulletCreator
{
    Task<string> ApplyAsync();
}