using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.UI.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Garage.Interface
{
    interface IGarageController
    {
        public Task<GarageWindowWriter> RequestGarageVehicles(PXPlayer player, Garages garage);
        public Task<GarageSpawns> GetFreeGarageSpawn(Garages garage);
    }
}
