using AltV.Net;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Administration.Models;
using PARADOX_RP.Game.Commands.Extensions;
using PARADOX_RP.Game.Login;
using PARADOX_RP.Game.MiniGames.Models;
using PARADOX_RP.Game.Team;
using PARADOX_RP.Models;
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
        public int Money { get; set; }
        public int BankMoney { get; set; }

        private bool _injured;
        public bool Injured
        {
            get => _injured;
            set
            {
                Emit("UpdateInjured", value);
                _injured = value;
            }
        }

        private bool _cuffed;
        public bool Cuffed
        {
            get => _cuffed;
            set
            {
                Emit("UpdateCuff", value);
                _cuffed = value;
            }
        }

        public SupportRankModel SupportRank { get; set; }
        public Teams Team { get; set; }
        public PlayerCustomization PlayerCustomization { get; set; }
        public PlayerTeamData PlayerTeamData { get; set; }
        public Invitation Invitation { get; set; }
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
            PlayerCustomization = null;
            PlayerTeamData = null;
            Invitation = null;
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

        public void Freeze(bool state)
        {
            Emit("Freeze", state);
        }

        public bool IsValid()
        {
            if (!LoggedIn) return false;
            if (SqlId < 1) return false;

            return true;
        }

        public bool CanInteract()
        {
            if (!LoggedIn) return false;
            if (CancellationToken != null) return false;
            if (Cuffed || Injured) return false;

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
