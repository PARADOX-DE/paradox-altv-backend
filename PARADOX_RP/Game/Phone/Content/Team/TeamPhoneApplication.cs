using AltV.Net.Async;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Phone.Content.Team.Models;
using PARADOX_RP.Game.Phone.Interfaces;
using PARADOX_RP.Game.Team;
using PARADOX_RP.UI;
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

        public async Task<bool> IsPermitted(PXPlayer player)
        {
            if (player.Team.Id != (int)TeamEnumeration.CIVILIAN) // not in any team
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        private void RequestTeamMembers(PXPlayer player, bool onlineState)
        {
            if (!player.CanInteract()) return;

            Dictionary<int, TeamPhoneApplicationPlayer> _factionMembers = new Dictionary<int, TeamPhoneApplicationPlayer>();
            if (onlineState)
                Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).Where(p => p.Team.Id == player.Team.Id).ForEach((p) =>
                {
                    _factionMembers.Add(p.Id, new TeamPhoneApplicationPlayer()
                    {
                        Id = p.Id,
                        Name = p.Username,
                        LastLogin = DateTime.Now
                    });
                });
            else
            {
                player.Team.Players.ForEach((p) =>
                {
                    _factionMembers.Add(p.Id, new TeamPhoneApplicationPlayer()
                    {
                        Id = p.Id,
                        Name = p.Username,
                        LastLogin = DateTime.Now
                    });
                });
            }

            //view callback responseTeamMembers
        }
    }
}
