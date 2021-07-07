using AltV.Net.Async;
using AltV.Net.Enums;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Interaction
{
    public sealed class VehicleInteractionModule : Module<VehicleInteractionModule>
    {
        private readonly IEventController _eventController;

        public VehicleInteractionModule(IEventController eventController) : base("VehicleInteraction")
        {
            _eventController = eventController;

            _eventController.OnClient<PXPlayer, PXVehicle>("LockVehicle", LockVehicle);
            _eventController.OnClient<PXPlayer, PXVehicle>("StartVehicleEngine", StartVehicleEngine);
        }

        public void StartVehicleEngine(PXPlayer player, PXVehicle target)
        {
            if (!player.CanInteract() || !player.IsValid()) return;
            if (target == null) return;

            if (target.OwnerId != player.SqlId) return;
            target.Engine = !target.Engine;

            player.SendNotification("Fahrzeug", target.Engine ? "Motor gestartet." : "Motor gestoppt.", NotificationTypes.SUCCESS);
        }

        public void LockVehicle(PXPlayer player, PXVehicle target)
        {
            if (!player.CanInteract() || !player.IsValid()) return;
            if (target == null) return;

            //TODO: add CanControl or smth like that
            if (target.OwnerId != player.SqlId) return;
            target.Locked = !target.Locked;

            if (target.Locked) player.SendNotification("Fahrzeug", "Fahrzeug zugeschlossen.", NotificationTypes.SUCCESS);
            else player.SendNotification("Fahrzeug", "Fahrzeug aufgeschlossen.", NotificationTypes.SUCCESS);
        }
    }
}
