﻿using AltV.Net.Async;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Misc.Position;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Injury.Extensions
{
    public static class InjuryClientExtensions
    {
        public static async Task ApplyInjury(this PXPlayer player)
        {
            //TODO: freeze zaebis
            player.Injured = true;
            player.InjuryTimeLeft = player.PlayerInjuryData.InjuryTimeLeft;

            await player.StartEffect(player.PlayerInjuryData.Injury.EffectName, player.PlayerInjuryData.Injury.Duration * 1000);
            await player.PlayAnimation(player.PlayerInjuryData.Injury.AnimationDictionary, player.PlayerInjuryData.Injury.AnimationName);

        }

        public static async Task<bool> Revive(this PXPlayer player, bool spawnAtCurrentPosition = true, bool keepMoney = true, bool keepInventory = true)
        {
            if (!await player.ExistsAsync()) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);

            if (spawnAtCurrentPosition) await player.SpawnAsync(player.Position);
            if (!keepMoney) player.Money = 0;
            if (!keepInventory) { /* TODO: clear inventory */ }

            player.Injured = false;
            player.InjuryTimeLeft = 0;

            await player.StopAnimation();

            await using (var px = new PXContext())
            {
                PlayerInjuryData dbPlayerInjury = await px.PlayerInjuryData.FindAsync(player.PlayerInjuryData.Id);
                if (dbPlayerInjury == null) return await Task.FromResult(false);

                dbPlayerInjury.InjuryId = 1;
                dbPlayerInjury.InjuryTimeLeft = 0;

                await px.SaveChangesAsync();
            }

            await player.SpawnAsync(PositionModule.Instance.Get(Positions.MEDICAL_DEPARTMENT));

            return await Task.FromResult(true);
        }
    }
}
