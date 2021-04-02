using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Administration.Models;
using PARADOX_RP.Game.Login;
using PARADOX_RP.Game.MiniGames.Models;
using PARADOX_RP.Game.Team;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Factories
{
    public enum DimensionTypes
    {
        WORLD,
        TEAMHOUSE
    }

    public enum NotificationTypes
    {
        SUCCESS,
        ERROR
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
        public Teams Team { get; set; }
        public PlayerTeamData PlayerTeamData { get; set; }
        public DimensionTypes DimensionType { get; set; }
        public DutyTypes DutyType { get; set; }
        public CancellationTokenSource CancellationToken { get; set; }
        public Dictionary<int, Clothes> Clothes { get; set; }
        public MinigameTypes Minigame { get; set; }
        internal PXPlayer(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            SqlId = -1;
            LoggedIn = false;
            Username = "";
            SupportRank = new SupportRankModel();
            Team = null;
            PlayerTeamData = null;
            DimensionType = DimensionTypes.WORLD;
            DutyType = DutyTypes.OFFDUTY;
            CancellationToken = null;
            Clothes = new Dictionary<int, Clothes>();
        }

        public void SendNotification(string Title, string Message, NotificationTypes notificationType)
        {
            this.SendChatMessage(Message);
            Emit("sendNotification", Title, Message, Enum.GetName(typeof(NotificationTypes), notificationType));
        }

        public void SetClothes(int component, int drawable, int texture)
        {
            Emit("SetClothes", component, drawable, texture);
        }

        public bool CanInteract()
        {
            if (LoggedIn) return false;
            if (CancellationToken != null) return false;

            return true;
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
