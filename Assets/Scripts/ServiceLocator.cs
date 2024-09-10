using System;
using System.Collections.Generic;

public class ServiceLocator
{
    public static Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void RegisterService<T>(T service)
    {
        _services.Add(typeof(T), service);     
    }

    public static T GetService<T>()
    {
        var key = typeof(T);

        if (!_services.ContainsKey(key))
        {
            throw new Exception($"Service of type {typeof(T)} not found");
        }

        return (T)_services[key];
    }
}

