using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Handlers.Team;
using PARADOX_RP.Handlers.Team.Interface;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Game.Team
{

    class TeamModule : ModuleBase<TeamModule>
    {
        public Dictionary<int, Teams> TeamList;
        private readonly ITeamHandler _teamHandler;

        public TeamModule(ITeamHandler teamHandler) : base("Team")
        {
            TeamList = new Dictionary<int, Teams>();

            //using (var px = new PXContext())
            //{
            //    foreach (Teams team in px.Teams)
            //    {
            //        TeamList.Add(team.Id, team);
            //        _teamHandler.LoadTeam(team);
            //    }
            //}

            _teamHandler = teamHandler;
        }

        public void InviteTeamMember(PXPlayer player, string inviteString)
        {
            if (!player.CanInteract()) return;

            PXPlayer invitePlayer = Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).FirstOrDefault(p => p.Name.ToLower().Contains(inviteString));
            if (invitePlayer == null) return;

            if (player.PlayerTeamData.Rank < 10)
            {
                player.Team.SendNotification("Du verfügst nicht über ausreichende Bereichtigungen...", NotificationTypes.ERROR);
                return;
            }

            if (invitePlayer.Team.Id != 1)
            {
                player.Team.SendNotification($"{inviteString} ist bereits Mitglied einer Fraktion.", NotificationTypes.ERROR);
                return;
            }

            WindowManager.Instance.Get<ConfirmationWindow>().Show(player, new ConfirmationWindowObject(player.Team.TeamName, $"{player.Username} hat dich eingeladen, um der Fraktion {player.Team.TeamName} beizutreten.", "TeamInviteAccept", "TeamInviteDecline"));
        }

        public void RequestTeamMembers(PXPlayer player, bool onlineState)
        {
            if (!player.CanInteract()) return;

            IEnumerable<PXPlayer> _factionMembers = null;
            if (onlineState) _factionMembers = Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).Where(p => p.Team.Id == player.Team.Id);

            //response to client
        }
    }
}
