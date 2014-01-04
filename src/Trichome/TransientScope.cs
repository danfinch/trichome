using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public class TransientScope : IScope {
        public object Resolve(Type type, Creator creator) {
            return creator.Create();
        }

        public void Dispose() { }
    }
}
