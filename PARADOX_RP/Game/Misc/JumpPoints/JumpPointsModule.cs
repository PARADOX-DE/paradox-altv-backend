using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using EntityStreamer;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Garage.Interface;
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

namespace PARADOX_RP.Game.JumpPoints
{
    class JumpPointsModule : ModuleBase<JumpPointsModule>, IEventKeyPressed, IEventModuleLoad
    {
        private Dictionary<int, Jumppoints> _JumpPoints = new Dictionary<int, Jumppoints>();

        private readonly IEventController _eventController;

        public JumpPointsModule(PXContext px, IEventController eventController) : base("JumpPoints")
        {
            _eventController = eventController;

            LoadDatabaseTable(px.JumpPoints, (Jumppoints jp) =>
            {
                _JumpPoints.Add(jp.Id, jp);
            });
        }

        public void OnModuleLoad()
        {
            if (Configuration.Instance.DevMode)
                _JumpPoints.ForEach((jp) =>
                {
                    MarkerStreamer.Create(MarkerTypes.MarkerTypeDallorSign, Vector3.Add(jp.Value.Position, new Vector3(0, 0, 1)), new Vector3(1, 1, 1), null, null, new Rgba(242, 137, 24, 255));
                    MarkerStreamer.Create(MarkerTypes.MarkerTypeDallorSign, Vector3.Add(jp.Value.EndPosition, new Vector3(0, 0, 1)), new Vector3(1, 1, 1), null, null, new Rgba(242, 137, 24, 255));
                });
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E && key != KeyEnumeration.L) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);
            if (!player.CanInteract()) return await Task.FromResult(false);

            Jumppoints dbJumpPoint = _JumpPoints.Values.FirstOrDefault(jp => jp.Position.Distance(player.Position) < 5 || jp.EndPosition.Distance(player.Position) < 5 && jp.Locked == false);
            if (dbJumpPoint == null) return await Task.FromResult(false);

            if (!dbJumpPoint.Vehicle && dbJumpPoint.Position.Distance(player.Position) > 2) return await Task.FromResult(false);

            if (key == KeyEnumeration.E)
            {
                if (dbJumpPoint.Position.Distance(player.Position) < 5)
                {
                    await EnterJumpPoint(player, dbJumpPoint);
                }
                else
                {
                    await ExitJumpPoint(player, dbJumpPoint);
                }
            }
            else if (key == KeyEnumeration.L) LockJumpPoint(player, dbJumpPoint);

            return await Task.FromResult(true);
        }

        public async Task EnterJumpPoint(PXPlayer player, Jumppoints jp)
        {
            if (jp.Locked) return;

            if (await player.IsInVehicleAsync() && jp.Vehicle == true)
            {
                player.Vehicle.Dimension = jp.EndDimension;

                player.Vehicle.Position = jp.EndPosition;
                player.Vehicle.Rotation = jp.EndRotation;
            }
            else if (!await player.IsInVehicleAsync())
            {
                player.Dimension = jp.EndDimension;
                player.Position = jp.EndPosition;
                player.Rotation = jp.EndRotation;
            }
        }

        public async Task ExitJumpPoint(PXPlayer player, Jumppoints jp)
        {
            if (jp.Locked) return;

            if (await player.IsInVehicleAsync() && jp.Vehicle == true)
            {
                player.Vehicle.Dimension = jp.Dimension;

                player.Vehicle.Position = jp.Position;
                player.Vehicle.Rotation = jp.Rotation;
            }
            else if (!await player.IsInVehicleAsync())
            {
                player.Dimension = jp.Dimension;
                player.Position = jp.Position;
                player.Rotation = jp.Rotation;
            }
        }

        public void LockJumpPoint(PXPlayer player, Jumppoints jp)
        {
            if (player.Team.Id == jp.TeamId && jp.LastBreaked.AddMinutes(30) < DateTime.Now)
            {
                jp.Locked = !jp.Locked;
                player.SendNotification(jp.Name, jp.Locked ? "Du hast die Tür Abgeschlossen." : "Du hast die Tür Aufgeschlossen.", jp.Locked ? NotificationTypes.ERROR : NotificationTypes.SUCCESS);
            }
        }

        public void BreakJumpPoint(PXPlayer player, Jumppoints jp)
        {
            //Aufbrechen
        }
    }
}
