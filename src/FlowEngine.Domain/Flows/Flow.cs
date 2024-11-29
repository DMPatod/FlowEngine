using FlowEngine.Domain.Flows.Interfaces;

namespace FlowEngine.Domain.Flows;

public class Flow(string name) : IFlow
{
    public string Name { get; } = name;
}