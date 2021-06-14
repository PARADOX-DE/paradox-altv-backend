using AltV.Net.Async;
using PARADOX_RP.Controllers.UI.Windows.Death;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Misc.Position;
using PARADOX_RP.UI;
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

            await player.StartEffect(player.PlayerInjuryData.Injury.EffectName, (player.PlayerInjuryData.Injury.Duration * 1000) * 60);
            await player.PlayAnimation(player.PlayerInjuryData.Injury.AnimationDictionary, player.PlayerInjuryData.Injury.AnimationName);

            WindowController.Instance.Get<DeathWindow>().Show(player);
        }

        public static async Task<bool> Revive(this PXPlayer player, bool spawnAtCurrentPosition = true, bool keepMoney = true, bool keepInventory = true)
        {
            if (!await player.ExistsAsync()) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);

            if (spawnAtCurrentPosition) await player.SpawnAsync(player.Position);
            else await player.SpawnAsync(PositionModule.Instance.Get(Positions.MEDICAL_DEPARTMENT));

            if (!keepMoney) player.Money = 0;
            if (!keepInventory) { /* TODO: clear inventory */ }

            player.Injured = false;
            player.InjuryTimeLeft = 0;
            player.PlayerInjuryData.InjuryTimeLeft = 0;

            await player.StopAnimation();
            await player.StopEffect();
            WindowController.Instance.Get<DeathWindow>().Hide(player);

            await using (var px = new PXContext())
            {
                PlayerInjuryData dbPlayerInjury = await px.PlayerInjuryData.FindAsync(player.PlayerInjuryData.Id);
                if (dbPlayerInjury == null) return await Task.FromResult(false);

                if (!keepMoney)
                {
                    Players dbPlayer = await px.Players.FindAsync(player.SqlId);
                    if (dbPlayer == null) return await Task.FromResult(false);

                    dbPlayer.Money = player.Money;
                }

                dbPlayerInjury.InjuryId = 1;
                dbPlayerInjury.InjuryTimeLeft = 0;

                await px.SaveChangesAsync();
            }

            return await Task.FromResult(true);
        }
    }
}
