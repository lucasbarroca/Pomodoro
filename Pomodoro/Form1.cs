using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pomodoro
{
    public partial class Form1 : Form
    {
        
        bool IsBreak = false;

        int _tmrSecs;
        int TimerSecs
        {
            get
            {
                return _tmrSecs;
            }

            set
            {
                _tmrSecs = value;

                if (IsBreak)
                    lblTime.Text = GetTimeString(BreakSecs - value);
                else
                    lblTime.Text = GetTimeString(WorkSecs - value);
            }
        }

        int _wrksecs;
        int WorkSecs
        {
            get
            {
                return _wrksecs;
            }

            set
            {
                _wrksecs = value;

                if (!IsBreak)
                    lblTime.Text = GetTimeString(value);
            }
        }

        int _brksecs;
        int BreakSecs
        {
            get
            {
                return _brksecs;
            }

            set
            {
                _brksecs = value;
                if (IsBreak)
                    lblTime.Text = GetTimeString(value);
            }
        }

        string GetTimeString(int secs)
        {
            if (secs > 3599)
                return (secs / 3600) + ":" + ((secs - ((secs / 3600) * 3600)) / 60).ToString("00") + ":" + (secs - ((secs / 3600) * 3600) - (((secs - ((secs / 3600) * 3600)) / 60) * 60)).ToString("00");
            if (secs > 59)
                return (secs / 60).ToString("00") + ":" + (secs - ((secs / 60) * 60)).ToString("00");
            else
                return secs.ToString("00");
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IsBreak = false;

            WorkSecs = 1500;
            BreakSecs = 300;

            TimerSecs = 0;

            TopMost = true;

            menuStrip1.MouseDown += Form1_MouseDown;
            label1.MouseDown+= Form1_MouseDown;

            alwaysVisibleToolStripMenuItem.CheckedChanged += AlwaysVisibleToolStripMenuItem_CheckedChanged;
        }

        private void AlwaysVisibleToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = alwaysVisibleToolStripMenuItem.Checked;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void workToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Value in minutes: ", "Work Time", (WorkSecs / 60).ToString());

            if (!string.IsNullOrEmpty(input))
            {
                int val = 0;
                if (int.TryParse(input, out val))
                    WorkSecs = 60 * val;
            }
        }

        private void breakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Value in minutes: ", "Break Time", (BreakSecs / 60).ToString());

            if (!string.IsNullOrEmpty(input))
            {
                int val = 0;
                if (int.TryParse(input, out val))
                    BreakSecs = 60 * val;
            }
        }

        private void btPlayPause_Click(object sender, EventArgs e)
        {
            if (tmrSec.Enabled)
                Pause();
            else
                Play();
        }

        void Play()
        {
            pgTime.Maximum = IsBreak ? BreakSecs : WorkSecs;
            if (TimerSecs == 0)
                pgTime.Value = 0;

            workToolStripMenuItem.Enabled = false;
            breakToolStripMenuItem.Enabled = false;

            btPlayPause.BackgroundImage = Properties.Resources.pause;
            btPlayPause.BackColor = Color.DeepPink;

            tmrSec.Enabled = true;
        }

        void Pause()
        {
            tmrSec.Enabled = false;
            btPlayPause.BackgroundImage = Properties.Resources.play;
            btPlayPause.BackColor = Color.DarkSeaGreen;

            workToolStripMenuItem.Enabled = true;
            breakToolStripMenuItem.Enabled = true;         
        }

        private void startStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tmrSec.Enabled)
                Pause();
            else
                Play();
        }

        private void tmrSec_Tick(object sender, EventArgs e)
        {
            TimerSecs += 1;
            pgTime.Value = TimerSecs;

            if (IsBreak)
            {
                if (TimerSecs == BreakSecs)
                {
                    Pause();
                    IsBreak = false;
                    TimerSecs = 0;

                    MessageBox.Show("Work time!", "Work time!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                 
            }
            else
            {
                if (TimerSecs == WorkSecs)
                {
                    Pause();
                    IsBreak = true;
                    TimerSecs = 0;

                    MessageBox.Show("Break time!", "Break time!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void workToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Pause();
            IsBreak = false;
            TimerSecs = 0;

            pgTime.Value = 0;
        }

        private void breakToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Pause();
            IsBreak = true;
            TimerSecs = 0;           

            pgTime.Value = 0;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
