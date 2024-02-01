using System.Text.Json;
using RDSService.Services;
using Xunit.Abstractions;

namespace RDSServiceTester;

public class RdsServiceTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public RdsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetActiveManagementServer_ReturnsResult()
    {
        var service = new RdsSessionService();
        var result = await service.GetActiveManagementServer();
        _testOutputHelper.WriteLine(result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetSessions_ReturnsResults()
    {
        var service = new RdsSessionService();
        var result = await service.GetSessions();
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(result));
        Assert.NotNull(result);
    }
}