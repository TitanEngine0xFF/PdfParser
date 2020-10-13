using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.Diagnostics;
using Aspose.Cells;
using Commonlib.DrawingExts;
using Commonlib.Utilities;
using pdfExtrator.Common;
using pdfExtrator.Model;
using Model.File;
using Models;
using BLL;

namespace pdfExtrator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();

        public bool DebugMode = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            UICarBeginPoint = this.UI_pdfCar.Location;

            if (DebugMode)
            {
                this.Text += "\t\t 警告: 你在调试模式中";
                if(Directory.Exists("resources")) Directory.CreateDirectory("resources");
                


            }
            else
            {
                if (Directory.Exists("resources")) Directory.Delete("resources", true);
                if (Directory.Exists("app.publish")) Directory.Delete("app.publish",true);
            }


            this.TopLevel = true;
            if (DebugMode)
            {
                panelDebug.Visible = true;
                this.Width = 1200;
                this.Height = 700;
            }

            //Aspose.Cells.License li = new Aspose.Cells.License();
            //li.SetLicense("lib\\AsposeLicense.lic");
            //Aspose.Cells.License li = new Aspose.Cells.License();
            //li.SetLicense("AsposeLicense.lic");

            //for (int i = 0; i < workbook.Worksheets[0].Cells.Rows.Count; i++)
            //{
            //    for (int j = 0; j < workbook.Worksheets[0].Cells.Columns.Count; j++)
            //    {
            //        var x=workbook.Worksheets[0].Cells[i, j];
            //    }
            //}
        }


        bool NextDay = true;
        int m_R1RowDPV = 5;
        int m_R1RowDRMS = 6;
        int m_R1RowBRMS = 7;

        int m_R1ColDPV = 3;
        int m_R1ColDRMS = 3;
        int m_R1ColBRMS = 3;

        int m_R1RowImg = 4;
        int m_R1ColImg = 3;

        //修改这全局变量得 先修改 Init 中的变量。
        //每增加一天 +=2 , 看清楚 ,现在只是测试 ,2是2个单元格,因为每天的数据占用2个单元格
        int DayColOffset = 0;
        //写入一天的数据后 才需要 + col偏移
        private void ResetCellIndex()
        {
            m_R1RowDPV = 5;
            m_R1RowDRMS = 6;
            m_R1RowBRMS = 7;

            m_R1RowImg = 4;
            m_R1ColImg = 3;

            m_R1ColDPV = 3;
            m_R1ColDRMS = 3;
            m_R1ColBRMS = 3;
        }

        public void ResetALL()
        {

            XData = new XlsxData();
            bllDateUpdate = new BLL_DateUpdate(this);
            UICarBpY = 46;

            ResetCellIndex();
            DayColOffset = 0;

            ParseDayTimer = 1;

            PdfFile.PathsDay.Clear();
            PdfFile.PathsNight.Clear();
            PdfFile.PdfFilesPath.Clear();
            infoPdfMeasure = new MeasurePdfData();

            BigpngFile?.Dispose();
        }

        /// <summary>
        /// 在ResetALL()里重置 , 每次重新打开都会重置。读取下一份pdf 时重置
        /// </summary>
        public Model.MeasurePdfData infoPdfMeasure = new Model.MeasurePdfData();

        public XlsxData XData = null;
        BLL_DateUpdate bllDateUpdate;

        Point UICarBeginPoint = Point.Empty;
        int UICarBpY = 55; //y=46
        int UICarbpX = 125;
        string PreRunPath = "";
        string PreOutPutExcel = "";

        private void btnRun_Click(object sender, EventArgs e)
        {
            bool P1Reach = false;
            Stopwatch gswTask = new Stopwatch();
            btnRun.Enabled = false;
            UI_pdfCar.Visible = true;

            BLL.ExcelFile.VerifyFile vef = new BLL.ExcelFile.VerifyFile(this);
            if (PreRunPath != "" && PreOutPutExcel !="" && PreRunPath == PdfFile.pathPdfDataFolder&& PreOutPutExcel== PdfFile.pathOutPutExcel)
            {

                switch (MessageBox.Show("是否再次导入同一个pdf文件夹的数据?", "", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        PreRunPath = "";
                        workbook.Dispose();
                        workbook = new Workbook(PdfFile.pathOutPutExcel);
                        vef = new BLL.ExcelFile.VerifyFile(this);
                        ResetALL();
                        vef.Analysis(workbook, XData);

                        break;
                    case DialogResult.No:
                        ClearInputPdfFolderPath();
                        break;
                    case DialogResult.Cancel:
                        EndMainProcedure();
                        return;
                        break;
                }
            }


            btnRun.Text = "处理中";
            UI_StatusText.Text = "请导入包含pdf文件夹";
            this.btnExtra.BackgroundImage = global::pdfExtrator.Properties.Resources.Excel_2013_256px_1180012_easyicon_net;

            ResetALL();




            if (!inputPathAlready)
                if (!ShowPDFDirDialog())
                {
                    EndMainProcedure();
                    return;
                }



            //主要还是状态维护费高的问题。 第二次按也需要复位状态。因此，要么不传递状态，每次都判断一下就好。
            if (PdfFile.pathPdfDataFolder == "" || !Directory.Exists(PdfFile.pathPdfDataFolder))
                if (!ShowPDFDirDialog())
                {
                    inputPathAlready = false;
                    EndMainProcedure();
                    return;
                }


            if (PdfFile.pathOutPutExcel == "" || !File.Exists(PdfFile.pathOutPutExcel))
                if (!OpenExcelFileDialog())
                {
                    outPathAlready = false;
                    EndMainProcedure();
                    return;
                }

            //if (UI_PDFFolder.Text == "" || !Directory.Exists(UI_PDFFolder.Text))
            //{
            //    if (!ShowPDFDirDialog())
            //    {
            //        inputPathAlready = false;
            //        EndMainProcedure();
            //        return;
            //    }
            //}


            //if (UI_excelPath.Text == "" || !File.Exists(UI_excelPath.Text))
            //{
            //    if (!OpenExcelFileDialog())
            //    {
            //        outPathAlready = false;
            //        EndMainProcedure();
            //        return;
            //    }
            //}


            this.btnExtra.Enabled = false;
            this.btnPdfPath.Enabled = false;
            //UI_DownArrow1.Visible = false;
            //UI_DownArrow2.Visible = false;
            
            // 修复 再次打开文件后 报错 , C# 这么爽的语言, 不用就是 走宝了
            pictureBox1.Image?.Dispose();

            if (!vef.Analysis(workbook, XData))
            {
                EndMainProcedure();
                return;
            }

            //以前的流程
            //分析目录与文件名();
            DirectoryInfo di = new DirectoryInfo(PdfFile.pathPdfDataFolder);

            //var listtmp = di.EnumerateDirTreeAllfilesDirs();
            FileHelper fh = new FileHelper();
            var listTmp=fh.GetDirAllFiles(di.FullName);
            List<string> listFileNames = new List<string>(20);

            for (int i = 0; i < listTmp.Item1.Count; i++)
                listFileNames.Add(listTmp.Item1[i]);
            if (listFileNames.Count < 0)
            {
                MessageBox.Show("您所选的文件夹 不存在pdf文件 ,请重新选择");
                EndMainProcedure();
                return;
            }

            PdfFile.PdfFilesPath.AddRange(listFileNames);
            UI_StatusText.Text = "文件处理中";

            //遍历 Day And Night All pdf file

            gswTask.Restart();
            ParseDayTimer = 1;

            processBar.Maximum = 1000;
            processBar.Value = 0;
            processBar.Step = 1;
            var preTimes = (processBar.Maximum / PdfFile.PdfFilesPath.Count) < 0 ? 1 : processBar.Maximum / PdfFile.PdfFilesPath.Count;



            for (int i = 0; i < PdfFile.PdfFilesPath.Count; i++)
            {
                g_i = i;
                TaskFinished = false;
                UI_StatusText.Text = "正在处理: " + PdfFile.PdfFilesPath[g_i];
                Task t1= Task.Factory.StartNew(() =>GetMeasureInfoAndData(PdfFile.PdfFilesPath[g_i]));

                while (true)
                {
                    RefreshUI(ref P1Reach);
                    if (t1.IsCompleted) break;

                    if (hasError)
                        break;
                    if (TaskFinished)
                        break;

                    if (processBar.Value < preTimes * (i + 1))
                    {
                        if (processBar.Value + 2 > processBar.Maximum)
                            processBar.Value = processBar.Maximum;
                        else
                            processBar.Value += 2;
                    }
                    else
                        processBar.Value = preTimes * (i + 1);
                    retry:
                    try
                    {
                        Application.DoEvents();
                    }
                    catch (Exception)
                    {
                        if (frmclosing)
                            break;


                        Thread.Sleep(20);
                        goto retry;
                    }
                    Thread.Sleep(15);
                }
                if (hasError)
                    goto ErrorEnd;

                if (frmclosing)
                    goto End;

                

                if (SaveExcelFile())
                {


                }
                infoPdfMeasure.ResetALL();
            }
        End:

            processBar.Value = processBar.Maximum;
            

            UI_StatusText.Text = "完成! ,耗时:" + gswTask.ElapsedMilliseconds + "ms\t\t共处理" + PdfFile.PdfFilesPath.Count + "个文件";
            this.btnExtra.BackgroundImage = global::pdfExtrator.Properties.Resources.Excel_Finished;
            EndMainProcedure();
            PreRunPath = PdfFile.pathPdfDataFolder;
            Thread.Sleep(200);
            return;

        ErrorEnd:
            UI_StatusText.Text = "状态: 日期错误 ,任务已终止";

            workbook.Dispose();
            workbook = new Workbook(PdfFile.pathOutPutExcel);
            vef = new BLL.ExcelFile.VerifyFile(this);
            vef.Analysis(workbook, XData);

            EndMainProcedure();

            hasError = false;
            return;
        }

        private void ClearInputPdfFolderPath()
        {
            inputPathAlready = false;
            UI_PDFFolder.Text = "";
            PdfFile.pathPdfDataFolder = "";
        }

        int g_i = 0;

        public bool SaveExcelFile()
        {
        RetrySave:
            try
            {
                workbook.Save(PdfFile.pathOutPutExcel);


            }
            catch (Exception e1)
            {
                if (MessageBox.Show(e1.Message + "\r\n 可点击【重试】,再次保存 ", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Retry)
                {
                    goto RetrySave;
                }
                else
                    return false;
            }
            return true;
        }

        private void EndMainProcedure()
        {
            processBar.Value = 0;
            UI_StatusText.Text = "状态:待机";
            btnRun.Text = "执行";
            UI_pdfCar.Location = UICarBeginPoint;
            UI_pdfCar.Visible = false;
            this.btnExtra.Enabled = true;
            this.btnPdfPath.Enabled = true;

            this.btnRun.Enabled = true;
            //UI_DownArrow1.Visible = true;
            //UI_DownArrow2.Visible = true;
        }



        private void RefreshUI(ref bool P1Reach)
        {

            //UICarBpY += 1;

            UICarbpX += 3;
            //145 255 
            if (UICarbpX < 542)
            {
                P1Reach = true;
                UI_pdfCar.Location = new Point(UICarbpX, UI_pdfCar.Location.Y);
            }


            if (UICarbpX >= 542)
            {
                UI_pdfCar.Location = UICarBeginPoint;
                UICarbpX = UICarBeginPoint.X;
            }

        }

        //public int findString(string src,string firstStr)
        //{
        //    //return  src.LastIndexOf('.',
        //}



        int ParseDayTimer = 1;

        bool OutPutBigPng = false;
        public void GetMeasureData(string fileName)
        {
            //Todo: write your algorithm
            //截图图片 , 重用g 对象名
            var bigImage = BigpngFile;

            if (OutPutBigPng)
            {
                bigImage.Save("1.png");
            }
            //bigImage.Save("1.png");



            this.BeginInvoke(new Action(() =>
            {
                tbxRBPoint.Text = "";
            }));

            //从文件名上抓取
            string FaceR = infoPdfMeasure.FaceR;
            //Size clipSize = new Size(40, 480);
            Size clipSize = new Size(60,600);

            StringBuilder sbxx = new StringBuilder(150);

            sbxx.Append("以下是" + FaceR + "类型的pdf, 数据抓取明细:" + ln);

            截图的位置[] clipPos = new 截图的位置[4];
            clipPos[0] = 截图的位置.上图左;
            clipPos[1] = 截图的位置.上图右;
            clipPos[2] = 截图的位置.下图左;
            clipPos[3] = 截图的位置.下图右;
            string[] logPicTag = { "图1左红点值", "图1左蓝点值" , "图1右红色值", "图1右蓝点值", "图2左红点值", "图2左蓝点值", "图2右红点值", "图2右蓝点值" };
            string[] logPicDiff = { "图1左红蓝差值", "图1右红蓝差值", "图2左红蓝差值", "图2右红蓝差值" };
            采样点位置[] PointPos = new 采样点位置[8];
            string[] 采样点Name = Enum.GetNames(typeof(采样点位置));

            //尺子的高度
            int rulerHeight = clipSize.Height;
            decimal[] realvals = new decimal[8];
            decimal[] valDiffs = new decimal[4];

            for (int i = 0; i < 4; i++)
            {
                var rbPointAll = GetRB_PointAllMetric(bigImage, clipSize, clipPos[i]);
                {
                    if(i==0) sbxx.Append("以下各截图区域对应的 红蓝点的坐标" + ln);

                    if (rbPointAll.OriginBluePoint.X == -1 || rbPointAll.OriginBluePoint.Y == -1 && rbPointAll.OriginRedPoint.X == -1 || rbPointAll.OriginRedPoint.Y == -1)
                    {
                        sbxx.Append("红蓝两点 其中一个 获取失败" + ln);
                    }
                    sbxx.Append("上图左坐标:" + ln);
                    sbxx.Append("红点:" + rbPointAll.OriginRedPoint.ToString() + "  " + rbPointAll.RGBSET[0].ToString() + ln);
                    sbxx.Append("蓝点:" + rbPointAll.OriginBluePoint.ToString() + "  " + rbPointAll.RGBSET[1].ToString() + ln);
                    if (OutPutBigPng)
                    {
                        sbxx.Append("小图坐标" + ln);
                        sbxx.Append("红点:" + rbPointAll.CaptureRedPoint.ToString() + "  " + rbPointAll.RGBSET[0].ToString() + ln);
                        sbxx.Append("蓝点:" + rbPointAll.CaptureBluePoint.ToString() + "  " + rbPointAll.RGBSET[1].ToString() + ln);
                    }
                    if (i == 0)
                        sbxx.Append("0.8 ~ -0.8 区间内 红蓝点 对应的 值:" + ln);

                    realvals[i * 2] = getRealValue(clipSize, rbPointAll.CaptureRedPoint, infoPdfMeasure.MetricMaxVal);
                    sbxx.Append("图1左红点值 =" + string.Format("{0:N4}", realvals[(int)(采样点位置)Enum.Parse(typeof(采样点位置), 采样点Name[i*2])]) + ln);
                    realvals[i * 2+1] = getRealValue(clipSize, rbPointAll.CaptureBluePoint, infoPdfMeasure.MetricMaxVal);
                    sbxx.Append("图1左蓝点值 =" + string.Format("{0:N4}", realvals[(int)(采样点位置)Enum.Parse(typeof(采样点位置), 采样点Name[i*2+1])]) + ln);
                }
            }

            sbxx.Append("以下是 各位置的红蓝点 差值" + ln);
            for (int i = 0; i < 4; i++)
            {
                valDiffs[i] = Math.Abs(realvals[i*2] - realvals[i*2+1]);
                sbxx.Append(logPicDiff[i] +"= " + string.Format("{0:N4}", valDiffs[i]) + ln);
            }

            var vmax1 = Math.Max(valDiffs[0], valDiffs[1]);
            var vmax2 = Math.Max(valDiffs[2], valDiffs[3]);
            infoPdfMeasure.Diff = Math.Max(vmax1, vmax2);

            getPdfDataSuccess = true;
            this.BeginInvoke(new Action(() =>
            {
                tbxRBPoint.AppendText(sbxx.ToString());
                using (StreamWriter sw = new StreamWriter("日志.txt",true))
                {
                    sw.WriteLine(DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString());
                    sw.WriteLine(sbxx.ToString());
                    sw.Close();
                }
                if(DebugMode) pictureBox1.Image = bigImage;
            }));

            //pictureBox2.Image = smallBitMap;

            //smallBitMap.Save("resources\\tmp.png", ImageFormat.Png  );

            //PDFViewer viewer = new PDFViewer();
            //viewer.TopMost = true;
            //viewer.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            //viewer.pictureBox2.Image = bigImage;
            //viewer.ShowDialog();
        }

        private void GetMeasureInfoAndData(string fileName)
        {

            infoPdfMeasure.PdfPath = fileName;
            //viewer.axAcroPDF1.LoadFile(ofd.FileName);

            //GetPdfFileNameData(fi);

            string pdfData = ReadPdBySpire(fileName);
            string xx=getPdfMeasureInfo(pdfData);
            Debug.WriteLine(xx);

            var pak2List = ParseSpireReadPdfData(pdfData).ToList();

            if (pak2List.Count<=0||pak2List[0] == 异常代码.通用)
            {
                goto ErrorEnd;
            }

            pak2List.RemoveAt(0);

            int idxStatus1 = -1;
            int idxStatus2 = -1;
            int t1 = 0;
            for (int i = 0; i < pak2List.Count; i++)
            {
                if (pak2List[i].Contains("Status R.M.S ="))
                {
                    t1++;
                    if (t1 == 1)
                    {
                        idxStatus1 = i;
                    }
                    else if (t1 == 2)
                    {
                        idxStatus2 = i;
                        break;
                    }
                }
            }
            if (idxStatus2 == -1)
            {
                MessageBox.Show("Status R.M.S 找不到, 状态信息没找到");
                goto ErrorEnd;
            }

            string status1 = pak2List[idxStatus1];
            string status2 = pak2List[idxStatus2];



            //Status R.M.S = 0.0687 (um)  P-V = 0.3424 (um)
            Regex regex = new Regex(@"(?<=\= )[0-9.]+");
            var result = regex.Matches(status1);
            if (result.Count > 1)
                if (result[0].Success && result[1].Success)
                    try
                    {
                        infoPdfMeasure.DRMS = decimal.Parse(result[0].Value);
                        infoPdfMeasure.DPV = decimal.Parse(result[1].Value);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message +
                            "DRMS 或 DPV 解析失败" +
                            "\r\n请联系 易胜科技");
                        goto ErrorEnd;
                    }
                else
                {
                    MessageBox.Show("DRMS 或 DPV 解析失败" +
                            "\r\n请联系 易胜科技");
                    goto ErrorEnd;
                }
            else
            {
                MessageBox.Show("DRMS 或 DPV 解析失败\r\n请联系 易胜科技");
                goto ErrorEnd;
            }


            var r = regex.Match(status2);
            if (r.Success)
            {
                try
                {
                    infoPdfMeasure.BRMS = decimal.Parse(r.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n" +
                        "BRMS 解析失败" +
                        "\r\n请联系 易胜科技");
                    goto ErrorEnd;
                }
            }
            else
            {
                MessageBox.Show("DRMS 或 DPV 解析失败" +
                        "\r\n请联系 易胜科技");
                goto ErrorEnd;
            }

            //for (int i = 0; i < workbook.Worksheets[0].Cells.Rows.Count; i++)
            //{
            //    for (int j = 0; j < workbook.Worksheets[0].Cells.Columns.Count; j++)
            //    {
            //    }
            //}


            SpirePdfSavePng(fileName);

            GetMeasureData(fileName);

            DayOrNight dn = DayOrNight.Day;

            if (infoPdfMeasure.MeasuDateTime.TimeOfDay> TimeSpan.FromHours(8) && infoPdfMeasure.MeasuDateTime.TimeOfDay <= TimeSpan.FromHours(20))
                dn = DayOrNight.Day;
            else
                dn = DayOrNight.Night;


            // 导入了前一天的数据 则跳过写入表格。不再提示
            if (!UpdateDate(dn))
            {
                goto ErrorEnd;
            }


            OutputExcelFile(dn, GenerateThumbImage());


            TaskFinished = true;
            return;

            ErrorEnd:
            this.Invoke(new Action(() =>
            {
                hasError = true;
            }));
            hasError = true;

            TaskFinished = false;
            return;
        }

        private string[] ParseSpireReadPdfData(string pdfData)
        {
            string SkipSurplusSpace = CommonFuncs.RemoveSurplusSpace(pdfData);

            var atts1 = SkipSurplusSpace.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int MetricMaxIndex = getMetricMaxIndex(atts1);
            if (MetricMaxIndex < atts1.Length)
            {
                decimal maxval = decimal.Parse(atts1[MetricMaxIndex].Trim());
                infoPdfMeasure.MetricMaxVal = maxval;
            }
            else
            {
                MessageBox.Show("异常：pdf非约定的格式。");
                return new string[] { 异常代码.通用 };
            }

            Regex regex = new Regex("[0-9]+#");
            var r = regex.Match(atts1[1]);
            if (r.Success)
            {
                try
                {
                    infoPdfMeasure.WId = int.Parse(r.Value.Replace("#", ""));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("穴号获取失败, 你所选的pdf不符合预设格式 "+ ex.Message);
                    return new string[] { 异常代码.通用 };
                }
            }
            else
            {
                MessageBox.Show("穴号获取失败, 你所选的pdf不符合预设格式");
                return new string[] { 异常代码.通用 };
            }

            bool TagGetMachineModelSuccessed = false;
            for (int i = 0; i < atts1.Length; i++)
            {
                if (atts1[i].ToUpper().Contains("PRM:"))
                {
                    Regex regex1 = new Regex("R1|R2");
                    var result1 = regex1.Match(atts1[i]);
                    if (result1.Success)
                    {
                        infoPdfMeasure.FaceR = result1.Value;
                        TagGetMachineModelSuccessed = true;
                        break;
                    }
                    else
                    {
                        MessageBox.Show("机型 获取失败, 你所选的pdf不符合预设格式");
                        return new string[] { 异常代码.通用 };
                    }

                }
            }

            if (!TagGetMachineModelSuccessed)
            {
                MessageBox.Show("机型 获取失败, 你所选的pdf不符合预设格式");
                return new string[] { 异常代码.通用 };
            }
            return atts1;
        }

        private static int getMetricMaxIndex(string[] atts1)
        {
            int MetricMaxIndex = 0;
            for (int i = 0; i < atts1.Length; i++)
            {
                if (atts1[i].Trim() == "Y axis Y Effect R")
                {
                    MetricMaxIndex = i;
                }
            }

            return MetricMaxIndex+1;
        }

        bool hasError = false;


        static readonly object taglock = new object();

        bool TaskFinished = false;


        private bool UpdateDate(DayOrNight dn)
        {
            var realDate = infoPdfMeasure.MeasuDateTime.Date;
            if (infoPdfMeasure.MeasuDateTime.Hour <= 8)
            {
                realDate = infoPdfMeasure.MeasuDateTime.Date.AddDays(-1);
            }
            infoPdfMeasure.RealDate = realDate;

            if (infoPdfMeasure.FaceR == "R1")
            {
                // 不断更新, 就如日期相同也要更新, 
                int colOffset = XData.posR1LastDate.Col;
                if (XData.posR1LastDate.Col == 0)
                {
                    colOffset = 3;
                    XData.posR1LastDate.Col = 3;
                }

                if (!XData.NoR1Date) //有日期 ,证明 这次并不是 首次写入,而是 打开之前就存在数据的
                {
                    if (infoPdfMeasure.RealDate.Subtract(XData.R1LastDate.Date).TotalDays > 1)
                    {
                        //向右 偏移2个单元格 , 因为 测试日期可能是不连续的, 上次3号, 这次5号, 但col也只+2
                        colOffset += 2;

                    }//放进来前一天的pdf 数据
                    else if (infoPdfMeasure.RealDate.Subtract(XData.R1LastDate.Date).TotalDays < 0)
                    {
                        //MessageBox.Show("警告: 导入R1的数据必须 比表格中 已存在R1的日期 要大!");
                        //XData.R1DateisWrong = true;
                        //this.BeginInvoke(new Action(() =>
                        //   {
                        //       UI_PDFFolder.Text = "";
                        //       PdfFile.pathPdfDataFolder = "";
                        //   }));
                        //return false;

                        if (XData.R2DateisWrong && XData.R1DateisWrong)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                UI_PDFFolder.Text = "";
                                PdfFile.pathPdfDataFolder = "";
                                UI_StatusText.Text = " R1 和R2 日期 都比之前的数据早";

                            }));
                            }
                    
                    }
                    // 同一天的数据, 但需要标记才能区分是首次还是 再次导入
                    else if (infoPdfMeasure.RealDate.Subtract(XData.R1LastDate.Date).TotalDays == 0)
                    {

                    }
                }

                //有日期,无日期,都要写入日期只是坐标不一样
                bllDateUpdate.updateR1DateCell(dn, colOffset);

                //啥都不用干
            }
            else if (infoPdfMeasure.FaceR == "R2")
            {
                int colOffset = XData.posR2LastDate.Col;
                if (XData.posR2LastDate.Col == 0)
                {
                    colOffset = 3;
                    XData.posR2LastDate.Col = 3;
                }

                if (!XData.NoR2Date)
                {
                    //首次导入,或者 再次 导入时 ,将会 在新一列 导入数据
                    if (infoPdfMeasure.RealDate.Subtract(XData.R2LastDate.Date).TotalDays > 1)
                    {
                        //colofs = (int)infoMeasure.MeasuDateTime.Date.Subtract(XData.R2LastDate.Date).TotalDays * 2;
                        colOffset += 2;

                    }
                    else if (infoPdfMeasure.RealDate.Subtract(XData.R2LastDate.Date).TotalDays < 0)//放进来前一天的pdf 数据
                    {
                        //MessageBox.Show("警告: 导入R2的数据必须 比表格中 已存在R2的日期 要大!");
                        XData.R2DateisWrong = true;

                        if (XData.R2DateisWrong&&XData.R1DateisWrong)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                UI_PDFFolder.Text = "";
                                PdfFile.pathPdfDataFolder = "";
                                UI_StatusText.Text = " R1 和R2 日期 都比之前的数据早";
                            }));
                            return false;
                        }
                    }
                    //跟上一次 导入的日期时一样的 , 需要连续导入因此,我们是无法判定到底是首次导入还是 连续导入的
                    else if (infoPdfMeasure.RealDate.Subtract(XData.R2LastDate.Date).TotalDays == 0)
                    {

                    }
                }

                bllDateUpdate.updateR2DateCell(dn, colOffset);
            }
            return true;
        }


        bool R2First = false;
        //这个是 为了防止跳了2次。。
        bool isJumpRows = false;
        /// <summary>
        /// 为了防止复位多次, 当然 日期变更 , 这个也应该 复位
        /// </summary>

        private void OutputExcelFile(DayOrNight dn, MemoryStream msThumbImage)
        {
            //this.pictureBox1.Image = BigpngFile;

            var wb = workbook.Worksheets[0];
            //正确的!!!! 把每次 R1RowDPV 作为参考点 , 那 判定值 相对于这个 参考点 就是+4 row. 因此 m_R1RowDPV+4+R2offset

            //因为是跟C6单元格比较 是固定的, 
            //wb.Cells[9, 2].Formula= "=IF(D6>$C$6,\"NG\",\"OK\")";
            //wb.Cells[9, 2].Formula = "=IF(C6>$C6,\"NG\",\"OK\")";
            var BaseFormula = "";

            //看我的提取代码的公因式
            int x = 0, y = 0;
            if (infoPdfMeasure.FaceR == "R1")
            {
                BaseFormula=wb.Cells[XData.R1.Row + 8, 2].R1C1Formula;
                x = XData.listR1穴号[infoPdfMeasure.WId - 1].Row;
                //向右偏移
                y = XData.posR1LastDate.Col;
                //向右偏移+1
                if (dn == DayOrNight.Night)
                    y += 1;
            }
            else if (infoPdfMeasure.FaceR == "R2")
            {
                BaseFormula = wb.Cells[XData.R2.Row + 8, 2].R1C1Formula;
                //这个穴号可以是空的,  文件可以没有 对应的
                x = XData.listR2穴号[infoPdfMeasure.WId - 1].Row;
                //向右偏移
                y = XData.posR2LastDate.Col;
                //向右偏移+1
                if (dn == DayOrNight.Night)
                    y += 1;
            }

            //为了调试用的
            if (infoPdfMeasure.WId == 4)
            {
            }

            //如果已经有值就跳过 ,不要赋值了了
            if (wb.Cells[x + 1, y].Type != CellValueType.IsNumeric)
            {

                Aspose.Cells.CellsFactory cellsFactory = new Aspose.Cells.CellsFactory();
                var Mystyle = wb.Cells[x + 5, y].GetStyle(true);

                Mystyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;

                wb.Pictures.Add(x, y, x + 1, y + 1, msThumbImage);

                //修改图片框
                var pic = wb.Pictures[wb.Pictures.Count - 1];
                pic.Left += 8;
                pic.Top += 8;
                pic.Width -= 16;
                pic.Height -= 16;

                wb.Cells[x + 1, y].PutValue(infoPdfMeasure.DPV);
                wb.Cells[x + 2, y].PutValue(infoPdfMeasure.DRMS);
                wb.Cells[x + 3, y].PutValue(infoPdfMeasure.BRMS);
                wb.Cells[x + 4, y].PutValue(decimal.Round(infoPdfMeasure.Diff, 2));
                wb.Cells[x + 5, y].R1C1Formula = BaseFormula;

                wb.Cells[x + 5, y].SetStyle(Mystyle);
            }


            //OldWriteExcelMethod(dn, msThumbImage, R2offset, wb, BaseFormula);
        }

        private MemoryStream GenerateThumbImage()
        {
            //BigpngFile
            //截的是pdf 文件中的上图
            MemoryStream msThumbImage = new MemoryStream();

            using (var waterMarkImg = new Bitmap((int)(244 * 3f), (int)(165 * 3f)))
            {
                using (var g1 = Graphics.FromImage(waterMarkImg))
                {
                    g1.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    g1.TranslateTransform(waterMarkImg.Width / 20, waterMarkImg.Height / 2);

                    g1.ScaleTransform(2, 2);
                    g1.RotateTransform(-30);

                    g1.DrawString("QFOPT", new System.Drawing.Font("微软雅黑", 60, FontStyle.Regular), Brushes.Gray, -20, 30);

                    g1.Save();

                }
                //waterMarkImg.Save("watermark.png");

                Bitmap SamllWaterMarkImg = new Bitmap(waterMarkImg, 244, 165);
                if(DebugMode)
                    SamllWaterMarkImg.Save("resources\\watermark.png");




                //这句代码居然会导致 函数堆栈崩溃 , 调到上层函数。。那就是说 bigfile2 对象是不正确的。
                //Bitmap Bigfile2 = (Bitmap)BigpngFile.Clone();

                var BigPngFile2 = BigpngFile.CopyBitmap();


                //剪辑大图 ,
                using (var ClipImage = new Bitmap(1073, 874))
                {
                    using (var g = Graphics.FromImage(ClipImage))
                    {
                        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                        g.DrawImage(BigPngFile2, new Rectangle(Point.Empty, ClipImage.Size), new Rectangle(275, 335, 1065, 864), GraphicsUnit.Pixel);

                        //g.DrawImage(waterMarkImg, new Point(10, 10));
                        g.Save();
                    }
                    ClipImage.Save("Tmp\\clipimg.png", ImageFormat.Png);
                    Commonlib.Utilities.WaterMarkMaker.AddWaterMark(ClipImage, SamllWaterMarkImg, 5, colNum: 3);

                    ClipImage.Save(msThumbImage, ImageFormat.Png);
                    ClipImage.Save("Tmp\\wmThumbImg.png", ImageFormat.Png);

                    ClipImage.Dispose();
                }
                SamllWaterMarkImg.Dispose();
                waterMarkImg.Dispose();
            }

            return msThumbImage;
        }

        public static class 异常代码
        {
            public static string 通用 = "0x9f";
        }

        /// <summary>
        /// 获取测试信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string  getPdfMeasureInfo(string str)
        {
            string FaceRx = "";

            StringBuilder sb = new StringBuilder();
            Regex regex = new Regex(@"\[[\(\0-9a-zA-Z\),#]*\]");
            var ret = regex.Match(str);
            if (ret.Success)
            {
                FaceRx = ret.Value;
                sb.AppendLine(ret.Value);
            }
            string prefix = "Measured atX;";
            var idxDate1 = str.IndexOf(prefix);

            string date = "";
            string status1 = "";
            string status2 = "";

            if (idxDate1 != -1)
            {
                idxDate1 = idxDate1 + prefix.Length;

                var idxDate2 = str.IndexOf(",", idxDate1, 21);
                if (idxDate2 != -1)
                {
                    date = str.Substring(idxDate1, idxDate2 - idxDate1);
                    sb.AppendLine(date);
                    try
                    {
                        infoPdfMeasure.MeasuDateTime = DateTime.Parse(date);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("日期解析错误 在getPdfMeasureInfo()中");
                        throw;
                    }
                    

                    string RMSStatusStr = "Status  R.M.S =";
                    var idxStatus1 = str.IndexOf(RMSStatusStr, idxDate2);
                    if (idxStatus1 != -1)
                    {
                        var idxNewLineSign = str.IndexOf("\r\n", idxStatus1 + RMSStatusStr.Length);
                        if (idxNewLineSign != -1)
                        {

                            status1 = str.Substring(idxStatus1, idxNewLineSign - idxStatus1);
                            sb.AppendLine(status1);
                            //找图2 的状态
                            var idxStatus2 = str.IndexOf(RMSStatusStr, idxNewLineSign);
                            if (idxStatus2 != -1)
                            {
                                var idxNewLineSign2 = str.IndexOf("\r\n", idxStatus2 + RMSStatusStr.Length);
                                if (idxNewLineSign2 != -1)
                                {
                                    status2 = str.Substring(idxStatus2, idxNewLineSign2 - idxStatus2);
                                    sb.AppendLine(status2);

                                    return sb.ToString();
                                }
                            }

                        }
                    }
                }

            }



            //infoMeasure.

            MessageBox.Show("解析失败: pdf文件不符合 约定的格式 ");
            sb.AppendLine("识别失败, 请联系技术人员");
            return 异常代码.通用;
        }

        string ln = "\r\n";

        /// <summary>
        /// 为了载入动画
        /// </summary>
        bool getPdfDataSuccess = false;

        Bitmap BigpngFile = null;
        private void SpirePdfSavePng(string fileName)
        {
            //ShowLoading
            this.BeginInvoke(new Action(() =>
            {
                //this.label1.Text = "请";
                this.label1.Visible = false; //true 启用 loading
                int cxx = 0;
                while (false) //true 启用
                {

                    this.label1.Location = new Point(this.Width / 2 - label1.Width / 2, this.Height / 3 - label1.Height / 2);

                    cxx++;
                    switch (cxx)
                    {
                        case 1:
                            this.label1.Text = "处";
                            break;
                        case 2:
                            this.label1.Text += "理";

                            break;
                        case 3:
                            this.label1.Text += "中";
                            break;
                        default:
                            if (cxx >= 3 && cxx <= 6)
                            {
                                this.label1.Text += ".";
                            }
                            else if (cxx > 6)
                            {
                                cxx = 0;
                            }
                            break;
                    }
                    Application.DoEvents();
                    if (getPdfDataSuccess)
                    {
                        this.label1.Visible = false;
                        //准备下一次触发
                        getPdfDataSuccess = false;
                        break;
                    }
                    Thread.Sleep(120);
                }
            }));

            var path = fileName;
            //实例化PdfDocument类，并加载测试文档
            using (Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument())
            {

                doc.LoadFromFile(path);
                Directory.CreateDirectory("Tmp");
                //实例化List类
                List<Image> ListImage = new List<Image>();
                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    // 获取 Spire.Pdf.PdfPageBase类对象
                    Spire.Pdf.PdfPageBase page = doc.Pages[i];
                    // 提取图片
                    Image[] images = page.ExtractImages();
                    if (images != null && images.Length > 0)
                    {
                        ListImage.AddRange(images);
                    }
                    ////pictureBox1.Image = images[0];
                    ////pictureBox2.Image = images[1];
                    ///
                    //遍历PDF每一页 //只能一页页地保存成图片
                    //将PDF页转换成Bitmap图形
                    BigpngFile = (Bitmap)doc.SaveAsImage(0, 200, 200);
                    //只保存第一张
                    //将Bitmap图形保存为Png格式的图片 //减少IO延迟
                    //BigpngFile.Save("Tmp\\big.png", ImageFormat.Png);
                }
                //doc.SaveToFile(@"Tmp\1.Svg", 0, 0, Spire.Pdf.FileFormat.SVG);
                doc.Close();
            }

            //var svgDocument = SvgDocument.Open("Tmp\\1.Svg");
            #region 备注..暂不用的代码


            //var width = 0;
            //var height = 0;

            //转成高清图 
            //if (width != 4800)// I resize my bitmap
            //{
            //    width = 4800;
            //    height = 4800 / 816 * 1056;
            //}

            //算好了的图片大小，图片大小更改时告诉我。
            //int width1 = (int)(816 * 2);
            //int height1 = (int)(1056 * 2);

            //Bitmap bitm = new Bitmap(width1, height1);
            //var g = Graphics.FromImage(bitm);
            //{
            //    Bitmap bitmap = svgDocument.Draw(width, height);

            //    //viewer.pictureBox2.Size = new Size(816, 1056);


            //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //    g.DrawImage(bitmap, new Rectangle(0, 0, width1, height1));
            //    g.Save();
            //    g.Dispose();

            //    bitmap.Dispose();
            //}
            //bitm.Save("Tmp\\1.png", System.Drawing.Imaging.ImageFormat.Png);

            #endregion

        }

        //
        //
        /// <summary>
        /// 陈天河发明的!   ;通过像素高度 计算出 每颗像素 所占的刻度值 
        /// </summary>
        /// <param name="clipSize"></param>
        /// <param name="RbPoint"></param>
        /// <returns></returns>
        private static decimal getRealValueOld(Size clipSize, Point RbPoint)
        {
            var rulerHeight = clipSize.Height;
            //每0.2的高度对应的像素点高度
            //每颗像素应对的值 0.0012598425
            //小数得放大成 整数，才能算比率的。。。
            var rate = (rulerHeight - RbPoint.Y) / (decimal)rulerHeight;
            //负数偏移  .,1600  是因为  -0.8 ~ 0.8  共16个区域 ,   
            // -0.8 是因为向负数坐标偏移(没办法,只能通过偏移来修正坐标系)
            var realval = 1600 * rate / 1000 - 0.8m;
            return realval;
        }

        private decimal getRealValue(Size clipSize, Point RbPoint, decimal MetricMax)
        {
            decimal realval = 0;

            decimal rulerHeight = clipSize.Height;
            //每0.2的高度对应的像素点高度
            //每颗像素应对的值 0.0012598425
            //小数得放大成 整数，才能算比率的。。
            decimal rate = (rulerHeight - RbPoint.Y) / (decimal)rulerHeight;
            //负数偏移  .,1600  是因为  -0.8 ~ 0.8  共16个区域 ,   
            // -0.8 是因为向负数坐标偏移(没办法,只能通过偏移来修正坐标系)

            //这个正确的, 但就是遍历那里有问题  - MetricMax 是为了 向负坐标偏移 ,  为啥要放大, 
            //因为 0.8x2 只会越来越小。。这就不是放大了，这叫缩小。。。0.01x100= 1*2=
            realval = MetricMax * 2 * 100 * rate / 100 - MetricMax;

            return realval;
        }

        enum 截图的位置
        {
            上图左,
            上图右,
            下图左,
            下图右
        }

        enum 采样点位置 : int
        {
            a上图左红,
            b上图左蓝,
            c上图右红,
            d上图右蓝,

            e下图左红,
            f下图左蓝,
            g下图右红,
            h下图右蓝,
        }

        private RBCurvePositons GetRB_PointOld(Bitmap bigImage, Size clipSize, 截图的位置 posi)
        {
            Point clipLP = Point.Empty;
            //Size clipSize = new Size(4, 241);

            //从文件名上抓取
            string FaceR = infoPdfMeasure.FaceR;
            switch (FaceR)
            {
                case "R1":
                    //要改
                    //clipLP = new Point(436, 636);
                    clipLP = new Point(403, 516);
                    //Size clipSize = new Size(40, 480);
                    break;
                case "R2":   //曲线位置往右偏移
                    clipLP = new Point(403, 1415);
                    break;
                default:
                    break;
            }
            Rectangle clipRect = new Rectangle(clipLP, clipSize);

            Bitmap smallBitMap = new Bitmap(clipRect.Width, clipRect.Height);
            int clipXoffset = 0;
            int clipYoffset = 0;

            //clipLP = new Point(403, 516);
            //Size clipSize = new Size(40, 480);
            //clipLP = new Point(403, 1415);
            switch (posi)
            {
                case 截图的位置.上图左:
                    clipXoffset = 0;
                    clipYoffset = 0;
                    break;
                case 截图的位置.上图右:

                    clipXoffset = 769;
                    //clipXoffset = 1173 - R1LP.X;  // 1173 ,636
                    clipYoffset = 0;
                    break;
                case 截图的位置.下图左:
                    clipXoffset = 0;
                    //clipYoffset = 1535 - R1LP.Y;  //1535
                    clipYoffset = 899;
                    break;
                case 截图的位置.下图右:
                    clipXoffset = 769;
                    clipYoffset = 899;
                    break;
                default:
                    break;
            }


            //截取的区域
            Rectangle clipRect1 = new Rectangle(clipRect.X + clipXoffset, clipRect.Y + clipYoffset, clipRect.Width, clipRect.Height);
            var g = Graphics.FromImage(smallBitMap);
            g.DrawImage(bigImage, new Rectangle(0, 0, smallBitMap.Width, smallBitMap.Height), clipRect1, GraphicsUnit.Pixel);
            g.Save();
            g.Dispose();
            //找红 ,蓝点,



            if(DebugMode)
                switch (posi)
                {
                    case 截图的位置.上图左:
                        smallBitMap.Save("resources\\output上左.png");
                        break;
                    case 截图的位置.上图右:
                        smallBitMap.Save("resources\\output上右.png");
                        break;
                    case 截图的位置.下图左:
                        smallBitMap.Save("resources\\output下左.png");
                        break;
                    case 截图的位置.下图右:
                        smallBitMap.Save("resources\\output下右.png");
                        break;
                    default:
                        break;
                }


            var result = BLL_Algorithms.GetCurve2Points(smallBitMap, (int)this.UI_RedLogic.Value, (int)this.UI_BlueLogic.Value, 3);

            var pointRB = result.Item1;

            Point OriginRedPoint, OriginBluePoint, CaptureRedPoint, CaptureBluePoint;
            //大图坐标
            OriginRedPoint = new Point(pointRB[0].X + clipRect1.X, pointRB[0].Y + clipRect1.Y);
            OriginBluePoint = new Point(pointRB[1].X + clipRect1.X, pointRB[1].Y + clipRect1.Y);
            //小图坐标
            CaptureRedPoint = new Point(pointRB[0].X, pointRB[0].Y);
            CaptureBluePoint = new Point(pointRB[1].X, pointRB[1].Y);

            smallBitMap.Dispose();
            return new RBCurvePositons(OriginRedPoint, OriginBluePoint, CaptureRedPoint, CaptureBluePoint, result.Item2);
        }

        private RBCurvePositons GetRB_PointAllMetric(Bitmap bigImage, Size clipSize , 截图的位置 posi)
        {
            Point clipLP = Point.Empty;
            //Size clipSize = new Size(4, 241);

            //从文件名上抓取
            string FaceR = infoPdfMeasure.FaceR;
            switch (FaceR)
            {
                case "R1":
                    //要改
                    //clipLP = new Point(436, 636);
                    //clipLP = new Point(403, 516);
                    clipLP = new Point(405, 456);
                    break;
                case "R2":   //曲线位置往右偏移
                    //clipLP = new Point(403, 1415);
                    clipLP = new Point(405, 1356);
                    break;
                default:
                    break;
            }
            Rectangle clipRect = new Rectangle(clipLP, clipSize);


            Bitmap smallBitMap = new Bitmap(clipRect.Width, clipRect.Height);
            int clipXoffset = 0;
            int clipYoffset = 0;
            switch (posi)
            {
                case 截图的位置.上图左:
                    clipXoffset = 0;
                    clipYoffset = 0;
                    break;
                case 截图的位置.上图右:
                    clipXoffset = 745; //1150 - 405
                    clipYoffset = 0;
                    break;
                case 截图的位置.下图左:
                    clipXoffset = 0;
                    clipYoffset = 900;
                    break;
                case 截图的位置.下图右:
                    clipXoffset = 745;// 1150 - 405; 
                    clipYoffset = 900;//1356-456;
                    break;
                default:
                    break;
            }


            //截取的区域
            Rectangle clipRect1 = new Rectangle(clipRect.X + clipXoffset, clipRect.Y + clipYoffset, clipRect.Width, clipRect.Height);
            var g = Graphics.FromImage(smallBitMap);
            g.DrawImage(bigImage, new Rectangle(0, 0, smallBitMap.Width, smallBitMap.Height), clipRect1, GraphicsUnit.Pixel);
            g.Save();
            g.Dispose();
            //找红 ,蓝点,

            Directory.CreateDirectory("resources");

            if (DebugMode)
                switch (posi)
                {
                    case 截图的位置.上图左:
                        smallBitMap.Save("resources\\output上左.png");
                        break;
                    case 截图的位置.上图右:
                        smallBitMap.Save("resources\\output上右.png");
                        break;
                    case 截图的位置.下图左:
                        smallBitMap.Save("resources\\output下左.png");
                        break;
                    case 截图的位置.下图右:
                        smallBitMap.Save("resources\\output下右.png");
                        break;
                    default:
                        break;
                }


            //老代码  var result = BLL_Algorithms.GetCurve2Points(smallBitMap, (int)this.UI_RedLogic.Value, (int)this.UI_BlueLogic.Value, 3);
            var result = BLL_Algorithms.GetCurve2PointsBigClip(smallBitMap, 20, 20, 3); 

            var pointRB = result.Item1;

            Point OriginRedPoint, OriginBluePoint, CaptureRedPoint, CaptureBluePoint;
            //大图坐标
            OriginRedPoint = new Point(pointRB[0].X + clipRect1.X, pointRB[0].Y + clipRect1.Y);
            OriginBluePoint = new Point(pointRB[1].X + clipRect1.X, pointRB[1].Y + clipRect1.Y);
            //小图坐标
            CaptureRedPoint = new Point(pointRB[0].X, pointRB[0].Y);
            CaptureBluePoint = new Point(pointRB[1].X, pointRB[1].Y);

            smallBitMap.Dispose();
            return new RBCurvePositons(OriginRedPoint, OriginBluePoint, CaptureRedPoint, CaptureBluePoint, result.Item2);
        }


        public string ReadPdBySpire(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

                //加载PDF文档
                doc.LoadFromFile(fileName);

                //实例化一个StringBuilder 对象
                StringBuilder content = new StringBuilder();

                //提取PDF所有页面的文本
                foreach (Spire.Pdf.PdfPageBase page in doc.Pages)
                {
                    content.Append(page.ExtractText());
                }

                return content.ToString();
            }
            else
            {
                MessageBox.Show("文件不存在");
            }

            return "";
        }


        //public string ReadPdfByiTextSharp(string fileName)
        //{
        //    if (System.IO.File.Exists(fileName))
        //    {
        //        //
        //        StringBuilder sbFileContent = new StringBuilder();
        //        //打开文件
        //        iTextSharp.text.pdf.PdfReader reader = null;

        //        reader = new iTextSharp.text.pdf.PdfReader(fileName);

        //        //循环各页（索引从1开始）
        //        for (int i = 1; i <= reader.NumberOfPages; i++)
        //        {
        //            sbFileContent.AppendLine(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i) + "\r\n");

        //            ////itextsharp.
        //            ////sbFileContent.AppendLine((reader, i));
        //        }


        //        sbFileContent = sbFileContent.Replace("\n", "\r\n");

        //        //LogHandler.LogWrite(string.Format(@"解析PDF文件{0}失败,错误:{1}", new string[] { fileName, ex.ToString() }));

        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader = null;
        //        }
        //        //
        //        return sbFileContent.ToString();

        //    }
        //    MessageBox.Show("文件不存在");
        //    return "";
        ////}


        ///// <summary>
        ///// 获取PDF页数
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public int GetPdfPageCount(string fileName)
        //{
        //    PdfReader reader = null;
        //    reader = new PdfReader(fileName);
        //    ////LogHandler.LogWrite(string.Format(@"加载PDF文件{0}失败,错误:{1}", new string[] { fileName, ex.ToString() }));
        //    reader.Close();

        //    return reader.NumberOfPages;
        //}

        bool frmclosing = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmclosing = true;
            getPdfDataSuccess = true;
            //viewer.axAcroPDF1.Dispose();
        }


        private void btnPdfPath_Click(object sender, EventArgs e)
        {
            ShowPDFDirDialog();
        }


        bool inputPathAlready = false;
        bool outPathAlready = false;

        private bool ShowPDFDirDialog()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;

            PdfFile.pathPdfDataFolder = "";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                PdfFile.pathPdfDataFolder = fbd.SelectedPath;
                this.UI_PDFFolder.Text = PdfFile.pathPdfDataFolder;
                inputPathAlready = true;
                return true;
            }
            else
            {
                inputPathAlready = false;
                PdfFile.pathPdfDataFolder = "";
                this.UI_PDFFolder.Text = "pdf文件夹:";
                return false;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void UI_BlueLogic_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnExcelFilePath(object sender, EventArgs e)
        {
            OpenExcelFileDialog();
        }

        private bool OpenExcelFileDialog()
        {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "2007电子表格|*.xlsx|电子表格|*xls";
                ofd.InitialDirectory = "D:";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //{
                    //    PdfFileInfo.pathPdfDataRoot = fbd.SelectedPath;
                    //    PdfFileInfo.pathPdfDataRootDay = PdfFileInfo.pathPdfDataRoot + "\\A Ban";
                    //    PdfFileInfo.pathPdfDataRootNight = PdfFileInfo.pathPdfDataRoot + "\\B Ban";
                    PdfFile.pathOutPutExcel = ofd.FileName;

                    this.UI_excelPath.Text = PdfFile.pathOutPutExcel;

                    outPathAlready = true;
                }
                else
                {
                    this.UI_excelPath.Text = "excel路径:";
                    outPathAlready = false;
                    EndMainProcedure();
                    return false;
                }

            try
            {
                workbook?.Dispose();
                workbook = new Workbook(PdfFile.pathOutPutExcel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                outPathAlready = false;
                this.UI_excelPath.Text = "excel路径:";
                EndMainProcedure();
                return false;
            }

            return true;
        }



        private void OldWriteExcelMethod(DayOrNight dn, MemoryStream msThumbImage, int R2offset, Worksheet wb, string BaseFormula)
        {
            //早上,与晚上 写入电子表格 的 坐标不一样
            if (dn == DayOrNight.Day)            // R2时+Row偏移, 得到R2,单元格
            {
                //if (NextDay)
                //{
                //    m_R1ColImg += 1;
                //    m_R1ColDPV += 1;
                //    m_R1ColDRMS += 1;
                //    m_R1ColBRMS += 1;
                //    NextDay = false;
                //}

                if (!R2First && infoPdfMeasure.FaceR == "R2")
                {
                    ParseDayTimer = 1;
                    R2First = true;
                }
                var widx = int.Parse(infoPdfMeasure.Workid.Replace("#", ""));
                if (widx != ParseDayTimer) //没有对应的 穴号就 ,留空 向下留空(6行)
                {
                    m_R1RowImg += 6;
                    m_R1RowDPV += 6;
                    m_R1RowDRMS += 6;
                    m_R1RowBRMS += 6;
                    isJumpRows = true;
                }
                var TestResultRow = m_R1RowDPV + 4 + R2offset;

                //这是相对 公式 , 相对于当前单元格的公式, 只要赋值了该公式就任何地方都能自动变换成 对应的Excel公式


                wb.Cells[TestResultRow, m_R1ColDPV + DayColOffset].R1C1Formula = BaseFormula;

                wb.Pictures.Add(m_R1RowImg + R2offset, m_R1ColImg + DayColOffset, m_R1RowImg + R2offset + 1, m_R1ColImg + DayColOffset + 1, msThumbImage);

                wb.Pictures[wb.Pictures.Count - 1].Left += 3;
                wb.Pictures[wb.Pictures.Count - 1].Top += 3;
                wb.Pictures[wb.Pictures.Count - 1].Width -= 3;
                wb.Pictures[wb.Pictures.Count - 1].Height -= 3;

                //wb.Cells[m_R1RowDPV + R2offset, m_R1ColDPV + DayColOffset].PutValue(infoMeasure.Workid);
                wb.Cells[m_R1RowDPV + R2offset, m_R1ColDPV + DayColOffset].PutValue(infoPdfMeasure.DPV);
                wb.Cells[m_R1RowDRMS + R2offset, m_R1ColDRMS + DayColOffset].PutValue(infoPdfMeasure.DRMS);
                wb.Cells[m_R1RowBRMS + R2offset, m_R1ColBRMS + DayColOffset].PutValue(infoPdfMeasure.BRMS);
                var RowDiff = m_R1RowBRMS + R2offset + 1; //下一行
                wb.Cells[RowDiff, m_R1ColBRMS + DayColOffset].PutValue(infoPdfMeasure.Diff.ToString("N2"));

            }
            else
            {
                var TestResultRow = m_R1RowDPV + 4 + R2offset;
                //wb.Cells[TestResultRow, m_R1ColDPV + DayColOffset+1].R1C1Formula = "=IF(R[-4]C[-1]>R[-4]C3,\"NG\",\"OK\")";

                wb.Cells[TestResultRow, m_R1ColDPV + DayColOffset + 1].R1C1Formula = BaseFormula;

                //CellHelper.FormulaTransform(

                //+1 ,+2 是因为向右偏移一个单元格
                wb.Pictures.Add(m_R1RowImg + R2offset, m_R1ColImg + DayColOffset + 1, m_R1RowImg + R2offset + 1, m_R1ColImg + DayColOffset + 2, msThumbImage);
                wb.Pictures[wb.Pictures.Count - 1].Left += 3;
                wb.Pictures[wb.Pictures.Count - 1].Top += 3;
                wb.Pictures[wb.Pictures.Count - 1].Width -= 3;
                wb.Pictures[wb.Pictures.Count - 1].Height -= 3;


                wb.Cells[m_R1RowDPV + R2offset, m_R1ColDPV + DayColOffset + 1].PutValue(infoPdfMeasure.DPV);
                wb.Cells[m_R1RowDRMS + R2offset, m_R1ColDRMS + DayColOffset + 1].PutValue(infoPdfMeasure.DRMS);
                wb.Cells[m_R1RowBRMS + R2offset, m_R1ColBRMS + DayColOffset + 1].PutValue(infoPdfMeasure.BRMS);
                var RowDiff = m_R1RowBRMS + R2offset + 1; //下一行
                wb.Cells[RowDiff, m_R1ColBRMS + DayColOffset + 1].PutValue(infoPdfMeasure.Diff.ToString("N2"));
                if (!isJumpRows)
                {
                    m_R1RowImg += 6;
                    m_R1RowDPV += 6;
                    m_R1RowDRMS += 6;
                    m_R1RowBRMS += 6;
                }
                else
                {
                    isJumpRows = false;
                }
            }
        }

        private void OldGetPdfFileNameData(FileInfo fi)
        {
            var namexx = fi.Name;
            namexx = namexx.Replace(fi.Extension, "");
            var atts = namexx.Split('-');


            infoPdfMeasure.MachineName = atts[0];

            if (atts[1] == "R1")
                infoPdfMeasure.FaceR = atts[1];
            else if (atts[1] == "R2")
                infoPdfMeasure.FaceR = atts[1];

            infoPdfMeasure.Workid = atts[2];
        }

        private void btnOpenExcel_Click(object sender, EventArgs e)
        {
            Process.Start("excel", PdfFile.pathOutPutExcel);
        }
    }
}

