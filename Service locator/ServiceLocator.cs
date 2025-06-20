using System;
using System.Collections.Generic;

namespace NTools
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new();
        private static readonly Dictionary<Type, Func<object>> NullFactory = new();

        /// <summary>
        /// Register a new service.
        /// OBS: if you want to register an interface, remember to specify the interface in T
        ///     eg. ServiceLocator.Register<IInterface>(new InterfaceImplementation())
        /// </summary>
        public static void Register<T> (T service, Func<object> nullFactory = null) where T : class
        {
            var type = typeof(T);
            Services[type] = service;
            
            if (nullFactory is null)
                return;
            
            NullFactory[type] = nullFactory;
        }

        public static T Resolve<T>() where T : class
        {
            var type = typeof(T);
            if (Services.TryGetValue(type, out var service))
                return (T)service;

            if (NullFactory.TryGetValue(type, out var factory))
                return (T)factory();

            throw new Exception($"Service {type} is not registered. - We should never reach here -");
        }

        /// <summary>
        /// Inject a custom NullFactory for a specific type
        /// </summary>
        public static void RegisterFactory<T> (Func<T> factory) where T : class
        {
            var type = typeof(T);
            NullFactory[type] = factory;
        }
    }
}