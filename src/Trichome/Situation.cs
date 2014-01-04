using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public abstract class Situation : ISituation {
        protected Container Container { get; private set; }

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
