using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
	/// <summary>
	/// Represents a container of bindings.
	/// </summary>
    public interface ISituation {
        void Load(Container container);
    }
}
