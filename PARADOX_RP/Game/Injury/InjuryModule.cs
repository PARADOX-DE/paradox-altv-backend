using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Team.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Events.Intervals;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Administration;
using PARADOX_RP.Game.Commands;
using PARADOX_RP.Game.Commands.Attributes;
using PARADOX_RP.Game.Injury.Extensions;
using PARADOX_RP.Game.Injury.Interfaces;
using PARADOX_RP.Game.MiniGames.Models;
using PARADOX_RP.Game.Misc.Position;
using PARADOX_RP.Game.Paintball;
using PARADOX_RP.Game.Team;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Injury
{
    public enum InjuryTypes
    {
        OTHER,
        KNOCK_OUT,
        CUT,
        SHOT
    }

    public sealed class InjuryModule : Module<InjuryModule>, ICommand, IEventPlayerDeath, IEventIntervalMinute
    {
        private readonly IEnumerable<ISpecialInjury> _specialInjuries;
        private readonly ITeamController _teamController;

        private Dictionary<uint, Injuries> _injuries = new Dictionary<uint, Injuries>();

        public InjuryModule(PXContext pxContext, IEnumerable<ISpecialInjury> specialInjuries, ITeamController teamController) : base("Injury")
        {
            _specialInjuries = specialInjuries;
            _teamController = teamController;

            _specialInjuries.ForEach((i) => { AltAsync.Log(i.GetType().FullName); });

            LoadDatabaseTable<Injuries>(pxContext.Injuries, (injury) => _injuries.Add(injury.Weapon, injury));
        }

        public async void OnPlayerDeath(PXPlayer player, PXPlayer killer, DeathReasons deathReason, uint weapon)
        {
            if (!player.IsValid()) return;

            if ((player.Dimension != 0 || player.DimensionType != DimensionTypes.WORLD) && player.Minigame == MinigameTypes.NONE)
            {
                // Falls der Spieler in einem Interior u.ä. ist:
                await AltAsync.Do(() =>
                {
                    if (player.LastWorldPosition == null) player.LastWorldPosition = player.Position;

                    player.DimensionType = DimensionTypes.WORLD;
                    player.Dimension = 0;
                    player.Spawn(player.LastWorldPosition, 0);
                });
            }
            else
            {
                // Special Injury (z.B: für Minigames)
                ISpecialInjury specialInjury = null;
                foreach (var i in _specialInjuries)
                {
                    bool hasSpecialInjury = await i.HasSpecialInjury(player);
                    if (hasSpecialInjury)
                    {
                        specialInjury = i;
                        break;
                    }
                }

                if (specialInjury != null)
                {
                    await specialInjury.ApplyInjury(player);
                    return;
                }
            }

            if (Configuration.Instance.DevMode) AltAsync.Log($"[DEATH] {player.Username} // REASON: {Enum.GetName(typeof(DeathReasons), deathReason)} // Weapon: {weapon}");

            if (_injuries.TryGetValue(weapon, out Injuries injury))
            {
                await player.SpawnAsync(player.Position);
                await using (var px = new PXContext())
                {
                    PlayerInjuryData injuryData = await px.PlayerInjuryData.FindAsync(player.PlayerInjuryData.Id);
                    injuryData.InjuryTimeLeft = injury.Duration;
                    injuryData.InjuryId = injury.Id;
                    injuryData.Injury = injury;

                    await px.SaveChangesAsync();

                    player.PlayerInjuryData = injuryData;
                }

                _teamController.SendNotification((int)TeamEnumeration.LSMC, "LSMC", "Es wurde eine verletzte Person gemeldet!", NotificationTypes.SUCCESS);

                await player.ApplyInjury();
            }
            else
            {
                //Injury not found in database 
                player.SendNotification("Verletzung", "Deine Verletzung ist nicht ausschlaggebend, du stehst nun wieder.", NotificationTypes.SUCCESS);
                await player.Revive();
            }
        }

        public async Task OnEveryMinute()
        {
            foreach (PXPlayer player in Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).Where(p => p.InjuryTimeLeft >= 1))
            {
                if (player.PlayerInjuryData == null) continue;

                if (player.InjuryTimeLeft > 1)
                {
                    player.InjuryTimeLeft--;

                    if (player.InjuryTimeLeft % 5 == 0)
                    {
                        await using var px = new PXContext();
                        var injuryData = await px.PlayerInjuryData.FindAsync(player.PlayerInjuryData.Id);
                        if (injuryData == null) continue;

                        injuryData.InjuryTimeLeft = player.InjuryTimeLeft;
                        player.PlayerInjuryData.InjuryTimeLeft = player.InjuryTimeLeft;

                        await px.SaveChangesAsync();
                    }

                    await player.PlayAnimation(player.PlayerInjuryData.Injury.AnimationDictionary, player.PlayerInjuryData.Injury.AnimationName);
                }
                else
                {
                    //REVIVE PLAYER
                    await FinishedPlayerDeath(player);
                    player.InjuryTimeLeft = 0;
                }
            }
        }

        public async Task FinishedPlayerDeath(PXPlayer player)
        {
            await player.Revive(false, false, false);

            player.SendNotification("Verletzung", $"Der Notfallmediziner hat deine Verletzung geheilt.", NotificationTypes.SUCCESS);
        }

        [Command("revive")]
        public async void CommandRevive(PXPlayer player, string targetString)
        {
            if (!player.IsValid()) return;
            if (!PermissionsModule.Instance.HasPermissions(player)) return;

            PXPlayer target = Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).FirstOrDefault(p => p.Username.ToLower().Contains(targetString.ToLower()));
            if (target == null)
            {
                player.SendNotification("Revive", "Person nicht gefunden!", NotificationTypes.ERROR);
                return;
            }

            if (target == null || !target.IsValid()) return;
            await target.Revive(true, true, true);
        }
    }
}
