using AltV.Net;
using AltV.Net.Enums;
using AltV.Net.Events;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Moderation;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Weapon
{
    class WeaponModule : Module<WeaponModule>, IEventPlayerWeaponChange
    {
        private readonly ILogger _logger;
        public WeaponModule(ILogger logger) : base("Weapon")
        {
            _logger = logger;
        }

        public async Task OnPlayerWeaponChange(PXPlayer player, uint oldWeapon, uint newWeapon)
        {
            if (newWeapon == (uint)WeaponModel.Fist || newWeapon < 1) return;

            if (player.PlayerWeapons.FirstOrDefault(p => (uint)p.WeaponHash == newWeapon) == null)
                await ModerationModule.Instance.BanPlayer(player);

            foreach (var weapon in player.PlayerWeapons)
            {
                _logger.Console(ConsoleLogType.SUCCESS, "Weapon", $"Waffe: {weapon.WeaponHash} {weapon.Ammo}");
            }

            _logger.Console(ConsoleLogType.SUCCESS, "Weapon", player.PlayerWeapons.FirstOrDefault(p => (uint)p.WeaponHash == newWeapon) == null ? "Hat keine Waffe in DB" : "Hat Waffe in DB");

        }
    }
}
