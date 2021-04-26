using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Models
{
    public class InstanceBase<T>
    {
        public static T Instance { get; set; }
    }

    public class Instance<T> : InstanceBase<T> where T : Instance<T>
    {
        public Instance()
        {
            Instance = (T)this;
        }
    }
}
