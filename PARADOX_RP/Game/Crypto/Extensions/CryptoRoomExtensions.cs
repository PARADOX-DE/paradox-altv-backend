using AltV.Net.Async;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Crypto.Extensions
{
    public static class CryptoRoomExtensions
    {
        // CryptoRoom can be upgraded up to 10 servers
        public static void LoadCryptoRoom(this PXPlayer player, int serverAmount) => player.EmitLocked("Crypto::LoadServer", serverAmount);
    }
}
