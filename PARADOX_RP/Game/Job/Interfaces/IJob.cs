using AltV.Net.Data;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Job.Interfaces
{
    public interface IJob
    {
        string Name { get; }
        Position Position { get; }
        bool HasJobRequirement(PXPlayer player);
        void StartJob(PXPlayer player);
        void EndJob(PXPlayer player);
    }
}
