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
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Game.Inventory.Models;
using PARADOX_RP.Game.Misc.Position;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
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

            _eventController.OnClient<PXPlayer, string>("SubmitCryptoRoomPIN", SubmitCryptoRoomPIN);
        }

        public void OnModuleLoad()
        {
            // Load CryptoRooms
            LoadDatabaseTable(_pxContext.CryptoRooms, async (CryptoRooms cryptoRoom) =>
            {
                _cryptoRooms.Add(cryptoRoom.Id, cryptoRoom);
                MarkerStreamer.Create(MarkerTypes.MarkerTypeNumber0, cryptoRoom.Position, new Vector3(1, 1, 1), null, null, new Rgba(37, 165, 202, 200));

                cryptoRoom.Inventory = await _inventoryController.LoadInventory(InventoryTypes.CRYPTOROOM, cryptoRoom.Id);
                if (cryptoRoom.Inventory == null) cryptoRoom.Inventory = await _inventoryController.CreateInventory(InventoryTypes.CRYPTOROOM, cryptoRoom.Id);
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

                if (player.DimensionType == DimensionTypes.CRYPTOROOM)

                    return false;

                else
                {
                    CryptoRooms cryptoRoom = _cryptoRooms.FirstOrDefault((c) => c.Value.Position.Distance(playerPos) < 3).Value;
                    if (cryptoRoom == null) return false;

                    WindowController.Instance.Get<InputBoxWindow>().Show(player, new InputBoxWindowWriter("Crypto-Room", "Um diesen Raum zu betreten, benötigst du einen 4-stelligen PIN.", "PIN", "Eintreten", "SubmitCryptoRoomPIN"));
                    return true;
                }
            }

            return false;
        }

        private async void SubmitCryptoRoomPIN(PXPlayer player, string password)
        {
            if (!player.CanInteract()) return;
            if (int.TryParse(password, out int pin))
            {
                var playerPos = Position.Zero; player.GetPositionLocked(ref playerPos);

                CryptoRooms cryptoRoom = _cryptoRooms.FirstOrDefault((c) => c.Value.Position.Distance(playerPos) < 3).Value;
                if (cryptoRoom == null) return;

                await EnterCryptoRoom(player, cryptoRoom);
            }
        }

        public async Task EnterCryptoRoom(PXPlayer player, CryptoRooms cryptoRoom)
        {
            if (cryptoRoom == null || cryptoRoom.Locked) return;
            var playerPos = Position.Zero; player.GetPositionLocked(ref playerPos);

            player.Dimension = cryptoRoom.Id; // CryptoRoom Id
            player.DimensionType = DimensionTypes.CRYPTOROOM;
            player.LastWorldPosition = playerPos;

            player.LoadCryptoRoom(cryptoRoom.Servers);

            await player.SetPositionAsync(_positionModule.Get(Positions.CRYPTO_ROOM));
        }

        public async Task LeaveCryptoRoom(PXPlayer player, CryptoRooms cryptoRoom)
        {
            if (cryptoRoom == null || cryptoRoom.Locked) return;
            
            player.Dimension = 0; // CryptoRoom Id
            player.DimensionType = DimensionTypes.WORLD;
            
            await player.SetPositionAsync(player.LastWorldPosition);
        }

        public Task<PXInventory> OnInventoryOpen(PXPlayer player, Position position)
        {
            if (player.DimensionType == DimensionTypes.CRYPTOROOM)
            {
                if (_cryptoRooms.TryGetValue(player.DimensionLocked, out CryptoRooms cryptoRoom))
                {
                    return Task.FromResult(cryptoRoom.Inventory);
                }
            }

            return Task.FromResult<PXInventory>(null);
        }

        public Task<bool?> CanAccessInventory(PXPlayer player, PXInventory inventory)
        {
            if (inventory.InventoryInfo.InventoryType != InventoryTypes.CRYPTOROOM) return Task.FromResult<bool?>(null);

            if (player.DimensionType == DimensionTypes.CRYPTOROOM)
            {
                CryptoRooms cryptoRoom = _cryptoRooms.FirstOrDefault((c) => c.Value.Inventory.Id == inventory.Id).Value;
                if (cryptoRoom != null)
                {
                    if (_cryptoRooms.TryGetValue(player.DimensionLocked, out CryptoRooms cryptoRooms))
                        return Task.FromResult<bool?>(true);

                    return Task.FromResult<bool?>(false);
                }
            }
            return Task.FromResult<bool?>(null);
        }
    }
}
