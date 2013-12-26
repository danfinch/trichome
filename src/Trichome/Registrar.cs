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
            registration.InstanceType = type;
            return this;
        }

        public Registrar To<T>() {
            registration.InstanceType = typeof(T);
            return this;
        }

        public Registrar As(object instance) {
            registration.Instance = instance;
            registration.IsInstanceRegistered = true;
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
    }
}
