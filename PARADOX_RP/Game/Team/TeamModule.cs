using AltV.Net;
using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Commands;
using PARADOX_RP.Game.Commands.Attributes;
using PARADOX_RP.Controllers.Team;
using PARADOX_RP.Controllers.Team.Interface;
using PARADOX_RP.Models;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PARADOX_RP.Controllers.Event.Interface;
using EntityStreamer;
using System.Numerics;
using AltV.Net.Data;

namespace PARADOX_RP.Game.Team
{
    // after adding teams in db => add to enum
    public enum TeamEnumeration
    {
        CIVILIAN = 1,
        LSPD,
        LSMC
    }

    class TeamModule : ModuleBase<TeamModule>, ICommand
    {
        public Dictionary<int, Teams> TeamList = new Dictionary<int, Teams>();
        private readonly IEventController _eventController;
        private readonly ITeamController _teamHandler;

        public TeamModule(PXContext pxContext, IEventController eventController, ITeamController teamHandler) : base("Team")
        {
            LoadDatabaseTable(pxContext.Teams, (Teams team) =>
            {
                TeamList.Add(team.Id, team);
            });

            _eventController = eventController;
            _teamHandler = teamHandler;

            _eventController.OnClient<PXPlayer, string>("InviteTeamMember", InviteTeamMember);
            _eventController.OnClient<PXPlayer>("TeamInviteAccept", TeamInviteAccept);
        }

        public override void OnPlayerConnect(PXPlayer player)
        {
            TeamList.ForEach((team) =>
            {
                if (Configuration.Instance.DevMode)
                {
                    MarkerStreamer.Create(MarkerTypes.MarkerTypeThickChevronUp, team.Value.SpawnPosition, new Vector3(1, 1, 1), new Vector3(0, 0, 0), null, new Rgba(37, 165, 202, 125));
                    TextLabelStreamer.Create($"[{Enum.GetName(typeof(TeamTypes), team.Value.TeamType)}] Team: {team.Value.TeamName}", team.Value.SpawnPosition);
                }
            });
        }

        public void InviteTeamMember(PXPlayer player, string inviteString)
        {
            if (!player.CanInteract()) return;

            PXPlayer invitePlayer = Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).FirstOrDefault(p => p.Username.ToLower().Contains(inviteString.ToLower()));
            if (invitePlayer == null)
            {
                player.Team.SendNotification(player, "Person nicht gefunden!", NotificationTypes.ERROR);
                return;
            }

            if (player.PlayerTeamData.Rank < 10)
            {
                player.Team.SendNotification(player, "Du verfügst nicht über ausreichende Bereichtigungen...", NotificationTypes.ERROR);
                return;
            }

            if (invitePlayer.Team.Id != 1)
            {
                player.Team.SendNotification(player, $"{inviteString} ist bereits Mitglied einer Fraktion.", NotificationTypes.ERROR);
                return;
            }

            invitePlayer.Invitation = new Invitation()
            {
                InviterId = player.SqlId,
                Team = player.Team
            };

            WindowManager.Instance.Get<ConfirmationWindow>().Show(player, new ConfirmationWindowWriter(player.Team.TeamName, $"{player.Username} hat dich eingeladen, um der Fraktion {player.Team.TeamName} beizutreten.", nameof(TeamInviteAccept), "TeamInviteDecline"));
        }

        public async void TeamInviteAccept(PXPlayer player)
        {
            WindowManager.Instance.Get<ConfirmationWindow>().Hide(player);

            if (!player.CanInteract()) return;
            if (player.Invitation == null || player.Invitation.Team == null) return;

            Invitation invitation = player.Invitation;
            await _teamHandler.SetPlayerTeam(player, invitation.Team.Id);
            player.Invitation = null;

            player.Team.SendNotification($"{player.Username} ist nun Mitglied der Fraktion.", NotificationTypes.SUCCESS);
        }


        [Command("set_team")]
        public void SetTeam(PXPlayer player)
        {
            InviteTeamMember(player, player.Username);
        }
    }
}
