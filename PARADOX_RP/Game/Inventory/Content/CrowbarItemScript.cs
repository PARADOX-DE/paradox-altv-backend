using AltV.Net.Async;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Game.JumpPoint;
using PARADOX_RP.Game.Misc.Progressbar.Extensions;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using PARADOX_RP.Core.Extensions;

namespace PARADOX_RP.Game.Inventory.Content
{
    public sealed class CrowbarItemScript : IItemScript
    {
        private readonly JumpPointModule _jumpPointModule;
        
        public CrowbarItemScript(JumpPointModule jumpPointModule)
        {
            _jumpPointModule = jumpPointModule;
        }

        public string ScriptName => "crowbar_itemscript";

        public async Task<bool> UseItem(PXPlayer player) 
        {
            // Jumppoints

            var jumpPoint = _jumpPointModule._jumpPoints.FirstOrDefault(j => j.Value.Position.Distance(player.Position) < j.Value.Range).Value;
            if (jumpPoint != null)
            {
                return await _jumpPointModule.BreakJumpPoint(player, jumpPoint);
            }

            return false;
        }
    }
}
