using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public class Registration {
        public Type BaseType;
        public Type InstanceType;
        public IScope Scope;
        public Resolution Resolution;
        public Creator Creator;
        public object CachedInstance;
        public Func<object> Factory;
    }
}
