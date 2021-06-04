using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Events
{
    interface IEventPlayerDisconnect
    {
        bool Enabled { get; }
        void OnPlayerDisconnect(PXPlayer player);
    }
}
