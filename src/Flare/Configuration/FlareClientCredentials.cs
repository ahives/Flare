namespace Flare.Configuration;

public record FlareClientCredentials
{
    public string Username { get; init; }
        
    public string Password { get; init; }
}