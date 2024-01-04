namespace Flare.Extensions;

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public static class TypeExtensions
{
    public static bool IsDerivedFrom(this Type type, Type fromType)
    {
        while (type is not null && type != typeof(object))
        {
            Type currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

            if (fromType == currentType)
                return true;

            type = type.BaseType;
        }

        return false;
    }

    public static Type Find(this Type[] types, Predicate<Type> predicate) => Array.Find(types, predicate);

    public static bool InheritsFromInterface(this Type type, Type findType)
    {
        return findType.IsGenericType
            ? type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == findType)
            : type.GetInterfaces().Any(x => x == findType);
    }

    public static Dictionary<string, Type> GetTypeMap(this List<Type> types, Func<Type, string> function)
    {
        Span<Type> memory = CollectionsMarshal.AsSpan(types.ToList());
        ref var ptr = ref MemoryMarshal.GetReference(memory);

        var map = new Dictionary<string, Type>();

        for (int i = 0; i < memory.Length; i++)
        {
            var slice = Unsafe.Add(ref ptr, i);
            string key = function(slice);

            if (key is null || map.ContainsKey(key))
                continue;

            map.Add(key, slice);
        }

        return map;
    }

    public static bool TryRegisterAll<T>(
        this IDictionary<string, Type> map,
        ConcurrentDictionary<string, T> cache,
        Func<Type, string, bool> function)
    {
        Span<string> memory = CollectionsMarshal.AsSpan(map.Keys.ToList());
        ref var ptr = ref MemoryMarshal.GetReference(memory);
        bool registered = true;

        for (int i = 0; i < memory.Length; i++)
        {
            var slice = Unsafe.Add(ref ptr, i);

            if (cache.ContainsKey(slice) || !map.TryGetValue(slice, out Type type))
                continue;

            registered &= function(type, slice);
        }

        return registered;
    }
}