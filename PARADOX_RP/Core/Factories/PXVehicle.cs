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
        public int OwnerId { get; set; }
        public bool HasRadio { get; set; }

        internal PXVehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            SqlId = -1;
            HasRadio = false;
        }

        public bool IsValid()
        {
            return SqlId > 0;
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
