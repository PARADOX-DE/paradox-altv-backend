using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Events
{
    interface IEventKeyPressed
    {
        Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key);
    }
}
