using System.Text.Json;
using RDSServiceClient;
using Xunit.Abstractions;

namespace RDSServiceTester;

public class RdsServiceClientTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly RdsSessionService _rdsSessionService =
        new(new HttpClient { BaseAddress = new Uri("http://rdscb01.royal.corp:5555") });

    public RdsServiceClientTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetSessions_ReturnsResults()
    {
        var result = await _rdsSessionService.GetSessions("rdscb01.royal.corp");
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(result));
        Assert.NotNull(result);
    }
}