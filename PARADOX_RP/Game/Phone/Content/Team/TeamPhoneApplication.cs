using AltV.Net.Async;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Phone.Content.Team.Models;
using PARADOX_RP.Game.Phone.Interfaces;
using PARADOX_RP.Game.Team;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows.Phone.Applications.Team;
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

        public TeamPhoneApplication(IEventController eventController)
        {
            eventController.OnClient<PXPlayer>("RequestTeamInfo", RequestTeamInfo);
        }

        public async Task<bool> IsPermitted(PXPlayer player)
        {
            if (player.Team.Id != (int)TeamEnumeration.CIVILIAN) // not in any team 
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        private void RequestTeamInfo(PXPlayer player)
        {
            if (!player.CanInteract()) return;

            List<TeamPhoneApplicationPlayer> _factionMembers = new List<TeamPhoneApplicationPlayer>();

            player.Team.Players.ForEach((p) =>
            {
                bool online = Pools.Instance.Find<PXPlayer>(PoolType.PLAYER, p.Id);

                _factionMembers.Add(new TeamPhoneApplicationPlayer()
                {
                    Id = p.Id,
                    Name = p.Username,
                    Online = online,
                    LastLogin = DateTime.Now
                });
            });

            string TeamName = player.Team.TeamName;
            bool IsLeader = player.PlayerTeamData.Rank >= 10;

            //view callback responseTeamMembers
            WindowController.Instance.Get<TeamListAppWindow>().ViewCallback(player, "ResponseTeamInfo", new TeamListAppWindowWriter(TeamName, _factionMembers, IsLeader));
        }
    }
}
