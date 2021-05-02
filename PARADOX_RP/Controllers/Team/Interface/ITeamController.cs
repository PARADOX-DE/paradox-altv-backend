using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Team.Interface
{
    public interface ITeamController
    {
        void LoadTeam(Teams team);

        public Task SetPlayerTeam(PXPlayer player, int teamId);
        public void SpawnPlayer(PXPlayer player);
        public void SendNotificationToDepartments(string Title, string Message, NotificationTypes notificationType);
        public void SendNotificationToBads(string Title, string Message, NotificationTypes notificationType);
        public void SendNotification(int Id, string Title, string Message, NotificationTypes notificationType);
    }
}
