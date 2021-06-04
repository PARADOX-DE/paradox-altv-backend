using AltV.Net.Async;
using AltV.Net.Data;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Arrival.Extensions;
using PARADOX_RP.Game.Login;
using PARADOX_RP.Game.Login.Extensions;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Arrival
{
    class ArrivalModule : ModuleBase<ArrivalModule>, IEventPlayerConnect
    {

        public Dictionary<Tuple<Gender, ComponentVariation>, Clothes> _arrivalClothes = null;
        public ArrivalModule(PXContext pxContext) : base("Arrival")
        {
            _arrivalClothes = new Dictionary<Tuple<Gender, ComponentVariation>, Clothes>();

            pxContext.Clothes.Where(c => c.Name.StartsWith("Einreise")).ForEach((arrivalCloth) =>
            {
                _arrivalClothes.Add(new Tuple<Gender, ComponentVariation>((Gender)arrivalCloth.Gender, (ComponentVariation)arrivalCloth.Component), arrivalCloth);
            });
        }

        private Position ArrivalPosition = new Position(-1062.1978f, -2712.8044f, 0.78686523f);

        public void OnPlayerConnect(PXPlayer player) =>  player.AddBlips("Flughafen", ArrivalPosition, 90, 0, 1, true);

        public async Task NewPlayerArrival(PXPlayer player)
        {
            await player.PlayArrivalCutscene();
            player.SendNotification("PARADOX RP", $"Du bist hiermit offiziell ein Bürger im Staate Los Santos.", NotificationTypes.SUCCESS);

            //todo: cutscene length
            await Task.Delay(25 * 1000);

            if (await player?.ExistsAsync())
                await player?.PreparePlayer(ArrivalPosition);
        }

        public Clothes GetArrivalClothing(Gender gender, ComponentVariation componentVariation) => Instance._arrivalClothes.FirstOrDefault(c => c.Value.Gender == (int)gender && c.Value.Component == (int)componentVariation).Value;
    }
}
