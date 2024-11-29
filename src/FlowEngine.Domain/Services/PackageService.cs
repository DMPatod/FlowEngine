using FlowEngine.Domain.Flows.Entities.Abstracts;
using FlowEngine.Domain.Flows.Interfaces;

namespace FlowEngine.Domain.Services;

public record Printed();

public class PackageService(string name) : ServiceBase(name, typeof(Printed), typeof(int))
{
    protected override async Task<object?> RunService(object input)
    {
        return await Task.FromResult(1);
    }
}