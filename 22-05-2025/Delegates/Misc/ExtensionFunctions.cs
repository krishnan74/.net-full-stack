using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Misc
{
    public static class ExtensionFunctions
    {
        public static bool StringValidationCheck(this string str)
        {
            if (str.Substring(0, 1).ToLower() == "s" && str.Length == 6)
                return true;
            return false;
        }
    }
}