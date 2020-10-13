using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.File
{
    public struct RCPosition
    {
        public int Row;
        public int Col;
        public Cell cell;

        public RCPosition(int row, int col) : this()
        {
            Row = row;
            Col = col;
        }
    }

    public class XlsxData
    {
        public RCPosition R1;
        public RCPosition R2;
        public RCPosition posR1LastDate;
        public RCPosition posR2LastDate;
        public DateTime R1LastDate;
        public DateTime R2LastDate;
        public bool NoR1Date = false;
        public bool NoR2Date = false;

        public RCPosition R1FirstNumberCell;
        public RCPosition R2FirstNumberCell;

        public List<RCPosition> listR1穴号 = new List<RCPosition>();
        public List<RCPosition> listR2穴号 = new List<RCPosition>();

        //public Dictionary<string , RCPosition> Cell
        public string ExcelFilePath = "";

        public List<DateTime> R1listAllDate = new List<DateTime>(10);
        public List<DateTime> R2listAllDate = new List<DateTime>(10);

        public bool R1DateisWrong = false;
        public bool R2DateisWrong = false;
    }
}
