﻿using AltV.Net.Async;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Team.Interface;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Injury.Extensions;
using PARADOX_RP.Game.Misc.Position;
using PARADOX_RP.Game.Team;
using PARADOX_RP.Utils;
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

    public class InjuryModule : ModuleBase<InjuryModule>
    {
        private IEventController _eventController;
        private ITeamController _teamController;

        //TODO: add dbInjury Model
        private readonly Dictionary<uint, Injuries> _injuries;

        public InjuryModule(IEventController eventController, ITeamController teamController) : base("Injury")
        {
            _eventController = eventController;
            _teamController = teamController;

            _injuries = new Dictionary<uint, Injuries>();
        }


        public override async void OnPlayerDeath(PXPlayer player, PXPlayer killer, uint weapon)
        {
            //InjuryModule only for injuries in dimension 0
            if (player.Dimension != 0) return;

            if (_injuries.TryGetValue(weapon, out Injuries injury))
            {
                await player.SpawnAsync(player.Position);
                player.Injured = true;

                await player.StartEffect(injury.EffectName, injury.Duration);

                _teamController.SendNotification((int)TeamEnumeration.LSMC, "LSMC", "Es wurde eine verletzte Person gemeldet!", NotificationTypes.SUCCESS);
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
                if (player.InjuryTimeLeft > 1)
                    player.InjuryTimeLeft--;
                else
                {
                    //REVIVE PLAYER
                    await FinishedPlayerDeath(player);
                    player.InjuryTimeLeft = 0;
                }

                if (Configuration.Instance.DevMode)
                    player.SendNotification("InjuryModule", $"Minute-Tick | InjuryTimeLeft: {player.InjuryTimeLeft}", NotificationTypes.SUCCESS);
            }
        }

        public async Task FinishedPlayerDeath(PXPlayer player)
        {
            await player.SpawnAsync(PositionModule.Instance.Get(Positions.MEDICAL_DEPARTMENT));
            await player.Revive(false, false, false);
            player.SendNotification("Verletzung", $"Der Notfallmediziner hat deine Verletzung geheilt.", NotificationTypes.SUCCESS);
        }
    }
}
