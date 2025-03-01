using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace T3Scheduler
{
    public partial class Form7 : Form
    {
        Form5 parent;
        Form1 mainform;
        string airline;
        int flightType;

        CheckBox[] flights;
        Label[] flightOperator;
        Label[] flightDestination;
        Label[] flightAirplanes;
        Label[] flightAirplanesSpecial;
        Label[] flightNumber;
        Label[] flightProbability;
        ArrayList[] listarray; 
        string[] listfilter;
        int nflights = 0;
        int nflightsTot = 0;
        int nflightsTot0 = 0;
        int openEdits = 0;
        System.Windows.Forms.Button AddRowBtn;

        int x = 0;
        int y = 0;
        int yshift = 0;
        int maxRow = 0;

        Hashtable T3Airports;

        public Form7(string airline, int flightType, Form5 parent, Form1 mainform)
        {
            InitializeComponent();
            this.parent = parent;
            this.mainform = mainform;
            this.airline = airline;
            this.flightType = flightType;
            this.Text = "Edit " + airline + " flight statistics";
            label1.Text = airline + " at " + parent.airport;
            //gates
            //flights
            x = 15;
            int topl = label3.Top + label3.Height + 3;
            y = topl;
            foreach (string kk in parent.flights[parent.n1].Keys)
            {
                Hashtable hh = (Hashtable)parent.flights[parent.n1][kk];
                foreach (string kkk in hh.Keys)
                {
                    string[] arr = kkk.Split(',');
                    if (arr[0] == airline)
                    {
                        nflights++;
                        nflightsTot += (int)hh[kkk];
                    }
                }
            }
            nflightsTot0 = nflightsTot;
            double pp = 0.0;
            if (parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0 != 0)
                pp = ((double)nflightsTot) / ((parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0));
            label5.Text = nflightsTot + " " + String.Format("{0,6:0.000}%", 100*pp);
            flights = new CheckBox[nflights];
            flightOperator = new Label[nflights];
            flightDestination = new Label[nflights];
            flightAirplanes = new Label[nflights];
            flightAirplanesSpecial = new Label[nflights];
            flightNumber = new Label[nflights];
            flightProbability = new Label[nflights];
            listarray = new ArrayList[nflights];
            listfilter = new string[nflights];
            maxRow = 0;
            ArrayList tmparr = new ArrayList(parent.flights[parent.n1].Keys);
            tmparr.Sort();
            foreach (string kk in tmparr)
            {
                Hashtable hh = (Hashtable)parent.flights[parent.n1][kk];
                foreach (string kkk in hh.Keys)
                {
                    string[] arr = kkk.Split(',');
                    if (arr[0] == airline)
                    {
                        AddRow(ref x, ref y, ref maxRow, arr, kk, (int)hh[kkk], true, "----");
                    }
                }
            }
            AddRowBtn =  new System.Windows.Forms.Button();
            AddRowBtn.Top = y;
            AddRowBtn.Left= x;
            AddRowBtn.Text = "Add flight";
            AddRowBtn.Click += AddNewRow;
            AddRowBtn.Parent = this;
            listBox1.Items.Clear();
            listBox1.SelectionMode = SelectionMode.MultiSimple;
            int k = 0;
            foreach(TerminalData td in mainform.T3TerminalsData)
            {
                //string itText = td.name + ": " + td.airlines + ": " + td.gates;
                string itText = td.name + ": " + td.gates;
                //if (itText.Length > 50) itText = itText.Substring(0, 47) + "...";
                string catchall = "Catch all()";
                if(parent.n1 == 1) catchall = "Catch all(regional)";
                if (parent.n1 == 2) catchall = "Catch all(cargo)";
                if (parent.n1 == 3) catchall = "Catch all(GA)";
                if (td.airlines.Contains(catchall))
                {
                    listBox1.Items.Add("*" + itText);
                }
                else
                {
                    listBox1.Items.Add(itText);
                }
                if (td.airlines.Replace("*","").Contains(airline))
                {
                    listBox1.SetSelected(k, true);
                }
                k++;
            }
            T3Airports = new Hashtable();
            comboBox1.SelectedIndex = 0;
            if (parent.airports[parent.n2].Keys.Count == 0) comboBox1.SelectedIndex = 1;
        }

        void AddRow(ref int x, ref int y, ref int i, string[] arr, string kk, int nflts, bool active, string AplDef)
        {
            var box1 = new CheckBox()
            {
                Top = y - 4,
                Left = x,
                Text = " ",
                Parent = this,
                Checked = active,
                Tag = "A" + i,
                Width = 20,
            };
            box1.CheckedChanged += OnActionBox;
            flights[i] = box1;
            x += box1.Width;
            string oper = arr[1];
            if (parent.n1 == 3) oper = "";
            var xlabel1 = new Label()
            {
                Top = y,
                Left = x,
                Parent = this,
                Text = oper,
                Tag = "B" + i,
                Width = 40,
            };
            xlabel1.Click += OnAction;
            flightOperator[i] = xlabel1;
            x += xlabel1.Width;
            var xlabel2 = new Label()
            {
                Top = y,
                Left = x,
                Parent = this,
                Text = kk,
                Tag = "C" + i,
                Width = 40,
            };
            xlabel2.Click += OnAction;
            flightDestination[i] = xlabel2;
            x += xlabel2.Width;
            var xlabel3 = new Label()
            {
                Top = y,
                Left = x,
                Parent = this,
                Text = String.Format("{0,4:0}", nflts),
                Tag = "D" + i,
                Width = 40,
            };
            xlabel3.Click += OnAction;
            flightNumber[i] = xlabel3;
            x += xlabel3.Width;
            double pp = 0.0;
            if (parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0 != 0)
                pp = ((double)nflts) / ((parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0));
            var xlabel4 = new Label()
            {
                Top = y,
                Left = x,
                Parent = this,
                Text = String.Format("{0,6:0.000}%", 100*pp),
                Tag = "X" + i,
                Width = 70,
            };
            xlabel4.Click += OnAction;
            flightProbability[i] = xlabel4;
            x += xlabel4.Width;
            string livery = "regular";
            if (arr.Length == 4)
            {
                if (arr[2] != "")
                {
                    if (arr[3] == "")
                    {
                        livery = arr[2];
                    }
                    else
                    {
                        livery = arr[2] + ',' + arr[3];
                    }
                }
            }
            else
            {
                livery = "";
            }
            var xlabel6 = new Label()
            {
                Top = y,
                Left = x,
                Parent = this,
                Text = livery,
                Tag = "F" + i,
                Width = 100,
            };
            xlabel6.Click += OnAction;
            flightAirplanesSpecial[i] = xlabel6;
            x += xlabel6.Width;
            string apps = "";
            if (parent.n2 == 0)
            {
                if (parent.airplanes[parent.n2].ContainsKey(kk + "-" + arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + arr[3]))
                    apps = parent.airplanes[parent.n2][kk + "-" + arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + arr[3]].ToString();
                else
                    apps = AplDef;
            }
            else
            {
                if (parent.airplanes[parent.n2].ContainsKey(kk))
                    apps = parent.airplanes[parent.n2][kk].ToString();
                else
                    apps = AplDef;
            }
            var xlabel5 = new Label()
            {
                Top = y,
                Left = x,
                Parent = this,
                Text = apps,
                Tag = "E" + i,
            };
            xlabel5.Click += OnAction;
            flightAirplanes[i] = xlabel5;
            x += xlabel5.Width;
            i++;
            if (yshift == 0) yshift = xlabel4.Height;
            y += yshift + 2;
            x = 15;
        }

        void AddNewRow(object sender, EventArgs e)
        {
            string[] arr;

            if (parent.n1 == 3)
            {
                arr = new string[2];
                arr[0] = airline;
                arr[1] = "";
            }
            else
            {
                arr = new string[4];
                arr[0] = airline;
                arr[1] = airline;
                arr[2] = "";
                arr[3] = "";
                if (parent.n1 == 0)
                    arr[2] = "regular";
                else if (parent.n1 == 1)
                    arr[2] = "regional";
                else if (parent.n1 == 2)
                    arr[2] = "cargo";
            }
            string kk = "----";
            foreach(string kk1 in parent.flights[parent.n1].Keys)
            {
                kk = kk1;
                break;
            }
            string apln = "----";
            foreach (AirlineData ad in mainform.T3AirlinesData)
            {
                if(ad.ICAO == airline)
                {
                    apln = ad.airplanes[0];
                    break;
                }
            }
            Array.Resize(ref flights, maxRow + 1);
            Array.Resize(ref flightOperator, maxRow + 1);
            Array.Resize(ref flightDestination, maxRow + 1);
            Array.Resize(ref flightAirplanes, maxRow + 1);
            Array.Resize(ref flightAirplanesSpecial, maxRow + 1);
            Array.Resize(ref flightNumber, maxRow + 1);
            Array.Resize(ref flightProbability, maxRow + 1);
            Array.Resize(ref listarray, maxRow + 1);
            Array.Resize(ref listfilter, maxRow + 1);
            this.AutoScroll = false;
            this.VerticalScroll.Value = 0;
            AddRow(ref x, ref y, ref maxRow, arr, kk, 0, false, apln);
            AddRowBtn.Top = y;
            AddRowBtn.Left = x;
            this.VerticalScroll.Value = this.VerticalScroll.Maximum;
            this.AutoScroll = true;
        }

        void OnActionBox(object sender, EventArgs e)
        {
            var CBox = (CheckBox)sender;
            string tag = (string)CBox.Tag;
            int n = int.Parse(tag.Substring(1));
            var CLbl = (Label)flightNumber[n];
            int nn = int.Parse(CLbl.Text);
            if(nn==0 && CBox.Checked)
            {
                MessageBox.Show("Number of flights must be greater than 0", "ERROR");
                CBox.Checked = false;
                return;
            }
            if (((Label)flightDestination[n]).Text == "----")
            {
                MessageBox.Show("Flight destination airport not selected", "ERROR");
                CBox.Checked = false;
                return;
            }
            if (((Label)flightAirplanes[n]).Text == "----")
            {
                MessageBox.Show("Flight airplanes not selected", "ERROR");
                CBox.Checked = false;
                return;
            }
            if (CBox.Checked) nflightsTot += nn; else nflightsTot -= nn;
            updateAllPerc();
            double pp = 0.0;
            if (parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot0 - nflightsTot != 0)
                pp = ((double)nflightsTot) / ((parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0));
            label5.Text = nflightsTot + " " + String.Format("{0,6:0.000}%", 100*pp);
        }

        void updateAllPerc()
        {
            for(int n=0; n< flights.Length; n++)
            {
                var CBox = (CheckBox)flights[n];
                var CLbl = (Label)flightNumber[n];
                int nn = int.Parse(CLbl.Text);
                if (!CBox.Checked) nn = 0;
                double pp = 0.0;
                if (parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0 != 0)
                    pp = ((double)nn) / ((parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0));
                flightProbability[n].Text = String.Format("{0,6:0.000}%", 100 * pp);
            }
        }

        ArrayList GetAllAirplanesGA()
        {
            ArrayList res = new ArrayList();
            foreach (AirplaneData adta in this.mainform.TS3AirplaneData)
            {
                if (adta.atype == "BUSINESS JET" || adta.atype == "TURBOPROP" || adta.atype == "PROP") res.Add(adta.ICAO);
            }
            res.Sort();
            return res;
        }

        void OnAction(object sender, EventArgs e)
        {
            var CLbl = (Label)sender;
            string tag = (string)CLbl.Tag;
            int n = int.Parse(tag.Substring(1));
            if (tag.StartsWith("B"))
            {
                if (CLbl.Text == "") return;
                var tmpobj = new ListBox();
                ArrayList tmpa = new ArrayList();
                tmpa.Add(airline);
                if (parent.n1 == 1)
                {
                    foreach (AirlineData adta in this.mainform.T3AirlinesData)
                    {
                        if (adta.ICAO.Contains("_"))
                        {
                            string[] arr = adta.ICAO.Split('_');
                            if (arr[1].Length == 3 && arr[0] == airline)
                            {
                                tmpa.Add(arr[1]);
                            }
                        }
                    }
                }
                int i = 0;
                foreach (string kk in tmpa)
                {
                    tmpobj.Items.Add(kk);
                    if (CLbl.Text == kk) tmpobj.SetSelected(i, true);
                    i++;
                }
                tmpobj.KeyDown += OnSubActionCKey;
                tmpobj.SelectedIndexChanged += OnSubActionB;
                tmpobj.MouseDown += OnSubActionEMClick;
                tmpobj.Top = CLbl.Top;
                tmpobj.Left = CLbl.Left;
                tmpobj.Tag = n;
                tmpobj.Parent = this;
                tmpobj.Width = CLbl.Width + 20;
                tmpobj.BringToFront();
                tmpobj.MouseHover += listbox2_MouseHover;
                tmpobj.Focus();
                openEdits++;
            }
            else if (tag.StartsWith("C"))
            {
                var tmpobj = new ListBox();
                if (comboBox1.SelectedIndex == 0 && parent.airports[parent.n2].Keys.Count > 0)
                {
                    listarray[n] = new ArrayList(parent.airports[parent.n2].Keys);
                }
                else
                {
                    listarray[n] = new ArrayList(T3Airports.Keys);
                }
                listarray[n].Sort();
                int i = 0;
                int nn = 0;
                foreach(string kk in listarray[n])
                {
                    tmpobj.Items.Add(kk);
                    if (CLbl.Text == kk) nn = i;
                    i++;
                }
                tmpobj.SelectedIndex = nn;
                tmpobj.KeyDown += OnSubActionCKeyList;
                tmpobj.KeyUp+= OnSubActionCKeyList1;
                tmpobj.SelectedIndexChanged += OnSubActionC;
                tmpobj.MouseDown += OnSubActionEMClick;
                tmpobj.Top = CLbl.Top;
                tmpobj.Left = CLbl.Left;
                tmpobj.Tag = n;
                tmpobj.Parent = this;
                tmpobj.Width = CLbl.Width+20;
                tmpobj.BringToFront();
                tmpobj.MouseHover += listbox2_MouseHover;
                tmpobj.Focus();
                listfilter[n] = "";
                openEdits++;
            }
            else if (tag.StartsWith("D"))
            {
                var tmpobj = new System.Windows.Forms.TextBox();
                tmpobj.Text = CLbl.Text;
                tmpobj.KeyDown += OnSubActionD;
                tmpobj.Top = CLbl.Top;
                tmpobj.Left = CLbl.Left;
                tmpobj.Tag = n;
                tmpobj.Parent = this;
                tmpobj.Width = CLbl.Width;
                tmpobj.BringToFront();
                tmpobj.MouseHover += textbox_MouseHover;
                tmpobj.Focus();
                openEdits++;
            }
            else if (tag.StartsWith("E"))
            {
                var tmpobj = new ListBox();
                ArrayList tmpa = new ArrayList();
                tmpobj.SelectionMode = SelectionMode.MultiSimple;
                if (parent.n2 == 0)
                {
                    string extra = "";
                    string txt = flightAirplanesSpecial[n].Text;
                    if (txt != "regular" && !txt.StartsWith("regional") && txt != "cargo")
                    {
                        extra = "_" + txt;
                    }
                    else if (flightOperator[n].Text != airline)
                    {
                        if (txt.Contains(","))
                        {
                            extra = "_" + txt.Split(',')[1];
                        }
                    }
                    string[] apls = null;
                    foreach (AirlineData adta in this.mainform.T3AirlinesData)
                    {
                        if (adta.ICAO == airline + extra) apls = adta.airplanes;
                    }
                    tmpa = new ArrayList(apls);
                }
                else
                {
                    string[] apls = null;
                    foreach (AirlineData adta in this.mainform.T3AirlinesData)
                    {
                        if (adta.ICAO == airline) apls = adta.airplanes;
                    }
                    if (apls == null)
                    {
                        tmpa = GetAllAirplanesGA();
                    }
                    else
                    {
                        tmpa = new ArrayList(apls);
                    }
                }
                tmpa.Sort();
                string aaaa = "," + CLbl.Text + ",";
                int i = 0;
                foreach (string kk in tmpa)
                {
                    tmpobj.Items.Add(kk);
                    if(aaaa.Contains("," + kk + ",")) tmpobj.SetSelected(i, true);
                    i++;
                }
                tmpobj.Top = CLbl.Top;
                tmpobj.Left = CLbl.Left;
                tmpobj.Tag = n;
                tmpobj.Parent = this;
                tmpobj.Width = 70;
                tmpobj.BringToFront();
                tmpobj.KeyDown += OnSubActionEKey;
                tmpobj.SelectedIndexChanged += OnSubActionE;
                tmpobj.MouseDown += OnSubActionEMClick1;
                tmpobj.MouseHover += listbox2_MouseHover;
                tmpobj.Focus();
                openEdits++;
            }
            else if (tag.StartsWith("F"))
            {
                if (CLbl.Text == "") return;
                var tmpobj = new ListBox();
                ArrayList tmpa = new ArrayList();
                if(CLbl.Text.StartsWith("regional"))
                {
                    tmpa.Add("regional");
                    foreach (AirlineData adta in this.mainform.T3AirlinesData)
                    {
                        if (adta.ICAO.Contains("_"))
                        {
                            string[] arr = adta.ICAO.Split('_');
                            if (arr[1] == flightOperator[n].Text && arr[0] == airline)
                            {
                                tmpa.Add("regional," + arr[1]);
                            }
                        }
                    }
                }
                else if(CLbl.Text == "cargo")
                {
                    tmpa.Add("cargo");
                }
                else
                {
                    tmpa.Add("regular");
                    foreach (AirlineData adta in this.mainform.T3AirlinesData)
                    {
                        if (adta.ICAO.Contains("_"))
                        {
                            string[] arr = adta.ICAO.Split('_');
                            if (arr[1].Length > 3 && arr[0] == airline)
                            {
                                tmpa.Add(arr[1]);
                            }
                        }
                    }
                }
                int i = 0;
                foreach (string kk in tmpa)
                {
                    tmpobj.Items.Add(kk);
                    if (CLbl.Text == kk) tmpobj.SetSelected(i, true);
                    i++;
                }
                tmpobj.KeyDown += OnSubActionCKey;
                tmpobj.SelectedIndexChanged += OnSubActionF;
                tmpobj.MouseDown += OnSubActionEMClick;
                tmpobj.Top = CLbl.Top;
                tmpobj.Left = CLbl.Left;
                tmpobj.Tag = n;
                tmpobj.Parent = this;
                tmpobj.Width = CLbl.Width + 20;
                tmpobj.BringToFront();
                tmpobj.MouseHover += listbox2_MouseHover;
                tmpobj.Focus();
                openEdits++;
            }
        }

        private void listbox2_MouseHover(object sender, EventArgs e)
        {
            var tmpobj = (ListBox)sender;
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(tmpobj, "Right click to close");
        }

        private void textbox_MouseHover(object sender, EventArgs e)
        {
            var tmpobj = (System.Windows.Forms.TextBox)sender;
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(tmpobj, "ENTER to change, ESC to cancel");
        }

        void OnSubActionB(object sender, EventArgs e)
        {
            var tmpobj = (ListBox)sender;
            flightOperator[(int)tmpobj.Tag].Text = (string)tmpobj.SelectedItem;
            updateAirplanes((int)tmpobj.Tag, 0);
            tmpobj.Dispose();
            openEdits--;
        }

        void OnSubActionC(object sender, EventArgs e)
        {
            var tmpobj = (ListBox)sender;
            flightDestination[(int)tmpobj.Tag].Text = (string)tmpobj.SelectedItem;
            tmpobj.Dispose();
            openEdits--;
        }

        void refreshAddList(ListBox tmpobj, int n)
        {
            tmpobj.Items.Clear();
            foreach (string kk in listarray[n])
            {
                if (listfilter[n].Length > 0)
                {
                    if (kk.StartsWith(listfilter[n]))
                        tmpobj.Items.Add(kk);
                }
                else
                {
                    tmpobj.Items.Add(kk);
                }
            }
        }

        void OnSubActionCKeyList1(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        void OnSubActionCKeyList(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var tmpobj = (ListBox)sender;
            int n = (int)tmpobj.Tag;
            if (e.KeyCode == Keys.Escape)
            {
                tmpobj.Dispose();
                openEdits--;
                return;
            }
            char c = (char)e.KeyCode;
            if (char.IsLetter(c))
            {
                listfilter[n] += c;
                listfilter[n] = listfilter[n].ToUpper();
                refreshAddList(tmpobj,n);
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
            if (e.KeyCode == Keys.Back)
            {
                if (listfilter[n].Length > 0)
                {
                    listfilter[n] = listfilter[n].Remove(listfilter[n].Length - 1);
                    refreshAddList(tmpobj,n);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
        }

        void OnSubActionCKey(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var tmpobj = (ListBox)sender;
            if (e.KeyCode == Keys.Escape)
            {
                tmpobj.Dispose();
                openEdits--;
                return;
            }
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
        }
        void OnSubActionD(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var tmpobj = (System.Windows.Forms.TextBox)sender;
            if (e.KeyCode == Keys.Escape)
            {
                tmpobj.Dispose();
                openEdits--;
                return;
            }
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            int n1 = 0;
            try
            {
                n1 = int.Parse(tmpobj.Text);
            }
            catch
            {
                MessageBox.Show("Not a positive integer", "ERROR");
                return;
            }
            if(n1<=0)
            {
                MessageBox.Show("Not a positive integer", "ERROR");
                return;
            }
            int n0 = int.Parse(flightNumber[(int)tmpobj.Tag].Text);
            flightNumber[(int)tmpobj.Tag].Text = tmpobj.Text;
            double pp;
            if (((CheckBox)flights[(int)tmpobj.Tag]).Checked)
            {
                nflightsTot += n1 - n0;
                pp = 0.0;
                if (parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0 != 0)
                    pp = ((double)nflightsTot) / ((parent.nflights[0] + parent.nflights[1] + parent.nflights[2] + parent.nflights[3] + nflightsTot - nflightsTot0));
                label5.Text = nflightsTot + " " + String.Format("{0,6:0.000}%", 100*pp);
            }
            else
            {
                n1 = 0;
            }
            updateAllPerc();
            tmpobj.Dispose();
            openEdits--;
        }

        void OnSubActionE(object sender, EventArgs e)
        {
            var tmpobj = (ListBox)sender;
            flightAirplanes[(int)tmpobj.Tag].Text = "";
            foreach(int i in tmpobj.SelectedIndices)
            {
                if (flightAirplanes[(int)tmpobj.Tag].Text != "") flightAirplanes[(int)tmpobj.Tag].Text += ",";
                flightAirplanes[(int)tmpobj.Tag].Text += (string)tmpobj.Items[i];
            }
            flightAirplanes[(int)tmpobj.Tag].Width = flightAirplanes[(int)tmpobj.Tag].Text.Length * 7;
            //flightDestination[(int)tmpobj.Tag].Text = (string)tmpobj.SelectedItem;
            //tmpobj.Dispose();
            //openEdits--;
        }

        void updateAirplanes(int n, int oper)
        {
            if(oper == 0)
            {
                if (parent.n1 == 0 || parent.n1 == 2) return;
                string extra = "";
                flightAirplanesSpecial[n].Text = "regional";
                string[] apls = null;
                foreach (AirlineData adta in this.mainform.T3AirlinesData)
                {
                    if (adta.ICAO == airline + extra) apls = adta.airplanes;
                }
                bool isOK = false;
                foreach(string aaa in apls)
                {
                    if (aaa == flightAirplanes[n].Text) isOK = true;
                }
                if (!isOK) flightAirplanes[n].Text = apls[0];
            }
            else if(oper == 1)
            {
                string extra = "";
                string txt = flightAirplanesSpecial[n].Text;
                if (txt != "regular" && !txt.StartsWith("regional") && txt != "cargo")
                {
                    extra = "_" + txt;
                }
                else if (flightOperator[n].Text != airline)
                {
                    if (txt.Contains(","))
                    {
                        extra = "_" + txt.Split(',')[1];
                    }
                }
                string[] apls = null;
                foreach (AirlineData adta in this.mainform.T3AirlinesData)
                {
                    if (adta.ICAO == airline + extra) apls = adta.airplanes;
                }
                bool isOK = false;
                foreach (string aaa in apls)
                {
                    if (aaa == flightAirplanes[n].Text) isOK = true;
                }
                if (!isOK) flightAirplanes[n].Text = apls[0];
            }
        }

        void OnSubActionF(object sender, EventArgs e)
        {
            var tmpobj = (ListBox)sender;
            flightAirplanesSpecial[(int)tmpobj.Tag].Text = (string)tmpobj.SelectedItem;
            updateAirplanes((int)tmpobj.Tag, 1);
            tmpobj.Dispose();
            openEdits--;
        }

        void OnSubActionEKey(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var tmpobj = (ListBox)sender;
            if (e.KeyCode == Keys.Escape)
            {
                tmpobj.Dispose();
                openEdits--;
                return;
            }
            if (e.KeyCode != Keys.Enter)
            {
                tmpobj.Dispose();
                openEdits--;
                return;
            }
        }

        void OnSubActionEMClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var tmpobj = (ListBox)sender;
            if (e.Button == MouseButtons.Right)
            {
                tmpobj.Dispose();
                openEdits--;
                return;
            }
        }

        void OnSubActionEMClick1(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var tmpobj = (ListBox)sender;
            if (e.Button == MouseButtons.Right)
            {
                if (tmpobj.SelectedIndices.Count == 0)
                {
                    MessageBox.Show("Please select at least one airplane!", "ERROR");
                    return;
                }
                tmpobj.Dispose();
                openEdits--;
                return;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openEdits>0)
            {
                MessageBox.Show("There are unfinished edits on the page", "ERROR");
                return;
            }
            //check for duplications and empty airplanes
            Hashtable hst = new Hashtable();
            string dups = "";
            string aprs = "";
            for (int j = 0; j < flights.Length; j++)
            {
                if (((CheckBox)flights[j]).Checked)
                {
                    if (hst.ContainsKey(flightOperator[j].Text + "-" + flightDestination[j].Text + "-" + flightAirplanesSpecial[j].Text))
                    {
                        dups += flightOperator[j].Text + "-" + flightDestination[j].Text + "-" + flightAirplanesSpecial[j].Text + "\n";
                    }
                    else
                    {
                        hst[flightOperator[j].Text + "-" + flightDestination[j].Text + "-" + flightAirplanesSpecial[j].Text] = "1";
                    }
                    if (flightAirplanes[j].Text == "----")
                    {
                        aprs += flightDestination[j].Text + " has missing airplanes\n";
                    }
                }
            }
            if(dups != "")
            {
                MessageBox.Show("Duplicate flights:\n\n" + dups, "ERROR");
                return;
            }
            if(aprs != "")
            {
                MessageBox.Show("Missing airplanes:\n\n" + aprs, "ERROR");
                return;
            }
            for (int j=0; j<parent.terminals.Count; j++)
            {
                TerminalData td = (TerminalData)parent.terminals[j];
                string itText = td.name + ": " + td.gates;
                int i = 0;
                if(listBox1.Items.Contains(itText))
                {
                    i = listBox1.Items.IndexOf(itText);
                }
                else
                {
                    i = listBox1.Items.IndexOf("*" + itText);
                }
                if (listBox1.SelectedIndices.Contains(i))
                {
                    if(!td.airlines.Contains(airline))
                    {
                        string newairlnes = td.airlines + "," + airline;
                        td.airlines = newairlnes;
                        parent.terminals[j] = td;
                    }
                }
                else
                {
                    if (td.airlines.Contains(airline))
                    {
                        string []  arr = td.airlines.Split(',');
                        string newairlnes = "";
                        foreach(string item in arr)
                        {
                            if(!item.Contains(airline))
                            {
                                if (newairlnes != "") newairlnes += ",";
                                newairlnes += item;
                            }
                        }
                        td.airlines = newairlnes;
                        parent.terminals[j] = td;
                    }
                }
            }
            //apply
            parent.deleteFlights(airline);
            for(int j = 0; j<flights.Length; j++)
            {
                if (((CheckBox)flights[j]).Checked)
                {
                    if (parent.n1 < 3)
                    {
                        //airplanes
                        string extra = flightAirplanesSpecial[j].Text.Split(',')[0];
                        string extra1 = "";
                        if (flightAirplanesSpecial[j].Text.Split(',').Length == 2) extra1 = flightAirplanesSpecial[j].Text.Split(',')[1];
                        if (extra == "regular") extra = "";
                        string key = flightDestination[j].Text + "-" + airline + "-" + flightOperator[j].Text + "-" + extra + "-" + extra1;
                        parent.airplanes[parent.n2][key] = flightAirplanes[j].Text;
                        //flights
                        if (parent.flights[parent.n1].ContainsKey(flightDestination[j].Text))
                        {
                            Hashtable hs = (Hashtable)parent.flights[parent.n1][flightDestination[j].Text];
                            hs[airline + "," + flightOperator[j].Text + "," + extra + "," + extra1] = int.Parse(flightNumber[j].Text);
                            parent.flights[parent.n1][flightDestination[j].Text] = hs;
                        }
                        else
                        {
                            Hashtable hs = new Hashtable();
                            hs[airline + "," + flightOperator[j].Text + "," + extra + "," + extra1] = int.Parse(flightNumber[j].Text);
                            parent.flights[parent.n1][flightDestination[j].Text] = hs;
                        }
                    }
                    else
                    {
                        //airplanes
                        string key = flightDestination[j].Text;
                        string[] arr = flightAirplanes[j].Text.Split(',');
                        if (parent.airplanes[parent.n2].ContainsKey(key))
                        {
                            string planes = (string)parent.airplanes[parent.n2][key];
                            int l = 0;
                            foreach (string np in arr)
                            {
                                if (!planes.Contains(np))
                                {
                                    planes += "," + np;
                                    l++;
                                }
                            }
                            if (l > 0) parent.airplanes[parent.n2][key] = planes;
                        }
                        else
                        {
                            parent.airplanes[parent.n2][key] = flightAirplanes[j].Text;
                        }
                        //flights
                        if (parent.flights[parent.n1].ContainsKey(key))
                        {
                            Hashtable hs = (Hashtable)parent.flights[parent.n1][key];
                            hs[airline + "," + parent.newGAcallsign] = int.Parse(flightNumber[j].Text);
                            parent.flights[parent.n1][key] = hs;
                        }
                        else
                        {
                            Hashtable hs = new Hashtable();
                            hs[airline + "," + parent.newGAcallsign] = int.Parse(flightNumber[j].Text);
                            parent.flights[parent.n1][key] = hs;
                        }
                    }
                }
            }
            parent.nflights[parent.n1] += nflightsTot - nflightsTot0;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (T3Airports.Count > 0) return;
            if (comboBox1.SelectedIndex == 0) return;
            //read TS3 airports - customized file in current db directory
            string path = "";
            if (!File.Exists(Path.Combine(parent.location, "Airports", parent.airport, "databases", parent.database, "airports.csv")))
            {
                if (File.Exists(Path.Combine(parent.location, "Airports", parent.airport, "databases", "default", "airports.csv")))
                {
                    path = Path.Combine(parent.location, "Airports", parent.airport, "databases", "default", "airports.csv");
                }
                else
                {
                    path = Path.Combine(parent.location, "Airports", "airports.csv");
                }
            }
            else
            {
                path = Path.Combine(parent.location, "Airports", parent.airport, "databases", parent.database, "airports.csv");
            }
            StreamReader ff = new StreamReader(path);
            ff.ReadLine();
            string txt;
            while ((txt = ff.ReadLine()) != null)
            {
                string[] vals = Form2.splitCSV(txt);
                T3Airports[vals[0]] = "1";
            }
            ff.Close();
        }
    }
}
