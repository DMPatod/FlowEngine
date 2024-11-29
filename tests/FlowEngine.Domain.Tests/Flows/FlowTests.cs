using FlowEngine.Domain.Flows;
using FlowEngine.Domain.Flows.Interfaces;
using FlowEngine.Domain.Services;

namespace FlowEngine.Domain.Tests.Flows;

public class FlowTests
{
    [Fact]
    public void Test1()
    {
        var serviceA = new FetchDataService("FetchDataService");
        var serviceB = new ApproveDataService("ApproveDataService");
        var serviceC = new PackageService("PackageService");
        
        var flow = new Flow("FlowA");
    }
}