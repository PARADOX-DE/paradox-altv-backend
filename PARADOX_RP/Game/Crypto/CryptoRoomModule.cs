using AltV.Net.Async;
using AltV.Net.Data;
using EntityStreamer;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Inventory;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Crypto.Extensions;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Game.Inventory.Models;
using PARADOX_RP.Game.Misc.Position;
using PARADOX_RP.Utils.Enums;
using PARADOX_RP.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Crypto
{
    public class CryptoRoomModule : ModuleBase<CryptoRoomModule>, IEventModuleLoad, IEventKeyPressed, IInventoriable
    {
        private readonly PXContext _pxContext;
        private readonly ILogger _logger;
        private readonly IEventController _eventController;
        private readonly PositionModule _positionModule;
        private readonly IInventoryController _inventoryController;

        public Dictionary<int, CryptoRooms> _cryptoRooms = new Dictionary<int, CryptoRooms>();

        public CryptoRoomModule(PXContext pxContext, ILogger logger, IEventController eventController, PositionModule positionModule, IInventoryController inventoryController) : base("CryptoRoom")
        {
            _pxContext = pxContext;
            _logger = logger;
            _eventController = eventController;
            _positionModule = positionModule;
            _inventoryController = inventoryController;
        }

        public void OnModuleLoad()
        {
            // Load CryptoRooms
            LoadDatabaseTable(_pxContext.CryptoRooms, (CryptoRooms cryptoRoom) =>
            {
                _cryptoRooms.Add(cryptoRoom.Id, cryptoRoom);
                MarkerStreamer.Create(MarkerTypes.MarkerTypeNumber0, cryptoRoom.Position, new Vector3(1, 1, 1), null, null, new Rgba(37, 165, 202, 200));
            });
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E && key != KeyEnumeration.L) return false;
            if (!player.IsValid()) return false;
            if (!player.CanInteract()) return false;

            if (key == KeyEnumeration.L)
            {
                if (player.DimensionType == DimensionTypes.CRYPTOROOM)
                {
                    player.SendNotification(ModuleName, "Lock Test", NotificationTypes.SUCCESS);
                    return true;
                }
            }

            if (key == KeyEnumeration.E)
            {
                // TODO: enter and leave room
                var playerPos = Position.Zero; player.GetPositionLocked(ref playerPos);

                CryptoRooms cryptoRoom = _cryptoRooms.FirstOrDefault((c) => c.Value.Position.Distance(playerPos) < 3).Value;
                if (cryptoRoom == null) return false;

                await EnterCryptoRoom(player, cryptoRoom);
            }

            return false;
        }

        public async Task EnterCryptoRoom(PXPlayer player, CryptoRooms cryptoRoom)
        {
            if (cryptoRoom == null || cryptoRoom.Locked) return;

            player.Dimension = cryptoRoom.Id; // CryptoRoom Id
            player.DimensionType = DimensionTypes.CRYPTOROOM;
            player.LoadCryptoRoom(cryptoRoom.Servers);

            await player.SetPositionAsync(_positionModule.Get(Positions.CRYPTO_ROOM));
        }

        public Task<PXInventory> OnInventoryOpen(PXPlayer player, Position position)
        {
            throw new NotImplementedException();
        }

        public Task<bool?> CanAccessInventory(PXPlayer player, PXInventory inventory)
        {
            throw new NotImplementedException();
        }
    }
}
