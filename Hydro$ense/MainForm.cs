using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HydroSense
{
    public partial class MainForm : Form
    {
        string m_inputExcel = "";
        string m_outputExcel = "";
        int m_maxIter;
        double m_tolerance;
        double m_deltad;
        TracySolver m_solver;

        public MainForm()
        {
            InitializeComponent();
            m_maxIter = Convert.ToInt32(textBoxMaxIter.Text);
            m_tolerance = Convert.ToDouble(textBoxTolerance.Text);
            m_deltad = Convert.ToDouble(textBoxDeltaD.Text);
            if (File.Exists("README.rtf"))
                richTextBoxReadme.Rtf = File.ReadAllText("README.rtf");
            else
                richTextBoxReadme.Text = "Error loading README file";
        }

        private void buttonInput_Click(object sender, System.EventArgs e)
        {
            if (openFileDialogInput.ShowDialog() == DialogResult.OK)
            {
                m_inputExcel = openFileDialogInput.FileName;
                textBoxInput.Text = m_inputExcel;
            }
        }

        private void buttonOutput_Click(object sender, System.EventArgs e)
        {
            saveFileDialogExcel.OverwritePrompt = false;
            if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
            {
                m_outputExcel = saveFileDialogExcel.FileName;
                textBoxOutput.Text = m_outputExcel;
            }
        }

        private void buttonViewLog_Click(object sender, EventArgs e)
        {
            string logName = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log";
            string log = Path.Combine(Path.GetTempPath(), logName);
            using (StreamWriter sw = new StreamWriter(log))
            {
                sw.Write(m_solver.log);
            }
            Process.Start(log);
        }

        private void buttonQuit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void buttonRun_Click(object sender, System.EventArgs e)
        {
            if (m_inputExcel == "")
            {
                SetTextInputValid(textBoxInput, false, "Please enter input excel file");
                return;
            }
            if (m_outputExcel == "")
            {
                SetTextInputValid(textBoxOutput, false, "Please enter output excel file");
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            toolStripProgressBar.Visible = true;
            toolStripStatusLabel.Text = "Running...";
            Cursor.Current = Cursors.WaitCursor;
            Refresh();

            ModelInput mi = new ModelInput();
            string msg = mi.ReadFromExcel(m_inputExcel);
            if (msg != string.Empty)
            {
                toolStripProgressBar.Visible = false;
                toolStripStatusLabel.Text = "Input Error: \"" + msg + "\"";
                Cursor.Current = Cursors.Default;
                return;
            }

            m_solver = new TracySolver(mi);
            m_solver.Solve(m_maxIter, m_tolerance, m_deltad);

            ModelOutput mOut = new ModelOutput(mi);
            mOut.ToExcel(m_outputExcel);

            sw.Stop();
            toolStripProgressBar.Visible = false;
            toolStripStatusLabel.Text = String.Format("Run finished in {0:0.00} seconds", sw.Elapsed.TotalSeconds);
            m_solver.log.AppendLine(String.Format("Run finished in {0:0.00} seconds", sw.Elapsed.TotalSeconds));
            Cursor.Current = Cursors.Default;
            buttonViewLog.Enabled = true;
        }

        private void textBoxInput_TextChanged(object sender, System.EventArgs e)
        {
            m_inputExcel = textBoxInput.Text;
            bool validInput = (textBoxInput.Text.Length > 0);
            SetTextInputValid(textBoxInput, validInput, "Please enter input excel file");
        }

        private void textBoxOutput_TextChanged(object sender, System.EventArgs e)
        {
            m_outputExcel = textBoxOutput.Text;
            bool validInput = (textBoxOutput.Text.Length > 0);
            SetTextInputValid(textBoxOutput, validInput, "Please enter output excel file");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to quit?", "",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void textBoxMaxIter_TextChanged(object sender, EventArgs e)
        {
            bool validInput = int.TryParse(textBoxMaxIter.Text, out m_maxIter);
            SetTextInputValid(textBoxMaxIter, validInput, "Invalid input for max solution iterations");
        }

        private void textBoxTolerance_TextChanged(object sender, EventArgs e)
        {
            bool validInput = double.TryParse(textBoxTolerance.Text, out m_tolerance);
            SetTextInputValid(textBoxTolerance, validInput, "Invalid input for convergence tolerance");
        }

        private void textBoxDeltaD_TextChanged(object sender, EventArgs e)
        {
            bool validInput = double.TryParse(textBoxDeltaD.Text, out m_deltad);
            SetTextInputValid(textBoxDeltaD, validInput, "Invalid input for numerical derivative increment");
        }

        private void SetTextInputValid(TextBox txtBox, bool valid, string msg)
        {
            if (valid)
            {
                toolStripStatusLabel.Text = "";
                txtBox.BackColor = Color.White;
                txtBox.ForeColor = Color.Black;
            }
            else
            {
                toolStripStatusLabel.Text = msg;
                txtBox.BackColor = Color.LightYellow;
                txtBox.ForeColor = Color.Red;
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            textBoxDeltaD.Text = "0.01";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["Run"])
            {
                buttonViewLog.Visible = true;
                buttonQuit.Visible = true;
                buttonRun.Visible = true;
            }
            else
            {
                buttonViewLog.Visible = false;
                buttonQuit.Visible = false;
                buttonRun.Visible = false;
            }
        }
        
    }
}
