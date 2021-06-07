using AltV.Net;
using AltV.Net.Enums;
using AltV.Net.Events;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Weapon
{
    class WeaponModule : ModuleBase<WeaponModule>, IEventPlayerWeaponChange
    {
        public WeaponModule() : base("Weapon")
        {

        }

        public async Task OnPlayerWeaponChange(PXPlayer player, uint oldWeapon, uint newWeapon)
        {
            if (player.PlayerWeapons.FirstOrDefault(p => (uint)p.WeaponHash == newWeapon) == null)
            {
                // CHEATED WEAPON
            }
        }
    }
}
