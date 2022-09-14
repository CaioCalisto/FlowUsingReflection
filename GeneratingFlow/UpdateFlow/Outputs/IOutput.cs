namespace UpdateFlow.Outputs;

public interface IOutput
{
    Task Save(string name);
}