using Aspose.Cells;
using Model.File;
using pdfExtrator;
using pdfExtrator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class BLL_DateUpdate
    {

        public bool R1DateUpdate=false;
        public bool R2DateUpdate=false;

        public bool R1DateDayUpdate = false;
        public bool R1DateNightUpdate = false;

        public bool R2DateDayUpdate = false;
        public bool R2DateNightUpdate = false;

        MainForm mfrm = null;

        public BLL_DateUpdate(MainForm mfrm)
        {
            this.mfrm = mfrm;
        }

        public void updateR2DateCell(DayOrNight dn, int colOffset)
        {
            Aspose.Cells.CellsFactory cellsFactory = new Aspose.Cells.CellsFactory();
            var DateStyle = cellsFactory.CreateStyle();
            DateStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            //DateStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Dotted;
            //DateStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Dotted;
            //DateStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //DateStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Dotted;

            //var TimeStyle = cellsFactory.CreateStyle();
            //TimeStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            //TimeStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Dotted;
            //TimeStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Dotted;
            //TimeStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //TimeStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Dotted;

            //string date1 = mfrm.infoMeasure.MeasuDateTime.ToString("yyyy年MM月dd日");
            DateTime date1 = mfrm.infoPdfMeasure.MeasuDateTime;
            string time1 = mfrm.infoPdfMeasure.MeasuDateTime.ToShortTimeString();

            if (mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 1, colOffset].Type == Aspose.Cells.CellValueType.IsNull)
            {
                //var str1 = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 1, colOffset].StringValue;

                mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 1, colOffset].PutValue(date1);
                mfrm.workbook.Worksheets[0].Cells.Merge(mfrm.XData.R2.Row + 1, colOffset, 1, 2);

                //mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 1, colofs].SetStyle(DateStyle);
                var style = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 1, colOffset].GetStyle(true);

                style.Custom = "yyyy\"年\"m\"月\"d\"日\"";

                mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 1, colOffset].SetStyle(style);

                mfrm.XData.R2LastDate = mfrm.infoPdfMeasure.RealDate;
                mfrm.XData.NoR2Date = false;
                mfrm.XData.posR2LastDate = new RCPosition(mfrm.XData.R2.Row + 1, colOffset);
            }

            if (dn == DayOrNight.Day)
            {
                var str2 = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 2, colOffset].StringValue;
                if (str2 == null || str2 == "")
                    mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 2, colOffset].PutValue(time1);
                else if (DateTime.Parse(str2) > DateTime.Parse(time1))
                    mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 2, colOffset].PutValue(time1);

                //水平居中
                //mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 2, colofs].SetStyle(TimeStyle);
            }
            else if (dn == DayOrNight.Night)
            {
                //右下角, 因此+1

                var str2 = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 2, colOffset + 1].StringValue;
                //把最迟的晚班写进去
                if (str2 == null || str2 == "")
                    mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 2, colOffset + 1].PutValue(time1);
                else if (DateTime.Parse(str2) < DateTime.Parse(time1))
                    mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 2, colOffset + 1].PutValue(time1);

                //水平居中
                //mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R2.Row + 2, colofs + 1].SetStyle(TimeStyle);
            }

        }

        public void updateR1DateCell(DayOrNight dn, int colofs)
        {
            Aspose.Cells.CellsFactory cellsFactory = new Aspose.Cells.CellsFactory();

            var Mystyle = cellsFactory.CreateStyle();
            Mystyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            //var Mystyle = cellsFactory.CreateStyle();
            //Mystyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Dotted;
            //Mystyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Dotted;
            //Mystyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //Mystyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Dotted;

            //Mystyle.SetBorder(Aspose.Cells.BorderType.LeftBorder| Aspose.Cells.BorderType.RightBorder| Aspose.Cells.BorderType.TopBorder|Aspose.Cells.BorderType.BottomBorder, Aspose.Cells.CellBorderType.Dotted, System.Drawing.Color.Black);
            var DefStyle = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 1, colofs].GetStyle();
            var 虚线类型 = DefStyle.Borders.DiagonalStyle;

            //string date1 = mfrm.infoMeasure.MeasuDateTime.ToString("yyyy年MM月dd日");
            DateTime date1 = mfrm.infoPdfMeasure.MeasuDateTime;

            string time1 = mfrm.infoPdfMeasure.MeasuDateTime.ToShortTimeString();
            var b1 = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 1, colofs].Type == Aspose.Cells.CellValueType.IsNull;
            if (b1)
            {
                mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 1, colofs].PutValue(date1);
                var style = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 1, colofs].GetStyle(true);

                style.Custom = "yyyy\"年\"m\"月\"d\"日\"";

                mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 1, colofs].SetStyle(style);

                //合并单元格
                mfrm.workbook.Worksheets[0].Cells.Merge(mfrm.XData.R1.Row + 1, colofs, 1, 2);

                //mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 1, colofs].SetStyle(Mystyle);
                mfrm.XData.R1LastDate = mfrm.infoPdfMeasure.RealDate;
                mfrm.XData.posR1LastDate = new RCPosition(mfrm.XData.R1.Row + 1, colofs);
                mfrm.XData.NoR1Date = false;
            }

            if (dn == DayOrNight.Day)
            {
                var str2 = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 2, colofs].StringValue;
                //把最早的早班写进去
                if (str2 == null || str2 == "")
                    mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 2, colofs].PutValue(time1);
                else if (DateTime.Parse(str2) > DateTime.Parse(time1))
                    mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 2, colofs].PutValue(time1);
                //mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 2, colofs].SetStyle(Mystyle);
            }
            else if (dn == DayOrNight.Night)
            {
                var str2 = mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 2, colofs + 1].StringValue;
                //把最迟的晚班写进去
                if (str2 == null || str2 == "")
                    mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 2, colofs + 1].PutValue(time1);
                else if (DateTime.Parse(str2) < DateTime.Parse(time1))
                    mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 2, colofs + 1].PutValue(time1);

                //mfrm.workbook.Worksheets[0].Cells[mfrm.XData.R1.Row + 2, colofs + 1].SetStyle(Mystyle);
            }
        }
    }
}
