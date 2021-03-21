using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Resources.Chat.Api;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Commands
{
    class ChatModule : ModuleBase<ChatModule>
    {

        public ChatModule() : base("Chat")
        {
            AltAsync.OnClient<PXPlayer, string>("chat:message", OnChatMessage);
        }

        public void OnChatMessage(PXPlayer player, string msg)
        {
            if (msg.Length == 0 || msg[0] == '/') return;

            string whiteColor = "{FFFFFF}";
            string prefixColor = "{FFFFFF}";

            Alt.Log($"{prefixColor}{player.Username}{whiteColor}: {msg}");
        }

        [CommandEvent(CommandEventType.CommandNotFound)]
        public void OnCommandNotFound(PXPlayer player, string cmd)
        {
            Alt.Log("{FF0000}[Server] {FFFFFF}Befehl nicht gefunden.");
        }
    }
}
