using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Team;
using PARADOX_RP.Handlers.Team.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Handlers.Team
{
    class TeamHandler : ITeamHandler
    {

        public void LoadTeam(Teams team)
        {
            AltAsync.CreateCheckpoint(AltV.Net.Elements.Entities.CheckpointType.CycleArrow, team.SpawnPosition, 2, 2, new AltV.Net.Data.Rgba(255, 0, 0, 255));
        }

        public async Task SetPlayerTeam(PXPlayer player, int teamId)
        {
            if (TeamModule.Instance.TeamList.TryGetValue(teamId, out Teams team))
            {
                player.Team = team;

                using (var px = new PXContext())
                {
                    Players dbPlayer = await px.Players.FirstOrDefaultAsync(p => p.Id == player.SqlId);
                    if (dbPlayer == null) return;

                    PlayerTeamData playerTeamData = dbPlayer.PlayerTeamData.FirstOrDefault();
                    if (playerTeamData == null) return;

                    playerTeamData.Joined = DateTime.Now;
                    playerTeamData.Payday = 0;
                    playerTeamData.Rank = 0;

                    dbPlayer.TeamsId = teamId;

                    await px.SaveChangesAsync();
                }
            }
        }

        public void SpawnPlayer(PXPlayer player)
        {
            if (player.Team == null) return;
            //player.Position = player.Team.SpawnPosition;
        }

        public void SendNotificationToDepartments(string Title, string Message, NotificationTypes notificationType)
        {
            TeamModule.Instance.TeamList.Values.ForEach((team) =>
            {
                if (team.TeamType == TeamTypes.NEUTRAL)
                    team.SendNotification(Message, notificationType);
            });
        }

        public void SendNotificationToBads(string Title, string Message, NotificationTypes notificationType)
        {
            TeamModule.Instance.TeamList.Values.ForEach((team) =>
            {
                if (team.TeamType == TeamTypes.BAD)
                    team.SendNotification(Message, notificationType);
            });
        }
    }
}
