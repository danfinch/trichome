using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public class TransientScope : IScope {
        public object Inject(Type type, Creator constructor) {
            throw new NotImplementedException();
        }
    }
}
