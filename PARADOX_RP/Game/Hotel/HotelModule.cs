using AltV.Net.Data;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Hotel
{
    class HotelModule : ModuleBase<HotelModule>, IEventModuleLoad, IEventPlayerConnect
    {
        public readonly string _hotelName = "Crastenburg Hotel";
        public readonly Position _blipPosition = new Position(-1237.5428f, -189.53406f, 41.61389f);

        public HotelModule() : base("Hotel")
        {

        }

        public void OnModuleLoad()
        {

        }

        public void OnPlayerConnect(PXPlayer player)
        {
            /* CREATE HOTEL BLIP */
            player.AddBlips(_hotelName, _blipPosition, 475, 56, 1, true);
        }
    }
}
