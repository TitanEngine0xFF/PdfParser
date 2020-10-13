using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdfExtrator.Model
{
    public enum DayOrNight
    {
        Day,
        Night,
    }
    public class MeasurePdfData
    {
        public string PdfPath = "";
        public DayOrNight dayOrNight = DayOrNight.Day;
        /// <summary>
        /// 默认值: QL288
        /// </summary>
        public string MachineName = "QL288";

        /// <summary>
        /// R1 or R2
        /// </summary>
        public string FaceR = "R1";

        /// <summary>
        ///  已弃用 请使用 WId
        /// </summary>
        [Obsolete("已弃用,请使用Wid")]
        public string Workid = "1#";
        public int WId = 0;

        public decimal DPV = 0;
        public decimal DRMS = 0;
        public decimal BRMS = 0;
        public decimal Diff = 0;

        public DateTime MeasuDateTime = new DateTime();
        //把 8点之前的 也算前天夜班
        public DateTime RealDate = new DateTime();

        public decimal MetricMaxVal = 0;

        public MeasurePdfData(string machineName, string faceR, string workid , DayOrNight _dayOrNight)
        {
            MachineName = machineName;
            FaceR = faceR;
            Workid = workid;
            dayOrNight = _dayOrNight;
        }

        public MeasurePdfData()
        {

        }

        public void ResetALL()
        {
            PdfPath ="";
            this.dayOrNight = DayOrNight.Day;
            MachineName = "";
            FaceR = "";
            Workid = "";
            WId = 0;
            DPV = 0;
            DRMS = 0;
            BRMS = 0;
            Diff = 0;
            MeasuDateTime = DateTime.MinValue;
            RealDate = DateTime.MinValue;
            MetricMaxVal = 0;
        }



        /// <summary>
        ///  
        /// </summary>
        /// <param name="dPV"></param>
        /// <param name="dRMS"></param>
        /// <param name="bRMS"></param>
        /// <param name="measuDateTime"></param>
        /// <param name="measuTime"></param>
        public void SetData(decimal dPV, decimal dRMS, decimal bRMS, DateTime measuDateTime)
        {
            DPV = dPV;
            DRMS = dRMS;
            BRMS = bRMS;
            MeasuDateTime = measuDateTime;
            
        }

        

    }
}
