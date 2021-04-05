using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Commands.Extensions
{
    public static class ChatClientExtensions
    {
        public static void SendChatMessage(this IPlayer player, string message)
        {
            player.Emit("chat:message", null, message);
        }
    }
}
