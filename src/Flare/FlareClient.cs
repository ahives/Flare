using System.Collections.Concurrent;
using System.Runtime.Serialization;
using Flare.Configuration;
using Flare.Extensions;

namespace Flare;

public class FlareClient
{
    private readonly FlareConfig _config;
    readonly ConcurrentDictionary<string, object> _cache;

    public FlareClient(FlareConfig config)
    {
        _config = config;
        _cache = new ConcurrentDictionary<string, object>();
            
        if (!TryRegisterAll())
            throw new FlareApiInitException("Could not register APIs.");
    }

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
                
        bool registered = RegisterInstance(typeMap[type.FullName], type.FullName, _config);

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
                
            registered = RegisterInstance(type.Value, type.Key, _config) & registered;
        }

        if (!registered)
            _cache.Clear();

        return registered;
    }

    bool RegisterInstance(Type type, string key, FlareConfig config)
    {
        try
        {
            var instance = CreateInstance(type, config);

            return instance is not null && _cache.TryAdd(key, instance);
        }
        catch
        {
            return false;
        }
    }

    bool RegisterInstance(Type type, string key)
    {
        try
        {
            var instance = CreateInstance(type);

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

    object? CreateInstance(Type type) =>
        type.IsDerivedFrom(typeof(FlareHttpClient))
            ? Activator.CreateInstance(type, _config)
            : Activator.CreateInstance(type);

    object CreateInstance(Type type, FlareConfig config) => Activator.CreateInstance(type, config);
}

public class FlareApiInitException : Exception
{
    public FlareApiInitException()
    {
    }

    protected FlareApiInitException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public FlareApiInitException(string? message) : base(message)
    {
    }

    public FlareApiInitException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}