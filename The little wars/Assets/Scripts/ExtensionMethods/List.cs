using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ExtensionMethods
{
    public static class ListExtensionMethods
    {
        private static readonly Random Rng = new Random();

        public static List<T> Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list.ToList();
        }

    }
}
