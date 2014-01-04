using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public class Registrar {
        Container container;
        Registration registration;
        internal Registrar(Container container, Registration registration) {
            this.container = container;
            this.registration = registration;
        }

        public Registrar To(Type type) {
            registration.Resolution = Resolution.Created;
            registration.InstanceType = type;
            return this;
        }

        public Registrar To<T>() {
            registration.Resolution = Resolution.Created;
            registration.InstanceType = typeof(T);
            return this;
        }

        public Registrar To(Type type, Func<object> factory) {
            registration.Resolution = Resolution.Factory;
            registration.Factory = factory;
            return this;
        }

        public Registrar To<T>(Func<T> factory) {
            registration.Resolution = Resolution.Factory;
            registration.Factory = () => (object)factory();
            return this;
        }

        public Registrar As(object instance) {
            registration.Resolution = Resolution.Cached;
            registration.CachedInstance = instance;
            return this;
        }

        public Registrar In(Type type) {
            registration.Scope = container.GetScope(type);
            return this;
        }

        public Registrar In<T>() where T : IScope, new() {
            registration.Scope = container.GetScope(typeof(T));
            return this;
        }

        public Container And {
            get {
                return container;
            }
        }
    }
}
