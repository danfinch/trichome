using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    /// <summary>
    /// Fast IoC container. Thread-safe. Only supports constructor injection. Favors constructors with most parameters.
    /// </summary>
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
                Resolution = Resolution.Created,
            };
            registration.Creator = new Creator(registration, this);
            return registration;
        }

        Registration CreateSubtypeRegistration(Type type, Registration parentRegistration) {
            var registration = new Registration {
                BaseType = type,
                InstanceType = parentRegistration.InstanceType,
                Scope = defaultScope,
                Resolution = Resolution.Created,
            };
            registration.Creator = new Creator(registration, parentRegistration);
            return registration;
        }

        /// <summary>
        /// Resolves an object instance for a given type.
        /// </summary>
        public object Resolve(Type type) {
            var registration = registrations.GetOrAdd(type, t => {
                var parent = registrations
                    .Where(p => p.Key.IsAssignableFrom(type))
                    .Select(p => p.Value)
                    .FirstOrDefault();
                return CreateSubtypeRegistration(type, parent);
            });
            return registration.Scope.Resolve(type, registration.Creator);
        }

        /// <summary>
        /// Create a binding for a specified base type.
        /// </summary>
        /// <returns>Registrar, a fluent interface for configuring the binding.</returns>
        public Registrar Bind(Type type) {
            var registration = CreateRegistration(type);
            registrations.TryAdd(type, registration);
            return new Registrar(this, registration);
        }

        /// <summary>
        /// Create a binding for a specified base type.
        /// </summary>
        /// <returns>Registrar, a fluent interface for configuring the binding.</returns>
        public Registrar Bind<T>() {
            return Bind(typeof(T));
        }

        /// <summary>
        /// Sets the default scope new bindings will receive.
        /// </summary>
        public Container SetDefaultScope(Type type) {
            defaultScope = GetScope(type);
            return this;
        }

        /// <summary>
        /// Sets the default scope new bindings will receive.
        /// </summary>
        public Container SetDefaultScope<T>() where T : IScope, new() {
            return SetDefaultScope(typeof(T));
        }

        public Container Situation<T>() where T : ISituation, new() {
            var situation = new T();
            situation.Load(this);
            return this;
        }

        public void Dispose() {
            foreach (var scope in scopes.Values) {
                scope.Dispose();
            }
        }
    }
}
