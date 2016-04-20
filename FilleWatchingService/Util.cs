using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilleWatchingService
{
    class Util
    {
        //Remove the dot(.) from the extension
        internal static string GetSimpleExtension(string fileExtension)
        {
            return fileExtension.Replace(".", "");
        }

    }
}
