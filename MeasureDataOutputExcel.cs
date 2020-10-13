using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdfExtrator
{
    public partial class MeasureDataOutputExcel : Form
    {
        public MeasureDataOutputExcel()
        {
            InitializeComponent();
        }

        string ParsePath = "";

        private void btnParser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (Directory.Exists("d:"))
                fbd.SelectedPath = "d:";
            else
                fbd.SelectedPath = "c:";

            if (Directory.Exists("z:"))
            {
                fbd.SelectedPath = "z:";
            }

            if (fbd.ShowDialog() == DialogResult.OK)
            {

                //MessageBox.Show(fbd.SelectedPath);
                tbxURL.Text = fbd.SelectedPath;
                ParsePath = tbxURL.Text;
            }

            string ABanPath = ParsePath + "\\A Ban";

            string [] paths=Directory.GetFiles(ABanPath, "*#.pdf");

            //for (int i = 0; i < paths.Length; i++)
            //{
            //    paths[i]

            //}


        }

        private void MeasureDataOutputExcel_Load(object sender, EventArgs e)
        {

        }
    }
}
