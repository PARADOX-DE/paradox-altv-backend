using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Administration
{
    class PermissionsModule : ModuleBase<PermissionsModule>
    {
        public PermissionsModule() : base("Permissions") { }

        public void HasPermissions(IPlayer player)
        {
            PXPlayer pxPlayer = (PXPlayer)player;
            HasPermissions(pxPlayer);
        }

        public void HasPermissions(PXPlayer player)
        {
            player.SupportRank.Permissions
        }
    }
}
