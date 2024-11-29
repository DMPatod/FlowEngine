using System.Text;
using FlowEngine.Domain.Flows.Entities.Abstracts;
using Newtonsoft.Json;

namespace FlowEngine.Domain.Services;

public record Request(string InitialData, string FetchedData);

public class ApproveDataService(string name) : ServiceBase(name, typeof(IList<Request>), typeof(Printed))
{
    private async Task<bool> SaveFile(Request request)
    {
        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"{Name}_{request.InitialData}.json");
            await using var stream = File.OpenWrite(path);
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
            await stream.WriteAsync(bytes);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected override async Task<object?> RunService(object input)
    {
        if (input is not List<Request> parsedInput)
        {
            throw new InvalidDataException("Input is not List<Request>");
        }

        var results = await Task.WhenAll(parsedInput.Select(
            async r => (r, await SaveFile(r))
        ));

        return results.Where(t => t.Item2).Select(t => t.r);
    }
}