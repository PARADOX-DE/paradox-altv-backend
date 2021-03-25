using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Factories
{
    public class PXVehicle : Vehicle
    {
        public int SqlId { get; set; }

        internal PXVehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            SqlId = -1;
        }
    }

    internal class PXVehicleFactory : IEntityFactory<IVehicle>
    {
        public IVehicle Create(IntPtr entityPointer, ushort id)
        {
            return new PXVehicle(entityPointer, id);
        }
    }
}
