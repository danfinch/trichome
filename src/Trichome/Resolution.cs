using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    /// <summary>
    /// Determines how a binding is resolved. For internal use.
    /// </summary>
    public enum Resolution {
        Creator,
        Cached,
        Factory,
    }
}
