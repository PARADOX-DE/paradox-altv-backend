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

namespace PARADOX_RP.Handlers
{
    class EventHandler : IEventHandler
    {
        private readonly IEnumerable<IModuleBase> _modules;
        public EventHandler(IEnumerable<IModuleBase> modules)
        {
            _modules = modules;

            AltAsync.OnClient<PXPlayer>("Pressed_E", PressedE);
            AltAsync.OnClient<PXPlayer>("Pressed_F9", PressedF9);
            AltAsync.OnPlayerConnect += OnPlayerConnect;
            AltAsync.OnPlayerDisconnect += OnPlayerDisconnect;
            AltAsync.OnColShape += OnColShape;
            AltAsync.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
            AltAsync.OnPlayerLeaveVehicle += OnPlayerLeaveVehicle;
        }


        public void Load()
        {
            _modules.ForEach(e =>
            {
                if (e.Enabled)
                    e.OnModuleLoad();
            });
        }

        private async void PressedE(PXPlayer player)
        {
            if (!player.LoggedIn) return;
            if (player.CancelProgressBar()) return;

            await _modules.ForEach(async e =>
            {
                if (e.Enabled)
                    if (await e.OnKeyPress(player, Utils.Enums.KeyEnumeration.E)) return;
            });
        }

        private async void PressedF9(PXPlayer player)
        {
            if (!player.LoggedIn) return;

            await _modules.ForEach(async e =>
            {
                if (e.Enabled)
                    if (await e.OnKeyPress(player, Utils.Enums.KeyEnumeration.F9)) return;
            });
        }

        private async Task OnPlayerConnect(IPlayer player, string reason)
        {
            PXPlayer pxPlayer = (PXPlayer)player;
            await _modules.ForEach(e =>
            {
                if (e.Enabled)
                    e.OnPlayerConnect(pxPlayer);
            });
        }

        private async Task OnPlayerDisconnect(IPlayer player, string reason)
        {
            PXPlayer pxPlayer = (PXPlayer)player;

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
            await _modules.ForEach(async e =>
            {
                if (e.Enabled) 
                    if (await e.OnPlayerLeaveVehicle(vehicle, player, seat)) return;
            });
        }

        private async Task OnPlayerEnterVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            await _modules.ForEach(async e =>
            {
                if (e.Enabled)
                    if (await e.OnPlayerEnterVehicle(vehicle, player, seat)) return;
            });
        }
    }
}
