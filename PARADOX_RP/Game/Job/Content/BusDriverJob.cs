using AltV.Net.Data;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Job.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Job.Content
{
    public enum BusDriverPositionTypes
    {
        START,
        STOP,
        BUS_STOP
    }

    public sealed class BusDriverJob : IJob
    {
        public string Name => "BusDriver";
        public Position Position => new Position(0, 0, 0);


        private Dictionary<BusDriverPositionTypes, Position> positions = new Dictionary<BusDriverPositionTypes, Position>();
        
        public BusDriverJob()
        {

        }

        public bool HasJobRequirement(PXPlayer player)
        {
            if (!player.IsValid()) return false;
            if (!player.CanInteract()) return false;

            return true;
        }

        public void StartJob(PXPlayer player)
        {

        }

        public void EndJob(PXPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}
