using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static Task ForEach<T>(this IEnumerable<T> elements, Action<T> action)
        {
            foreach (var element in elements)
            {
                action(element);
            }

            return Task.CompletedTask;
        }

        public static async Task ForEach<T>(this IEnumerable<T> sequence, Func<T, Task<bool>> action)
        {
            foreach (T obj in sequence)
            {
                if (!await action(obj))
                    break;
            }
        }
    }
}
