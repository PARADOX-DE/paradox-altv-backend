using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Administration;
using PARADOX_RP.Game.MiniGames;
using PARADOX_RP.Game.MiniGames.Models;
using AltV.Net.Enums;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using PARADOX_RP.Core.Extensions;
using System.Linq;
using PARADOX_RP.Utils;
using PARADOX_RP.Game.Team;

namespace PARADOX_RP.Game.Commands
{
    class ChatModule : ModuleBase<ChatModule>, IScript
    {
        private readonly IEnumerable<ModuleBase> _modules;

        public ChatModule() : base("Chat") {
        }

        [ClientEvent("chat:message")]
        public void OnChatMessage(IPlayer player, string msg)
        {
            if (msg.Length == 0 || msg[0] == '/') return;
            player.SendChatMessage(msg);
        }

        [CommandEvent(CommandEventType.CommandNotFound)]
        public void OnCommandNotFound(IPlayer player, string cmd)
        {
            player.SendChatMessage("{FF0000}[Server] {FFFFFF}Befehl nicht gefunden.");
        }

        [Command("minigame")]
        public void enterMinigameCommand(PXPlayer player, string minigameModule)
        {
            MinigameTypes _minigameType = Enum.Parse<MinigameTypes>(minigameModule);

            MinigameModule.Instance.ChooseMinigame(player, _minigameType);
        }

        [Command("veh")]
        public async void veh(PXPlayer player, string vehicleModel)
        {
            try
            {
                await AltAsync.CreateVehicle(Alt.Hash(vehicleModel), player.Position, new Rotation(0, 0, 0));
            }
            catch
            {
                AltAsync.Log("Vehicle-Hash not found.");
            }
        }

        [Command("pos")]
        public void pos(PXPlayer player, string positionName)
        {
            AltAsync.Log($"{positionName} | {player.Position.X.ToString().Replace(",", ".")}, {player.Position.Y.ToString().Replace(",", ".")}, {player.Position.Z.ToString().Replace(",", ".")}");
        }

        [Command("rot")]
        public void rot(PXPlayer player, string positionName)
        {
            AltAsync.Log($"{positionName} | {player.Rotation.Pitch.ToString().Replace(",", ".")}, {player.Rotation.Roll.ToString().Replace(",", ".")}, {player.Rotation.Yaw.ToString().Replace(",", ".")}");
        }

        [Command("aduty")]
        public async void aduty(PXPlayer player)
        {
            await AdministrationModule.Instance.OnKeyPress(player, KeyEnumeration.F9);
        }

        [Command("module")]
        public void SetModuleState(PXPlayer player, string moduleName, bool state)
        {
            _modules.FirstOrDefault(m => m.ModuleName.ToLower() == moduleName.ToLower()).Enabled = state;
        }

        [Command("config")]
        public void ChangeConfigEntry(PXPlayer player, string entry, string value)
        {
            switch (entry)
            {
                case "devmode":
                    Configuration.Instance.DevMode = Convert.ToBoolean(value);
                    break;

                case "radio_url":
                    Configuration.Instance.VehicleRadioURL = value;
                    break;
            }
        }

        [Command("setteam")]
        public void SetTeam(PXPlayer player)
        {
            Console.WriteLine("SetTeam");
            TeamModule.Instance.InviteTeamMember(player, player.Username);
        }
    }
}
