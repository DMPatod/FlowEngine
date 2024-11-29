using FlowEngine.Domain.Services;

namespace FlowEngine.Domain.Tests.Flows.Entities;

public class ServiceTests
{
    [Fact]
    public async Task Test_Service()
    {
        var serviceA = new FetchDataService("FetchDataService");
        var serviceB = new ApproveDataService("ApproveDataService");
        var serviceC = new PackageService("PackageService");

        serviceA.AddNextService(serviceB);
        serviceB.AddNextService(serviceC);

        var outputA = await serviceA.ExecuteAsync(new Doc(["A", "B", "C", "D", "E"]));
        Assert.NotNull(outputA);
        Assert.IsAssignableFrom<IList<Request>>(outputA);
        Assert.Equal(5, ((List<Request>)outputA).Count);

        var outputB = await serviceB.ExecuteAsync(outputA);
        Assert.NotNull(outputB);

        var result = await serviceC.ExecuteAsync(outputB);
        Assert.NotNull(result);
    }

    [Fact]
    public void Service_Must_Not_Allow_Connect_With_Services_With_Different_Inputs()
    {
        var serviceA = new FetchDataService("FetchDataService");
        var serviceC = new PackageService("PackageService");

        Assert.Throws<Exception>(() => serviceA.AddNextService(serviceC));
    }
}