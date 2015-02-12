using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
	/// <summary>
	/// Convenience base class for defining situations (containers for bindings).
	/// </summary>
    public abstract class Situation : ISituation {
        protected Container Container { get; private set; }

		/// <summary>
		/// Create a binding for a specified base type.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
        protected Registrar Bind<T>() {
            return Container.Bind<T>();
        }

        public void Load(Container container) {
            Container = container;
            Load();
        }

        public abstract void Load();
    }
}
