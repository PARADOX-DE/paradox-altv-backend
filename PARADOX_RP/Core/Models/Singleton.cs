using PARADOX_RP.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Models
{
    public class SingletonBase<T> : ISingleton
    {
        public static T Instance { get; set; }
    }

    public class Singleton<T> : SingletonBase<T> where T : Singleton<T>
    {
        public Singleton()
        {
            Instance = (T)this;
        }
    }
}
