using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.File
{
    //为了美观,优雅的代码。
    class PdfFile
    {
      public static string pathPdfDataFolder = "";
      public static string pathPdfDataRootDay = "";
      public static string pathPdfDataRootNight = "";
      public static string pathOutPutExcel = "";
        public static string pathInputExcel = "";

        public static List<string> PdfFilesPath= new List<string>(16);
      public static List<string> PathsDay = new List<string>(16);
      public static List<string> PathsNight = new List<string>(16);
    }
}
