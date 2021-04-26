using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Controllers.Event;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public virtual Task<bool> OnColShapeEntered(PXPlayer player, IColShape col) { return Task.FromResult(false); }
        public virtual Task OnPlayerEnterVehicle(IVehicle vehicle, IPlayer player, byte seat) { return Task.CompletedTask; }
        public virtual Task OnPlayerLeaveVehicle(IVehicle vehicle, IPlayer player, byte seat) { return Task.CompletedTask; }

        public IEnumerable<T> LoadDatabaseTable<T>(IQueryable queryable, Action<T>? action = null) where T : class
        {
            try
            {
                List<T> items = new List<T>();
                foreach (T item in queryable)
                {
                    if (item == null) continue;
                    action?.Invoke(item);
                    items.Add(item);
                }

                return items;
            }
            catch (Exception e) { AltV.Net.Alt.Log(e.Message); }

            return null;
        }
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
