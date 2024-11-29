using FlowEngine.Domain.Flows.Interfaces;

namespace FlowEngine.Domain.Flows.Entities.Abstracts;

public abstract class ServiceBase(string name, Type inputType, Type outputType, bool initNode = false) : IService
{
    private readonly List<IService> _nextServices = [];
    public List<object> Inputs { get; } = [];
    public string Name { get; protected set; } = name;
    public Type InputType { get; protected set; } = inputType;
    public Type OutputType { get; protected set; } = outputType;
    public async Task<object?> ExecuteAsync(object input)
    {
        if (!InputType.IsInstanceOfType(input))
        {
            throw new Exception($"Invalid input type. Expected {InputType}, got {input.GetType()}");
        }

        if (!initNode)
        {
            if (!Inputs.Contains(input))
            {
                throw new Exception($"Input not found in request list.");
            }   
        }

        var output = await RunService(input);
        if (output is null)
        {
            return null;
        }
        
        if (!OutputType.IsInstanceOfType(output))
        {
            throw new Exception($"Invalid output type. Expected {OutputType}, got {output.GetType()}");
        }
        
        foreach (var service in _nextServices)
        {
            service.AddInput(output);
        }

        return output;
    }
    public void AddNextService(IService service)
    {
        if (service.InputType != OutputType)
        {
            throw new Exception($"Input type {InputType} is not the same as output type {OutputType}");
        }
        _nextServices.Add(service);
    }
    public void AddInput(object input)
    {
       Inputs.Add(input);
    }
    protected abstract Task<object?> RunService(object input);
}