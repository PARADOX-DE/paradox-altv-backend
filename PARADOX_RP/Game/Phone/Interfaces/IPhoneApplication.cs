using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Phone.Interfaces
{
    public interface IPhoneApplication
    {
        string ApplicationName { get; }
        Task<bool> IsPermitted(PXPlayer player);
    }
}
