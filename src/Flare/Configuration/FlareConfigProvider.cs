namespace Flare.Configuration;

public class FlareConfigProvider :
    IFlareConfigProvider
{
    public FlareConfig Configure(Action<FlareConfigurator> configurator)
    {
        // if (configurator is null)
        //     return ConfigCache.Default;
            
        var impl = new FlareConfigProviderImpl();
        configurator?.Invoke(impl);

        FlareConfig config = impl.Settings;

        // return Validate(config) ? config : ConfigCache.Default;
        return config;
    }

    public FlareConfig Configure(string file)
    {
        return null;
    }

    
    class FlareConfigProviderImpl :
        FlareConfigurator
    {
        public FlareConfig Settings { get; private set; }

        public void Client(Action<ClientConfigurator> configurator)
        {
            // if (configurator is null)
            //     _config = ConfigCache.Default.Broker;

            var impl = new ClientConfiguratorImpl();
            configurator?.Invoke(impl);

            Settings = impl.Settings.Value;
        }

        
        class ClientConfiguratorImpl :
            ClientConfigurator
        {
            string _url;
            TimeSpan _timeout;
            string _username;
            string _password;
            string _apiKey;
            private ApiVersion _version;

            public Lazy<FlareConfig> Settings { get; }

            public ClientConfiguratorImpl()
            {
                Settings = new Lazy<FlareConfig>(
                    () => new ()
                    {
                        Url = _url,
                        ApiVersion = _version,
                        Timeout = _timeout,
                        ApiKey = _apiKey,
                        Credentials = !string.IsNullOrWhiteSpace(_username) && !string.IsNullOrWhiteSpace(_password)
                            ? new() {Username = _username, Password = _password}
                            : default
                    }, LazyThreadSafetyMode.PublicationOnly);
            }

            public void ConnectTo(string url) => _url = url;

            public void TimeoutAfter(TimeSpan timeout) => _timeout = timeout;

            public void UsingCredentials(string username, string password)
            {
                _username = username;
                _password = password;
            }

            public void UsingApiKey(string apiKey) => _apiKey = apiKey;

            public void UsingApiVersion(ApiVersion version) => _version = version;
        }
    }
}