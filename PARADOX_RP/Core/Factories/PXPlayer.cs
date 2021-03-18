using AltV.Net;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Game.Administration.Models;
using PARADOX_RP.Game.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Factories
{
    public enum DimensionTypes
    {
        WORLD
    }

    public enum NotificationTypes
    {
        SUCCESS
    }

    public enum DutyTypes
    {
        OFFDUTY,
        ONDUTY,
        ADMINDUTY
    }

    public class PXPlayer : Player
    {
        public int SqlId { get; set; }
        public bool LoggedIn { get; set; }
        public string Username { get; set; }
        public SupportRankModel SupportRank { get; set; }
        public DimensionTypes DimensionType { get; set; }
        public DutyTypes DutyType { get; set; }

        internal PXPlayer(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            SqlId = -1;
            LoggedIn = false;
            Username = "";
            SupportRank = new SupportRankModel();
            DimensionType = DimensionTypes.WORLD;
            DutyType = DutyTypes.OFFDUTY;
        }

        public void SendNotification(string Title, string Message, NotificationTypes notificationType)
        {
            Emit("sendNotification", Title, Message, Enum.GetName(typeof(NotificationTypes), notificationType));
        }
    }

    internal class PXPlayerFactory : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IntPtr entityPointer, ushort id)
        {
            return new PXPlayer(entityPointer, id);
        }
    }
}
