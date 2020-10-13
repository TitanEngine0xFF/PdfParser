
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdfExtrator
{
    public partial class PDFViewer : Form
    {
        public PDFViewer()
        {
            InitializeComponent();
        }

        public Bitmap bmp1 = new Bitmap(964 - 56, 862 - 123);
        public Bitmap bmp2 = new Bitmap(964-56, 862-123);


        private void PDFViewer_Load(object sender, EventArgs e)
        {
            //ReadPdfString()

            this.BeginInvoke(new Action(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                while (true)
                {
                    if (stopwatch.ElapsedMilliseconds > 300)
                    {
                        break;
                    }
                    Application.DoEvents();
                    Thread.Sleep(10);
                }

                //axAcroPDF1.setViewRect(120, 450, 330, 0);
                Thread.Sleep(250);
                using (Graphics g = Graphics.FromImage(bmp1))
                {
                    g.CopyFromScreen(56, 123, 0, 0, new Size(964, 862));

                    //pictureBox1.Image = bmp;
                }

                //axAcroPDF1.setViewRect(120, 130, 330, 0);
                Thread.Sleep(250);
                using (Graphics g = Graphics.FromImage(bmp2))
                {
                    g.CopyFromScreen(53, 87, 0, 0, new Size(995, 847));

                    //pictureBox1.Image = bmp;
                }
                Thread.Sleep(200);
                //this.Close();
            }));
        }

        //private void ReadPdfString()
        //{
        //    iTextSharp.text.pdf.PdfReader pdf = new iTextSharp.text.pdf.PdfReader("1.pdf");
        //    byte[] b = null;
        //    for (int i = 0; i < pdf.NumberOfPages; i++)
        //    {
        //        ////pdf.GetPageResources(i).GetAsString(new iTextSharp.text.pdf.PdfName(""
        //        Array.Resize(ref b, 0);
        //        b = pdf.GetPageContent(i);
        //    }

        //    PdfReaderContentParser parser = new PdfReaderContentParser(pdf);

        //    ITextExtractionStrategy strategy;
        //    ////strategy = parser.ProcessContent<PdfTextExtractor>(1, PdfTextExtractor.GetTextFromPage);



        //    //将文本内容赋值给一个富文本框
        //    //richTextBox1.Rtf = strategy.GetResultantText();
        //    //richTextBox1.Text = PdfTextExtractor.GetTextFromPage(pdf, 1);
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            //axAcroPDF1.setViewRect(120, 130, 330, 0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
