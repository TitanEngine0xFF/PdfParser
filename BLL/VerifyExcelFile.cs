using Aspose.Cells;
using Model.File;
using pdfExtrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLL.ExcelFile
{
    class VerifyFile
    {
        public VerifyFile(MainForm frm1)
        {
            frm = frm1;
        }

        MainForm frm = null;

        bool R1DateTimeColFound = false;
        bool R2DateTimeColFound = false;
        bool R1FirstValueFound = false;
        bool R2FirstValueFound = false;

        public bool Analysis(Workbook wb, XlsxData xdata)
        {
            xdata.ExcelFilePath = wb.FileName;
            var ws = wb.Worksheets[0].Cells;
            bool R1Finish = false, R2Finish = false;
            for (int i = 0; i < ws.Rows.Count; i++)
            {
                if (ws[i, 0].StringValue.Contains("R1"))
                {
                    xdata.R1 = new RCPosition(i, 0);
                    R1Finish = true;
                }

                if (ws[i, 0].StringValue.Contains("R2"))
                {
                    xdata.R2 = new RCPosition(i, 0);
                    R2Finish = true;
                }

                if (R1Finish && R2Finish)
                    break;
            }

            if (!R1Finish || !R2Finish)
            {
                MessageBox.Show("您所选的电子表格文件, 格式不符合 预设格式。\r\n若要修改预设格式请联系 开发人员, 深圳易胜科技有限公司, EasySon \r\n调试信息: 找不到【R1或R2】单元格");
                return false;
            }


            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();


            int R1WorkerCounter = 0;
            int R2WorkerCounter = 0;
            
            int StepR1 = 3;
            int StepR2 = 3;
            for (int i = 0; i < ws.Rows.Count; i++)
            {
                //它遍历了整个 表格 65535列
                for (int j = 0; j < ws.Columns.Count; j++)
                {
                    //if (sw.ElapsedMilliseconds > 300)
                    //    goto End;

                    if (i == xdata.R1.Row + 1)
                    {
                        if (!R1DateTimeColFound)
                        {
                            if (j == 3)
                            {
                                if (ws[i, j].Type == CellValueType.IsNull)
                                {
                                    xdata.NoR1Date = true;
                                    //这是一个  没有填过日期 的表格。。。
                                    R1DateTimeColFound = true;
                                }
                                else if (ws[i, j].Type == CellValueType.IsString)
                                {
                                    if (ws[i, j].StringValue == "")
                                    {
                                        xdata.NoR1Date = true;
                                        //这是一个  没有填过日期 的表格。。。
                                        R1DateTimeColFound = true;
                                    }
                                    else
                                    {
                                        var strxx = ws[i, j].StringValue;
                                        DateTime dt = new DateTime();
                                        try
                                        {
                                            dt = DateTime.Parse(ws[i, j].StringValue);
                                        }
                                        catch (Exception)
                                        {

                                            System.Windows.Forms.MessageBox.Show("文件解析错误");
                                            return false;
                                        }
                                        //xdata.posR1LastDate = new RCPosition(i, j);
                                        //xdata.R1LastDate = ws[i, j].DateTimeValue;
                                        StepR1 += 2;
                                    }
                                }
                                else if (ws[i, j].Type == CellValueType.IsDateTime)
                                {
                                    if (ws[i, j].DateTimeValue == DateTime.MinValue)
                                    {
                                        xdata.NoR1Date = true;
                                        //这是一个  没有填过日期 的表格。。。
                                        R1DateTimeColFound = true;
                                        goto forEnd;
                                    }
                                    xdata.R1listAllDate.Add(ws[i, j].DateTimeValue);

                                    //xdata.posR1LastDate = new RCPosition(i, j);
                                    //xdata.R1LastDate = ws[i, j].DateTimeValue;
                                    StepR1 += 2;
                                }



                            }
                            else if (j == StepR1)
                            {
                                //这里已更新 ,
                                if (!IdentifyR1FirstColDate(xdata, ws, i, j))
                                {
                                    return false;
                                }

                                StepR1 += 2;
                            }
                        }
                    }

                    if (i == xdata.R2.Row + 1)
                        if (!R2DateTimeColFound)
                        {
                            if (j == 3)
                            {

                                if (ws[i, j].Type == CellValueType.IsNull)
                                {
                                    xdata.NoR2Date = true;
                                    //这是一个  没有填过日期 的表格。。。
                                    R2DateTimeColFound = true;
                                }
                                else if (ws[i, j].Type == CellValueType.IsString)
                                {

                                    if (ws[i, j].StringValue == "")
                                    {
                                        xdata.NoR1Date = true;
                                        //这是一个  没有填过日期 的表格。。。
                                        R1DateTimeColFound = true;
                                    }
                                    else
                                    {
                                        var strxx = ws[i, j].StringValue;
                                        DateTime dt = new DateTime();
                                        try
                                        {
                                            dt = DateTime.Parse(ws[i, j].StringValue);
                                        }
                                        catch (Exception)
                                        {

                                            System.Windows.Forms.MessageBox.Show("文件解析错误");
                                            return false;
                                        }
                                        //xdata.posR1LastDate = new RCPosition(i, j);
                                        //xdata.R1LastDate = ws[i, j].DateTimeValue;
                                        StepR2 += 2;
                                    }
                                }
                                else if (ws[i, j].Type == CellValueType.IsDateTime)
                                {
                                    if (ws[i, j].DateTimeValue ==DateTime.MinValue)
                                    {
                                        xdata.NoR2Date = true;
                                        //这是一个  没有填过日期 的表格。。。
                                        R2DateTimeColFound = true;
                                        goto forEnd;
                                    }

                                    xdata.R2listAllDate.Add(ws[i, j].DateTimeValue);
                                    //xdata.posR1LastDate = new RCPosition(i, j);
                                    //xdata.R1LastDate = ws[i, j].DateTimeValue;
                                    StepR2 += 2;
                                }
                            }
                            else if (j == StepR2)
                            {
                                //这里已更新 ,
                                if (!IdentifyR2FirstColDate(xdata, ws, i, j))
                                {
                                    return false;
                                }

                                StepR2 += 2;
                            }
                        }
                    //确认穴号个数

                    if (R2DateTimeColFound && R1DateTimeColFound)
                        goto forEnd;
                }
            }

            if (!R1DateTimeColFound && !R2DateTimeColFound)
            {
                MessageBox.Show("您所选的电子表格文件, 格式不符合 预设格式,日期单元格审核不通过");
                return false;
            }

        forEnd:


            for (int i = 0; i < ws.Rows.Count; i++)
            {
                if (i >= xdata.R1.Row + 3 && i < xdata.R2.Row)
                {

                    if (ws[i, 0].StringValue.Trim() == (R1WorkerCounter + 1) + "#")
                    {
                        R1WorkerCounter++;

                        xdata.listR1穴号.Add(new RCPosition(i, 0));
                        //因为结尾会i++; 
                        i += 5;
                    }
                }
                else if (i >= xdata.R2.Row + 3 && i < 200)
                {
                    if (ws[i, 0].StringValue.Trim() == (R2WorkerCounter + 1) + "#")
                    {
                        R2WorkerCounter++;
                        xdata.listR2穴号.Add(new RCPosition(i, 0));
                        //因为结尾会i++; 
                        i += 5;
                    }
                }

            }

            if (xdata.listR1穴号.Count == 0)
            {
                MessageBox.Show("您所选的电子表格文件, 格式不符合 预设格式\r\n若要修改预设格式请联系 开发人员, 深圳易胜科技有限公司, EasySon ;\r\n调试信息: R1穴号 单元格 为0个 ");
                return false;
            }
            if (xdata.listR2穴号.Count == 0)
            {
                MessageBox.Show("您所选的电子表格文件, 格式不符合 预设格式\r\n若要修改预设格式请联系 开发人员, 深圳易胜科技有限公司, EasySon ;\r\n调试信息: R2穴号 单元格 为0个");
                return false;
            }

            if (frm.DebugMode)
                MessageBox.Show("excel布局校验成功");

            return true;
        }

        private bool IdentifyR1FirstColDate(XlsxData xdata, Cells ws, int i, int j)
        {
            if (ws[i, j].Type == CellValueType.IsString)
            {
                if (ws[i, j].StringValue == "")
                {
                    //这是一个  没有填过日期 的表格。。。
                    R1DateTimeColFound = true;

                    xdata.posR1LastDate = new RCPosition(i, j - 2);
                    xdata.R1LastDate = ws[i, j - 2].DateTimeValue;
                    return true;
                }
                else
                {

                    DateTime dt = new DateTime();
                    try
                    {
                        dt = DateTime.Parse((string)ws[i, j].Value);
                    }
                    catch (Exception)
                    {

                        System.Windows.Forms.MessageBox.Show("日期字符串解析错误");
                        return false;
                    }
                    //xdata.posR1LastDate = new RCPosition(i, j);
                    //xdata.R1LastDate = dt;
                    //R1DateTimeColFound = true;
                }
            }
            else if (ws[i, j].Type == CellValueType.IsNull)
            {
                if (ws[i, j - 2].Type == CellValueType.IsDateTime)
                {
                    xdata.R1LastDate = ws[i, j - 2].DateTimeValue;
                }
                else
                {


                    DateTime dt = new DateTime();
                    try
                    {
                        dt = DateTime.Parse(ws[i, j - 2].StringValue);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("日期字符串解析错误");
                        return false;
                    }
                    xdata.R1LastDate = dt;
                }

                //这是一个  没有填过日期 的表格。。。
                R1DateTimeColFound = true;
                xdata.posR1LastDate = new RCPosition(i, j - 2);

                return true;
            }
            else if (ws[i, j].Type == CellValueType.IsDateTime)
            {
                xdata.R1listAllDate.Add(ws[i, j].DateTimeValue);
            }
            else
            {
                //这里还有另一种可能 是日期 类型的 ,但以后再完善
                System.Windows.Forms.MessageBox.Show("文件解析错误");
                return false;
            }


            return true;
        }

        private bool IdentifyR2FirstColDate(XlsxData xdata, Cells ws, int i, int j)
        {
            if (ws[i, j].Type == CellValueType.IsString)
            {
                if (ws[i, j].StringValue == "")
                {
                    xdata.NoR2Date = false;
                    //这是一个  没有填过日期 的表格。。。
                    R2DateTimeColFound = true;

                    xdata.posR2LastDate = new RCPosition(i, j - 2);
                    xdata.R2LastDate = ws[i, j -2].DateTimeValue;
                    return true;
                }
                else
                {
                    var strxx = ws[i, j].StringValue;
                    DateTime dt = new DateTime();
                    try
                    {
                        dt = DateTime.Parse(ws[i, j].StringValue);
                    }
                    catch (Exception)
                    {

                        System.Windows.Forms.MessageBox.Show("日期字符串解析错误");
                        return false;
                    }
                    //xdata.posR1LastDate = new RCPosition(i, j);
                    //xdata.R1LastDate = dt;
                    //R1DateTimeColFound = true;
                }
            }
            else if (ws[i, j].Type == CellValueType.IsNull)
            {
                if (ws[i, j - 2].Type == CellValueType.IsDateTime)
                {
                    xdata.R2LastDate = ws[i, j - 2].DateTimeValue;
                }
                else
                {
                    DateTime dt = new DateTime();
                    try
                    {
                        dt = DateTime.Parse(ws[i, j - 2].StringValue);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("日期字符串解析错误");
                        return false;
                    }
                    xdata.R2LastDate = dt;
                }

                //这是一个  没有填过日期 的表格。。。
                xdata.NoR2Date = false;

                R2DateTimeColFound = true;
                xdata.posR2LastDate = new RCPosition(i, j - 2);

                return true;
            }
            else if (ws[i, j].Type == CellValueType.IsDateTime)
            {
                xdata.R2listAllDate.Add(ws[i, j].DateTimeValue);
            }
            else
            {
                //这里还有另一种可能 是日期 类型的 ,但以后再完善
                System.Windows.Forms.MessageBox.Show("文件解析错误");
                return false;
            }


            return true;
        }
    }

}