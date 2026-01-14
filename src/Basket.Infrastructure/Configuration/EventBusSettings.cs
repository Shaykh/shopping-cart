namespace Basket.Infrastructure.Configuration;

public class EventBusSettings
{
    public const string SectionName = "EventBusSettings";

    public string HostAddress { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
