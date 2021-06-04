using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Events
{
    interface IEventPlayerLogin
    {
        bool Enabled { get; }
        void OnPlayerLogin(PXPlayer player);
    }
}
