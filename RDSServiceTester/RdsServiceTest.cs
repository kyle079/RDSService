using System.Text.Json;
using RDSService.Services;
using RDSServiceLibrary.Models;
using Xunit.Abstractions;

namespace RDSServiceTester;

public class RdsServiceTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly RdsSessionService _rdsSessionService = new();

    public RdsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetActiveManagementServer_ReturnsResult()
    {
        var result = await _rdsSessionService.GetActiveManagementServer("rdscb01.royal.corp");
        _testOutputHelper.WriteLine(result);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetSessions_ReturnsResults()
    {
        var result = await _rdsSessionService.GetSessions("rdscb01.royal.corp");
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(result));
        Assert.NotNull(result);
    }
}