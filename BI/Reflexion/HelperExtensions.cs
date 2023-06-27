using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Reflexion
{
    public static class HelperExtensions
    {
        public static T GetEnumValue<T>(this int prop) where T : struct, IConvertible
        {
            return (T)Convert.ChangeType(prop, typeof(T));
        }

        public static T GetEnumValue<T>(this int? prop) where T : struct, IConvertible
        {
            if (prop == null)
                prop = 0; 

            return (T)Convert.ChangeType(prop, typeof(T));
        }
    }
}
