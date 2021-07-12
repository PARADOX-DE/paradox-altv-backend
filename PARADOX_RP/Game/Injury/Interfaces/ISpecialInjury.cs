using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Injury.Interfaces
{
    public interface ISpecialInjury
    {
        Task<bool> HasSpecialInjury(PXPlayer player);
        Task ApplyInjury(PXPlayer player);
    }
}
