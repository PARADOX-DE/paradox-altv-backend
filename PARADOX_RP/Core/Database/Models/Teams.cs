using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public enum TeamTypes
    {
        NEUTRAL,
        DEPARTMENT,
        BAD
    }

    public class Teams
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public TeamTypes TeamType { get; set; }
        public string SpawnPosition { get; set; }

        public void SendNotification(string Title, string Message, NotificationTypes notificationType)
        {
            foreach(PXPlayer player in Pools.Instance.Get(PoolType.PLAYER))
            {
                player.SendNotification(Title, Message, notificationType);
            }
        }
    }
}
