namespace Basket.Infrastructure.Configuration;

public class CacheSettings
{
    public const string SectionName = "CacheSettings";

    public string ConnectionString { get; set; } = string.Empty;
}
