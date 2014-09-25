namespace HydroSense
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelModelInput = new System.Windows.Forms.Label();
            this.labelModelOutput = new System.Windows.Forms.Label();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.buttonInput = new System.Windows.Forms.Button();
            this.buttonOutput = new System.Windows.Forms.Button();
            this.labelMaxIter = new System.Windows.Forms.Label();
            this.textBoxMaxIter = new System.Windows.Forms.TextBox();
            this.labelTolerance = new System.Windows.Forms.Label();
            this.textBoxTolerance = new System.Windows.Forms.TextBox();
            this.buttonViewLog = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.openFileDialogInput = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogExcel = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Run = new System.Windows.Forms.TabPage();
            this.Advanced = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDeltaD = new System.Windows.Forms.TextBox();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.Readme = new System.Windows.Forms.TabPage();
            this.richTextBoxReadme = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Run.SuspendLayout();
            this.Advanced.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.Readme.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.Controls.Add(this.labelModelInput, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelModelOutput, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxInput, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxOutput, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonInput, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonOutput, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelMaxIter, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxMaxIter, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelTolerance, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxTolerance, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(564, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelModelInput
            // 
            this.labelModelInput.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelModelInput.AutoSize = true;
            this.labelModelInput.Location = new System.Drawing.Point(3, 20);
            this.labelModelInput.Name = "labelModelInput";
            this.labelModelInput.Size = new System.Drawing.Size(129, 13);
            this.labelModelInput.TabIndex = 0;
            this.labelModelInput.Text = "Model Input Spreadsheet:";
            // 
            // labelModelOutput
            // 
            this.labelModelOutput.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelModelOutput.AutoSize = true;
            this.labelModelOutput.Location = new System.Drawing.Point(3, 50);
            this.labelModelOutput.Name = "labelModelOutput";
            this.labelModelOutput.Size = new System.Drawing.Size(137, 13);
            this.labelModelOutput.TabIndex = 1;
            this.labelModelOutput.Text = "Model Output Spreadsheet:";
            // 
            // textBoxInput
            // 
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.Location = new System.Drawing.Point(148, 17);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(328, 20);
            this.textBoxInput.TabIndex = 2;
            this.textBoxInput.TextChanged += new System.EventHandler(this.textBoxInput_TextChanged);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.Location = new System.Drawing.Point(148, 47);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(328, 20);
            this.textBoxOutput.TabIndex = 3;
            this.textBoxOutput.TextChanged += new System.EventHandler(this.textBoxOutput_TextChanged);
            // 
            // buttonInput
            // 
            this.buttonInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInput.Location = new System.Drawing.Point(482, 15);
            this.buttonInput.Name = "buttonInput";
            this.buttonInput.Size = new System.Drawing.Size(79, 24);
            this.buttonInput.TabIndex = 4;
            this.buttonInput.Text = "Open";
            this.buttonInput.UseVisualStyleBackColor = true;
            this.buttonInput.Click += new System.EventHandler(this.buttonInput_Click);
            // 
            // buttonOutput
            // 
            this.buttonOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutput.Location = new System.Drawing.Point(482, 45);
            this.buttonOutput.Name = "buttonOutput";
            this.buttonOutput.Size = new System.Drawing.Size(79, 24);
            this.buttonOutput.TabIndex = 5;
            this.buttonOutput.Text = "Save";
            this.buttonOutput.UseVisualStyleBackColor = true;
            this.buttonOutput.Click += new System.EventHandler(this.buttonOutput_Click);
            // 
            // labelMaxIter
            // 
            this.labelMaxIter.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelMaxIter.AutoSize = true;
            this.labelMaxIter.Location = new System.Drawing.Point(3, 80);
            this.labelMaxIter.Name = "labelMaxIter";
            this.labelMaxIter.Size = new System.Drawing.Size(117, 13);
            this.labelMaxIter.TabIndex = 10;
            this.labelMaxIter.Text = "Max Solution Iterations:";
            // 
            // textBoxMaxIter
            // 
            this.textBoxMaxIter.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxMaxIter.Location = new System.Drawing.Point(148, 77);
            this.textBoxMaxIter.Name = "textBoxMaxIter";
            this.textBoxMaxIter.Size = new System.Drawing.Size(50, 20);
            this.textBoxMaxIter.TabIndex = 11;
            this.textBoxMaxIter.Text = "5000";
            this.textBoxMaxIter.TextChanged += new System.EventHandler(this.textBoxMaxIter_TextChanged);
            // 
            // labelTolerance
            // 
            this.labelTolerance.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTolerance.AutoSize = true;
            this.labelTolerance.Location = new System.Drawing.Point(3, 110);
            this.labelTolerance.Name = "labelTolerance";
            this.labelTolerance.Size = new System.Drawing.Size(125, 13);
            this.labelTolerance.TabIndex = 12;
            this.labelTolerance.Text = "Convergence Tolerance:";
            // 
            // textBoxTolerance
            // 
            this.textBoxTolerance.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxTolerance.Location = new System.Drawing.Point(148, 107);
            this.textBoxTolerance.Name = "textBoxTolerance";
            this.textBoxTolerance.Size = new System.Drawing.Size(50, 20);
            this.textBoxTolerance.TabIndex = 14;
            this.textBoxTolerance.Text = "0.015";
            this.textBoxTolerance.TextChanged += new System.EventHandler(this.textBoxTolerance_TextChanged);
            // 
            // buttonViewLog
            // 
            this.buttonViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonViewLog.Enabled = false;
            this.buttonViewLog.Location = new System.Drawing.Point(3, 303);
            this.buttonViewLog.Name = "buttonViewLog";
            this.buttonViewLog.Size = new System.Drawing.Size(75, 34);
            this.buttonViewLog.TabIndex = 9;
            this.buttonViewLog.Text = "View Log";
            this.buttonViewLog.UseVisualStyleBackColor = true;
            this.buttonViewLog.Click += new System.EventHandler(this.buttonViewLog_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuit.Location = new System.Drawing.Point(412, 303);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(75, 34);
            this.buttonQuit.TabIndex = 7;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonRun.Location = new System.Drawing.Point(499, 303);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 34);
            this.buttonRun.TabIndex = 6;
            this.buttonRun.Text = "Run Solver";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // openFileDialogInput
            // 
            this.openFileDialogInput.Filter = "Excel file (*.xls(x))|*.xls;*.xlsx|All files (*.*)|*.*";
            // 
            // saveFileDialogExcel
            // 
            this.saveFileDialogExcel.Filter = "Excel file (*.xls(x))|*.xls;*.xlsx|All files (*.*)|*.*";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 340);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar.Visible = false;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel2.Controls.Add(this.buttonViewLog, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonQuit, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonRun, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(584, 340);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.Run);
            this.tabControl1.Controls.Add(this.Advanced);
            this.tabControl1.Controls.Add(this.Readme);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(578, 294);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // Run
            // 
            this.Run.Controls.Add(this.tableLayoutPanel1);
            this.Run.Location = new System.Drawing.Point(4, 22);
            this.Run.Name = "Run";
            this.Run.Padding = new System.Windows.Forms.Padding(3);
            this.Run.Size = new System.Drawing.Size(570, 268);
            this.Run.TabIndex = 0;
            this.Run.Text = "Run";
            this.Run.UseVisualStyleBackColor = true;
            // 
            // Advanced
            // 
            this.Advanced.Controls.Add(this.tableLayoutPanel3);
            this.Advanced.Location = new System.Drawing.Point(4, 22);
            this.Advanced.Name = "Advanced";
            this.Advanced.Padding = new System.Windows.Forms.Padding(3);
            this.Advanced.Size = new System.Drawing.Size(570, 268);
            this.Advanced.TabIndex = 1;
            this.Advanced.Text = "Advanced";
            this.Advanced.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.textBoxDeltaD, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonDefault, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(564, 262);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Numerical Derivative Increment:";
            // 
            // textBoxDeltaD
            // 
            this.textBoxDeltaD.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxDeltaD.Location = new System.Drawing.Point(168, 17);
            this.textBoxDeltaD.Name = "textBoxDeltaD";
            this.textBoxDeltaD.Size = new System.Drawing.Size(50, 20);
            this.textBoxDeltaD.TabIndex = 1;
            this.textBoxDeltaD.Text = "0.01";
            this.textBoxDeltaD.TextChanged += new System.EventHandler(this.textBoxDeltaD_TextChanged);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonDefault.Location = new System.Drawing.Point(243, 15);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(75, 23);
            this.buttonDefault.TabIndex = 2;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // Readme
            // 
            this.Readme.Controls.Add(this.richTextBoxReadme);
            this.Readme.Location = new System.Drawing.Point(4, 22);
            this.Readme.Name = "Readme";
            this.Readme.Size = new System.Drawing.Size(570, 268);
            this.Readme.TabIndex = 2;
            this.Readme.Text = "Readme";
            this.Readme.UseVisualStyleBackColor = true;
            // 
            // richTextBoxReadme
            // 
            this.richTextBoxReadme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxReadme.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxReadme.Name = "richTextBoxReadme";
            this.richTextBoxReadme.ReadOnly = true;
            this.richTextBoxReadme.Size = new System.Drawing.Size(570, 268);
            this.richTextBoxReadme.TabIndex = 0;
            this.richTextBoxReadme.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainForm";
            this.Text = "Hydro$ense";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.Run.ResumeLayout(false);
            this.Advanced.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.Readme.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelModelInput;
        private System.Windows.Forms.Label labelModelOutput;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.OpenFileDialog openFileDialogInput;
        private System.Windows.Forms.Button buttonInput;
        private System.Windows.Forms.Button buttonOutput;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExcel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Button buttonViewLog;
        private System.Windows.Forms.Label labelMaxIter;
        private System.Windows.Forms.TextBox textBoxMaxIter;
        private System.Windows.Forms.Label labelTolerance;
        private System.Windows.Forms.TextBox textBoxTolerance;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Run;
        private System.Windows.Forms.TabPage Advanced;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDeltaD;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.TabPage Readme;
        private System.Windows.Forms.RichTextBox richTextBoxReadme;

    }
}

