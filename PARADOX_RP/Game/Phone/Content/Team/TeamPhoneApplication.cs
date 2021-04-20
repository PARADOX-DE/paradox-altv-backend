using AltV.Net.Async;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Phone.Interfaces;
using PARADOX_RP.Game.Team;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Phone.Content
{
    class TeamPhoneApplication : IPhoneApplication
    {
        public string ApplicationName { get => "TeamListApp"; }

        public TeamPhoneApplication()
        {

            AltAsync.OnClient<PXPlayer, bool>("RequestTeamMembers", RequestTeamMembers);
        }

         

        private void RequestTeamMembers(PXPlayer player, bool onlineState)
        {
            if (!player.CanInteract()) return;

            IEnumerable<PXPlayer> _factionMembers = null;
            if (onlineState) _factionMembers = Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).Where(p => p.Team.Id == player.Team.Id);
            else
            {

            }
            

        }
    }
}
