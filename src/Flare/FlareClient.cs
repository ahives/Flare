namespace Flare;

using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Headers;
using Configuration;
using Exceptions;
using Extensions;

public class FlareClient :
    IFlareClient
{
    readonly ConcurrentDictionary<string, object> _cache;
    readonly HttpClient _client;

    public FlareClient(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _cache = new ConcurrentDictionary<string, object>();

        if (!TryRegisterAll())
            throw new FlareApiInitException("Could not register APIs.");
    }

    public FlareClient(FlareConfig config)
    {
        _client = GetClient(config);
        _cache = new ConcurrentDictionary<string, object>();

        if (!TryRegisterAll())
            throw new FlareApiInitException("Could not register APIs.");
    }

    public FlareClient(IFlareConfigProvider configProvider, string file) : this(configProvider.Configure(file))
    {
    }

    // public FlareClient(Action<FlareConfigurator> configurator) : this(configurator)
    // {
    //     configurator();
    // }

    public T API<T>()
        where T : FlareAPI
    {
        Type type = typeof(T);

        if (type is null || string.IsNullOrWhiteSpace(type.FullName))
            throw new FlareApiInitException($"Failed to find implementation class for interface {typeof(T)}");

        var typeMap = GetTypeMap(typeof(T));

        if (!typeMap.ContainsKey(type.FullName))
            return default!;

        if (_cache.ContainsKey(type.FullName))
            return (T) _cache[type.FullName];
                
        bool registered = RegisterInstance(typeMap[type.FullName], type.FullName, _client);

        if (registered)
            return (T) _cache[type.FullName];

        return default;
    }

    public bool IsRegistered(string key) => _cache.ContainsKey(key);
        
    public IReadOnlyDictionary<string, object> GetObjects() => _cache;

    // public void CancelPendingRequest() => _client.CancelPendingRequests();

    public bool TryRegisterAll()
    {
        var typeMap = GetTypeMap(GetType());
        bool registered = true;

        foreach (var type in typeMap)
        {
            if (_cache.ContainsKey(type.Key))
                continue;
                
            registered = RegisterInstance(type.Value, type.Key, _client) & registered;
        }

        if (!registered)
            _cache.Clear();

        return registered;
    }

    bool RegisterInstance(Type type, string key, HttpClient client)
    {
        try
        {
            var instance = CreateInstance(type, client);

            return instance is not null && _cache.TryAdd(key, instance);
        }
        catch
        {
            return false;
        }
    }

    IDictionary<string, Type> GetTypeMap(Type findType)
    {
        var types = findType.Assembly.GetTypes();
        var interfaces = types
            .Where(x => typeof(FlareAPI).IsAssignableFrom(x) && x.IsInterface && x != typeof(FlareAPI))
            .ToList();
        var typeMap = new Dictionary<string, Type>();

        for (int i = 0; i < interfaces.Count; i++)
        {
            var type = types.Find(x => interfaces[i].IsAssignableFrom(x) && x is {IsInterface: false, IsAbstract: false});

            if (type is null)
                continue;

            string name = interfaces[i].FullName;
            if (string.IsNullOrWhiteSpace(name))
                continue;

            typeMap.Add(name, type);
        }

        return typeMap;
    }

    object CreateInstance(Type type, HttpClient client) =>
        Activator.CreateInstance(type, client);

    HttpClient GetClient(FlareConfig config)
    {
        var uri = new Uri($"{config.Url}/");
        var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential(config.Credentials.Username, config.Credentials.Password)
        };
            
        var client = new HttpClient(handler){BaseAddress = uri};
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (config.Timeout != TimeSpan.Zero)
            client.Timeout = config.Timeout;

        return client;
    }
}