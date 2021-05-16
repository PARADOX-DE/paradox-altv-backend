using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Extensions
{
    internal static class EnumerationExtensions
    {
        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }
    }
}
