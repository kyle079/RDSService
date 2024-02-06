#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace RDSServiceClient;

public class RdsServiceClientOptions
{
    public RdsServiceClientOptions() { }
    public RdsServiceClientOptions(string baseUrl)
    {
        BaseUrl = baseUrl;
    }

    public string BaseUrl { get; set; }
}