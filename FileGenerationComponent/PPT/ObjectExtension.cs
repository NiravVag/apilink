using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileGenerationComponent.PPT
{
    public static class ObjectExtension
    {
       public static string GetString(this object s, bool isPPt)
        {
            float output;
            if (isPPt && !s.ToString().Contains(" ") && float.TryParse(s.ToString(), out output) && output > 0)
                return string.Format(CultureInfo.InvariantCulture, "{0:#,#}", output);

            if (s is DateTime)
                return ((DateTime)s).ToString("MM/dd/yyyy");

            return System.Security.SecurityElement.Escape(s.ToString()); 
            
        }
    }
}
