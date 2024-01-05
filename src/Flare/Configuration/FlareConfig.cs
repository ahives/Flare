namespace Flare.Configuration;

public record FlareConfig
{
    public string Url { get; init; }

    public TimeSpan Timeout { get; init; }

    public string ApiKey { get; init; }

    public ApiVersion ApiVersion { get; init; }

    public FlareClientCredentials Credentials { get; init; }
}