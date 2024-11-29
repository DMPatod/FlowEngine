namespace FlowEngine.Domain.Flows.Interfaces;

public interface IService
{
    string Name { get; }
    Type InputType { get; }
    Type OutputType { get; }
    List<object> Inputs { get; }
    Task<object?> ExecuteAsync(object input);
    void AddNextService(IService service);
    void AddInput(object input);
}