using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ExtensionMethods
{
    public static class Enumeration
    {
        public static bool In(this Enum e, params Enum[] elements) 
        { 
            foreach (var elem in elements)
            {
                if (e.Equals(elem))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
