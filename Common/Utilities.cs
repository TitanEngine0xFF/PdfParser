using Model.File;
using pdfExtrator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commonlib.Utilities
{
    class CommonFuncs
    {
        public static void 分析目录与文件名()
        {
            //这是生成 对应文件名的目录
            DirectoryInfo dinfo = new DirectoryInfo(PdfFile.pathPdfDataFolder);

            //忽略 早晚班 其中一个不存在的pdf ,
            for (int j = 0; j < 2; j++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    string MachineName = dinfo.Name;

                    string RX = "-R1-";
                    if (j == 0)
                        RX = "-R1-";
                    else
                        RX = "-R2-";

                    string dayPdfpath = PdfFile.pathPdfDataRootDay + "\\" + MachineName + RX + i + "#.pdf";
                    string nightPdfpath = PdfFile.pathPdfDataRootNight + "\\" + MachineName + RX + i + "#.pdf";

                    if (!System.IO.File.Exists(nightPdfpath))
                    {
                        continue;
                    }

                    if (!System.IO.File.Exists(dayPdfpath))
                    {

                        //MessageBox.Show(pdfFilePathsDay.Last() + "文件不存在");
                        //this.btnExtra.Enabled = true;
                        //return;
                        continue;
                    }
                    else
                    {
                        if (RX == "-R1-")
                        {
                            //R1Count++;
                        }
                        //PdfFile.GenfileNames.Add(MachineName + RX + i + "#.pdf");
                        PdfFile.PathsDay.Add(dayPdfpath);
                        PdfFile.PathsNight.Add(nightPdfpath);
                    }
                }
            }
        }

        public static string RemoveSurplusSpace(string pdfData)
        {
            var tmpchars = pdfData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder rebuildStr = new StringBuilder();
            for (int i = 0; i < tmpchars.Length; i++)
            {
                var idx1 = tmpchars[i].IndexOf("\r\n");
                if (idx1 != -1)
                {
                    tmpchars[i] = tmpchars[i].Insert(idx1, " ");
                }
                else {
                    tmpchars[i] += " ";
                }
                
                rebuildStr.Append(tmpchars[i]);
            }
            string SkipSurplusSpace = rebuildStr.ToString().Trim();
            return SkipSurplusSpace;
        }
    }
}
