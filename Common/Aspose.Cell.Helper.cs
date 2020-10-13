using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commonlib.Utilities
{
    class CellHelper
    {
        public  static void FormulaTransform(Aspose.Cells.Cell src, Aspose.Cells.Cell dest)
        {
            src.R1C1Formula= "IF(F6>$C$6,\"NG\",\"OK\")";
            var for1 = src.R1C1Formula;
            dest.R1C1Formula = for1;
        }
    }
}
