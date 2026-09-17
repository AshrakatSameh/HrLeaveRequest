namespace Hr.Infrastructure.ExternalServices.DummyJson;

public class DummyJsonOptions
{
    public const string SectionName = "DummyJson";

    public string BaseUrl { get; set; } = "https://dummyjson.com/";

    public int TimeoutSeconds { get; set; } = 5;

    public int CacheMinutes { get; set; } = 10;
}
