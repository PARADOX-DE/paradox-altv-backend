using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

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

        public bool HasPermissions(PXPlayer player, [CallerMemberName] string callerName = null)
        {
            return player.SupportRank.Permissions.FirstOrDefault(i => i.Permission.CallerName == callerName) != null;
        }
    }
}
