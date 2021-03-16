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

        void OnModuleLoad();
        Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key);
        void OnPlayerDeath(PXPlayer player, PXPlayer killer, uint reason);
        void OnPlayerConnect(PXPlayer player);
        void OnPlayerDisconnect(PXPlayer player);
        void OnPlayerLogin(PXPlayer player);
    }
}
