using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    /// <summary>
    /// Fluent registration for a binding.
    /// </summary>
    public class Registrar {
        Container container;
        Registration registration;
        internal Registrar(Container container, Registration registration) {
            this.container = container;
            this.registration = registration;
        }

        /// <summary>
        /// Resolve this service to the given type.
        /// </summary>
        public Registrar To(Type type) {
            registration.Resolution = Resolution.Creator;
            registration.InstanceType = type;
            return this;
        }

        /// <summary>
        /// Resolve this service to the given type.
        /// </summary>
        public Registrar To<T>() {
            registration.Resolution = Resolution.Creator;
            registration.InstanceType = typeof(T);
            return this;
        }

        /// <summary>
        /// Resolve this service to the given factory.
        /// </summary>
        public Registrar To(Type type, Func<object> factory) {
            registration.Resolution = Resolution.Factory;
            registration.Factory = factory;
            return this;
        }

        /// <summary>
        /// Resolve this service to the given factory.
        /// </summary>
        public Registrar To<T>(Func<T> factory) {
            registration.Resolution = Resolution.Factory;
            registration.Factory = () => (object)factory();
            return this;
        }

        /// <summary>
        /// Resolve this service to the given value.
        /// </summary>
        public Registrar As(object instance) {
            registration.Resolution = Resolution.Cached;
            registration.CachedInstance = instance;
            return this;
        }

        /// <summary>
        /// Resolve this service in the given scope.
        /// </summary>
        public Registrar In(Type type) {
            registration.Scope = container.GetScope(type);
            return this;
        }

        /// <summary>
        /// Resolve this service in the given scope.
        /// </summary>
        public Registrar In<T>() where T : IScope, new() {
            registration.Scope = container.GetScope(typeof(T));
            return this;
        }

        /// <summary>
        /// Returns the container for chaining purposes.
        /// </summary>
        public Container And {
            get {
                return container;
            }
        }
    }
}
