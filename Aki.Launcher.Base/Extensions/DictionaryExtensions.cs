/* DictionaryExtension.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */


using System.Collections.Generic;
using System.Linq;

namespace Aki.Launcher.Extensions
{
    public static class DictionaryExtensions
    {
        public static TKey GetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> Dic, TValue value)
        {
            List<TKey> Keys = Dic.Keys.ToList();

            for (int x = 0; x < Keys.Count(); x++)
            {
                TValue tempValue;

                if (Dic.TryGetValue(Keys[x], out tempValue))
                {
                    if (tempValue != null && tempValue.Equals(value))
                    {
                        return Keys[x];
                    }
                }
            }

            return default;
        }
    }
}
