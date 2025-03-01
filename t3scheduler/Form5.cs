using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace T3Scheduler
{
    public partial class Form5 : Form
    {
        Form1 parent;

        public int[] nflights;
        public Hashtable[] airports;
        public Hashtable[] airplanes;
        public Hashtable[] flights;
        public int n1 = 0;
        public int n2 = 0;
        public string airport;
        public string database;
        public ArrayList terminals;
        public string location;

        System.Windows.Forms.Label[] airlinesInfo;
        ComboBox[] airlinesAction;
        System.Windows.Forms.Label[] airlinesLabel;

        public string newGAcallsign = "";

        private string listfilter;
        private ArrayList listarray;

        public Form5(Form1 parent, string airport, string database, string location)
        {
            InitializeComponent();
            this.Text = "Edit Schedule Statistics";
            this.parent = parent;
            this.airport = airport;
            this.database = database;   
            this.location = location;
            airports = new Hashtable[4];
            flights = new Hashtable[4];
            airplanes = new Hashtable[2];
            nflights= new int[4];

            terminals = parent.T3TerminalsData;

            nflights[0] = parent.nflights;
            nflights[2] = parent.nflightsC;
            nflights[1] = parent.nflightsR;
            nflights[3] = parent.nflightsga;
            airports[0] = parent.airports;
            airports[1] = parent.airportsR;
            airports[2] = parent.airportsC;
            airplanes[0] = parent.airplanes;
            flights[0] = parent.flights;
            flights[1] = parent.flightsR;
            flights[2] = parent.flightsC;
            airports[3] = parent.airportsga;
            airplanes[1] = parent.airplanesga;
            flights[3] = parent.flightsga;

            FlightTypeSelect.SelectedIndex = 0;

            updateForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.airlines = new Hashtable();
            Hashtable hhh = GetAirlines(flights[0]);
            foreach (string kk in hhh.Keys) parent.airlines[kk] = "1";

            parent.airlinesR = new Hashtable();
            hhh = GetAirlines(flights[1]);
            foreach (string kk in hhh.Keys) parent.airlinesR[kk] = "1";

            parent.airlinesC = new Hashtable();
            hhh = GetAirlines(flights[2]);
            foreach (string kk in hhh.Keys) parent.airlinesC[kk] = "1";

            parent.airlinesGA = new Hashtable();
            hhh = GetAirlines(flights[3]);
            foreach (string kk in hhh.Keys) parent.airlinesGA[kk] = "1";

            string aprl = "";
            for (int i = 0; i < 4; i++)
            {
                airports[i] = new Hashtable();
                int i2 = 0;
                if (i == 3) i2 = 1;
                foreach(string kk in flights[i].Keys)
                {
                    Hashtable hh = (Hashtable)flights[i][kk];
                    int k = 0;
                    foreach(string kkk in hh.Keys)
                    {
                        k += (int)hh[kkk];
                        if (i2 == 0)
                        {
                            string[] arr = kkk.Split(',');
                            if (!airplanes[i2].ContainsKey(kk + "-" + arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + arr[3]))
                                aprl += "Missing airplanes for flight " + kk + "-" + arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + arr[3] + "\n";
                        }
                    }
                    airports[i][kk] = k;
                    if(i2 == 1)
                    {
                        if (!airplanes[i2].ContainsKey(kk))
                            aprl += "Missing airplanes for GA flight destination " + kk + "\n";
                    }
                }
            }
            if(aprl != "")
            {
                MessageBox.Show("Missing airplanes:\n\n" + aprl, "ERROR");
                return;
            }

            parent.nflights = nflights[0];
            parent.nflightsC = nflights[2];
            parent.nflightsR = nflights[1];
            parent.nflightsga = nflights[3];
            parent.airports = airports[0];
            parent.airportsR = airports[1];
            parent.airportsC = airports[2];
            parent.airplanes = airplanes[0];
            parent.flights = flights[0];
            parent.flightsR = flights[1];
            parent.flightsC = flights[2];
            parent.airportsga = airports[3];
            parent.airplanesga = airplanes[1];
            parent.flightsga = flights[3];

            parent.T3TerminalsData = terminals;

            parent.form5applied = true;

            this.Close();
        }

        void updateallstat()
        {
            allstat.Text = "  Total flights: " + (nflights[0] + nflights[1] + nflights[2] + nflights[3]).ToString() + " => ";
            allstat.Text += " regular: " + nflights[0] + "; "; 
            allstat.Text += " regional: " + nflights[1] + "; "; 
            allstat.Text += " cargo: " + nflights[2] + "; "; 
            allstat.Text += " GA: " + nflights[3] + "; "; 
        }

        Hashtable GetAirlines(Hashtable flts)
        {
            Hashtable res = new Hashtable();
            foreach (string kk in flts.Keys)
            {
                Hashtable hh = (Hashtable)flts[kk];
                foreach (string kkk in hh.Keys)
                {
                    string[] arr = kkk.Split(',');
                    if (res.ContainsKey(arr[0]))
                    {
                        int i = (int)res[arr[0]];
                        res[arr[0]] = i + (int)hh[kkk];
                    }
                    else
                    {
                        res[arr[0]] = (int)hh[kkk];
                    }
                }
            }
            return res;
        }

        string GetAirlineCall(Hashtable flts, string arln)
        {
            foreach (string kk in flts.Keys)
            {
                Hashtable hh = (Hashtable)flts[kk];
                foreach (string kkk in hh.Keys)
                {
                    string[] arr = kkk.Split(',');
                    if (arr[0] == arln) return arr[1];
                }
            }
            return "";
        }

        private void updateForm()
        {
            n1 = FlightTypeSelect.SelectedIndex;
            if (n1 == 3) n2 = 1; else n2 = 0;

            label2.Text = "";

            int xshift = 200;
            int yshift = 25;
            int maxmax = 15;
            int x = 15;
            int topl = label1.Top + label1.Height + FlightTypeSelect.Height + 8;
            int y = topl;

            Hashtable arlss = GetAirlines(flights[n1]);

            if(airlinesLabel != null)
            {
                for(int i=0; i<airlinesLabel.Length; i++)
                {
                    airlinesLabel[i].Dispose();
                    airlinesInfo[i].Dispose();
                    airlinesAction[i].Dispose();
                }
            }
            airlinesLabel = new System.Windows.Forms.Label[arlss.Keys.Count];
            airlinesInfo = new System.Windows.Forms.Label[arlss.Keys.Count];
            airlinesAction = new ComboBox[arlss.Keys.Count];

            ArrayList tmpk = new ArrayList(arlss.Keys);
            tmpk.Sort();
            for (int i = 0; i < tmpk.Count; i++)
            {
                int nn = (int)arlss[tmpk[i]];
                var label1 = new System.Windows.Forms.Label()
                {
                    Top = y,
                    Left = x,
                    Parent = this,
                    Text = (string)tmpk[i],
                    Width = 40,
                };
                airlinesLabel[i] = label1;

                var label2 = new System.Windows.Forms.Label()
                {
                    Top = y,
                    Left = x + 41,
                    Parent = this,
                    Font = new Font("Consolas", this.label1.Font.Size),
                    Text = String.Format("{0,4:0} {1,6:0.000}", nn, (100.0 * (double)nn / (nflights[0] + nflights[1] + nflights[2] + nflights[3]))) + "%",
                    Width = 80,
                };
                airlinesInfo[i] = label2;

                var action = new ComboBox()
                {
                    Top = y,
                    Left = x +124,
                    Parent= this,
                    Width= 50,
                    Tag = (string)tmpk[i],
                };
                action.Items.Add("Edit");
                action.Items.Add("Remove");
                action.SelectedIndexChanged += OnComboBoxIndexChanged;
                airlinesAction[i] = action;

                if ((i + 1) % maxmax == 0)
                {
                    y = topl;
                    x += xshift;
                }
                else
                {
                    y += yshift;
                }
            }
            updateallstat();
        }

        void OnComboBoxIndexChanged(object sender, EventArgs e)
        {
            var CBox = (ComboBox)sender;
            string arln = (string)CBox.Tag;
            if (CBox.SelectedIndex == 0)
            {
                //edit
                if (n2 == 1) newGAcallsign = GetAirlineCall(flights[n1], arln);
                Form7 form7 = new Form7(arln, FlightTypeSelect.SelectedIndex, this, parent);
                form7.ShowDialog();
                updateForm();
            }
            else if(CBox.SelectedIndex == 1)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete airline " + arln + " and all asscoated flights?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //delete flights
                    deleteFlights(arln);
                    updateForm();
                    return;
                }
            }
            else
            {
                return;
            }
            CBox.SelectedIndex = -1;
        }

        public void deleteFlights(string arln)
        {
            int newnf = 0;
            ArrayList tmpf;
            Hashtable newtmp;
            tmpf = new ArrayList(flights[n1].Keys);
            Hashtable newtmp1 = new Hashtable();
            foreach (string kk in tmpf)
            {
                Hashtable hh = (Hashtable)flights[n1][kk];
                newtmp = new Hashtable();
                int ii = 0;
                foreach (string kkk in hh.Keys)
                {
                    if (!kkk.StartsWith(arln + ","))
                    {
                        newnf += (int)hh[kkk];
                        newtmp[kkk] = hh[kkk];
                    }
                    else
                    {
                        ii++;
                    }
                }
                if (newtmp.Count > 0) newtmp1[kk] = newtmp;
            }
            flights[n1] = newtmp1;
            nflights[n1] = newnf;
            //delete airplanes if not GA
            if (n1 < 3)
            {
                tmpf = new ArrayList(airplanes[n2].Keys);
                newtmp = new Hashtable();
                foreach (string kk in tmpf)
                {
                    bool pass = true;
                    string[] arr = kk.Split('-');
                    if (arr[1] == arln)
                    {
                        if (arr[3] == "regional" && n1 == 1) pass = false;
                        if (arr[3] == "cargo" && n1 == 2) pass = false;
                        if (n1 == 0) pass = false;
                    }
                    if (pass) newtmp[kk] = airplanes[n2][kk];
                }
                airplanes[n2] = newtmp;
            }
        }

        private void FlightTypeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateForm();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog folderBrowserDialog1 = new SaveFileDialog();
            folderBrowserDialog1.Filter = "Text (.txt)|*.txt";
            folderBrowserDialog1.FileName = "T3Scheduler_" + airport + "_" + database + "_STAT.txt";
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            StreamWriter fw = new StreamWriter(folderBrowserDialog1.FileName);
            for(int i=0; i<2; i++)
            {
                fw.WriteLine("===airplanes " + i + " start===");
                foreach (string kk in airplanes[i].Keys)
                {
                    fw.WriteLine(kk + "\t" + airplanes[i][kk]);
                }
                fw.WriteLine("===airplanes " + i + " end===");
            }
            for (int i = 0; i < 4; i++)
            {
                fw.WriteLine("===flights " + i + " start===");
                foreach (string kk in flights[i].Keys)
                {
                    Hashtable hh = (Hashtable)flights[i][kk];
                    fw.WriteLine("- " + kk);
                    foreach (string kkk in hh.Keys)
                    {
                        fw.WriteLine(kkk + "\t" + hh[kkk]);
                    }
                }
                fw.WriteLine("===flights " + i + " end===");
            }
            fw.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            folderBrowserDialog1.Filter = "Text (.txt)|*.txt";
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            nflights[0] = 0;
            nflights[2] = 0;
            nflights[1] = 0;
            nflights[3] = 0;
            airports[0] = new Hashtable();
            airports[1] = new Hashtable();
            airports[2] = new Hashtable();
            airplanes[0] = new Hashtable();
            flights[0] = new Hashtable();
            flights[1] = new Hashtable();
            flights[2] = new Hashtable();
            airports[3] = new Hashtable();
            airplanes[1] = new Hashtable();
            flights[3] = new Hashtable();
            StreamReader fr = new StreamReader(folderBrowserDialog1.FileName);
            string txt = "";
            for (int i = 0; i < 2; i++)
            {
                txt = fr.ReadLine();
                if(txt != "===airplanes " + i + " start===") { MessageBox.Show("Invalid File Format", "ERROR"); return; }
                while((txt=fr.ReadLine()) != null)
                {
                    if (txt == "===airplanes " + i + " end===") break;
                    string[] arr = txt.Split('\t');
                    airplanes[i][arr[0]] = arr[1];
                }
                
            }
            for (int i = 0; i < 4; i++)
            {
                txt = fr.ReadLine();
                if (txt != "===flights " + i + " start===") { MessageBox.Show("Invalid File Format", "ERROR"); return; }
                string apt = "";
                Hashtable hhh = new Hashtable();
                while ((txt = fr.ReadLine()) != null)
                {
                    if (txt == "===flights " + i + " end===")
                    {
                        if (apt != "") flights[i][apt] = hhh;
                        break;
                    }
                    if(txt.StartsWith("- "))
                    {
                        if (apt != "") flights[i][apt] = hhh;
                        hhh = new Hashtable();
                        apt = txt.Substring(2);
                        continue;
                    }
                    string[] arr = txt.Split('\t');
                    hhh[arr[0]] = int.Parse(arr[1]);
                    nflights[i] += int.Parse(arr[1]);
                }
            }
            fr.Close();
            updateForm();
        }

        private void listbox2_MouseHover(object sender, EventArgs e)
        {
            var tmpobj = (ListBox)sender;
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(tmpobj, "Right click to close");
        }

        void OnSubActionB(object sender, EventArgs e)
        {
            var tmpobj = (ListBox)sender;
            string newAirline = (string)tmpobj.SelectedItem;
            tmpobj.Dispose();
            label2.Text = "";
            Form7 form7 = new Form7(newAirline, FlightTypeSelect.SelectedIndex, this, parent);
            form7.ShowDialog();
            updateForm();
        }

        void refreshAddList(ListBox tmpobj)
        {
            tmpobj.Items.Clear();
            foreach (string kk in listarray)
            {
                if (listfilter.Length > 0)
                {
                    if (kk.StartsWith(listfilter))
                        tmpobj.Items.Add(kk);
                }
                else
                {
                    tmpobj.Items.Add(kk);
                }
            }
        }

        void OnSubActionCKey1(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled= true;
        }

        void OnSubActionCKey(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var tmpobj = (ListBox)sender;
            if (e.KeyCode == Keys.Escape)
            {
                label2.Text = "";
                tmpobj.Dispose();
                return;
            }
            char c = (char)e.KeyCode;
            if(char.IsLetter(c))
            {
                listfilter += c;
                listfilter = listfilter.ToUpper();
                refreshAddList(tmpobj);
                e.Handled= true;
                e.SuppressKeyPress = true;
                return;
            }
            if(e.KeyCode == Keys.Back)
            {
                if (listfilter.Length > 0)
                {
                    listfilter = listfilter.Remove(listfilter.Length - 1);
                    refreshAddList(tmpobj);
                }
                e.Handled= true;
                e.SuppressKeyPress = true;
                return;
            }
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
        }

        void OnSubActionEMClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var tmpobj = (ListBox)sender;
            if (e.Button == MouseButtons.Right)
            {
                label2.Text = "";
                tmpobj.Dispose();
                return;
            }
        }

        private void textbox_MouseHover(object sender, EventArgs e)
        {
            var tmpobj = (TextBox)sender;
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(tmpobj, "ENTER to change, ESC to cancel");
        }

        void OnSubActionD(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var tmpobj = (TextBox)sender;
            if (e.KeyCode == Keys.Escape)
            {
                label2.Text = "";
                tmpobj.Dispose();
                return;
            }
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            string newAirlineStr = (string)tmpobj.Text;
            if(newAirlineStr.Split(',').Length!=2)
            {
                MessageBox.Show("Invalid new airline or GA id format!", "ERROR");
                return;
            }
            string newAirline = newAirlineStr.Split(',')[0].ToUpper();
            Hashtable arlss = GetAirlines(flights[n1]);
            if(arlss.ContainsKey(newAirline))
            {
                MessageBox.Show("This airline or GA id already exists on this page!", "ERROR");
                return;
            }

            newGAcallsign = newAirlineStr.Split(',')[1].ToUpper();
            tmpobj.Dispose();
            label2.Text = "";

            Form7 form7 = new Form7(newAirline, FlightTypeSelect.SelectedIndex, this, parent);
            form7.ShowDialog();
            updateForm();
        }

        private void object_LostFocus(object sender, EventArgs e)
        {
            var tmpobj = (ListBox)sender;
            label2.Text = "";
            tmpobj.Dispose();
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            var tmpobj = (TextBox)sender;
            label2.Text = "";
            tmpobj.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (n1 < 3)
            {
                Hashtable arlss = GetAirlines(flights[n1]);
                var tmpobj = new ListBox();
                listarray = new ArrayList();
                foreach (AirlineData adta in parent.T3AirlinesData)
                {
                    if (adta.ICAO.Length <= 4 && !arlss.ContainsKey(adta.ICAO))
                    {
                        listarray.Add(adta.ICAO);
                    }
                }
                listarray.Sort();
                foreach (string kk in listarray)
                {
                    tmpobj.Items.Add(kk);
                }
                tmpobj.Height = 200;
                tmpobj.KeyDown += OnSubActionCKey;
                tmpobj.KeyUp += OnSubActionCKey1;
                tmpobj.SelectedIndexChanged += OnSubActionB;
                tmpobj.MouseDown += OnSubActionEMClick;
                tmpobj.Top = button5.Top - tmpobj.Bottom;
                tmpobj.Left = button5.Left;
                tmpobj.Tag = "C";
                tmpobj.Parent = this;
                tmpobj.Width = 50;
                tmpobj.BringToFront();
                tmpobj.MouseHover += listbox2_MouseHover;
                tmpobj.LostFocus += object_LostFocus;
                tmpobj.Focus();
                label2.Text = "Choose from the list of airlines";
                listfilter = "";
            }
            else
            {
                var tmpobj = new TextBox();
                tmpobj.Text = "";
                tmpobj.KeyDown += OnSubActionD;
                tmpobj.Top = button5.Top - tmpobj.Bottom;
                tmpobj.Left = button5.Left;
                tmpobj.Tag = "B";
                tmpobj.Parent = this;
                tmpobj.Width = 100;
                tmpobj.BringToFront();
                tmpobj.MouseHover += textbox_MouseHover;
                tmpobj.LostFocus += TextBox_LostFocus;
                tmpobj.Focus();
                tmpobj.Focus();
                label2.Text = "Type new airline or GA code followed by call sign, examples (no quotes) 'JRE,JET SHARE' or 'N,' or 'C-,'";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            nflights[0] = 0;
            nflights[2] = 0;
            nflights[1] = 0;
            nflights[3] = 0;
            airports[0] = new Hashtable();
            airports[1] = new Hashtable();
            airports[2] = new Hashtable();
            airplanes[0] = new Hashtable();
            flights[0] = new Hashtable();
            flights[1] = new Hashtable();
            flights[2] = new Hashtable();
            airports[3] = new Hashtable();
            airplanes[1] = new Hashtable();
            flights[3] = new Hashtable();

            updateForm();
        }
    }
}
