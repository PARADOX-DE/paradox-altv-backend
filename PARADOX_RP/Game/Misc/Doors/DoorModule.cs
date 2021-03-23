using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Misc.Doors
{
    class DoorModule : ModuleBase<DoorModule>
    {
        public DoorModule() : base("Door") { }

        public override Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if(key == KeyEnumeration.L)
            {
                //TODO lock & unlock door

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public override void OnModuleLoad()
        {

        }

        public void LockDoor()
        {

        }
    }
}
