using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Cache.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Cache
{
    public class CacheModule : ModuleBase<CacheModule>
    {
        public CacheModule() : base("Cache")
        {

        }

        public void Cache<T>(PXPlayer player) where T : ICachable
        {

        }
    }
}
