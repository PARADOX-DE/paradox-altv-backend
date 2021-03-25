using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Handlers.Team
{
    class TeamHandler
    {
        public void SendNotificationToDepartments(string Title, string Message, NotificationTypes notificationType)
        {
            TeamModule.Instance.TeamList.Values.ForEach((team) =>
            {
                if (team.TeamType == TeamTypes.NEUTRAL)
                    team.SendNotification(Title, Message, notificationType);
            });
        }

        public void SendNotificationToBads(string Title, string Message, NotificationTypes notificationType)
        {
            TeamModule.Instance.TeamList.Values.ForEach((team) =>
            {
                if (team.TeamType == TeamTypes.BAD)
                    team.SendNotification(Title, Message, notificationType);
            });
        }
    }
}
