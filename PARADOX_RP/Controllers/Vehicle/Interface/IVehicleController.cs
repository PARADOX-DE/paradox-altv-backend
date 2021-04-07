using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Vehicle.Interface
{
    interface IVehicleController
    {
        Task CreateVehicle(Vehicles dbVehicle);
    }
}
