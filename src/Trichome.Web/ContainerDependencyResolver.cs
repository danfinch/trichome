using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Trichome.Web {
    public class ContainerDependencyResolver : IDependencyResolver {
        Container container;
        public ContainerDependencyResolver(Container container) {
            this.container = container;
        }

        public object GetService(Type serviceType) {
            return container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return new object[] { };
        }
    }
}
