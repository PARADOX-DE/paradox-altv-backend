using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Phone.Interfaces
{
    interface IPhoneApplication
    {
        public string ApplicationName { get; }
        public Task<bool> IsPermitted(PXPlayer player);
    }
}
