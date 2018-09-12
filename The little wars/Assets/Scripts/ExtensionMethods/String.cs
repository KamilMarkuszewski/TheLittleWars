using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool In(this string str, params string[] elements)
        {
            return elements.Any(e => e.Equals(str));
        }

    }
}
