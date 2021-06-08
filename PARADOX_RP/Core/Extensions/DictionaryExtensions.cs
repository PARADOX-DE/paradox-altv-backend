using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Extensions
{
    internal static class DictionaryExtensions
    {
        internal static void ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
                                      TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }
    }
}
