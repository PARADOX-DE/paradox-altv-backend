using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using EntityStreamer;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Garage.Interface;
using PARADOX_RP.Controllers.Team.Interface;
using PARADOX_RP.Controllers.Vehicle.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Vehicle;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.JumpPoint
{
    class JumpPointModule : ModuleBase<JumpPointModule>, IEventKeyPressed, IEventModuleLoad
    {
        private Dictionary<int, JumpPoints> _jumpPoints = new Dictionary<int, JumpPoints>();

        private readonly IEventController _eventController;
        private readonly ITeamController _teamController;

        public JumpPointModule(PXContext px, ITeamController teamController, IEventController eventController) : base("JumpPoint")
        {
            _eventController = eventController;
            _teamController = teamController;

            LoadDatabaseTable(px.JumpPoints.Include(p => p.Permissions).Include(d => d.Destination), (JumpPoints jumpPoint) =>
            {
                _jumpPoints.Add(jumpPoint.Id, jumpPoint);
            });
        }

        public void OnModuleLoad()
        {
            if (Configuration.Instance.DevMode)
                _jumpPoints.ForEach((jp) => MarkerStreamer.Create(MarkerTypes.MarkerTypePlaneModel, jp.Value.Position, new Vector3(1, 1, 1), null, null, new Rgba(242, 137, 24, 255)));
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E && key != KeyEnumeration.L) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);
            if (!player.CanInteract()) return await Task.FromResult(false);

            var dbJumpPoint = _jumpPoints.Values.FirstOrDefault(jp => jp.Position.Distance(player.Position) < jp.Range);
            if (dbJumpPoint == null) return await Task.FromResult(false);

            if (key == KeyEnumeration.E) await EnterJumpPoint(player, dbJumpPoint);
            if (key == KeyEnumeration.L) await ToggleJumpPointLock(player, dbJumpPoint);

            return true;
        }

        private async Task EnterJumpPoint(PXPlayer player, JumpPoints jumpPoint)
        {
            if (jumpPoint.Locked) return;
            if (jumpPoint.Destination == null) return;

            await AltAsync.Do(() =>
            {
                if (!player.Exists) return;

                if (player.IsInVehicle)
                {
                    if (jumpPoint.VehicleAccess)
                    {
                        PXVehicle vehicle = (PXVehicle)player.Vehicle;
                        if (vehicle == null || !vehicle.Exists) return;

                        vehicle.Position = jumpPoint.Destination.Position;
                        vehicle.Rotation = jumpPoint.Destination.Rotation;
                        vehicle.Dimension = jumpPoint.Destination.Dimension;
                    }
                    return;
                }
                else
                {
                    player.Position = jumpPoint.Destination.Position;
                    player.Rotation = jumpPoint.Destination.Rotation;
                    player.Dimension = jumpPoint.Destination.Dimension;
                }
            });
        }

        private async Task ToggleJumpPointLock(PXPlayer player, JumpPoints jumpPoint)
        {
            bool Accessible = false;

            if (jumpPoint.Permissions.Count > 0)
                foreach (var permission in jumpPoint.Permissions) { 
                    if (_teamController.CanAccess(player, permission.Team.Id)) Accessible = true;
                }
            else Accessible = true;
            
            if (Accessible)
            {
                if (jumpPoint.LastBreaked.AddMinutes(15) > DateTime.Now)
                {
                    player.SendNotification(jumpPoint.Name, "Dieser Zugangspunkt ist momentan beschädigt. Versuche es später erneut!", NotificationTypes.ERROR);
                    return;
                }

                jumpPoint.Locked = !jumpPoint.Locked;
                jumpPoint.Destination.Locked = jumpPoint.Locked; // Das Ende des Zugangspunkt soll dann den selben Status haben wie der Anfang.

                player.SendNotification(jumpPoint.Name, $"Zugangspunkt wurde {(jumpPoint.Locked ? "zugeschlossen" : "aufgeschlossen")}.", NotificationTypes.SUCCESS);
            }

            await Task.CompletedTask;
        }

        public void BreakJumpPoint(PXPlayer player, JumpPoints jumpPoint)
        {
            //Aufbrechen
        }
    }
}
