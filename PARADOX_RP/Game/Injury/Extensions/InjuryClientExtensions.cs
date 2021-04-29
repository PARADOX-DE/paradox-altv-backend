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
        public static Task Revive(this PXPlayer player, bool spawnAtCurrentPosition = true, bool keepMoney = true, bool keepInventory = true)
        {
            if (!player.Exists) return Task.FromResult(false);
            if (!player.IsValid()) return Task.FromResult(false);

            if (spawnAtCurrentPosition) player.Spawn(player.Position, 0);
            if (!keepMoney) player.Money = 0;
            if (!keepInventory) { /* TODO: clear inventory */ }

            player.Injured = false;

            using(var px = new PXContext())
            {
                Players dbPlayer = px.Players.Find(player.SqlId);
                if (dbPlayer == null) return Task.FromResult(false);

                //TODO: clear db injurystate
                px.SaveChanges();
            }

            return Task.FromResult(true);
        }
    }
}
