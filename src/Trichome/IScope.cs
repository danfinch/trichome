﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trichome {
    public interface IScope : IDisposable {
        object Resolve(Type type, Creator creator);
    }
}
