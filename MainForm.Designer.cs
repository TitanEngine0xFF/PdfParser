namespace pdfExtrator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tbxOutPut = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxRBPoint = new System.Windows.Forms.TextBox();
            this.UI_RedLogic = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.UI_BlueLogic = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelDebug = new System.Windows.Forms.Panel();
            this.cbxTestImport = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ui_Testnum = new System.Windows.Forms.NumericUpDown();
            this.processBar = new System.Windows.Forms.ProgressBar();
            this.UI_PDFFolder = new System.Windows.Forms.Label();
            this.UI_excelPath = new System.Windows.Forms.Label();
            this.btnPdfPath = new System.Windows.Forms.Button();
            this.btnExtra = new System.Windows.Forms.Button();
            this.UI_pdfCar = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.UI_StatusText = new System.Windows.Forms.Label();
            this.btnOpenExcel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UI_RedLogic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UI_BlueLogic)).BeginInit();
            this.panelDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_Testnum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UI_pdfCar)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxOutPut
            // 
            this.tbxOutPut.Location = new System.Drawing.Point(448, 2);
            this.tbxOutPut.Multiline = true;
            this.tbxOutPut.Name = "tbxOutPut";
            this.tbxOutPut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxOutPut.Size = new System.Drawing.Size(338, 314);
            this.tbxOutPut.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 70F);
            this.label1.Location = new System.Drawing.Point(26, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 107);
            this.label1.TabIndex = 5;
            this.label1.Text = "处理中...";
            this.label1.Visible = false;
            // 
            // tbxRBPoint
            // 
            this.tbxRBPoint.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.tbxRBPoint.Location = new System.Drawing.Point(448, 322);
            this.tbxRBPoint.Multiline = true;
            this.tbxRBPoint.Name = "tbxRBPoint";
            this.tbxRBPoint.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxRBPoint.Size = new System.Drawing.Size(338, 521);
            this.tbxRBPoint.TabIndex = 3;
            this.tbxRBPoint.Text = "tbxRBPoint";
            // 
            // UI_RedLogic
            // 
            this.UI_RedLogic.Location = new System.Drawing.Point(125, 3);
            this.UI_RedLogic.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.UI_RedLogic.Name = "UI_RedLogic";
            this.UI_RedLogic.Size = new System.Drawing.Size(40, 21);
            this.UI_RedLogic.TabIndex = 6;
            this.UI_RedLogic.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "G,B<X 判为红点 X:";
            // 
            // UI_BlueLogic
            // 
            this.UI_BlueLogic.Location = new System.Drawing.Point(125, 30);
            this.UI_BlueLogic.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.UI_BlueLogic.Name = "UI_BlueLogic";
            this.UI_BlueLogic.Size = new System.Drawing.Size(40, 21);
            this.UI_BlueLogic.TabIndex = 6;
            this.UI_BlueLogic.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.UI_BlueLogic.ValueChanged += new System.EventHandler(this.UI_BlueLogic_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "R,G<X 判为蓝点 X:";
            // 
            // panelDebug
            // 
            this.panelDebug.Controls.Add(this.cbxTestImport);
            this.panelDebug.Controls.Add(this.label1);
            this.panelDebug.Controls.Add(this.pictureBox1);
            this.panelDebug.Controls.Add(this.label4);
            this.panelDebug.Controls.Add(this.label3);
            this.panelDebug.Controls.Add(this.ui_Testnum);
            this.panelDebug.Controls.Add(this.tbxOutPut);
            this.panelDebug.Controls.Add(this.UI_BlueLogic);
            this.panelDebug.Controls.Add(this.tbxRBPoint);
            this.panelDebug.Controls.Add(this.label2);
            this.panelDebug.Controls.Add(this.UI_RedLogic);
            this.panelDebug.Location = new System.Drawing.Point(963, 12);
            this.panelDebug.Name = "panelDebug";
            this.panelDebug.Size = new System.Drawing.Size(516, 666);
            this.panelDebug.TabIndex = 8;
            this.panelDebug.Visible = false;
            // 
            // cbxTestImport
            // 
            this.cbxTestImport.AutoSize = true;
            this.cbxTestImport.Location = new System.Drawing.Point(14, 61);
            this.cbxTestImport.Name = "cbxTestImport";
            this.cbxTestImport.Size = new System.Drawing.Size(96, 16);
            this.cbxTestImport.TabIndex = 10;
            this.cbxTestImport.Text = "测试二次导入";
            this.cbxTestImport.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(4, 112);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(478, 554);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "测试个数";
            // 
            // ui_Testnum
            // 
            this.ui_Testnum.Location = new System.Drawing.Point(125, 82);
            this.ui_Testnum.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ui_Testnum.Name = "ui_Testnum";
            this.ui_Testnum.Size = new System.Drawing.Size(40, 21);
            this.ui_Testnum.TabIndex = 6;
            this.ui_Testnum.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ui_Testnum.ValueChanged += new System.EventHandler(this.UI_BlueLogic_ValueChanged);
            // 
            // processBar
            // 
            this.processBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.processBar.Location = new System.Drawing.Point(0, 319);
            this.processBar.Name = "processBar";
            this.processBar.Size = new System.Drawing.Size(697, 26);
            this.processBar.TabIndex = 9;
            // 
            // UI_PDFFolder
            // 
            this.UI_PDFFolder.AutoSize = true;
            this.UI_PDFFolder.Location = new System.Drawing.Point(244, 43);
            this.UI_PDFFolder.Name = "UI_PDFFolder";
            this.UI_PDFFolder.Size = new System.Drawing.Size(65, 12);
            this.UI_PDFFolder.TabIndex = 7;
            this.UI_PDFFolder.Text = "pdf文件夹:";
            // 
            // UI_excelPath
            // 
            this.UI_excelPath.AutoSize = true;
            this.UI_excelPath.Location = new System.Drawing.Point(244, 122);
            this.UI_excelPath.Name = "UI_excelPath";
            this.UI_excelPath.Size = new System.Drawing.Size(65, 12);
            this.UI_excelPath.TabIndex = 7;
            this.UI_excelPath.Text = "excel路径:";
            // 
            // btnPdfPath
            // 
            this.btnPdfPath.BackgroundImage = global::pdfExtrator.Properties.Resources.pdf_128px_1176741_easyicon_net;
            this.btnPdfPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPdfPath.Location = new System.Drawing.Point(75, 20);
            this.btnPdfPath.Name = "btnPdfPath";
            this.btnPdfPath.Size = new System.Drawing.Size(130, 132);
            this.btnPdfPath.TabIndex = 1;
            this.btnPdfPath.UseVisualStyleBackColor = true;
            this.btnPdfPath.Click += new System.EventHandler(this.btnPdfPath_Click);
            // 
            // btnExtra
            // 
            this.btnExtra.BackgroundImage = global::pdfExtrator.Properties.Resources.Excel_2013_256px_1180012_easyicon_net;
            this.btnExtra.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExtra.Location = new System.Drawing.Point(503, 20);
            this.btnExtra.Name = "btnExtra";
            this.btnExtra.Size = new System.Drawing.Size(130, 132);
            this.btnExtra.TabIndex = 1;
            this.btnExtra.UseVisualStyleBackColor = true;
            this.btnExtra.Click += new System.EventHandler(this.btnExcelFilePath);
            // 
            // UI_pdfCar
            // 
            this.UI_pdfCar.BackgroundImage = global::pdfExtrator.Properties.Resources.pdf_128px_1205624_easyicon_net;
            this.UI_pdfCar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.UI_pdfCar.Location = new System.Drawing.Point(120, 58);
            this.UI_pdfCar.Name = "UI_pdfCar";
            this.UI_pdfCar.Size = new System.Drawing.Size(46, 55);
            this.UI_pdfCar.TabIndex = 10;
            this.UI_pdfCar.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(96, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 28);
            this.label5.TabIndex = 7;
            this.label5.Text = "第一步";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // btnRun
            // 
            this.btnRun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRun.Font = new System.Drawing.Font("微软雅黑", 20F);
            this.btnRun.Location = new System.Drawing.Point(275, 227);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(168, 62);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "执行";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(537, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 28);
            this.label6.TabIndex = 12;
            this.label6.Text = "第二步";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(82, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 17);
            this.label7.TabIndex = 7;
            this.label7.Text = "(选择pdf报告文件夹)";
            this.label7.Click += new System.EventHandler(this.label5_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(499, 183);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "(选择输出excel报告文件)";
            this.label8.Click += new System.EventHandler(this.label5_Click);
            // 
            // UI_StatusText
            // 
            this.UI_StatusText.AutoSize = true;
            this.UI_StatusText.Location = new System.Drawing.Point(1, 304);
            this.UI_StatusText.Name = "UI_StatusText";
            this.UI_StatusText.Size = new System.Drawing.Size(59, 12);
            this.UI_StatusText.TabIndex = 13;
            this.UI_StatusText.Text = "状态:待机";
            // 
            // btnOpenExcel
            // 
            this.btnOpenExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOpenExcel.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.btnOpenExcel.Location = new System.Drawing.Point(503, 227);
            this.btnOpenExcel.Name = "btnOpenExcel";
            this.btnOpenExcel.Size = new System.Drawing.Size(182, 62);
            this.btnOpenExcel.TabIndex = 1;
            this.btnOpenExcel.Text = "打开输出表格";
            this.btnOpenExcel.UseVisualStyleBackColor = true;
            this.btnOpenExcel.Click += new System.EventHandler(this.btnOpenExcel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 345);
            this.Controls.Add(this.UI_StatusText);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.processBar);
            this.Controls.Add(this.btnOpenExcel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnPdfPath);
            this.Controls.Add(this.btnExtra);
            this.Controls.Add(this.panelDebug);
            this.Controls.Add(this.UI_pdfCar);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.UI_excelPath);
            this.Controls.Add(this.UI_PDFFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(713, 384);
            this.MinimumSize = new System.Drawing.Size(713, 384);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF报表生成工具 1.7";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UI_RedLogic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UI_BlueLogic)).EndInit();
            this.panelDebug.ResumeLayout(false);
            this.panelDebug.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_Testnum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UI_pdfCar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnExtra;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbxOutPut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxRBPoint;
        private System.Windows.Forms.NumericUpDown UI_RedLogic;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown UI_BlueLogic;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelDebug;
        private System.Windows.Forms.Button btnPdfPath;
        private System.Windows.Forms.ProgressBar processBar;
        private System.Windows.Forms.CheckBox cbxTestImport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ui_Testnum;
        private System.Windows.Forms.Label UI_PDFFolder;
        private System.Windows.Forms.Label UI_excelPath;
        private System.Windows.Forms.PictureBox UI_pdfCar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label UI_StatusText;
        private System.Windows.Forms.Button btnOpenExcel;
    }
}