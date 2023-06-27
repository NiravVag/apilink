using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BI.Utilities
{
   public static class FileParser
    {
        public static byte[] ReadFiletoByteArray(string filename)
        {
            if (!File.Exists(filename))
                return null;
            try
            {
                return File.ReadAllBytes(filename);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
