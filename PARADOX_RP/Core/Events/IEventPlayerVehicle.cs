using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Events
{
    interface IEventPlayerVehicle
    {
        bool Enabled { get; }
        Task OnPlayerEnterVehicle(IVehicle vehicle, IPlayer player, byte seat);
        Task OnPlayerLeaveVehicle(IVehicle vehicle, IPlayer player, byte seat);
    }
}
