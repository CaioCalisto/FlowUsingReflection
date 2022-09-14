namespace UpdateFlow.Transformation.Specials;

public interface SpecialCreator
{
    Task<string> ApplyAsync();
}