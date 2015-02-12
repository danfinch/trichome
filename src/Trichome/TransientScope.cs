using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
	/// <summary>
	/// In this scope, every resolution will return a new instance.
	/// </summary>
    public class TransientScope : IScope {
        public object Resolve(Type type, Creator creator) {
            return creator.Create();
        }

        public void Dispose() { }
    }
}
