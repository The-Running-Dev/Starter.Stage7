using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace Starter.Bootstrapper
{
    public class IocWrapper
    {
        public IServiceProvider Container;

        private IocWrapper() { }

        public IocWrapper(IServiceProvider container)
        {
            Container = container;
        }

        /// <summary>
        /// Gets the instance of the dependency resolver
        /// </summary>
        public static IocWrapper Instance
        {
            get
            {
                if (_instance != null)
                {
                    lock (SyncObject)
                    {
                        return _instance;
                    }
                }

                lock (SyncObject)
                {
                    if (_instance != null)
                    {
                        return _instance;
                    }

                    _instance = new IocWrapper();
                }

                return _instance;
            }
            set
            {
                lock (SyncObject)
                {
                    _instance = value;
                }
            }
        }

        public T GetService<T>() where T : class
        {
            return Instance.Container.GetService<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            return Instance.Container.GetServices<T>();
        }

        public object GetService(Type serviceType)
        {
            return Instance.Container.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Instance.Container.GetServices(serviceType);
        }

        private static readonly object SyncObject = new object();

        private static IocWrapper _instance;
    }
}
