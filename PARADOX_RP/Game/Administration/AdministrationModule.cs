using PARADOX_RP.Core.Module;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PARADOX_RP.Utils.Enums;
using AltV.Net.Async;
using AltV.Net;

namespace PARADOX_RP.Game.Administration
{
    class AdministrationModule : ModuleBase<AdministrationModule>
    {
        public AdministrationModule() : base("Administration") { }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.F9)
            {
                if (player.DutyType != DutyTypes.ADMINDUTY) await EnterAduty(player);
                else await LeaveAduty(player);

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task EnterAduty(PXPlayer player)
        {
            if (PermissionsModule.Instance.HasPermissions(player))
            {
                player.DutyType = DutyTypes.ADMINDUTY;
            }
        }

        public async Task LeaveAduty(PXPlayer player)
        {
            if (PermissionsModule.Instance.HasPermissions(player))
            {
                if (player.DutyType != DutyTypes.ADMINDUTY) return;

                player.DutyType = DutyTypes.OFFDUTY;
            }
        }
    }
}
