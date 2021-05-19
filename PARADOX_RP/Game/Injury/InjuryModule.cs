using AltV.Net.Async;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Team.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Administration;
using PARADOX_RP.Game.Commands;
using PARADOX_RP.Game.Commands.Attributes;
using PARADOX_RP.Game.Injury.Extensions;
using PARADOX_RP.Game.Misc.Position;
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

    class InjuryModule : ModuleBase<InjuryModule>, ICommand
    {
        private IEventController _eventController;
        private ITeamController _teamController;

        //TODO: add dbInjury Model
        private readonly Dictionary<uint, Injuries> _injuries = new Dictionary<uint, Injuries>();

        public InjuryModule(PXContext pxContext, IEventController eventController, ITeamController teamController) : base("Injury")
        {
            _eventController = eventController;
            _teamController = teamController;

            LoadDatabaseTable<Injuries>(pxContext.Injuries, (injury) => _injuries.Add(injury.Weapon, injury));
        }

        public override async void OnPlayerDeath(PXPlayer player, PXPlayer killer, DeathReasons deathReason, uint weapon)
        {
            //InjuryModule only for injuries in dimension 0
            if (player.Dimension != 0) return;

            if (Configuration.Instance.DevMode) AltAsync.Log($"[DEATH] {player.Username} // REASON: {Enum.GetName(typeof(DeathReasons), deathReason)} // Weapon: {weapon}");
            if (!player.IsValid()) return;

            if (_injuries.TryGetValue(weapon, out Injuries injury))
            {
                await player.SpawnAsync(player.Position);
                await using (var px = new PXContext())
                {
                    PlayerInjuryData injuryData = await px.PlayerInjuryData.FindAsync(player.PlayerInjuryData.Id);
                    injuryData.InjuryTimeLeft = injury.Duration;
                    injuryData.InjuryId = injury.Id;

                    await px.SaveChangesAsync();
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

        public override async Task OnEveryMinute()
        {
            foreach (PXPlayer player in Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).Where(p => p.InjuryTimeLeft >= 1))
            {
                if (Configuration.Instance.DevMode)
                    AltAsync.Log($"Minute-Tick | InjuryTimeLeft: {player.InjuryTimeLeft}");

                if (player.PlayerInjuryData == null) continue;

                if (player.InjuryTimeLeft > 1)
                {
                    player.InjuryTimeLeft--;

                    if (player.InjuryTimeLeft % 5 == 0)
                    {
                        await using (var px = new PXContext())
                        {
                            var injuryData = await px.PlayerInjuryData.FindAsync(player.PlayerInjuryData.Id);
                            if (injuryData == null) continue;

                            injuryData.InjuryTimeLeft = player.InjuryTimeLeft;
                            player.PlayerInjuryData.InjuryTimeLeft = player.InjuryTimeLeft;

                            await px.SaveChangesAsync();
                        }
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
            AltAsync.Log("command");
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
