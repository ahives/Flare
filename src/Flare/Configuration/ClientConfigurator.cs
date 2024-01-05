namespace Flare.Configuration;

public interface ClientConfigurator
{
    /// <summary>
    /// Specify the Ops Genie url to connect to.
    /// </summary>
    /// <param name="url"></param>
    void ConnectTo(string url);

    /// <summary>
    /// Specify the maximum time before the HTTP request to the RAbbitMQ server will fail.
    /// </summary>
    /// <param name="timeout"></param>
    void TimeoutAfter(TimeSpan timeout);
        
    /// <summary>
    /// Specify the user credentials to connect to the RabbitMQ server.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    void UsingCredentials(string username, string password);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="apiKey"></param>
    void UsingApiKey(string apiKey);

    void UsingApiVersion(ApiVersion version);
}

public enum ApiVersion
{
    V2
}