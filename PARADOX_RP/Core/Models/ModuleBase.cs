using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Module
{
    public abstract class ModuleBase : IModuleBase
    {
        public bool Enabled { get; set; }
        public string ModuleName { get; set; }

        public virtual void OnModuleLoad() { }
        public virtual Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key) { return Task.FromResult(false); }
        public virtual void OnPlayerDeath(PXPlayer player, PXPlayer killer, uint reason) { }
        public virtual void OnPlayerConnect(PXPlayer player) { }
        public virtual void OnPlayerDisconnect(PXPlayer player) { }
        public virtual void OnPlayerLogin(PXPlayer player) { }
    }

    public abstract class ModuleBase<T> : ModuleBase where T : ModuleBase<T>
    {
        public static T Instance { get; set; }

        public ModuleBase(string ModuleName, bool Enabled = true)
        {
            this.ModuleName = ModuleName;
            this.Enabled = Enabled;

            Instance = (T)this;
        }
    }
}
