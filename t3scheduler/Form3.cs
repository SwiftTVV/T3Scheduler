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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace T3Scheduler
{
    public partial class Form3 : Form
    {
        int maxflights;
        int maxgaflights;
        int defarrdep;
        int starthr;
        int numhrs;
        Form1 parent;
        int maxcargo;
        int maxregional;
        string root;

        TrackBar flightsMaster = null;
        TrackBar GAflightsMaster = null;
        TrackBar CflightsMaster = null;
        TrackBar RflightsMaster = null;
        TrackBar arrdepMaster = null;

        TrackBar[] flights;
        TrackBar[] GAflights;
        TrackBar[] arrdep;
        TrackBar[] Cflights;
        TrackBar[] Rflights;
        Label[] flightsLabels;
        Label[] GAflightsLabels;
        Label[] arrdepLabels;
        Label[] legends;
        Label[] RflightsLabels;
        Label[] CflightsLabels;

        const int trackBarWidth = 50;
        const int trackBarHeight = 250;
        const int CtrackBarHeight = 150;
        const int RtrackBarHeight = 150;
        const int GAtrackBarHeight = 80;
        const int ARRDEPtrackBarHeight = 60;

        class tinfo
        {
            public int i;
            public Label t;

            public tinfo(int i0, Label t0)
            {
                this.i = i0;
                this.t = t0;
            }
        }

        public Form3(int maxflights, int maxgaflights, double maxcargoIN, double maxregionalIN, int defarrdep, int numhrs, int starthr, string root, Form1 parent)
        {
            InitializeComponent();
            this.Text = "flights profile";
            this.maxflights = maxflights;
            this.maxgaflights = maxgaflights;
            this.defarrdep = defarrdep;
            this.starthr = starthr;
            this.numhrs = numhrs;
            this.parent = parent;
            this.root= root;
            this.maxcargo = (int)Math.Round(maxcargoIN * maxflights / 100.0);   
            this.maxregional = (int)Math.Round(maxregionalIN * maxflights / 100.0); ;

            flights = new TrackBar[numhrs];
            Cflights= new TrackBar[numhrs];
            Rflights= new TrackBar[numhrs]; 
            GAflights = new TrackBar[numhrs];
            arrdep = new TrackBar[numhrs];
            flightsLabels = new Label[numhrs];
            CflightsLabels= new Label[numhrs];
            RflightsLabels= new Label[numhrs];
            GAflightsLabels = new Label[numhrs];
            arrdepLabels = new Label[numhrs];
            legends = new Label[numhrs];
            label1.Text = "r\ne\ng\nu\nl\na\nr\n\n\nf\nl\ni\ng\nh\nt\ns";
            label2.Text = "G\nA";
            label3.Text = "A\nR\nR\n/\nD\nE\nP";
            label4.Text = "c\na\nr\ng\no";
            label5.Text = "r\ne\ng\ni\no\nn\na\nl";

            int y = 10;
            int x = 90;
            for (int i = 0; i < numhrs; i++)
            {
                var trackBar = new TrackBar()
                {
                    Top = y,
                    Left = x,
                    Width = trackBarWidth,
                    Height = trackBarHeight,
                    Minimum = 0,
                    Maximum = 10 * ((int)(1.2 * maxflights / 10) + 1),
                    Value = maxflights,
                    Parent = this,
                    Orientation = Orientation.Vertical,
                    TickFrequency = 10,
                    SmallChange = 1,
                    LargeChange = 1,
                    Visible = true,

                };
                if (parent.flights.Count == 0) trackBar.Enabled = false;
                var label = new Label()
                {
                    Top = y + trackBar.Height,
                    Left = x,
                    Text = maxflights.ToString(),
                    Width = trackBar.Width,
                    Parent = this,
                    Visible = true,
                };
                var trackBarR = new TrackBar()
                {
                    Top = y + trackBar.Height + label.Height,
                    Left = x,
                    Width = trackBarWidth,
                    Height = RtrackBarHeight,
                    Minimum = 0,
                    Maximum = 10 * ((int)(1.2*maxregional / 10) + 1),
                    Value = maxregional,
                    Parent = this,
                    Orientation = Orientation.Vertical,
                    TickFrequency = 10,
                    SmallChange = 1,
                    LargeChange = 1,
                    Visible = true,
                };
                if (parent.flightsR.Count == 0) trackBarR.Enabled = false;
                var labelR = new Label()
                {
                    Top = y + trackBar.Height + label.Height + trackBarR.Height,
                    Left = x,
                    Text = maxregional.ToString(),
                    Width = trackBar.Width,
                    Parent = this,
                    Visible = true,
                };
                var trackBarC = new TrackBar()
                {
                    Top = y + trackBar.Height + label.Height + trackBarR.Height + labelR.Height,
                    Left = x,
                    Width = trackBarWidth,
                    Height = CtrackBarHeight,
                    Minimum = 0,
                    Maximum = 10 * ((int)1.2*(maxcargo / 10) + 1),
                    Value = maxcargo,
                    Parent = this,
                    Orientation = Orientation.Vertical,
                    TickFrequency = 10,
                    SmallChange = 1,
                    LargeChange = 1,
                    Visible = true,
                };
                if (parent.flightsC.Count == 0) trackBarC.Enabled = false;
                var labelC = new Label()
                {
                    Top = y + trackBar.Height + label.Height + trackBarR.Height + labelR.Height + trackBarC.Height,
                    Left = x,
                    Text = maxcargo.ToString(),
                    Width = trackBar.Width,
                    Parent = this,
                    Visible = true,
                };
                var trackBarGA = new TrackBar()
                {
                    Top = y + trackBar.Height + label.Height + trackBarR.Height + labelR.Height + trackBarC.Height + labelC.Height,
                    Left = x,
                    Width = trackBarWidth,
                    Height = GAtrackBarHeight,
                    Minimum = 0,
                    Maximum = 10 * ((int)(1.2*maxgaflights / 10) + 1),
                    Value = maxgaflights,
                    Parent = this,
                    Orientation = Orientation.Vertical,
                    TickFrequency = 5,
                    SmallChange = 1,
                    LargeChange = 1,
                    Visible = true,
                };
                if (parent.flightsga.Count == 0) trackBarGA.Enabled = false;
                var labelGA = new Label()
                {
                    Top = y + trackBar.Height + label.Height + trackBarR.Height + labelR.Height + trackBarC.Height + labelC.Height + trackBarGA.Height,
                    Left = x,
                    Text = maxgaflights.ToString(),
                    Width = trackBar.Width,
                    Parent = this,
                    Visible = true,
                };
                var trackBarARRDEP = new TrackBar()
                {
                    Top = y + trackBar.Height + label.Height + trackBarR.Height + labelR.Height + trackBarC.Height + labelC.Height + trackBarGA.Height + labelGA.Height,
                    Left = x,
                    Width = trackBarWidth,
                    Height = ARRDEPtrackBarHeight,
                    Minimum = 0,
                    Maximum = 8,
                    Value = defarrdep,
                    Parent = this,
                    Orientation = Orientation.Vertical,
                    TickFrequency = 4,
                    SmallChange = 1,
                    LargeChange = 1,
                    Visible = true,
                };
                var labelARRDEP = new Label()
                {
                    Top = y + trackBar.Height + label.Height + trackBarR.Height + labelR.Height + trackBarC.Height + labelC.Height + trackBarGA.Height + labelGA.Height + trackBarARRDEP.Height,
                    Left = x,
                    Text = ((defarrdep+1)*10).ToString()+"/"+ (100 - (defarrdep + 1) * 10).ToString(),
                    Width = trackBar.Width,
                    Parent = this,
                    Visible = true,
                };


                var label1 = new Label()
                {
                    Top = 17 + y + trackBar.Height + label.Height + trackBarR.Height + labelR.Height + trackBarC.Height + labelC.Height + trackBarGA.Height + labelGA.Height + trackBarARRDEP.Height + labelARRDEP.Height,
                    Left = x,
                    Text = (starthr + i).ToString("D2") + ":00",
                    Width = trackBar.Width,
                    Parent = this,
                    Visible = true,
                };

                legends[i] = label1;
                flightsLabels[i] = label;
                CflightsLabels[i] = labelC;
                RflightsLabels[i] = labelR;
                GAflightsLabels[i] = labelGA;
                arrdepLabels[i] = labelARRDEP;

                trackBar.Tag = new tinfo(i, label);
                if (parent.flightsprofile != null) trackBar.Value = parent.flightsprofile[i];
                trackBar.ValueChanged += OnTrackBarValueChanged;

                trackBarC.Tag = new tinfo(i, labelC);
                if (parent.Cflightsprofile != null) trackBarC.Value = parent.Cflightsprofile[i];
                trackBarC.ValueChanged += OnCTrackBarValueChanged;

                trackBarR.Tag = new tinfo(i, labelR);
                if (parent.Rflightsprofile != null) trackBarR.Value = parent.Rflightsprofile[i];
                trackBarR.ValueChanged += OnRTrackBarValueChanged;

                trackBarGA.Tag = labelGA;
                if (parent.gaflightsprofile != null) trackBarGA.Value = parent.gaflightsprofile[i];
                trackBarGA.ValueChanged += OnGATrackBarValueChanged;

                trackBarARRDEP.Tag = labelARRDEP;
                if (parent.arrdepprofile != null) trackBarARRDEP.Value = parent.arrdepprofile[i];
                trackBarARRDEP.ValueChanged += OnarrdepValueChanged;

                x += trackBar.Width;
                flights[i] = trackBar;
                Rflights[i] = trackBarR;
                Cflights[i] = trackBarC;
                GAflights[i] = trackBarGA;
                arrdep[i] = trackBarARRDEP;
            }
        }
        void OnTrackBarValueChanged(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBarLoc = (TrackBar)sender;
            // get associated label
            tinfo tt = (tinfo)trackBarLoc.Tag;
            if (((TrackBar)Cflights[tt.i]).Value + ((TrackBar)Rflights[tt.i]).Value > trackBarLoc.Value)
            {
                trackBarLoc.Value = ((TrackBar)Cflights[tt.i]).Value + ((TrackBar)Rflights[tt.i]).Value;
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                if (String.IsNullOrEmpty(ToolTip1.GetToolTip(trackBarLoc)))
                    ToolTip1.SetToolTip(trackBarLoc, "(regular flights) >= (regional) + (cargo)");
            }

            var associatedLabel = tt.t;
            associatedLabel.Text = trackBarLoc.Value.ToString();
        }

        void OnCTrackBarValueChanged(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBarLoc = (TrackBar)sender;
            // get associated label
            tinfo tt = (tinfo)trackBarLoc.Tag;
            if (trackBarLoc.Value + ((TrackBar)Rflights[tt.i]).Value > ((TrackBar)flights[tt.i]).Value)
            {
                trackBarLoc.Value = ((TrackBar)flights[tt.i]).Value - ((TrackBar)Rflights[tt.i]).Value;
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                if (String.IsNullOrEmpty(ToolTip1.GetToolTip(trackBarLoc)))
                    ToolTip1.SetToolTip(trackBarLoc, "(regular flights) >= (regional) + (cargo)");
            }

            var associatedLabel = tt.t;
            associatedLabel.Text = trackBarLoc.Value.ToString();
        }

        void OnRTrackBarValueChanged(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBarLoc = (TrackBar)sender;
            // get associated label
            tinfo tt = (tinfo)trackBarLoc.Tag;
            if (trackBarLoc.Value + ((TrackBar)Cflights[tt.i]).Value > ((TrackBar)flights[tt.i]).Value)
            {
                trackBarLoc.Value = ((TrackBar)flights[tt.i]).Value - ((TrackBar)Cflights[tt.i]).Value;
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                if (String.IsNullOrEmpty(ToolTip1.GetToolTip(trackBarLoc)))
                    ToolTip1.SetToolTip(trackBarLoc, "(regular flights) >= (regional) + (cargo)");
            }

            var associatedLabel = tt.t;
            associatedLabel.Text = trackBarLoc.Value.ToString();
        }

        void OnGATrackBarValueChanged(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBar = (TrackBar)sender;
            // get associated label
            var associatedLabel = (Label)trackBar.Tag;
            associatedLabel.Text = trackBar.Value.ToString();
        }

        void OnarrdepValueChanged(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBar = (TrackBar)sender;
            // get associated label
            var associatedLabel = (Label)trackBar.Tag;
            int i = trackBar.Value;
            associatedLabel.Text = ((i + 1) * 10).ToString() + "/" + (100 - (i + 1) * 10).ToString();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            checkBox1.Checked = false;
            for (int i = 0; i < numhrs; i++)
            {
                flights[i].Value = maxflights;
                GAflights[i].Value = maxgaflights;
                Rflights[i].Value = maxregional;
                Cflights[i].Value = maxcargo;
                arrdep[i].Value = defarrdep;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i=0; i< numhrs; i++)
            {
                parent.flightsprofile[i] = flights[i].Value;
                parent.gaflightsprofile[i] = GAflights[i].Value;
                parent.arrdepprofile[i] = arrdep[i].Value;
                parent.Rflightsprofile[i] = Rflights[i].Value;
                parent.Cflightsprofile[i] = Cflights[i].Value;
            }
            parent.form3applied = true;
            this.Close();
        }

        private void readFromSchedule(bool original)
        {
            checkBox2.Checked = false;
            checkBox1.Checked = false;
            Cursor.Current = Cursors.WaitCursor;
            int[] deps = new int[numhrs];
            int[] arrs = new int[numhrs];
            for (int i = 0; i < numhrs; i++)
            {
                deps[i] = 0;
                arrs[i] = 0;
                flights[i].Value = 0;
                Cflights[i].Value = 0;
                Rflights[i].Value = 0;
                GAflights[i].Value = 0;
            }
            string path = root;
            if (original) path = Path.Combine(root, "backup");
            StreamReader fr = new StreamReader(Path.Combine(path, "schedule.csv"));
            string txt = fr.ReadLine();
            while ((txt = fr.ReadLine()) != null)
            {
                string[] arr = Form2.splitCSV(txt);
                for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Trim();
                int cindx = -1;
                if (arr[7] == "")
                {
                    string[] arr1 = arr[6].Split(":".ToCharArray());
                    int hr = int.Parse(arr1[0]);
                    if (hr >= starthr && hr < starthr + numhrs)
                    {
                        cindx = hr - starthr;
                    }
                    else
                    {
                        continue;
                    }
                    arrs[cindx]++;
                }
                else
                {
                    string[] arr1 = arr[7].Split(":".ToCharArray());
                    int hr = int.Parse(arr1[0]);
                    if (hr >= starthr && hr < starthr + numhrs)
                    {
                        cindx = hr - starthr;
                    }
                    else
                    {
                        continue;
                    }
                    deps[cindx]++;
                }
                if (arr[9] != "cargo" && arr[9] != "regional" && flights[cindx].Enabled)
                {
                    if (flights[cindx].Value == flights[cindx].Maximum) flights[cindx].Maximum = (int)(flights[cindx].Maximum * 1.1);
                    flights[cindx].Value++;
                }
                if (arr[9] == "cargo" && Cflights[cindx].Enabled)
                {
                    if (Cflights[cindx].Value == Cflights[cindx].Maximum) Cflights[cindx].Maximum = (int)(Cflights[cindx].Maximum * 1.1);
                    Cflights[cindx].Value++;
                }
                if (arr[9] == "regional" && Rflights[cindx].Enabled)
                {
                    if (Rflights[cindx].Value == Rflights[cindx].Maximum) Rflights[cindx].Maximum = (int)(Rflights[cindx].Maximum * 1.1);
                    Rflights[cindx].Value++;
                }
            }
            fr.Close();
            fr = new StreamReader(Path.Combine(path, "ga.csv"));
            txt = fr.ReadLine();
            while ((txt = fr.ReadLine()) != null)
            {
                string[] arr = Form2.splitCSV(txt);
                for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Trim();
                int cindx = -1;
                if (arr[6] == "")
                {
                    string[] arr1 = arr[5].Split(":".ToCharArray());
                    int hr = int.Parse(arr1[0]);
                    if (hr >= starthr && hr < starthr + numhrs)
                    {
                        cindx = hr - starthr;
                    }
                    else
                    {
                        continue;
                    }
                    arrs[cindx]++;
                }
                else
                {
                    string[] arr1 = arr[6].Split(":".ToCharArray());
                    int hr = int.Parse(arr1[0]);
                    if (hr >= starthr && hr < starthr + numhrs)
                    {
                        cindx = hr - starthr;
                    }
                    else
                    {
                        continue;
                    }
                    deps[cindx]++;
                }
                if (GAflights[cindx].Enabled)
                {
                    if (GAflights[cindx].Value == GAflights[cindx].Maximum) GAflights[cindx].Maximum = (int)(GAflights[cindx].Maximum * 1.1);
                    GAflights[cindx].Value++;
                }
            }
            fr.Close();
            for (int i = 0; i < numhrs; i++)
            {
                if (arrs[i] + deps[i] == 0)
                {
                    arrdep[i].Value = 4;
                }
                else
                {
                    int k = (int)((10 * arrs[i] / (arrs[i] + deps[i]))) - 1;
                    if (k == 9) k = 8;
                    if (k == -1) k = 0;
                    arrdep[i].Value = k;
                }
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Profile loaded!", "Message");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            readFromSchedule(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            readFromSchedule(false);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog folderBrowserDialog1 = new SaveFileDialog();
            folderBrowserDialog1.Filter = "Text (.txt)|*.txt";
            folderBrowserDialog1.FileName = "T3Scheduler_hprofile_" + numhrs + "hrs.txt";
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            StreamWriter fw = new StreamWriter(folderBrowserDialog1.FileName);
            fw.WriteLine(numhrs);
            for (int i = 0; i < numhrs; i++)
            {
                fw.WriteLine(flights[i].Value + " " + Cflights[i].Value + " " + Rflights[i].Value + " " + GAflights[i].Value + " " + arrdep[i].Value);
            }
            fw.Close();
            MessageBox.Show("Profile saved!", "Message");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            folderBrowserDialog1.Filter = "Text (.txt)|*.txt";
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            StreamReader fr = new StreamReader(folderBrowserDialog1.FileName);
            string txt = fr.ReadLine();
            int nn = -1;
            try
            {
                nn = int.Parse(txt);
            }
            catch
            {
                MessageBox.Show("Invalid file format", "ERROR");
                return;
            }
            if(nn != numhrs)
            {
                MessageBox.Show("Incompatible profile!\nProfile you are loading is for " + nn + " hours, current profile is for " + numhrs + " hours", "ERROR");
                return;
            }
            for (int i = 0; i < numhrs; i++)
            {
                txt = fr.ReadLine();
                string[] arr = txt.Split(' ');
                try
                {
                    flights[i].Value = int.Parse(arr[0]);
                    Cflights[i].Value = int.Parse(arr[1]);
                    Rflights[i].Value = int.Parse(arr[2]);
                    GAflights[i].Value = int.Parse(arr[3]);
                    arrdep[i].Value = int.Parse(arr[4]);
                }
                catch 
                {
                    MessageBox.Show("Invalid format in line " + (i + 2).ToString(), "ERROR");
                    fr.Close();
                    return;
                }
            }
            fr.Close();
            MessageBox.Show("Profile loaded!", "Message");
        }

        private TrackBar CopyTrackBar(TrackBar trackBar)
        {
            TrackBar newtrackBar = new TrackBar();
            newtrackBar.Left = 30;
            newtrackBar.Top = trackBar.Top;
            newtrackBar.Maximum = trackBar.Maximum;
            newtrackBar.Minimum = trackBar.Minimum;
            newtrackBar.Value = trackBar.Value;
            newtrackBar.Parent = this;
            newtrackBar.Orientation = Orientation.Vertical;
            newtrackBar.TickFrequency = 10;
            newtrackBar.SmallChange = 1;
            newtrackBar.LargeChange = 1;
            newtrackBar.Height = trackBar.Height;
            return newtrackBar;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked && boxes == 0)
            {
                boxes = 1;
                checkBox2.Checked = false;
            }
            boxes = 0;
            if(!checkBox1.Checked)
            {
                if(flightsMaster != null) { flightsMaster.Dispose(); flightsMaster = null; }
                if (GAflightsMaster != null) { GAflightsMaster.Dispose(); GAflightsMaster = null; }
                if (RflightsMaster != null) { RflightsMaster.Dispose(); RflightsMaster = null; }
                if (CflightsMaster != null) { CflightsMaster.Dispose(); CflightsMaster = null; }
                if (arrdepMaster != null) { arrdepMaster.Dispose(); arrdepMaster = null; }
            }
            else
            {
                flightsMaster = CopyTrackBar(flights[0]);
                flightsMaster.ValueChanged += OnTrackBarValueChangedMaster;
                if(parent.flights.Count == 0)flightsMaster.Enabled= false;

                CflightsMaster = CopyTrackBar(Cflights[0]);
                CflightsMaster.ValueChanged += OnCTrackBarValueChangedMaster;
                if (parent.flightsC.Count == 0) CflightsMaster.Enabled = false;

                RflightsMaster = CopyTrackBar(Rflights[0]);
                RflightsMaster.ValueChanged += OnRTrackBarValueChangedMaster;
                if (parent.flightsR.Count == 0) RflightsMaster.Enabled = false;

                GAflightsMaster = CopyTrackBar(GAflights[0]);
                GAflightsMaster.ValueChanged += OnGATrackBarValueChangedMaster;
                if (parent.flightsga.Count == 0) GAflightsMaster.Enabled = false;

                arrdepMaster = CopyTrackBar(arrdep[0]);
                arrdepMaster.ValueChanged += OnarrdepValueChangedMaster;
            }
        }

        void OnTrackBarValueChangedMaster(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBarLoc = (TrackBar)sender;
            // get associated label
            if (((TrackBar)CflightsMaster).Value + ((TrackBar)RflightsMaster).Value > trackBarLoc.Value)
            {
                trackBarLoc.Value = ((TrackBar)CflightsMaster).Value + ((TrackBar)RflightsMaster).Value;
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                if (String.IsNullOrEmpty(ToolTip1.GetToolTip(trackBarLoc)))
                    ToolTip1.SetToolTip(trackBarLoc, "(regular flights) >= (regional) + (cargo)");
            }

            for(int i=0; i<flights.Length; i++)
            {
                flights[i].Value = trackBarLoc.Value;
            }

         }

        void OnCTrackBarValueChangedMaster(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBarLoc = (TrackBar)sender;
            // get associated label
            tinfo tt = (tinfo)trackBarLoc.Tag;
            if (trackBarLoc.Value + ((TrackBar)RflightsMaster).Value > ((TrackBar)flightsMaster).Value)
            {
                trackBarLoc.Value = ((TrackBar)flightsMaster).Value - ((TrackBar)RflightsMaster).Value;
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                if (String.IsNullOrEmpty(ToolTip1.GetToolTip(trackBarLoc)))
                    ToolTip1.SetToolTip(trackBarLoc, "(regular flights) >= (regional) + (cargo)");
            }

            for (int i = 0; i < Cflights.Length; i++)
            {
                Cflights[i].Value = trackBarLoc.Value;
            }
        }

        void OnRTrackBarValueChangedMaster(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBarLoc = (TrackBar)sender;
            // get associated label
            tinfo tt = (tinfo)trackBarLoc.Tag;
            if (trackBarLoc.Value + ((TrackBar)CflightsMaster).Value > ((TrackBar)flightsMaster).Value)
            {
                trackBarLoc.Value = ((TrackBar)flightsMaster).Value - ((TrackBar)CflightsMaster).Value;
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                if (String.IsNullOrEmpty(ToolTip1.GetToolTip(trackBarLoc)))
                    ToolTip1.SetToolTip(trackBarLoc, "(regular flights) >= (regional) + (cargo)");
            }

            for (int i = 0; i < Rflights.Length; i++)
            {
                Rflights[i].Value = trackBarLoc.Value;
            }
        }

        void OnGATrackBarValueChangedMaster(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBar = (TrackBar)sender;
            // get associated label

            for (int i = 0; i < GAflights.Length; i++)
            {
                GAflights[i].Value = trackBar.Value;
            }

        }

        void OnarrdepValueChangedMaster(object sender, EventArgs e)
        {
            // get trackbar, which generated event
            var trackBar = (TrackBar)sender;
            // get associated label

            for (int i = 0; i < arrdep.Length; i++)
            {
                arrdep[i].Value = trackBar.Value;
            }

        }

        private int flightsMasterVal0;
        private int CflightsMasterVal0;
        private int RflightsMasterVal0;
        private int GAflightsMasterVal0;
        int boxes = 0;

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && boxes == 0)
            {
                boxes = 2;
                checkBox1.Checked = false;
            }
            boxes = 0;
            if (!checkBox2.Checked)
            {
                if (flightsMaster != null) { flightsMaster.Dispose(); flightsMaster = null; }
                if (GAflightsMaster != null) { GAflightsMaster.Dispose(); GAflightsMaster = null; }
                if (RflightsMaster != null) { RflightsMaster.Dispose(); RflightsMaster = null; }
                if (CflightsMaster != null) { CflightsMaster.Dispose(); CflightsMaster = null; }
                //if (arrdepMaster != null) { arrdepMaster.Dispose(); arrdepMaster = null; }
            }
            else
            {
                flightsMaster = CopyTrackBar(flights[0]);
                flightsMaster.Maximum = flightsMaster.Maximum * 2;
                flightsMaster.Value = (flightsMaster.Maximum + flightsMaster.Minimum) / 2;
                flightsMasterVal0 = flightsMaster.Value;
                flightsMaster.ValueChanged += OnTrackBarValueChangedMasterA;
                if (parent.flights.Count == 0) flightsMaster.Enabled = false;

                CflightsMaster = CopyTrackBar(Cflights[0]);
                CflightsMaster.Maximum = CflightsMaster.Maximum * 2;
                CflightsMaster.Value = (CflightsMaster.Maximum + CflightsMaster.Minimum) / 2;
                CflightsMasterVal0 = CflightsMaster.Value;
                CflightsMaster.ValueChanged += OnCTrackBarValueChangedMasterA;
                if (parent.flightsC.Count == 0) CflightsMaster.Enabled = false;

                RflightsMaster = CopyTrackBar(Rflights[0]);
                RflightsMaster.Maximum = RflightsMaster.Maximum * 2;
                RflightsMaster.Value = (RflightsMaster.Maximum + RflightsMaster.Minimum) / 2;
                RflightsMasterVal0 = RflightsMaster.Value;
                RflightsMaster.ValueChanged += OnRTrackBarValueChangedMasterA;
                if (parent.flightsR.Count == 0) RflightsMaster.Enabled = false;

                GAflightsMaster = CopyTrackBar(GAflights[0]);
                GAflightsMaster.Maximum = GAflightsMaster.Maximum * 2;
                GAflightsMaster.Value = (GAflightsMaster.Maximum + GAflightsMaster.Minimum) / 2;
                GAflightsMasterVal0 = GAflightsMaster.Value;
                GAflightsMaster.ValueChanged += OnGATrackBarValueChangedMasterA;
                if (parent.flightsga.Count == 0) GAflightsMaster.Enabled = false;
            }
        }

        void OnTrackBarValueChangedMasterA(object sender, EventArgs e)
        {
            var trackBarLoc = (TrackBar)sender;
            int vvv = trackBarLoc.Value - flightsMasterVal0;
            flightsMasterVal0 = trackBarLoc.Value;

            for (int i = 0; i < flights.Length; i++)
            {
                int k = flights[i].Value + vvv;
                if(k> flights[i].Maximum)k= flights[i].Maximum;
                if (k < 0) k = 0;
                flights[i].Value = k;
            }
        }

        void OnCTrackBarValueChangedMasterA(object sender, EventArgs e)
        {
            var trackBarLoc = (TrackBar)sender;
            int vvv = trackBarLoc.Value - CflightsMasterVal0;
            CflightsMasterVal0 = trackBarLoc.Value;

            for (int i = 0; i < Cflights.Length; i++)
            {
                int k = Cflights[i].Value + vvv;
                if (k > Cflights[i].Maximum) k = Cflights[i].Maximum;
                if (k < 0) k = 0;
                Cflights[i].Value = k;
            }
        }

        void OnRTrackBarValueChangedMasterA(object sender, EventArgs e)
        {
            var trackBarLoc = (TrackBar)sender;
            int vvv = trackBarLoc.Value - RflightsMasterVal0;
            RflightsMasterVal0 = trackBarLoc.Value;

            for (int i = 0; i < Rflights.Length; i++)
            {
                int k = Rflights[i].Value + vvv;
                if (k > Rflights[i].Maximum) k = Rflights[i].Maximum;
                if (k < 0) k = 0;
                Rflights[i].Value = k;
            }
        }

        void OnGATrackBarValueChangedMasterA(object sender, EventArgs e)
        {
            var trackBarLoc = (TrackBar)sender;
            int vvv = trackBarLoc.Value - GAflightsMasterVal0;
            GAflightsMasterVal0 = trackBarLoc.Value;

            for (int i = 0; i < GAflights.Length; i++)
            {
                int k = GAflights[i].Value + vvv;
                if (k > GAflights[i].Maximum) k = GAflights[i].Maximum;
                if (k < 0) k = 0;
                GAflights[i].Value = k;
            }

        }
    }
}
