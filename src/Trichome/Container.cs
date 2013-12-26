using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public class Container : IDisposable {
        ConcurrentDictionary<Type, Registration> registrations = new ConcurrentDictionary<Type, Registration>();
        Dictionary<Type, IScope> scopes = new Dictionary<Type, IScope>();
        object[] empty = new object[] { };
        IScope defaultScope;

        public Container() {
            defaultScope = GetScope(typeof(TransientScope));
        }

        internal IScope GetScope(Type type) {
            if (scopes.ContainsKey(type)) {
                return scopes[type];
            }
            var scope = type.GetConstructor(Type.EmptyTypes).Invoke(empty) as IScope;
            scopes[type] = scope;
            return scope;
        }

        Registration CreateRegistration(Type type) {
            var registration = new Registration {
                BaseType = type,
                InstanceType = type,
                Scope = defaultScope,
            };
            registration.Creator = new Creator(registration, this);
            return registration;
        }

        public object Inject(Type type) {
            var registration = registrations.GetOrAdd(type, CreateRegistration);
            return registration.Scope.Inject(type, registration.Creator);
        }

        public Registrar Bind(Type type) {
            var registration = CreateRegistration(type);
            registrations.TryAdd(type, registration);
            return new Registrar(this, registration);
        }

        public Registrar Bind<T>() {
            return Bind(typeof(T));
        }

        public Container SetDefaultScope(Type type) {
            defaultScope = GetScope(type);
            return this;
        }

        public Container SetDefaultScope<T>() where T : IScope, new() {
            return SetDefaultScope(typeof(T));
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}
