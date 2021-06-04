using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Module
{
    interface IModuleBase
    {
        bool Enabled { get; set; }
        string ModuleName { get; set; }

        void OnPlayerConnect(PXPlayer player);
        void OnPlayerDisconnect(PXPlayer player);
        void OnPlayerLogin(PXPlayer player);

        Task OnEveryMinute();

        Task<bool> OnColShapeEntered(PXPlayer player, IColShape col);
        Task OnPlayerEnterVehicle(IVehicle vehicle, IPlayer player, byte seat);
        Task OnPlayerLeaveVehicle(IVehicle vehicle, IPlayer player, byte seat);
    }
}
