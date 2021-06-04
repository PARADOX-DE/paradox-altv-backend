using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Events
{
    interface IEventColshape
    {
        bool Enabled { get; }
        Task<bool> OnColShapeEntered(PXPlayer player, IColShape col);
    }
}
