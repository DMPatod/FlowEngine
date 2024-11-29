using FlowEngine.Domain.Flows.Entities.Abstracts;
using Newtonsoft.Json;

namespace FlowEngine.Domain.Services;

public record Doc(List<string> Data);

public class FetchDataService(string name) : ServiceBase(name, typeof(Doc), typeof(IList<Request>), true)
{
    private async Task<string> FetchData()
    {
        using var client = new HttpClient();
        var response =
            await client.GetAsync(new Uri("https://www.randomnumberapi.com/api/v1.0/randomstring?min=5&max=20"));
        response.EnsureSuccessStatusCode();
        var contentStr = await response.Content.ReadAsStringAsync();
        var content = JsonConvert.DeserializeObject<List<string>>(contentStr)!.FirstOrDefault();
        return content ?? throw new ApplicationException("Could not fetch random string");
    }

    protected override async Task<object?> RunService(object input)
    {
        if (input is not Doc parsedInput)
        {
            throw new ArgumentException($"Input must be of type {nameof(Doc)}");
        }

        var data = await Task.WhenAll(parsedInput.Data.Select(
            async s => new Request(s, await FetchData())
        ));

        return data;
    }
}