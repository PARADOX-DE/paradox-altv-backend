using AltV.Net.Async;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Injury.Extensions
{
    public static class InjuryClientExtensions
    {
        public static async Task<bool> Revive(this PXPlayer player, bool spawnAtCurrentPosition = true, bool keepMoney = true, bool keepInventory = true)
        {
            if (!await player.ExistsAsync())  return await Task.FromResult(false);
            if (!player.IsValid())  return await Task.FromResult(false);

            if (spawnAtCurrentPosition) await player.SpawnAsync(player.Position);
            if (!keepMoney) player.Money = 0;
            if (!keepInventory) { /* TODO: clear inventory */ }

            player.Injured = false;
            player.InjuryTimeLeft = 0;

            await using(var px = new PXContext())
            {
                Players dbPlayer = await px.Players.FindAsync(player.SqlId);
                if (dbPlayer == null) return await Task.FromResult(false);

                //TODO: clear db injurystate
                await px.SaveChangesAsync();
            }

             return await Task.FromResult(true);
        }
    }
}
