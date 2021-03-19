using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Login.Extensions
{
    static class LoginClientExtensions
    {
        public static void PreparePlayer(this PXPlayer client) {
            client.Emit("PreparePlayer", null);
            //TODO: Client-Side Prepare Function
        }
    }
}
