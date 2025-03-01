using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace T3Scheduler
{
    public partial class Form2 : Form
    {
        string T3location;
        string airport;
        string database;
        Form1 parent;

        Hashtable airlines;
        Hashtable airlinesGA;
        Hashtable airplanes;
        Hashtable colors;
        Hashtable airportsT3S;
        Hashtable airportsSSS;
        Hashtable airportsSSSnew;
        Hashtable airportsNEW;

        Hashtable AirlineSpecial;

        string[] numw = { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINER" };
        string letters = "abcdefghijklmnopqrstuvwxyz";
        string lettersnums = "";
        string[] natostr = { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliett", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa", "Quebec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-ray", "Yankee", "Zulu", "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINER" };

        public static string[] splitCSV(string input)
        {
            //if (input.Contains("\",\""))
            //{
            //    string pattern = @"""\s*,\s*""";
            //
                // input.Substring(1, input.Length - 2) removes the first and last " from the string
            //    string[] tokens = System.Text.RegularExpressions.Regex.Split(
            //        input.Substring(1, input.Length - 2), pattern);
            //    return tokens;
            //}
            if(input.Contains(",\""))
            {
                string tmpstr = "";
                bool inside = false;
                char lastc = '\t';
                for (int ii = 0; ii < input.Length - 1; ii++)
                {
                    if (ii > 0) lastc = input[ii - 1];
                    if (ii == 0 && input[ii] == '"') continue;
                    if (input[ii] == ',')
                    {
                        if (input[ii + 1] == '"' && lastc == '"')
                        {
                            tmpstr += '\t';
                            inside = true;
                            continue;
                        }
                        else if (lastc == '"')
                        {
                            tmpstr += "\t";
                            inside = false;
                            continue;
                        }
                        else if (input[ii + 1] == '"')
                        {
                            tmpstr += "\t";
                            inside = true;
                            continue;
                        }

                        if(inside)
                        {
                            tmpstr += ',';
                        }
                        else
                        {
                            tmpstr+= "\t";
                        }
                    }
                    else if (input[ii] == '"')
                    {
                        if (lastc == ',') continue;
                        if (input[ii + 1] == ',') continue;
                        tmpstr += input[ii];
                    }
                    else
                    {
                        tmpstr += input[ii];
                    }
                }
                if(input.EndsWith(","))
                {
                    tmpstr += "\t";
                }
                else if (!input.EndsWith("\""))
                {
                    tmpstr += input[input.Length - 1];
                }
                return tmpstr.Split('\t');
            }
            else
            {
                return input.Split(',');
            }
        }
        private string getNATO(string str)
        {
            string nato = "";
            for (int i = 0; i < str.Length; i++)
            {
                int j = lettersnums.IndexOf(str[i]);
                if (nato != "") nato += " ";
                if (j > -1) nato += natostr[j];
            }
            return nato.ToUpper();
        }
        public Form2(string T3loc, string airp, string datab, Form1 myparent)
        {
            InitializeComponent();
            this.Text = "T3Scheduler Import";
            T3location = T3loc;
            airport = airp;
            database = datab;
            parent = myparent;

            label1.Text = "";
            label2.Text = "";
            label4.Text = "";
            missliveries.SelectedIndex= 1;  

            string[] airpls = Directory.GetDirectories(Path.Combine(T3location, "Airplanes"));
            foreach (string airpd in airpls)
            {
                if (new DirectoryInfo(airpd).Name == "default") continue;
                airplanedb.Items.Add(new DirectoryInfo(airpd).Name);
            }
            airplanedb.SelectedIndex = 0;

            //read airplanes all
            string[] apls = Directory.GetDirectories(Path.Combine(T3location, "Airplanes", (string)airplanedb.SelectedItem));
            string txt;
            airplanes = new Hashtable();
            colors = new Hashtable();
            foreach (string aplstr in apls)
            {
                string apl = new DirectoryInfo(aplstr).Name;
                //label4.Text += "'" + apl + "'\n";
                string[] arlns = Directory.GetDirectories(Path.Combine(T3location, "Airplanes", (string)airplanedb.SelectedItem, apl));
                Hashtable arlnsH = new Hashtable();
                foreach (string arln in arlns)
                {
                    string arlnN = new DirectoryInfo(arln).Name;
                    arlnsH[arlnN] = "1";
                    if (colors.ContainsKey(arlnN))
                    {
                        colors[arlnN] = (string)colors[arlnN] + "," + apl;
                    }
                    else
                    {
                        colors[arlnN] = apl;
                    }
                    //label4.Text += " " + arlnN;
                }
                //label4.Text += "\n";
                airplanes[apl] = arlnsH;
            }

            //read airlines 
            StreamReader ff = new StreamReader(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "airlines.csv"));
            ff.ReadLine();
            airlines = new Hashtable();
            while ((txt = ff.ReadLine()) != null)
            {
                string[] vals = splitCSV(txt);
                string sss = vals[0].ToUpper();
                airlines[sss] = vals;
            }
            ff.Close();
            ff = new StreamReader(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "ga.csv"));
            ff.ReadLine();
            airlinesGA = new Hashtable();
            string nums = "0123456789";
            string[] numw = { "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINER", "ZERO" };
            while ((txt = ff.ReadLine()) != null)
            {
                string[] vals = splitCSV(txt);
                if (vals[0].Contains("-")) continue;
                string nme = "";
                for (int i = 0; i < vals[0].Length; i++)
                {
                    if (nums.Contains(vals[0][i])) break;
                    nme += vals[0][i];
                }
                nme = nme.Trim();
                if (nme.Length != 3) continue;
                string nmefull = "";
                string[] tmpstr = vals[1].Split(" ".ToCharArray());
                for (int i = 0; i < tmpstr.Length; i++)
                {
                    if (numw.Contains(tmpstr[i])) break;
                    if (nmefull != "") nmefull += " ";
                    nmefull += tmpstr[i];
                }
                string[] vals1 = { nme, nmefull, "GA", "GA" };
                airlinesGA[nme] = vals1;
            }
            ff.Close();
            //special cases
            AirlineSpecial = new Hashtable();
            AirlineSpecial["JANET"] = "WWW";

            button3.Enabled= false;
            button5.Enabled= false;
            button6.Enabled = false;
            airportsearchoption.SelectedIndex = 1;
            airportupdateoption.SelectedIndex = 1;

            letters = letters.ToUpper();
            lettersnums = letters + nums;

            //read TS3 airports - customized file in current db directory
            if(!File.Exists(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "airports.csv")))
            {
                if(File.Exists(Path.Combine(T3location, "Airports", (string)airport, "databases", "default", "airports.csv")))
                {
                    File.Copy(Path.Combine(T3location, "Airports", (string)airport, "databases", "default", "airports.csv"), 
                        Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "airports.csv"));
                }
                else
                {
                    File.Copy(Path.Combine(T3location, "Airports", "airports.csv"),
                        Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "airports.csv"));
                }
            }
            ff = new StreamReader(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "airports.csv"));
            ff.ReadLine();
            airportsT3S = new Hashtable();
            while ((txt=ff.ReadLine())!=null) 
            {
                string[] vals = splitCSV(txt);
                airportsT3S[vals[0]] = vals;
            }
            ff.Close();
            //read T3Scheduler included airports
            ff = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "airports-iata-icao.csv"));
            ff.ReadLine();
            airportsSSS = new Hashtable();
            while ((txt = ff.ReadLine()) != null)
            {
                string[] vals = splitCSV(txt);
                airportsSSS[vals[3]] = vals;
            }
            airportsSSSnew = new Hashtable();
            ff.Close();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = folderBrowserDialog1.FileName;
                if (label1.Text != "" && label2.Text != "") button3.Enabled = true;
                button5.Enabled = false;
                button6.Enabled = false;
                label4.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.Text = folderBrowserDialog1.FileName;
                if (label1.Text != "" && label2.Text != "") button3.Enabled = true;
                button5.Enabled = false;
                button6.Enabled = false;
                label4.Text = "";
            }
        }

        Hashtable arrflights;
        Hashtable arrflightsGA;
        Hashtable depflights;
        Hashtable depflightsGA;
        Hashtable airlinesL;
        Hashtable airplanesL;
        Hashtable airlinesLB;
        Hashtable airplanesLB;
        Hashtable liveries;

        class flight
        {
            public string info;
            public int fid;
            public string fidS;
            public string arp;
            public string aplane;
            public string ftype;
            public flight(string info, int fid, string arp, string aplane, string ftype, string fidS)
            {
                this.info = info;
                this.fid = fid;
                this.arp = arp;
                this.aplane = aplane;
                this.ftype = ftype;
                this.fidS = fidS;
            }
        }

        Hashtable MappingsAirlines;
        Hashtable MappingsAirplanes;
        Hashtable MappingsLiveries;
        private bool readMappings()
        {
            MappingsAirlines = new Hashtable();
            MappingsAirplanes = new Hashtable();
            MappingsLiveries = new Hashtable();
            if (label5.Text == "(optional)") return true;
            StreamReader fr = new StreamReader(label5.Text);
            string txt;
            int section = 0;
            while ((txt=fr.ReadLine()) != null)
            {
                if (txt.StartsWith("#")) continue;
                if(txt.StartsWith("=="))
                {
                    if (txt != "==airlines==" && txt != "==airplanes==" && txt != "==liveries==")
                    {
                        label4.Text = "ERROR in mappings file - invalid section '" + txt + "'";
                        fr.Close();
                        return false;
                    }
                    section++;
                    if (section > 3)
                    {
                        label4.Text = "ERROR in mappings file - too many sections";
                        fr.Close();
                        return false;
                    }
                    continue;
                }
                string[] arr = txt.Split(',');
                if (section == 1)
                {
                    MappingsAirlines[arr[0]] = arr[1];
                }
                else if (section == 2)
                {
                    if (!airplanes.ContainsKey(arr[1]) && arr[1] != "----")
                    {
                        if (arr[1] == "newPlaneICAO")
                        {
                            label4.Text = "ERROR in mappings file - 'newPlaneICAO' must be replaced with valid ICAO code present in TS3 database\n ";
                            label4.Text += "                         otherwise just delete this line from mappings file ";
                        }
                        else
                        {
                            label4.Text = "ERROR in mappings file - missing/invalid airplane code " + arr[1];
                        }
                        fr.Close();
                        return false;
                    }
                    MappingsAirplanes[arr[0]] = arr[1];
                }
                else if (section == 3)
                {
                    string[] arr1 = arr[1].Split("-".ToCharArray());
                    if (colors.ContainsKey(arr1[0]))
                    {
                        if (!((string)(colors[arr1[0]])).Contains(arr1[1]))
                        {
                            if (arr[1] == "newPlane")
                            {
                                label4.Text = "ERROR in mappings file - 'newPlane' must be replaced with valid ICAO code present in TS3 database\n ";
                                label4.Text += "                         otherwise just delete this line from mappings file ";
                            }
                            else
                            {
                                label4.Text = "ERROR in mappings file - missing/invalid airplane in livery mapping " + txt;
                            }
                            fr.Close();
                            return false;
                        }
                    }
                    else
                    {
                        label4.Text = "ERROR in mappings file - missing/invalid airline in livery mapping " + txt;
                        fr.Close();
                        return false;
                    }
                    MappingsLiveries[arr[0]] = arr[1];
                }
                else
                {
                    label4.Text = "ERROR in mappings file - no section header at the start";
                    fr.Close();
                    return false;
                }
            }
            fr.Close();
            return true;
        }

        string airportError;

        private string getAirport(string txt, bool ignoreerror)
        {
            int i1 = txt.IndexOf("(");
            int i2 = txt.IndexOf(")");
            string airport = "";
            if (i1 == -1 && i2 == -1)
            {
                airport = txt.Replace(" ", "");
            }
            else
            {
                if (i1 == -1) return "XXXX";
                if (i2 == -1) return "XXXX";
                airport = txt.Substring(i1 + 1, i2 - i1 - 1);
                airport = airport.Replace(" ", "");
            }
            if(airport.Length>4)
            {
                if(airport.Contains("/"))
                {
                    string[] arr3 = airport.Split('/');
                    airport = arr3[1].Trim();
                    if (airport.Length == 3) airport = arr3[0].Trim();
                }
            }
            if (airportsT3S.ContainsKey(airport)) return airport;
            if (airportsNEW.ContainsKey(airport))
            {
                if (airportupdateoption.SelectedIndex == 1)
                {
                    return airport;
                }
                else
                {
                    return (string)airportsNEW[airport];
                }
            }
            //find airport in full list
            if (!airportsSSS.ContainsKey(airport))
            {
                if (!ignoreerror)
                {
                    //try to find 
                    string[] newarr = getairportinfo(airport);
                    if(newarr == null)
                    {
                        //return "" and ignore
                        return "";
                    }
                    airportsSSS[airport] = newarr;
                    airportsSSSnew[airport] = newarr;
                }
                else
                {
                    if (airportError != "") airportError += "\n";
                    airportError += "Unknown airport " + airport;
                    return airport;
                }
            }
            //find closest airport from T3S
            string[] aSSS = (string[])airportsSSS[airport];
            double lat = double.Parse(aSSS[5]);
            double lon = double.Parse(aSSS[6]);
            double dist = 9999.0;
            string airportT3S = "";
            string[] airportT3Sinfo = null;
            foreach (string aaa in airportsT3S.Keys)
            {
                string [] aT3S = (string[])airportsT3S[aaa];
                double lat1 = double.Parse(aT3S[2]);
                double lon1 = double.Parse(aT3S[3]);
                double latdist = Math.Abs(lat-lat1);
                double londist = Math.Abs(lon-lon1);
                if (londist > 360.0) londist = londist - 360.0;
                if (dist > londist + latdist)
                {
                    dist = londist + latdist;
                    airportT3S = aaa;
                    airportT3Sinfo = aT3S;
                }
            }
            if (airportupdateoption.SelectedIndex == 1)
            {
                string[] newarr = new string[7];
                newarr[0] = aSSS[3];
                newarr[1] = aSSS[4];
                newarr[2] = double.Parse(aSSS[5]).ToString("F2");
                newarr[3] = double.Parse(aSSS[6]).ToString("F2");
                newarr[4] = aSSS[2];
                newarr[5] = airportT3Sinfo[5];
                newarr[6] = aSSS[0];
                airportsNEW[airport] = newarr;
                if (airportError != "") airportError += "\n";
                airportError += "airport " + airport + " will be added to Tower Simulator 3 list";
                return airport;
            }
            else
            {
                airportsNEW[airport] = airportT3S;
                if (airportError != "") airportError += "\n";
                airportError += "airport " + airport + " will be replaced by Tower Simulator 3 airport " + airportT3S;
                return airportT3S;
            }
        }

        private string[] getairportinfo(string airport)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            string webData = "";
            string[] result = null;

            try
            {
                result = new string[7];
                webData = wc.DownloadString("http://www.airnav.com/airport/" + airport);
                if (!webData.Contains(airport.ToUpper())) return null;
                int i1 = webData.IndexOf(airport.ToUpper());
                webData = webData.Substring(i1 + 9);
                i1 = webData.IndexOf("<b>") + 3;
                int i2 = webData.IndexOf("</b>");
                string aname = webData.Substring(i1, i2 - i1);
                webData = webData.Substring(i2 + 8);
                webData = webData.Substring(webData.IndexOf("<b>") + 3);
                i2 = webData.IndexOf("</b>");
                result[4] = webData.Substring(0, i2);
                i1 = webData.IndexOf("</font>");
                string[] tmparr = webData.Substring(0, i1).Split(',');
                result[1] = tmparr[1].Trim();
                result[0] = tmparr[2].Trim();
                webData = webData.Substring(webData.IndexOf("FAA Identifier"));
                webData = webData.Substring(webData.IndexOf("<TD>") + 4);
                result[2] = webData.Substring(0, 3);
                result[3] = airport;
                webData = webData.Substring(webData.IndexOf("Lat/Long"));
                webData = webData.Substring(webData.IndexOf("valign=top>") + 11);
                string tmp = webData.Substring(0, webData.IndexOf("</TD>"));
                tmp = tmp.Replace("<BR>", "|");
                tmparr = tmp.Split('|');
                tmparr = tmparr[2].Split(',');
                result[5] = tmparr[0];
                result[6] = tmparr[1];
                label4.Text += "\nNew airport data from Internet:\n";
                for (int ii = 0; ii < 7; ii++)
                {
                    if (ii > 0) label4.Text += ",";
                    label4.Text += "'" + result[ii] + "'";
                }
                label4.Text += "\n";
            }
            catch
            {
                return null;
            }
            return result;
        }

        private string getTime(string txt)
        {
            string[] arr = txt.Split(" ".ToCharArray());
            if (arr.Length < 2) return "";
            int hr = 0;
            int i = 1;
            try
            {
                hr = int.Parse(arr[i].Substring(0, 2));
            }
            catch 
            {
                try 
                {
                    i = 0;
                    hr = int.Parse(arr[i].Substring(0, 2));
                }
                catch { return ""; } 
            }
            string mn = arr[i].Substring(3, 2);
            if (arr[i].Length > 5)
            {
                string tt = arr[i].Substring(5);
                if (tt.StartsWith("PM") && hr != 12) hr += 12;
            }
            return hr.ToString("D2") + ":" + mn;
        }

        string nums = "0123456789";
        private string flightidrnd(string flightid, ref Random rnd)
        {
            int ii = 0;
            StringBuilder fff = new StringBuilder(flightid);
            if (flightid.StartsWith("-")) ii = 1;
            int i = rnd.Next(ii, flightid.Length);
            if (nums.Contains(flightid[i]))
            {
                if(flightid[i] == '9')
                {
                    fff[i] = '0';
                }
                else
                {
                    fff[i]++;
                }
            }
            else
            {
                if (flightid[i] == 'Z')
                {
                    fff[i] = 'A';
                }
                else
                {
                    fff[i]++;
                }
            }
            return fff.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            //read T3Scheduler included airports
            StreamReader ffr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "airports-iata-icao.csv"));
            ffr.ReadLine();
            string txt;
            airportsSSS = new Hashtable();
            while ((txt = ffr.ReadLine()) != null)
            {
                string[] vals = splitCSV(txt);
                airportsSSS[vals[3]] = vals;
            }
            ffr.Close();

            label4.Text = "";
            if (!readMappings())
            {
                Cursor.Current = Cursors.Default;
                Application.DoEvents();
                return;
            }
            int line = 0;
            int flightsL = 0;
            string nums = "0123456789";
            airlinesL = new Hashtable();
            airplanesL = new Hashtable();
            airlinesLB = new Hashtable();
            airplanesLB = new Hashtable();
            liveries = new Hashtable();
            arrflights = new Hashtable();
            arrflightsGA = new Hashtable();
            depflights = new Hashtable();
            depflightsGA = new Hashtable();
            Hashtable GAtimes = new Hashtable();
            Hashtable allflights = new Hashtable();
            airportsNEW = new Hashtable();
            airportError = "";
            int nga = 0;
            int narl = 0;
            int linesarl = 0;
            int linesarp = 0;
            int linesliv = 0;
            int nnn = 0;
            Hashtable fids = new Hashtable();
            StreamReader fr = new StreamReader(label1.Text);
            while ((txt = fr.ReadLine()) != null)
            {
                line++;
                if (allflights.ContainsKey(txt)) continue;
                if (txt.Contains("<span") || txt.Contains("Near ")) continue;
                allflights[txt] = "1";
                bool validf = true;
                int flightnumber = -1;
                string flightid = "";
                string[] arr = txt.Split(new char[] { '\t' });
                if (arr.Length < 5)
                {
                    label4.Text += "WARNING: ARR line " + line + " splits to " + arr.Length + " elements, should be 5 or more, ignoring\n";
                    continue;
                }
                if(comboBox1.SelectedIndex==1)
                {
                    for(int i=1; i<arr.Length-1; i++)
                        arr[i] = arr[i + 1];
                    arr[arr.Length - 1] = "";
                }
                else
                {
                    if (arr[1].Length > 4)
                    {
                        MessageBox.Show("Second column length is more than 4 characters, looks like premium file, while regular is selected.\n" + arr[1], "ERROR");
                        label4.Text = "";
                        fr.Close();
                        return;
                    }
                }
                bool skip = false;
                for(int i=0; i<5; i++)
                {
                    if (arr[i].Trim() == "")
                    {
                        label4.Text += "WARNING: column " + (i + 1).ToString() + " in ARR line " + line + " is empty. Ignoring.\n";
                        skip = true;
                    }    
                }
                if (skip) continue;
                if ((string)MappingsAirplanes[arr[1]] == "----") continue;
                if (getAirport(arr[2], false) == "") continue;
                flightsL++;
                string arl = "";
                if (arr[0].Contains("-"))
                {
                    arl = arr[0].Split('-')[0];
                    flightid = "-" + arr[0].Split('-')[1];
                }
                else
                {
                    bool hhh = true;
                    for (int i = 0; i < arr[0].Length; i++)
                    {
                        if (nums.Contains(arr[0][i])) { hhh = false; }
                        if (hhh)
                        {
                            arl += arr[0][i];
                        }
                        else
                        {
                            flightid += arr[0][i];
                        }
                    }
                }
                bool isga = false;
                try
                {
                    flightnumber = int.Parse(arr[0].Substring(arl.Length));
                }
                catch { }
                if (AirlineSpecial.ContainsKey(arl)) arl = (string)AirlineSpecial[arl];
                string tme = getTime(arr[4]);
                if (tme == "")
                {
                    label4.Text = "Invalid arrival time in line " + line + " arrivals file '" + arr[4] + "'\n" + txt.Replace("\t", "   ");
                    Cursor.Current = Cursors.Default;
                    Application.DoEvents();
                    return;
                }
                if (arl.Length != 3 || flightid.Contains("-"))
                {
                    isga = true;
                    nga++;
                    if (GAtimes.ContainsKey(arr[0]))
                    {
                        int mm = int.Parse(tme.Substring(0, 2)) * 60 + int.Parse(tme.Substring(3, 2));
                        string tme1 = (string)GAtimes[arr[0]];
                        int mm1 = int.Parse(tme1.Substring(0, 2)) * 60 + int.Parse(tme1.Substring(3, 2));
                        int mmdiff = Math.Abs(mm - mm1);
                        if (mmdiff < 40)
                        {
                            if (mm > mm1)
                            {
                                int hr1 = int.Parse(tme.Substring(0, 2)) + 1;
                                if (hr1 > 24) hr1 = hr1 - 24;
                                tme = hr1.ToString("D2") + tme.Substring(2);
                            }
                            else
                            {
                                int hr1 = int.Parse(tme.Substring(0, 2)) - 1;
                                if (hr1 < 0) hr1 = hr1 + 24;
                                tme = hr1.ToString("D2") + tme.Substring(2);
                            }
                        }
                    }
                    else
                    {
                        
                        GAtimes[arr[0]] = tme;
                    }
                    while (fids.ContainsKey(arl + flightid))
                    {
                        flightid = flightidrnd(flightid, ref rnd);
                    }
                    fids[arl + flightid] = "1";
                }
                else if(airlinesGA.ContainsKey(arl) || MappingsAirlines.ContainsKey(arl))
                {
                    isga = true;
                    nga++;
                    while (fids.ContainsKey(arl + flightnumber))
                    {
                        flightnumber++;
                    }
                    fids[arl + flightnumber] = "1";
                }
                else if(airlines.ContainsKey(arl))
                {
                    narl++;
                    airlinesL[arl] = "1";
                    while (fids.ContainsKey(arl + flightnumber))
                    {
                        flightnumber++;
                    }
                    fids[arl + flightnumber] = "1";
                }
                else
                {
                    //label4.Text += "Unknown airline " + arl + " line " + line + "\n";
                    airlinesLB[arl] = "0";
                    linesarl++;
                    validf = false;
                }
                if (!airplanes.ContainsKey(arr[1]) && !MappingsAirplanes.ContainsKey(arr[1]))
                {
                    //label4.Text += "Unknown airplane '" + arr[1] + "' line " + line + "\n";
                    airplanesLB[arr[1]] = "1";
                    linesarp++;
                    validf = false;
                }
                else
                {
                    if (!airplanes.ContainsKey(arr[1]))
                    {
                        arr[1] = (string)MappingsAirplanes[arr[1]];
                    }
                    Hashtable airlinesH = (Hashtable)airplanes[arr[1]];
                    if (!airlinesH.ContainsKey(arl) && !MappingsLiveries.ContainsKey(arl + "-" + arr[1]))
                    {
                        //label4.Text += "No livery for airplane '" + arr[1] + "' and airline " + arl + " line " + line + "\n";
                        liveries[arl + "-" + arr[1]] = "1";
                        linesliv++;
                        if (missliveries.SelectedIndex == 0) validf = false;
                    }
                    airplanesL[arr[1]] = "1";
                    if (validf) { nnn++; }
                }
                if (validf)
                {
                    if (isga)
                    {
                        string arl1 = arl;
                        string arpl = arr[1];
                        string callsign = "";
                        if (MappingsLiveries.ContainsKey(arl + "-" + arr[1]))
                        {
                            string[] arr2 = ((string)MappingsLiveries[arl + "-" + arr[1]]).Split("-".ToCharArray());
                            arl1 = arr2[0];
                            arpl = arr2[1];
                        }
                        if (arl1.Length > 1)
                        {
                            if (MappingsAirlines.ContainsKey(arl1))
                            {
                                callsign= (string)MappingsAirlines[arl1];
                            }
                            else if (airlinesGA.ContainsKey(arl1))
                            {
                                callsign = ((string[])airlinesGA[arl1])[1];
                            }
                            else
                            {
                                callsign = getNATO(arl1);
                            }
                        }
                        flight ff = new flight(arr[0], flightnumber, getAirport(arr[2], true), arpl, "", flightid);
                        if (ff.arp == "") ff.arp = airport;
                        if (arl.Length != 3 || flightid.Contains("-"))
                        {
                            ff.info = arl + flightid;
                            ff.ftype = getNATO(ff.info);
                        }
                        else
                        {
                            ff.info = arl1 + flightnumber;
                            ff.ftype = callsign + " " + getNATO(flightnumber.ToString());
                        }
                        arrflightsGA.Add(tme + "." + line.ToString("D6"), ff);
                    }
                    else
                    {
                        string arl1 = arl;
                        string arpl = arr[1];
                        if (MappingsLiveries.ContainsKey(arl + "-" + arr[1]))
                        {
                            string[] arr2 = ((string)MappingsLiveries[arl + "-" + arr[1]]).Split("-".ToCharArray());
                            arl1 = arr2[0];
                            arpl = arr2[1];
                        }
                        flight ff = new flight(arl1 + "," + arl1, flightnumber, getAirport(arr[2], true), arpl, "", "");
                        if (ff.arp == "") ff.arp = airport;
                        arrflights.Add(tme + "." + line.ToString("D6"), ff);
                    }
                }
            }
            fr.Close();
            fr = new StreamReader(label2.Text);
            line = 0;
            while ((txt = fr.ReadLine()) != null)
            {
                line++;
                if (allflights.ContainsKey(txt)) continue;
                if (txt.Contains("<span") || txt.Contains("Near ")) continue;
                allflights[txt] = "1";
                bool validf = true;
                int flightnumber = -1;
                string flightid = "";
                string[] arr = txt.Split(new char[] { '\t' });
                if (arr.Length < 5)
                {
                    label4.Text += "WARNING: DEP line " + line + " splits to " + arr.Length + " elements, should be 5 or more, ignoring\n";
                    continue;
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    for (int i = 1; i < arr.Length - 1; i++)
                        arr[i] = arr[i + 1];
                    arr[arr.Length - 1] = "";
                }
                else
                {
                    if (arr[1].Length > 4)
                    {
                        MessageBox.Show("Second column length is more than 4 characters, looks like premium file, while regular is selected.\n" + arr[1], "ERROR");
                        label4.Text = "";
                        fr.Close();
                        return;
                    }
                }
                bool skip = false;
                for (int i = 0; i < 5; i++)
                {
                    if (arr[i].Trim() == "")
                    {
                        label4.Text += "WARNING: column " + (i + 1).ToString() + " in DEP line " + line + " is empty. Ignoring.\n";
                        skip = true;
                    }
                }
                if (skip) continue;
                if ((string)MappingsAirplanes[arr[1]] == "----") continue;
                if (getAirport(arr[2], false) == "") continue;
                flightsL++;
                string arl = "";
                if (arr[0].Contains("-"))
                {
                    arl = arr[0].Split('-')[0];
                    flightid = "-" + arr[0].Split('-')[1];
                }
                else
                {
                    bool hhh = true;
                    for (int i = 0; i < arr[0].Length; i++)
                    {
                        if (nums.Contains(arr[0][i])) { hhh = false; }
                        if (hhh)
                        {
                            arl += arr[0][i];
                        }
                        else
                        {
                            flightid += arr[0][i];
                        }
                    }
                }
                bool isga = false;
                try
                {
                    flightnumber = int.Parse(arr[0].Substring(arl.Length));
                }
                catch { }
                if (AirlineSpecial.ContainsKey(arl)) arl = (string)AirlineSpecial[arl];
                string tme = getTime(arr[3]);
                if (tme == "")
                {
                    label4.Text = "Invalid departure time in line " + line + " depratures file '" + arr[3] + "'\n" + txt.Replace("\t", "   ");
                    Cursor.Current = Cursors.Default;
                    Application.DoEvents();
                    return;
                }
                if (arl.Length != 3 || flightid.Contains("-"))
                {
                    isga = true;
                    nga++;
                    if (GAtimes.ContainsKey(arr[0]))
                    {
                        int mm = int.Parse(tme.Substring(0, 2)) * 60 + int.Parse(tme.Substring(3, 2));
                        string tme1 = (string)GAtimes[arr[0]];
                        int mm1 = int.Parse(tme1.Substring(0, 2)) * 60 + int.Parse(tme1.Substring(3, 2));
                        int mmdiff = Math.Abs(mm - mm1);
                        if (mmdiff < 40)
                        {
                            if (mm > mm1)
                            {
                                int hr1 = int.Parse(tme.Substring(0, 2)) + 1;
                                if (hr1 > 24) hr1 = hr1 - 24;
                                tme = hr1.ToString("D2") + tme.Substring(2);
                            }
                            else
                            {
                                int hr1 = int.Parse(tme.Substring(0, 2)) - 1;
                                if (hr1 < 0) hr1 = hr1 + 24;
                                tme = hr1.ToString("D2") + tme.Substring(2);
                            }
                        }
                    }
                    else
                    {

                        GAtimes[arr[0]] = tme;
                    }
                    while (fids.ContainsKey(arl + flightid))
                    {
                        flightid = flightidrnd(flightid, ref rnd);
                    }
                    fids[arl + flightid] = "1";
                }
                else if (airlinesGA.ContainsKey(arl) || MappingsAirlines.ContainsKey(arl))
                {
                    isga = true;
                    nga++;
                    while (fids.ContainsKey(arl + flightnumber))
                    {
                        flightnumber++;
                    }
                    fids[arl + flightnumber] = "1";
                }
                else if (airlines.ContainsKey(arl))
                {
                    narl++;
                    airlinesL[arl] = "1";
                    while (fids.ContainsKey(arl + flightnumber))
                    {
                        flightnumber++;
                    }
                    fids[arl + flightnumber] = "1";
                }
                else
                {
                    //label4.Text += "Unknown airline " + arl + " line " + line + "\n";
                    airlinesLB[arl] = "0";
                    linesarl++;
                    validf = false;
                }
                if (!airplanes.ContainsKey(arr[1]) && !MappingsAirplanes.ContainsKey(arr[1]))
                {
                    //label4.Text += "Unknown airplane '" + arr[1] + "' line " + line + "\n";
                    airplanesLB[arr[1]] = "1";
                    linesarp++;
                    validf = false;
                }
                else
                {
                    if (!airplanes.ContainsKey(arr[1]))
                    {
                        arr[1] = (string)MappingsAirplanes[arr[1]];
                    }
                    Hashtable airlinesH = (Hashtable)airplanes[arr[1]];
                    if (!airlinesH.ContainsKey(arl) && !MappingsLiveries.ContainsKey(arl + "-" + arr[1]))
                    {
                        //label4.Text += "No livery for airplane '" + arr[1] + "' and airline " + arl + " line " + line + "\n";
                        liveries[arl + "-" + arr[1]] = "1";
                        linesliv++;
                        if (missliveries.SelectedIndex == 0) validf = false;
                    }
                    airplanesL[arr[1]] = "1";
                    if (validf) { nnn++; }
                }
                if (validf)
                {
                    if (isga)
                    {
                        string arl1 = arl;
                        string arpl = arr[1];
                        string callsign = "";
                        if (MappingsLiveries.ContainsKey(arl + "-" + arr[1]))
                        {
                            string[] arr2 = ((string)MappingsLiveries[arl + "-" + arr[1]]).Split("-".ToCharArray());
                            arl1 = arr2[0];
                            arpl = arr2[1];
                        }
                        if (arl1.Length > 1)
                        {
                            if (MappingsAirlines.ContainsKey(arl1))
                            {
                                callsign = (string)MappingsAirlines[arl1];
                            }
                            else if (airlinesGA.ContainsKey(arl1))
                            {
                                callsign = ((string[])airlinesGA[arl1])[1];
                            }
                            else
                            {
                                callsign = getNATO(arl1);
                            }
                        }
                        flight ff = new flight(arr[0], flightnumber, getAirport(arr[2], true), arpl, "", flightid);
                        if (ff.arp == "") ff.arp = airport;
                        if (arl.Length != 3 || flightid.Contains("-"))
                        {
                            ff.info = arl + flightid;
                            ff.ftype = getNATO(ff.info);
                        }
                        else
                        {
                            ff.info = arl1 + flightnumber;
                            ff.ftype = callsign + " " + getNATO(flightnumber.ToString());
                        }
                        depflightsGA.Add(tme + "." + line.ToString("D6"), ff);
                    }
                    else
                    {
                        string arl1 = arl;
                        string arpl = arr[1];
                        if (MappingsLiveries.ContainsKey(arl + "-" + arr[1]))
                        {
                            string[] arr2 = ((string)MappingsLiveries[arl + "-" + arr[1]]).Split("-".ToCharArray());
                            arl1 = arr2[0];
                            arpl = arr2[1];
                        }
                        flight ff = new flight(arl1 + "," + arl1, flightnumber, getAirport(arr[2], true), arpl, "", "");
                        if (ff.arp == "") ff.arp = airport;
                        depflights.Add(tme + "." + line.ToString("D6"), ff);
                    }
                }
            }
            fr.Close();

            label4.Text += "\n==================================================\n\n";
            label4.Text += airlinesL.Count + " airlines " + airplanesL.Count + " airplanes " + flightsL + " flights (" + narl + " regular and " + nga + " GA)\n\n";
            parent.form2share = airlinesL.Count + " airlines " + airplanesL.Count + " airplanes " + nnn + " flights (" + narl + " regular and " + nga + " GA)";
            if (airportError != "") label4.Text += "\n";
            label4.Text += airportError + "\n\n";
            label4.Text += "Missing airlines/GA: " + airlinesLB.Count + " affecting " + linesarl + " flights\n";
            label4.Text += "Missing airplanes:" + airplanesLB.Count + " affecting " + linesarp + " flights\n";
            label4.Text += "Missing liveries:" + liveries.Count + " affecting " + linesliv + " flights\n\n";

            label4.Text += "Valid remaining flights: " + nnn + "\n\n";
            if (airlinesLB.Count > 0)
            {
                ArrayList tmpk = new ArrayList(airlinesLB.Keys);
                tmpk.Sort();
                foreach (string ttt in tmpk)
                {
                    label4.Text += "Unknown airline or GA '" + ttt + "'\n";
                }
            }
            label4.Text += "\n";
            if (airplanesLB.Count> 0) 
            {
                ArrayList tmpk = new ArrayList(airplanesLB.Keys);
                tmpk.Sort();
                foreach (string ttt in tmpk)
                {
                    label4.Text += "Unknown airplane " + ttt + "\n";
                }
            }
            label4.Text += "\n";
            if (liveries.Count > 0)
            {
                ArrayList tmpk = new ArrayList(liveries.Keys);
                tmpk.Sort();
                foreach (string ttt in tmpk)
                {
                    string[] arr1 = ttt.Split(new char[] { '-' });
                    label4.Text += "No livery for airline '" + arr1[0] + "' and airplane '" + arr1[1] + "'\n";
                }
            }
            button5.Enabled= true;
            button6.Enabled = true;
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label5.Text = folderBrowserDialog1.FileName;
                button5.Enabled = false;
                button6.Enabled = false;
                label4.Text = "";
            }
        }

        private string[] ARPLsuggest(string ARPL)
        {
            string[] res = new string[2];
            res[0] = "";
            res[1] = "";
            ArrayList tmpk = new ArrayList(airplanes.Keys);
            tmpk.Sort();
            string errors = "";
            foreach (string ttt in tmpk)
            {
                if (ttt.Length >= 2 && ARPL.Length >= 2)
                {
                    if (ARPL.Substring(0, 2) == ttt.Substring(0, 2))
                    {
                        if (res[0] != "") res[0] += ",";
                        res[0] += ttt;
                    }
                }
                if (ttt.Length >= 3 && ARPL.Length >= 3)
                {
                    if (ARPL.Substring(0, 3) == ttt.Substring(0, 3))
                    {
                        if (res[1] != "") res[1] += ",";
                        res[1] += ttt;
                    }
                }
                if(ttt.Length<4)
                {
                    if (errors != "") errors += "\n";
                    errors += "'" + ttt + "'";
                }
            }
            if(errors != "")
            {
                MessageBox.Show("Strange plane ICAOs detected in database!\n" + errors, "WARNING", MessageBoxButtons.OK);
            }
            if(ARPL.Length<4)
            {
                MessageBox.Show("Strange plane ICAOs detected in input!\n'" + ARPL + "'", "WARNING", MessageBoxButtons.OK);
            }
            return res;
        }



        private void button5_Click(object sender, EventArgs e)
        {
            FileInfo ff = new FileInfo(label1.Text);
            string sgf = Path.Combine(ff.DirectoryName, "mappings.txt");
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            folderBrowserDialog1.InitialDirectory = ff.DirectoryName;
            folderBrowserDialog1.FileName = "mappings.txt";
            folderBrowserDialog1.CheckFileExists = false;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                sgf = folderBrowserDialog1.FileName;
            }
            else
            {
                return;
            }
            StreamWriter fw = new StreamWriter(sgf);
            fw.WriteLine("# Any line starting with '#' is a comment and ignored");
            fw.WriteLine("# lines starting with == are section headers and must stay in order");
            fw.WriteLine("# If you don't want to map, just remove relevant lines (not section headers)");
            fw.WriteLine("# but you can't leave lines with 'newPlaneICAO' or 'newPlane' in file!");
            fw.WriteLine("==airlines==");
            fw.WriteLine("# All new airlines will be used in GA flights file");
            fw.WriteLine("# There will be no liveries for new airlines resulting in white planes");
            foreach (string txt in MappingsAirlines.Keys)
            {
                fw.WriteLine(txt + "," + MappingsAirlines[txt]);
            }
            if (airlinesLB.Count > 0)
            {
                ArrayList tmpk = new ArrayList(airlinesLB.Keys);
                tmpk.Sort();
                foreach (string ttt in tmpk)
                {
                    fw.WriteLine(ttt+",CALLSIGN");
                }
            }
            fw.WriteLine("==airplanes==");
            fw.WriteLine("# If you want ignore airplane use ---- and new ICAO");
            foreach (string txt in MappingsAirplanes.Keys)
            {
                fw.WriteLine(txt + "," + MappingsAirplanes[txt]);
            }
            if (airplanesLB.Count > 0)
            {
                ArrayList tmpk = new ArrayList(airplanesLB.Keys);
                tmpk.Sort();
                foreach (string ttt in tmpk)
                {
                    fw.WriteLine(ttt + ",newPlaneICAO");
                    string[] sug = ARPLsuggest(ttt);
                    if (sug[1] != "")
                    {
                        fw.WriteLine("#possible match: " + sug[1]);
                    }
                    else if (sug[0] != "")
                    {
                        fw.WriteLine("#possible match: " + sug[0]);
                    }
                    else
                    {
                        fw.WriteLine("#no suggestions");
                    }
                }
            }
            fw.WriteLine("==liveries==");
            foreach (string txt in MappingsLiveries.Keys)
            {
                fw.WriteLine(txt + "," + MappingsLiveries[txt]);
            }
            if (liveries.Count > 0)
            {
                ArrayList tmpk = new ArrayList(liveries.Keys);
                tmpk.Sort();
                foreach (string ttt in tmpk)
                {
                    string[] arr1 = ttt.Split(new char[] { '-' });
                    fw.WriteLine(ttt + "," + arr1[0]+ "-newPLANE");
                    fw.WriteLine("#" + arr1[0] + " liveries: " + colors[arr1[0]]);
                }
            }
            fw.Close();
            label4.Text = "File '" + sgf + "' generated";
            button5.Enabled = false;
            button6.Enabled = false;
            label5.Text = "(optional)";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            StreamReader fr = new StreamReader(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "schedule.csv"));
            string header = fr.ReadLine();
            fr.Close();
            StreamWriter fw;
            try
            {
                fw = new StreamWriter(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "schedule.csv"));
            }
            catch
            {
                MessageBox.Show("Cannot open schedule.csv for writing!\nWriting to schedule_new.csv instead.\nYou will need to copy the file manually to schedule.csv", "WARNING");
                fw = new StreamWriter(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "schedule_new.csv"));
            }
            fw.WriteLine(header);
            ArrayList tmpk = new ArrayList(arrflights.Keys);
            tmpk.Sort();
            foreach (var kvp in tmpk)
            {
                flight ff = (flight)arrflights[kvp];
                fw.WriteLine(ff.info + "," + ff.fid + "," + ff.aplane + "," + ff.arp + "," + airport + "," + ((string)kvp).Substring(0, 5) + ",,," + ff.ftype);
            }
            tmpk = new ArrayList(depflights.Keys);
            tmpk.Sort();
            foreach (var kvp in tmpk)
            {
                flight ff = (flight)depflights[kvp];
                fw.WriteLine(ff.info + "," + ff.fid + "," + ff.aplane + "," + airport + "," + ff.arp + ",," + ((string)kvp).Substring(0, 5) + ",," + ff.ftype);
            }
            fw.Close();
            fr = new StreamReader(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "ga.csv"));
            header = fr.ReadLine();
            fr.Close();
            try
            {
                fw = new StreamWriter(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "ga.csv"));
            }
            catch
            {
                MessageBox.Show("Cannot open ga.csv for writing!\nWriting to ga_new.csv instead.\nYou will need to copy the file manually to ga.csv", "WARNING");
                fw = new StreamWriter(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "ga_new.csv"));
            }
            fw.WriteLine(header);
            tmpk = new ArrayList(arrflightsGA.Keys);
            tmpk.Sort();
            foreach (var kvp in tmpk)
            {
                flight ff = (flight)arrflightsGA[kvp];
                fw.WriteLine(ff.info + "," + ff.ftype + "," + ff.aplane + "," + ff.arp + "," + airport + "," + ((string)kvp).Substring(0, 5) + ",,,0,0,0,Default,");
            }
            tmpk = new ArrayList(depflightsGA.Keys);
            tmpk.Sort();
            foreach (var kvp in tmpk)
            {
                flight ff = (flight)depflightsGA[kvp];
                fw.WriteLine(ff.info + "," + ff.ftype + "," + ff.aplane + "," + airport + "," + ff.arp + ",," + ((string)kvp).Substring(0, 5) + ",,0,0,0,Default,");
            }
            fw.Close();
            if(airportsNEW.Count>0 && airportupdateoption.SelectedIndex == 1)
            {
                string backup = "airports.csv." + DateTime.Now.ToString("yyyyMMddHHmmss");
                File.Copy(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "airports.csv"),
                    Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, backup));
                fw = new StreamWriter(Path.Combine(T3location, "Airports", (string)airport, "databases", (string)database, "airports.csv"), true);
                foreach(string app in airportsNEW.Keys)
                {
                    string[] arr = (string[])airportsNEW[app];
                    for(int i=0; i<7; i++)
                    {
                        if (i > 0) fw.Write(",");
                        fw.Write("\"" + arr[i] + "\"");
                    }
                    fw.WriteLine();
                }
                fw.Close();
            }
            if(airportsSSSnew.Count>0)
            {
                fw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "airports-iata-icao.csv"), true);
                foreach (string app in airportsSSSnew.Keys)
                {
                    string[] arr = (string[])airportsSSSnew[app];
                    for (int i = 0; i < 7; i++)
                    {
                        if (i > 0) fw.Write(",");
                        fw.Write("\"" + arr[i] + "\"");
                    }
                    fw.WriteLine();
                }
                fw.Close();
                airportsSSSnew = new Hashtable();
            }
            label4.Text = "Schedule generated\n\n";
            button5.Enabled = false;
            button6.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void missliveries_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = "";
            button5.Enabled = false;
            button6.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label4.Text = "";
            button5.Enabled = false;
            button6.Enabled = false;
        }

        private void airplanedb_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = "";
        }
    }
}
