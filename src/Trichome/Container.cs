using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public class Container {
        Dictionary<Type, Registration> registrations = new Dictionary<Type, Registration>();
        Dictionary<Type, IScope> scopes = new Dictionary<Type, IScope>();
        object[] empty = new object[] { };

        internal IScope GetScope(Type type) {
            if (scopes.ContainsKey(type)) {
                return scopes[type];
            }
            var scope = type.GetConstructor(Type.EmptyTypes).Invoke(empty) as IScope;
            scopes[type] = scope;
            return scope;
        }

        public object Inject(Type type) {
            throw new NotImplementedException();
        }

        public Registrar Bind<T>() {
            var registration = new Registration {
                BaseType = typeof(T),
                InstanceType = typeof(T),
            };
            registration.Creator = new Creator(registration, this);
            registrations.Add(typeof(T), registration);
            return new Registrar(this, registration);
        }
    }
}
