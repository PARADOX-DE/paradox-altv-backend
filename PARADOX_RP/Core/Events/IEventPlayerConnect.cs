using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Events
{
    interface IEventPlayerConnect
    {
        bool Enabled { get; }
        void OnPlayerConnect(PXPlayer player);
        void OnPlayerLogin(PXPlayer player);
        void OnPlayerDisconnect(PXPlayer player);
    }
}
