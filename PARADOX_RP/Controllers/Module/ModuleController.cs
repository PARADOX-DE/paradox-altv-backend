using AltV.Net;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using PARADOX_RP.Core.Extensions;
using AltV.Net.Async;
using PARADOX_RP.Core.Factories;
using System.Threading.Tasks;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Game.Misc.Progressbar.Extensions;
using PARADOX_RP.Utils;
using PARADOX_RP.Controllers.Interval.Interface;
using PARADOX_RP.Controllers.Login.Interface;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Utils.Enums;
using PARADOX_RP.Core.Events;

namespace PARADOX_RP.Controllers
{
    class ModuleController : IModuleController
    {
        private readonly IEnumerable<IModuleBase> _modules;

        private readonly IEnumerable<IEventKeyPressed> _keyPressedEvents;
        private readonly IEnumerable<IEventModuleLoad> _moduleLoadEvents;
        private readonly IEnumerable<IEventPlayerDeath> _playerDeathEvents;

        public ModuleController(IEnumerable<IModuleBase> modules,
        /*
         * INSERT EVENT ENUMERABLES HERE
         */

        IEnumerable<IEventKeyPressed> keyPressedEvents,
        IEnumerable<IEventModuleLoad> moduleLoadEvents,
        IEnumerable<IEventPlayerDeath> playerDeathEvents,

        IEventController eventController, ILoginController loginController, IIntervalController intervalController)
        {
            _modules = modules;

            _keyPressedEvents = keyPressedEvents;
            _moduleLoadEvents = moduleLoadEvents;
            _playerDeathEvents = playerDeathEvents;
            
            eventController.OnClient<PXPlayer>("Pressed_I", PressedI);
            eventController.OnClient<PXPlayer>("Pressed_Y", PressedY);
            eventController.OnClient<PXPlayer>("Pressed_E", PressedE);
            eventController.OnClient<PXPlayer>("Pressed_F9", PressedF9);
            eventController.OnClient<PXPlayer>("PlayerReady", OnPlayerConnect);

            AltAsync.OnPlayerDead += OnPlayerDead;
            AltAsync.OnPlayerConnect += PlayerConnectDebug;
            AltAsync.OnPlayerDisconnect += OnPlayerDisconnect;
            AltAsync.OnColShape += OnColShape;
            AltAsync.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
            AltAsync.OnPlayerLeaveVehicle += OnPlayerLeaveVehicle;

            //TODO: add module timer controller
            intervalController.SetInterval(1000 * 60, async (s, e) =>
            {
                await loginController.SavePlayers();

                await _modules.ForEach(async e =>
                {
                    if (e.Enabled)
                        await e.OnEveryMinute();
                });
            });
        }

        private Task PlayerConnectDebug(IPlayer player, string reason)
        {
            Alt.LogFast("=================");
            Alt.LogFast($"LOGIN: {player.Name}");
            Alt.LogFast($"SCID: {player.SocialClubId}");
            Alt.LogFast($"HWID: {player.HardwareIdHash}");
            Alt.LogFast($"HWID2: {player.HardwareIdExHash}");
            Alt.LogFast("=================");

            return Task.FromResult(true);
        }

        private async Task OnPlayerDead(IPlayer client, IEntity killerEntity, uint weapon)
        {
            PXPlayer player = (PXPlayer)client;
            PXPlayer killer = null;

            DeathReasons deathReason = killerEntity switch
            {
                IPlayer attackerPlayer when attackerPlayer == client => DeathReasons.SELF,
                IPlayer killerPlayer => DeathReasons.PLAYER,
                IVehicle vehicle => DeathReasons.VEHICLE,
                _ => DeathReasons.WORLD
            };

            await _playerDeathEvents.ForEach(e =>
             {
                 if (e.Enabled)
                     e.OnPlayerDeath(player, killer, deathReason, weapon);
             });
        }

        public void Load()
        {
            _moduleLoadEvents.ForEach(e =>
            {
                if (e.Enabled)
                    e.OnModuleLoad();
            });
        }

        private async void PressedE(PXPlayer player)
        {
            if (!player.LoggedIn) return;
            if (player.CancelProgressBar()) return;

            await _keyPressedEvents.ForEach(async e =>
            {
                if (e.Enabled)
                    if (await e.OnKeyPress(player, KeyEnumeration.E)) return false;

                return true;
            });
        }

        private async void PressedI(PXPlayer player)
        {
            if (!player.LoggedIn) return;
            if (player.CancelProgressBar()) return;

            await _keyPressedEvents.ForEach(async e =>
            {
                if (e.Enabled)
                    if (await e.OnKeyPress(player, Utils.Enums.KeyEnumeration.I)) return false;

                return true;
            });
        }

        private async void PressedY(PXPlayer player)
        {
            if (!player.LoggedIn) return;

            await _keyPressedEvents.ForEach(async e =>
            {
                if (e.Enabled)
                    if (await e.OnKeyPress(player, Utils.Enums.KeyEnumeration.Y)) return false;

                return true;
            });
        }

        private async void PressedF9(PXPlayer player)
        {
            if (!player.LoggedIn) return;

            await _keyPressedEvents.ForEach(async e =>
            {
                if (e.Enabled)
                    if (await e.OnKeyPress(player, Utils.Enums.KeyEnumeration.F9)) return false;

                return true;
            });
        }

        private async void OnPlayerConnect(PXPlayer pxPlayer)
        {
            await _modules.ForEach(e =>
            {
                if (e.Enabled)
                    e.OnPlayerConnect(pxPlayer);
            });
        }

        private async Task OnPlayerDisconnect(IPlayer player, string reason)
        {
            PXPlayer pxPlayer = (PXPlayer)player;

            if (pxPlayer.LoggedIn)
                Pools.Instance.Remove(pxPlayer.SqlId, pxPlayer);

            await _modules.ForEach(e =>
            {
                if (e.Enabled)
                    e.OnPlayerDisconnect(pxPlayer);
            });

            pxPlayer.LoggedIn = false;
        }

        private async Task OnColShape(IColShape colShape, IEntity targetEntity, bool state)
        {

            PXPlayer pxPlayer = (PXPlayer)targetEntity;
            await _modules.ForEach(e =>
            {
                if (e.Enabled && state)
                    e.OnColShapeEntered(pxPlayer, colShape);
            });
        }


        private async Task OnPlayerLeaveVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            await player.EmitAsync("playerLeaveVehicle", vehicle, seat);

            await _modules.ForEach(async e =>
            {
                if (e.Enabled)
                    await e.OnPlayerLeaveVehicle(vehicle, player, seat);
            });
        }

        private async Task OnPlayerEnterVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            await player.EmitAsync("playerEnterVehicle", vehicle, seat);

            await _modules.ForEach(async e =>
            {
                if (e.Enabled)
                    await e.OnPlayerEnterVehicle(vehicle, player, seat);
            });
        }
    }
}
