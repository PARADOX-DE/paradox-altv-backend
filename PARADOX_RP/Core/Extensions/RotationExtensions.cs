using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Extensions
{
    internal static class RotationExtensions
    {
        internal static float YawToDegree(this float Yaw)
        {
            return (float)(Yaw * 180 / Math.PI);
        } 
    }
}