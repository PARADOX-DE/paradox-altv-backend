using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Interface
{
    interface IApplication
    {
        string Name { get; }
        string Author { get; }

        void Start();
        void Stop();
    }
}