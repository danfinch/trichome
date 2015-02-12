using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    /// <summary>
    /// In this scope, only one instance of a requested service will ever be resolved.
    /// </summary>
    public class SingletonScope : IScope {
        ConcurrentDictionary<Type, object> instances = new ConcurrentDictionary<Type, object>();

        public object Resolve(Type type, Creator creator) {
            return instances.GetOrAdd(type, (t) => creator.Create());
        }

        public void Dispose() {
            foreach (var instance in instances.Values) {
                if (instance is IDisposable) {
                    (instance as IDisposable).Dispose();
                }
            }
            instances.Clear();
        }
    }
}
