using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome.Web {
    public class ContainerDependencyResolver {
        Container container;
        public ContainerDependencyResolver(Container container) {
            this.container = container;
        }
    }
}
