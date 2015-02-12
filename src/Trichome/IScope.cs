using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
	/// <summary>
	/// Represents the method for managing lifetime of a service.
	/// </summary>
    public interface IScope : IDisposable {
        object Resolve(Type type, Creator creator);
    }
}
