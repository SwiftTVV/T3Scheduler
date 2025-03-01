using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Net.Sockets;

namespace T3Scheduler
{
    public partial class Form1 : Form
    {
        string disclaimer = "This program is released 'as-is' without any warranty, responsibility, or liability. Use at your own risk!";

        public static string VERSION = "0.97.5.7";

        public Hashtable airports;
        public Hashtable airportsR;
        public Hashtable airportsC;
        public Hashtable airplanes;
        public Hashtable flights;
        public Hashtable flightsR;
        public Hashtable flightsC;
        public int nflights = 0;
        public int nflightsC = 0;
        public int nflightsR = 0;
        public Hashtable airportsga;
        public Hashtable airplanesga;
        public Hashtable airplanesgaAll;
        public Hashtable flightsga;
        public int nflightsga = 0;
        string fnerror = "";

        public Hashtable airlineFnumbers;
        public Hashtable airlineFnumberR;
        public Hashtable airlines;
        public Hashtable airlinesR;
        public Hashtable airlinesC;
        public Hashtable airlinesGA;

        public int[] flightsprofile = null; 
        public int[] gaflightsprofile = null;
        public int[] Cflightsprofile = null;
        public int[] Rflightsprofile = null;
        public int[] arrdepprofile = null; 

        public string form2share = "";

        public bool form4applied = false;
        Color button10DefColor;
        public bool form5applied = false;
        Color button11DefColor;

        public string rootdatabase = "default";

        string header = "";
        string headerga = "";
        public string headerT = "";
        int maxperhr = 0;
        int maxperhrga = 0;
        string nums = "0123456789";
        string[] numw = { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINER" };
        string letters = "abcdefghijklmnopqrstuvwxyz";
        string lettersnums = "";
        string[] natostr = { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliett", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa", "Quebec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-ray", "Yankee", "Zulu", "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINER" };

        StreamWriter fwdebug;
        public Form1()
        {
            InitializeComponent();
            this.Text = "T3Scheduler " + VERSION;
            configFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            configFile = Path.Combine(configFile, "T3Helper");
            if (!Directory.Exists(configFile)) Directory.CreateDirectory(configFile);
            string discFile = Path.Combine(configFile, "Sch.txt");
            if (!File.Exists(discFile))
            {
                if (MessageBox.Show(disclaimer, "Disclaimer", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) { this.Close(); }
                File.Create(discFile).Close();
            }
            configFile = Path.Combine(configFile, "config.txt");
            label6.Text = "";
            label6.ForeColor = Color.Red;
            label3.Text = "";
            label11.Text = "";
            airportdbs = new Hashtable();
            if (File.Exists(configFile))
            {
                StreamReader ff = new StreamReader(configFile);
                T3location.Text = ff.ReadLine();
                ff.Close();
                if (!File.Exists(Path.Combine(T3location.Text, "Airports", "airports.csv")))
                {
                    T3location.Text = "Select Tower Simulator 3 location";
                }
                else
                {
                    initialize_cntr();
                }
            }
            else
            {
                label6.Text = "Select Tower Simulator 3 location";
                T3location.Text = "";
            }
            panel1.Visible = false;
            arrdepratio.SelectedIndex = 4;
            tstart.SelectedIndex = 12;
            letters = letters.ToUpper();
            lettersnums = letters + nums;
            comboBox1.SelectedIndex = 0;
            button9.Visible= false;
            nhr.LostFocus += nhr_DeFocus;
            cargoperhr.LostFocus += cargoperhr_DeFocus;
            regionalperhr.LostFocus += regionalperhr_DeFocus;
            rndperhr.LostFocus += rndperhr_DeFocus;
            comboBox2.SelectedIndex = 0;
            if ((string)database.SelectedItem == "default")
                button7.Enabled = false;
            else
                button7.Enabled = true;
            cargoperhr.Enabled = false; 
            regionalperhr.Enabled = false;
            button10DefColor = SystemColors.Control;
            button11DefColor = SystemColors.Control; 
        }

        public ArrayList TS3AirplaneData;
        public ArrayList T3AirlinesData;
        public ArrayList T3TerminalsData;

        private void readAirlinesAndAirplanes()
        {
            TS3AirplaneData = new ArrayList();
            T3AirlinesData = new ArrayList();
            Hashtable T3AirlinesDataTMP = new Hashtable();
            string[] airpls = Directory.GetDirectories(Path.Combine(T3location.Text, "Airplanes", rootdatabase));
            foreach (string airpl in airpls)
            {
                AirplaneData AirplaneDataItem = new AirplaneData();
                string apname = new DirectoryInfo(airpl).Name;
                StreamReader ff = new StreamReader(Path.Combine(airpl,apname + ".csv"));
                string txt = "";
                while((txt = ff.ReadLine()) != null)
                {
                    string[] arr = txt.Split(',');
                    if (arr[0] == "icao") AirplaneDataItem.ICAO = arr[1];
                    if (arr[0] == "name") AirplaneDataItem.name = arr[1];
                    if (arr[0] == "weight class") AirplaneDataItem.wclass = arr[1];
                    if (arr[0] == "approach speed class") AirplaneDataItem.sclass = arr[1];
                    if (arr[0] == "category") AirplaneDataItem.atype = arr[1];
                }
                ff.Close();
                txt = "";
                string[] airlnss = Directory.GetDirectories(airpl);
                foreach (string airln in airlnss)
                {
                    string arlname = new DirectoryInfo(airln).Name;
                    string arlicao = arlname;
                    //if (arlname.Contains("_")) arlicao = arlname.Split('_')[0];
                    if (txt != "") txt += "|";
                    txt += arlname;
                    if(T3AirlinesDataTMP.ContainsKey(arlname))
                    {
                        AirlineData AirlineDataItem = (AirlineData)T3AirlinesDataTMP[arlname];
                        AirlineDataItem.airplanesStr += "|" + apname;
                        T3AirlinesDataTMP[arlname] = AirlineDataItem;
                    }
                    else
                    {
                        AirlineData AirlineDataItem = new AirlineData();
                        AirlineDataItem.airplanesStr = apname;
                        AirlineDataItem.ICAO = arlicao;
                        T3AirlinesDataTMP[arlname] = AirlineDataItem;
                    }
                }
                AirplaneDataItem.airlines = txt.Split('|');
                TS3AirplaneData.Add(AirplaneDataItem);
            }
            ArrayList aaa   = new ArrayList(T3AirlinesDataTMP.Keys);
            foreach(string arlname in aaa)
            {
                AirlineData AirlineDataItem = (AirlineData)T3AirlinesDataTMP[arlname];
                AirlineDataItem.airplanes = AirlineDataItem.airplanesStr.Split('|');
                T3AirlinesData.Add(AirlineDataItem);
            }
            T3TerminalsData = new ArrayList();
            StreamReader ff1 = new StreamReader(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "terminals.csv"));
            headerT = ff1.ReadLine();
            string txt1 = "";
            while ((txt1 = ff1.ReadLine()) != null)
            {
                string [] arr = Form2.splitCSV(txt1);
                TerminalData ttt = new TerminalData();
                ttt.name = arr[0];
                ttt.airlines = arr[1];
                ttt.gates = arr[2];
                T3TerminalsData.Add(ttt);
            }
            ff1.Close();
        }

        private void Rndperhr_LostFocus(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private string configFile = "";
        Hashtable airportdbs;

        private void initialize_cntr()
        {
            string AirportsDir = Path.Combine(T3location.Text, "Airports");
            string[] arpr = Directory.GetDirectories(AirportsDir);
            if(arpr.Length == 0) 
            {
                string txt = "I cannot find any airports in\n";
                txt += T3location.Text + "\\Airports\\\n\n";
                txt += "Maybe you moved the game or deleted parts of it? Please select proper Tower Simulator 3 location or repair the game.";
                MessageBox.Show(txt);
                return;
            }
            foreach (string apt in arpr)
            {
                airport.Items.Add(new DirectoryInfo(apt).Name);
                string[] dbs = Directory.GetDirectories(Path.Combine(AirportsDir, apt, "databases"));
                if(dbs.Length == 0)
                {
                    string txt = "I cannot find any databases in \n";
                    txt += T3location.Text + "\\Airports\\databases\\"+ new DirectoryInfo(apt).Name + "\n\n";
                    txt += "Looks like your airport "+ new DirectoryInfo(apt).Name + " is incomplete or corrupted. Please repair the game or remove airport.";
                    MessageBox.Show(txt);
                    return;
                }
                airportdbs.Add(new DirectoryInfo(apt).Name, dbs);
            }

            string AirplanesDir = Path.Combine(T3location.Text, "Airplanes");
            string[] airps = Directory.GetDirectories(AirplanesDir);
            foreach (string airp in airps)
            {
                adatabase.Items.Add(new DirectoryInfo(airp).Name);
            }
            adatabase.SelectedIndex = -1;
            airport.SelectedIndex = 0;
        }

        private void airport_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            database.Items.Clear();
            foreach (string item in (string[])airportdbs[(string)airport.SelectedItem])
            {
                database.Items.Add(new DirectoryInfo(item).Name);
            }
            panel1.Visible = false;
            label3.Text = "";
            label11.Text = "";
            database.SelectedIndex = 0;
        }

        private void database_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (database.SelectedIndex == -1) return;
            
            adatabase.SelectedIndex = -1;
            if (T3AirlinesData == null) adatabase_SelectedIndexChanged(sender, e);
            if (T3AirlinesData == null) return;
            try
            {
                StreamWriter fw = new StreamWriter(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "T3Scheduler2.txt"));
                fw.WriteLine("===AIRLINES");
                foreach (AirlineData aline in T3AirlinesData) fw.WriteLine(aline.ICAO + " " + aline.airplanesStr);
                fw.WriteLine("===AIRPLANES");
                foreach (AirplaneData aplane in TS3AirplaneData) fw.WriteLine(aplane.ICAO + " " + String.Join("|", aplane.airlines));
                fw.Close();
            }
            catch
            {
                string tmptxt = "I don't have permision to write to Tower Simulator 3 database directories!\n\n";
                tmptxt += "Usually it happens when Tower Simulator 3 is installed in a system folder like 'Program Files'.\n\n";
                tmptxt += "The solution is to run T3Scheduler as administrator, for example you can right-click on it and select 'Run as administrator'\n\n";
                tmptxt += "You can also right-click on it, select 'Properties', then 'Compatibility' and then check checkbox 'Run this program as administrator'.";
                MessageBox.Show(tmptxt, "ERROR", MessageBoxButtons.OK);
                return;
            }
        }

        private void updateAirports(ref Hashtable aprts, string arp)
        {
            if (aprts.ContainsKey(arp))
            {
                int i = (int)(aprts[arp]) + 1;
                aprts[arp] = i;
            }
            else
            {
                aprts[arp] = 1;
            }
        }

        private void updateFlights(ref Hashtable flts, string arp, string [] arr)
        {
            if (flts.ContainsKey(arp))
            {
                Hashtable hh = (Hashtable)flts[arp];
                string kk = arr[0] + "," + arr[1] + "," + arr[9] + "," + arr[10];
                if (hh.ContainsKey(kk))
                {
                    int i = (int)hh[kk];
                    hh[kk] = i + 1;
                    flts[arp] = hh;
                }
                else
                {
                    hh[kk] = 1;
                    flts[arp] = hh;
                }
            }
            else
            {
                Hashtable hh = new Hashtable();
                string kk = arr[0] + "," + arr[1] + "," + arr[9]+ "," + arr[10];
                hh[kk] = 1;
                flts[arp] = hh;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadScheduleStat(true, true, "T3Scheduler.txt");
        }

        private void ReadScheduleStat(bool original, bool init, string dfile)
        {

            button10.BackColor= SystemColors.Control;
            button11.BackColor= SystemColors.Control;
            maxperhr = 0;
            maxperhrga = 0;
            label11.Text = "";

            airports = new Hashtable();
            airportsR = new Hashtable();
            airportsC = new Hashtable();
            airplanes = new Hashtable();
            flights = new Hashtable();
            flightsR = new Hashtable();
            flightsC = new Hashtable();
            Hashtable airplanetypes = new Hashtable();
            airlineFnumbers= new Hashtable();
            airlineFnumberR =  new Hashtable();
            airlines = new Hashtable();
            airlinesR = new Hashtable();
            airlinesC = new Hashtable();
            airlinesGA = new Hashtable();

            string basepath = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem);

            if (init)
            {
                if (!Directory.Exists(Path.Combine(basepath, "backup")))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(basepath, "backup"));
                        File.Copy(Path.Combine(basepath, "schedule.csv"), Path.Combine(basepath, "backup", "schedule.csv"));
                        File.Copy(Path.Combine(basepath, "ga.csv"), Path.Combine(basepath, "backup", "ga.csv"));
                        File.Copy(Path.Combine(basepath, "terminals.csv"), Path.Combine(basepath, "backup", "terminals.csv"));
                    }
                    catch
                    {
                        string tmptxt = "I don't have permision to write to Tower Simulator 3 database directories!\n\n";
                        tmptxt += "Usually it happens when Tower Simulator 3 is installed in a system folder like 'Program Files'.\n\n";
                        tmptxt += "The solution is to run T3Scheduler as administrator, for example you can right-click on it and select 'Run as administrator'\n\n";
                        tmptxt += "You can also right-click on it, select 'Properties', then 'Compatibility' and then check checkbox 'Run this program as administrator'.";
                        MessageBox.Show(tmptxt, "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                }
                else
                {
                    if (!File.Exists(Path.Combine(basepath, "schedule.csv")))
                    {
                        MessageBox.Show("schedule.csv is missing in '" + basepath + "'", "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                    if (!File.Exists(Path.Combine(basepath, "ga.csv")))
                    {
                        MessageBox.Show("ga.csv is missing in '" + basepath + "'", "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                    if (!File.Exists(Path.Combine(basepath, "terminals.csv")))
                    {
                        MessageBox.Show("terminals.csv is missing in '" + basepath + "'", "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                    try
                    {
                        if (!File.Exists(Path.Combine(basepath, "backup", "schedule.csv")))
                            File.Copy(Path.Combine(basepath, "schedule.csv"), Path.Combine(basepath, "backup", "schedule.csv"));
                        if (!File.Exists(Path.Combine(basepath, "backup", "ga.csv")))
                            File.Copy(Path.Combine(basepath, "ga.csv"), Path.Combine(basepath, "backup", "ga.csv"));
                        if (!File.Exists(Path.Combine(basepath, "backup", "terminals.csv")))
                            File.Copy(Path.Combine(basepath, "terminals.csv"), Path.Combine(basepath, "backup", "terminals.csv"));
                    }
                    catch
                    {
                        string tmptxt = "I don't have permission to write to Tower Simulator 3 database directories! [2]\n\n";
                        tmptxt += "Path: '" + basepath + "'\n\n";
                        tmptxt += "Usually it happens when Tower Simulator 3 is installed in a system folder like 'Program Files'.\n\n";
                        tmptxt += "The solution is to run T3Scheduler as administrator, for example you can right-click on it and select 'Run as administrator'\n\n";
                        tmptxt += "You can also right-click on it, select 'Properties', then 'Compatibility' and then check checkbox 'Run this program as administrator'.";
                        MessageBox.Show(tmptxt, "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                }
            }
            string basefile = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "backup");
            if (!original) basefile = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem);
            Hashtable allairports = new Hashtable();
            StreamReader ff = new StreamReader(Path.Combine(basefile, "schedule.csv"));
            header = ff.ReadLine();
            int[] arrivals = new int[24];
            int[] departures = new int[24];
            int[] gaarrivals = new int[24];
            int[] gadepartures = new int[24];
            int[] cargos= new int[24];
            int[] regionals = new int[24];
            int[] heavies= new int[24];
            string[] xlabels = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            for (int i = 0; i < 24; i++)
            {
                arrivals[i] = 0;
                departures[i] = 0;
                gaarrivals[i] = 0;
                gadepartures[i] = 0;
                regionals[i] = 0;
                cargos[i] = 0;
                heavies[i] = 0;
            }
            string txt;
            int lineF = 1;
            while ((txt = ff.ReadLine()) != null)
            {
                lineF++; 
                string[] arr = Form2.splitCSV(txt);
                for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Trim();
                bool dep = false;
                string arp = arr[4];
                if (arr[6] == "")
                {
                    dep = true;
                    arp = arr[5];
                }
                Array.Resize(ref arr, 11);
                if (arr[9].Contains(","))
                {
                    arr[10] = arr[9].Split(',')[1].Trim();
                    arr[9] = arr[9].Split(',')[0].Trim();
                }
                else
                {
                    arr[10] = "";
                }
                if (arr[9] != "regional" && arr[9] != "cargo") updateAirports(ref airports, arp);
                if (arr[9] == "regional") updateAirports(ref airportsR, arp);
                if (arr[9] == "cargo") updateAirports(ref airportsC, arp);
                allairports[arp] = "1";
                airplanetypes[arr[3]] = "1";
                string extra = "";
                //if(arr[9] != "regional" && arr[9] != "cargo" && arr[9] != "") extra = arr[9];
                extra = arr[9] + "-" + arr[10];
                if (airplanes.ContainsKey(arp + "-" + arr[0] + "-" + arr[1] + "-" + extra))
                {
                    string s = (string)airplanes[arp + "-" + arr[0] + "-" + arr[1] + "-" + extra];
                    if (!s.Split(",".ToCharArray()).Contains(arr[3]))
                    {
                        airplanes[arp + "-" + arr[0] + "-" + arr[1] + "-" + extra] = s + "," + arr[3];
                    }
                }
                else
                {
                    airplanes[arp + "-" + arr[0] + "-" + arr[1] + "-" + extra] = arr[3];
                }
                if (arr[9] != "regional" && arr[9] != "cargo") updateFlights(ref flights, arp, arr);
                if (arr[9] == "regional") updateFlights(ref flightsR, arp, arr);
                if (arr[9] == "cargo") updateFlights(ref flightsC, arp, arr);
                if(dep)
                {
                    int ii = 0;
                    try
                    {
                        ii = int.Parse(arr[7].Split(":".ToArray())[0]);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid time in line " + lineF + " of backup\\schedule.csv\n" + txt, "schedule.csv ERROR");
                        ff.Close();
                        return;
                    }
                    departures[ii]++;
                    if (arr[9] == "regional") regionals[ii]++;
                    if (arr[9] == "cargo") cargos[ii]++;
                }
                else
                {
                    int ii = 0;
                    try
                    {
                        ii = int.Parse(arr[6].Split(":".ToArray())[0]);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid time in line " + lineF + " of backup\\schedule.csv\n" + txt, "schedule.csv ERROR");
                        ff.Close();
                        return;
                    }
                    arrivals[ii]++;
                    if (arr[9] == "regional") regionals[ii]++;
                    if (arr[9] == "cargo") cargos[ii]++;
                }
                if (arr[9] == "regional")
                {
                    airlinesR[arr[0]] = "1";
                }
                else if (arr[9] == "cargo")
                {
                    airlinesC[arr[0]] = "1";
                }
                else
                {
                    airlines[arr[0]] = "1";
                }
            }
            ff.Close();

            airportsga = new Hashtable();
            airplanesga = new Hashtable();
            airplanesgaAll = new Hashtable();
            flightsga = new Hashtable();
            ff = new StreamReader(Path.Combine(basefile, "ga.csv"));
            headerga = ff.ReadLine();
            lineF = 1;
            while ((txt = ff.ReadLine()) != null)
            {
                lineF++;
                string[] arr = Form2.splitCSV(txt);
                for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Trim();
                bool dep = false;
                string arp = arr[3];
                if (arr[5] == "")
                {
                    dep = true;
                    arp = arr[4];
                }
                if (airportsga.ContainsKey(arp))
                {
                    int i = (int)(airportsga[arp]) + 1;
                    airportsga[arp] = i;
                }
                else
                {
                    airportsga[arp] = 1;
                }
                allairports[arp] = "1";
                airplanetypes[arr[2]] = "1";
                airplanesgaAll[arr[2]] = "1";
                if (airplanesga.ContainsKey(arp))
                {
                    string s = (string)airplanesga[arp];
                    if (!s.Split(",".ToCharArray()).Contains(arr[2]))
                    {
                        airplanesga[arp] = s + "," + arr[2];
                    }
                }
                else
                {
                    airplanesga[arp] = arr[2];
                }
                //- present => substr(0,indeoxof('-'))
                //3 letters then number => substr(0,3)
                //else => first letter, then numbers
                string nme = "";
                if (arr[0].Contains("-"))
                {
                    nme = arr[0].Split('-')[0] + "-";
                }
                else
                {
                    for (int i = 0; i < arr[0].Length; i++)
                    {
                        if (nums.Contains(arr[0][i])) break;
                        nme += arr[0][i];
                    }
                }
                string nmefull = "";
                if (nme.Length == 3 && !nme.Contains("-"))
                {
                    string[] tmpstr = arr[1].Split(" ".ToCharArray());
                    for (int i = 0; i < tmpstr.Length; i++)
                    {
                        if (numw.Contains(tmpstr[i])) break;
                        if (nmefull != "") nmefull += " ";
                        nmefull += tmpstr[i];
                    }
                }
                airlinesGA[nme] = "1";
                if (flightsga.ContainsKey(arp))
                {
                    Hashtable hh = (Hashtable)flightsga[arp];                    
                    string kk = nme + "," + nmefull;
                    if (hh.ContainsKey(kk))
                    {
                        int i = (int)hh[kk];
                        hh[kk] = i + 1;
                        flightsga[arp] = hh;
                    }
                    else
                    {
                        hh[kk] = 1;
                        flightsga[arp] = hh;
                    }
                }
                else
                {
                    Hashtable hh = new Hashtable();
                    string kk = nme + "," + nmefull; 
                    hh[kk] = 1;
                    flightsga[arp] = hh;
                }
                if (dep)
                {
                    int ii = 0;
                    try
                    {
                        ii = int.Parse(arr[6].Split(":".ToArray())[0]);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid time in line " + lineF + " of backup\\ga.csv\n" + txt, "ga.csv ERROR");
                        ff.Close();
                        return;
                    }
                    gadepartures[ii]++;
                }
                else
                {
                    int ii = 0;
                    try
                    {
                        ii = int.Parse(arr[5].Split(":".ToArray())[0]);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid time in line " + lineF + " of backup\\ga.csv\n" + txt, "ga.csv ERROR");
                        ff.Close();
                        return;
                    }
                    gaarrivals[ii]++;
                }
            }
            ff.Close();

            DumpStatInfo(dfile);

            label3.Text = allairports.Count + " airports, " + airplanetypes.Keys.Count + " airplane types, " + (nflights+nflightsga+nflightsC+nflightsR).ToString() + " total flights ";
            label3.Text += "[" + (nflights).ToString() + " regular, " + nflightsR + " regional, " + nflightsC + " cargo, " + nflightsga + " GA]";
            chart1.Series["dep"].Points.Clear();
            chart1.Series["arr"].Points.Clear();
            chart1.Series["ga dep"].Points.Clear();
            chart1.Series["ga arr"].Points.Clear();
            int maxcargoes = 0;
            int maxregionals = 0;
            for (int i=0; i<24; i++)
            {
                if (maxperhr < departures[i] + arrivals[i]) maxperhr = departures[i] + arrivals[i];
                if (maxperhrga < gadepartures[i] + gaarrivals[i]) maxperhrga = gadepartures[i] + gaarrivals[i];
                if (maxcargoes < cargos[i]) maxcargoes = cargos[i];
                if (maxregionals < regionals[i]) maxregionals = regionals[i];
                chart1.Series["dep"].Points.AddXY(xlabels[i], departures[i]);
                chart1.Series["arr"].Points.AddXY(xlabels[i], arrivals[i]);
                chart1.Series["ga dep"].Points.AddXY(xlabels[i], gadepartures[i]);
                chart1.Series["ga arr"].Points.AddXY(xlabels[i], gaarrivals[i]);
            }
            chart1.ChartAreas[0].AxisX.Interval = 2;
            chart1.Titles.Clear();
            chart1.Titles.Add("Original Schedule - " + (string)airport.SelectedItem + " - " + (string)database.SelectedItem + " [T3Scheduler " + VERSION + "]");
            flightsperhr.Text = maxperhr.ToString();
            gaflightsperhr.Text = maxperhrga.ToString();
            if (maxperhr == 0) flightsperhr.Enabled = false; else flightsperhr.Enabled = true;
            if (maxperhrga == 0) gaflightsperhr.Enabled = false; else gaflightsperhr.Enabled = true;
            cargoperhr.Text = (100.0*(float)maxcargoes/maxperhr).ToString("N2");
            if(maxcargoes == 0) cargoperhr.Enabled = false; else cargoperhr.Enabled = true; 
            regionalperhr.Text = (100.0*(float)maxregionals/maxperhr).ToString("N2");
            if(maxregionals== 0) regionalperhr.Enabled = false; else regionalperhr.Enabled = true;
            panel1.Visible = true;
            initFLightNumberRanges();
        }

        private int dumpOrderedHash(Hashtable hh, StreamWriter fw, bool val, bool isInt, bool probabilty)
        {
            int k = 0;
            int total = 0;
            if(isInt && probabilty)
            {
                foreach(string kk in hh.Keys)
                {
                    total += (int)hh[kk];
                }
            }
            ArrayList tmpk = new ArrayList(hh.Keys);
            tmpk.Sort();
            for (int i = 0; i < tmpk.Count; i++)
            {
                string kk = (string)tmpk[i];
                if (val)
                {
                    if (isInt)
                    {
                        if (probabilty)
                        {
                            fw.WriteLine(kk + " " + String.Format("{0,4:0} {1,6:0.000}%", (int)hh[kk], (100*(double)((int)hh[kk])) / total));
                        }
                        else
                        {
                            fw.WriteLine(kk + " " + ((int)hh[kk]).ToString());
                        }
                    }
                    else
                    {
                        fw.WriteLine(kk + " " + (string)hh[kk]);
                    }
                }
                else
                {
                    fw.WriteLine(kk);
                }
                k++;
            }
            return k;
        }

        private void dumpFLights(Hashtable ttt, StreamWriter fw, ref int nflightsL)
        {
            ArrayList tmpk = new ArrayList(ttt.Keys);
            tmpk.Sort();
            for (int i = 0; i < tmpk.Count; i++)
            {
                string kk = (string)tmpk[i];
                Hashtable hh = (Hashtable)ttt[kk];
                fw.WriteLine("- " + kk);
                ArrayList tmpk1 = new ArrayList(hh.Keys);
                tmpk1.Sort();
                for (int ii = 0; ii < tmpk1.Count; ii++)
                {
                    string kkk = (string)tmpk1[ii];
                    fw.WriteLine(kkk + " " + hh[kkk]);
                    nflightsL += (int)hh[kkk];
                }
            }
        }

        private int GetAllFlightNum(Hashtable ttt)
        {
            int res = 0;
            foreach (string kk in ttt.Keys)
            {
                Hashtable hh = (Hashtable)ttt[kk];
                foreach (string kkk in hh.Keys)
                {
                    res += (int)hh[kkk];
                }
            }
            return res;
        }

        private int[] GetAirlineFlightNum(string arl, Hashtable ttt)
        {
            int[] res = new int[2] { 0, 0 };
            foreach(string kk in ttt.Keys)
            {
                Hashtable hh = (Hashtable)ttt[kk];
                foreach(string kkk in hh.Keys)
                {
                    if (kkk.StartsWith(arl + ",")) res[0] += (int)hh[kkk];
                    res[1] += (int)hh[kkk];
                }
            }
            return res;
        }

        private void dumpAirlines(Hashtable hh, StreamWriter fw, Hashtable ff, int total)
        {
            ArrayList tmpk = new ArrayList(hh.Keys);
            tmpk.Sort();
            for (int i = 0; i < tmpk.Count; i++)
            {
                string kk = (string)tmpk[i];
                int[] rrr = GetAirlineFlightNum(kk, ff);
                if (total > 0)
                {
                    fw.WriteLine(kk + " " + String.Format("{0,4:0} {1,6:0.000}% {1,6:0.000}%", rrr[0], (100 * (double)rrr[0]) / rrr[1], (100 * (double)rrr[0]) / total));
                }
                else
                {
                    fw.WriteLine(kk + " " + String.Format("{0,4:0} {1,6:0.000}%", rrr[0], (100 * (double)rrr[0]) / rrr[1]));
                }
            }
        }

        private bool DumpStatInfo(string fname)
        {
            nflights = 0;
            nflightsC = 0;
            nflightsR = 0;
            nflightsga = 0;

            int total1 = GetAllFlightNum(flights);
            total1 += GetAllFlightNum(flightsR);
            total1 += GetAllFlightNum(flightsC);

            StreamWriter fw;

            try
            {
                fw = new StreamWriter(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, fname));
            }
            catch
            {
                string tmptxt = "I don't have permision to write to Tower Simulator 3 database directories!\n\n";
                tmptxt += "Usually it happens when Tower Simulator 3 is installed in a system folder like 'Program Files'.\n\n";
                tmptxt += "The solution is to run T3Scheduler as administrator, for example you can right-click on it and select 'Run as administrator'\n\n";
                tmptxt += "You can also right-click on it, select 'Properties', then 'Compatibility' and then check checkbox 'Run this program as administrator'.";
                MessageBox.Show(tmptxt, "ERROR", MessageBoxButtons.OK);
                return false;
            }
            fw.WriteLine("===airports N start===");
            dumpOrderedHash(airports, fw, true, true, true);
            fw.WriteLine("===airports N end===");
            fw.WriteLine("===airports C start===");
            dumpOrderedHash(airportsC, fw, true, true, true);
            fw.WriteLine("===airports C end===");
            fw.WriteLine("===airports R start===");
            dumpOrderedHash(airportsR, fw, true, true, true);
            fw.WriteLine("===airports R end===");
            fw.WriteLine("===airlines N start===");
            dumpAirlines(airlines, fw, flights, total1);
            fw.WriteLine("===airlines N end===");
            fw.WriteLine("===airlines R start===");
            dumpAirlines(airlinesR, fw, flightsR, total1);
            fw.WriteLine("===airlines R end===");
            fw.WriteLine("===airlines C start===");
            dumpAirlines(airlinesC, fw, flightsC, total1);
            fw.WriteLine("===airlines C end===");
            fw.WriteLine("===airplanes start===");
            dumpOrderedHash(airplanes, fw, true, false, false);
            fw.WriteLine("===airplanes end===");
            fw.WriteLine("===flights N start===");
            dumpFLights(flights, fw, ref nflights);
            fw.WriteLine("===flights N end===");
            fw.WriteLine("===flights C start===");
            dumpFLights(flightsC, fw, ref nflightsC);
            fw.WriteLine("===flights C end===");
            fw.WriteLine("===flights R start===");
            dumpFLights(flightsR, fw, ref nflightsR);
            fw.WriteLine("===flights R end===");
            fw.WriteLine("===GA airports start===");
            dumpOrderedHash(airportsga, fw, true, true, true);
            fw.WriteLine("===GA airports end===");
            fw.WriteLine("===GA airlines start===");
            dumpAirlines(airlinesGA, fw, flightsga, 0);
            fw.WriteLine("===GA airlines end===");
            fw.WriteLine("===GA airplanes start===");
            dumpOrderedHash(airplanesga, fw, true, false, false);
            fw.WriteLine("===GA airplanes end===");
            fw.WriteLine("===GA flights start===");
            dumpFLights(flightsga, fw, ref nflightsga);
            fw.WriteLine("===GA flights end===");
            fw.Close();

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(Path.Combine(folderBrowserDialog1.SelectedPath, "Airports", "airports.csv")))
                {
                    T3location.Text = "Select Tower Simulator 3 location";
                    MessageBox.Show("Cannot find file \"" + Path.Combine(folderBrowserDialog1.SelectedPath, "Airports", "airports.csv") + "\"\nPlease verify that Tower Simulator 3 location is correct");
                    return;
                }
                T3location.Text = folderBrowserDialog1.SelectedPath;
                StreamWriter ff = new StreamWriter(configFile);
                ff.WriteLine(T3location.Text);
                ff.Close();
                initialize_cntr();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string basepath = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem);
            File.Copy(Path.Combine(basepath, "backup", "schedule.csv"), Path.Combine(basepath, "schedule.csv"), true);
            File.Copy(Path.Combine(basepath, "backup", "ga.csv"), Path.Combine(basepath, "ga.csv"), true);
            label11.Text = "Original schedule restored!";
            button2_Click(sender, e);
        }

        class flight
        {
            public string info;
            public int fid;
            public string arp;
            public string aplane;
            public string ftype;
            public flight(string info, int fid, string arp, string aplane, string ftype)
            {
                this.info = info;
                this.fid = fid;
                this.arp = arp;
                this.aplane = aplane;
                this.ftype = ftype;
            }
        }

        private static int getAirpIndx(double[] alist, double vv)
        {
            for(int j=0; j<alist.Length; j++)
            {
                if (vv < alist[j]) return j;
            }
            return alist.Length - 1;
        }

        private string getNid(ref Random rnd, string prefix)
        {
            string Nid = "";
            if (prefix.EndsWith("-"))
            {

                Nid = prefix;
                int nn = 4;
                if (prefix.Length == 3) nn = 3;
                for (int i = 0; i < nn; i++)
                {
                    Nid += lettersnums[rnd.Next(0, lettersnums.Length)];
                }
            }
            else
            {
                int fffid = 0;
                if (rnd.NextDouble() > 0.5)
                {
                    fffid = rnd.Next(10, 100);
                }
                else
                {
                    fffid = rnd.Next(101, 1000);
                }
                Nid = prefix + fffid.ToString() + lettersnums[rnd.Next(0, letters.Length)];
                Nid += lettersnums[rnd.Next(0, letters.Length)];
            }
            return Nid;
        }

        private string getNATO(string str)
        {
            string nato = "";
            for(int i=0; i<str.Length; i++) 
            {
                if (str[i] == '-') continue;
                int j = lettersnums.IndexOf(str[i]);
                if (nato != "") nato += " ";
                if(j>-1)nato += natostr[j];
            }
            return nato.ToUpper();
        }

        private void initFLightNumberRanges()
        {
            airlineFnumberR = new Hashtable();
            if (airlineFnumbers.Count == 0)
            {
                int[] kk = new int[2];
                kk[0] = 1;
                kk[1] = 9999;
                ArrayList tmp1 = new ArrayList();
                tmp1.Add(kk);
                airlineFnumberR["---"] = tmp1;
                form4applied = false;
                return;
            }
            Hashtable rangesAll = new Hashtable();
            foreach (string arl in airlineFnumbers.Keys)
            {
                Hashtable tmp = new Hashtable();
                string[] aaa = ((string)airlineFnumbers[arl]).Split(',');
                foreach (string rng in aaa)
                {
                    string[] rrr = rng.Split('-');
                    int[] ii = new int[2];
                    ii[0] = int.Parse(rrr[0]);
                    ii[1] = int.Parse(rrr[1]);
                    tmp[ii[0]] = ii;
                    if (rangesAll.ContainsKey(ii[0]))
                    {
                        int[] jj = (int[])rangesAll[ii[0]];
                        if (jj[1] < ii[1]) rangesAll[ii[0]] = ii;
                    }
                    else
                    {
                        rangesAll[ii[0]] = ii;
                    }
                }
                ArrayList tmpk = new ArrayList(tmp.Keys);
                tmpk.Sort();
                ArrayList ranges = new ArrayList();
                for (int i = 0; i < tmpk.Count; i++)
                {
                    ranges.Add(tmp[tmpk[i]]);
                }
                airlineFnumberR[arl] = ranges;
            }
            ArrayList tmpkk = new ArrayList(rangesAll.Keys);
            tmpkk.Sort();
            int[] iiA = new int[2];
            iiA[0] = 0;
            iiA[1] = 0;
            ArrayList rangesA = new ArrayList();
            for (int i = 0; i < tmpkk.Count; i++)
            {
                int[] ll = (int[])rangesAll[tmpkk[i]];
                if (iiA[1] < ll[0])
                {
                    int[] iiB = new int[2];
                    iiB[0] = iiA[1] + 1;
                    iiB[1] = ll[0] - 1;
                    rangesA.Add(iiB);
                    iiA[0] = ll[0];
                    iiA[1] = ll[1];
                }
                else
                {
                    iiA[1] = ll[1];
                }
            }
            if (iiA[1]<9999)
            {
                int[] iiB = new int[2];
                iiB[0] = iiA[1] + 1;
                iiB[1] = 9999;
                rangesA.Add(iiB);
            }
            //airlineFnumberR["---"] = rangesA;
            //ranges can overlap for different airlines
            iiA[0] = 1;
            iiA[1] = 9999;
            ArrayList rangesA1 = new ArrayList();
            rangesA1.Add(iiA);
            airlineFnumberR["---"] = rangesA;
            label11.Text = "Unrestricted flight numbers: ";
            for (int i = 0; i < rangesA.Count; i++)
            {
                int[] ll = (int[])rangesA[i];
                if (label11.Text != "Unrestricted flight numbers: ") label11.Text += ",";
                label11.Text += ll[0] + "-" + ll[1];
            }
            form4applied = true;
        }

        private int generateFlightNumber(ref Hashtable fids, string airline, string oper, ref Random rnd)
        {
            ArrayList ranges;
            if (airlineFnumberR.ContainsKey(airline + "-" + oper))
            {
                ranges = (ArrayList)airlineFnumberR[airline + "-" + oper];
            }
            else
            {
                ranges = (ArrayList)airlineFnumberR["---"];
            }
            int NN = 0;
            for(int i=0; i<ranges.Count; i++)
            {
                int[] ii= (int[])ranges[i];
                NN += ii[1] - ii[0] + 1;
            }
            bool done = false;
            int fffid = 0;
            int iter = 0;
            while (!done)
            {
                iter++;
                if (iter < 1000)
                {
                    int kk = rnd.Next(0, NN);
                    for(int i=0; i<ranges.Count; i++)
                    {
                        int[] ii = (int[])ranges[i];
                        if (kk + ii[0] <= ii[1])
                        {
                            fffid = kk + ii[0];
                            break;
                        }
                        else
                        {
                            kk = kk - (ii[1] - ii[0] + 1);
                        }
                    }
                    if (!fids.ContainsKey(airline + "-" + fffid))
                    {
                        fids[airline + "-" + fffid] = "1";
                        done = true;
                    }
                }
                else
                {
                    if(iter == 1000)
                    {
                        if(!fnerror.Contains("\nProblem setting unique flight number(s) for " + airline))
                        {
                            fnerror += "\nProblem setting unique flight number(s) for " + airline;
                        }
                    }
                    if (rnd.NextDouble() > 0.5)
                    {
                        fffid = rnd.Next(101, 1000);
                    }
                    else
                    {
                        fffid = rnd.Next(1001, 10000);
                    }
                    if (!fids.ContainsKey(airline + "-" + fffid))
                    {
                        fids[airline + "-" +fffid] = "1";
                        done = true;
                    }
                    if(iter == 2000)
                    {
                        fnerror.Replace("\nProblem setting unique flight number(s) for " + airline, "\nCannot set unique flight number(s) for " + airline + " " + fffid);
                        done = true;
                    }
                }
            }
            return fffid;
        }

        private void createff(ref Hashtable mst, ref Random rnd, ref string[] airportsArr, ref double[] airportsArrP, ref Hashtable fids, ref Hashtable tmes, int hourindx, int tstartSelectedIndex, ref Hashtable fltsIn)
        {
            flight ff = new flight("", 0, "", "", "");
            double dd = rnd.NextDouble();
            int ijk = getAirpIndx(airportsArrP, dd);
            ff.arp = airportsArr[ijk];
            Hashtable flts = (Hashtable)fltsIn[ff.arp];
            ArrayList allflt = new ArrayList();
            int jjmax = 0;
            foreach (string sss in flts.Keys)
            {
                int k = (int)flts[sss];
                for (int iii = 0; iii < k; iii++) allflt.Add(sss);
                jjmax += k;
            }
            int jj = rnd.Next(0, jjmax);
            string[] sarr = ((string)allflt[jj]).Split(",".ToCharArray());
            string[] apls = ((string)airplanes[ff.arp + "-" + sarr[0] + "-" + sarr[1] + "-" + sarr[2] + "-" + sarr[3]]).Split(",".ToCharArray());
            jj = rnd.Next(0, apls.Length);
            ff.aplane = apls[jj];
            ff.info = sarr[0] + "," + sarr[1];
            ff.ftype = sarr[2];
            if (sarr[3] != "") ff.ftype = '"' + sarr[2] + ", " + sarr[3] + '"';
            bool done = false;
            ff.fid = generateFlightNumber(ref fids, sarr[0], sarr[1], ref rnd); ;
            string tme = "";
            done = false;
            int iter = 0;
            while (!done)
            {
                iter++;
                int minute = rnd.Next(0, 60000);
                int hour = tstartSelectedIndex + hourindx;
                tme = hour.ToString("D2") + ":" + minute.ToString("D5");
                if (!tmes.ContainsKey(tme))
                {
                    tmes[tme] = "1";
                    done = true;
                }
                if(iter == 1000)
                {
                    if (!fnerror.Contains("\nCannot set unique time " + sarr[0]))
                    {
                        fnerror += "\nCannot set unique time " + sarr[0];
                    }
                    done = true;
                }
            }
            mst[tme] = ff;
        }

        private void createffGA(ref Hashtable mst, ref Random rnd, ref string[] airportsArr, ref double[] airportsArrP, ref Hashtable fids, ref Hashtable tmes, int hourindx, int tstartSelectedIndex)
        {
            flight ff = new flight("", 0, "", "", "");
            double dd = rnd.NextDouble();
            ff.arp = airportsArr[getAirpIndx(airportsArrP, dd)];
            string[] apls = ((string)airplanesga[ff.arp]).Split(",".ToCharArray());
            int jj = rnd.Next(0, apls.Length);
            ff.aplane = apls[jj];
            Hashtable flts = (Hashtable)flightsga[ff.arp];
            ArrayList allflt = new ArrayList();
            int jjmax = 0;
            foreach (string sss in flts.Keys)
            {
                int k = (int)flts[sss];
                for (int iii = 0; iii < k; iii++) allflt.Add(sss);
                jjmax += k;
            }
            jj = rnd.Next(0, jjmax);
            string[] sarr = ((string)allflt[jj]).Split(",".ToCharArray());
            bool done = false;
            int fffid = 0;
            string fffidN = "";
            int iter = 0;
            while (!done)
            {
                iter++;
                if (sarr[1] == "")
                {
                    fffidN = getNid(ref rnd, sarr[0]);
                    if (!fids.ContainsKey(sarr[0] + "-" + fffidN))
                    {
                        fids[sarr[0] + "-" + fffid] = "1";
                        done = true;
                    }
                }
                else
                {
                    if (rnd.NextDouble() > 0.5)
                    {
                        fffid = rnd.Next(9, 100);
                    }
                    else
                    {
                        fffid = rnd.Next(100, 1000);
                    }
                    if (!fids.ContainsKey(sarr[0] + "-" + fffid))
                    {
                        fids[sarr[0] + "-" + fffid] = "1";
                        done = true;
                    }
                }
                if (iter == 2000)
                {
                    if (!fnerror.Contains("\nCannot set unique flight number(s) for GA " + sarr[0]))
                    {
                        fnerror += "\nCannot set unique flight number(s) for GA " + sarr[0];
                    }
                    done = true;
                }
            }
            ff.fid = fffid;
            if (sarr[1] == "")
            {
                ff.info = fffidN;
                ff.ftype = getNATO(ff.info);
            }
            else
            {
                ff.info = sarr[0] + fffid.ToString();
                ff.ftype = sarr[1] + " " + getNATO(fffid.ToString());
            }
            string tme = "";
            done = false;
            iter = 0;
            while (!done)
            {
                iter++; 
                int minute = rnd.Next(0, 60000);
                int hour = tstartSelectedIndex + hourindx;
                tme = hour.ToString("D2") + ":" + minute.ToString("D5");
                if (!tmes.ContainsKey(tme))
                {
                    tmes[tme] = "1";
                    done = true;
                }
                if (iter == 1000)
                {
                    if (!fnerror.Contains("\nCannot set unique GA time " + sarr[0]))
                    {
                        fnerror += "\nCannot set unique GA time " + sarr[0];
                    }
                    done = true;
                }
            }
            mst[tme] = ff;
        }

        private void SetAirportP(ref string[] airportsArr, ref double[] airportsArrP, ref Hashtable apts, int nf)
        {
            int i = 0;
            double rrlast = 0.0;
            //fwdebug.WriteLine("-------------");
            foreach (string aa in apts.Keys)
            {
                airportsArr[i] = aa;
                double rr = (double)((double)((int)apts[aa]) / nf);
                airportsArrP[i] = rrlast + rr;
                //fwdebug.WriteLine("= " + i + " " + aa + " " + rr.ToString() + " " + (rrlast + rr).ToString());
                rrlast += rr;
                i++;
            }
            //fwdebug.WriteLine("-------------");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hashtable deps = new Hashtable();
            Hashtable arrs = new Hashtable();
            Hashtable depsga = new Hashtable();
            Hashtable arrsga = new Hashtable();
            Hashtable fids = new Hashtable();
            Hashtable tmes = new Hashtable();

            //fwdebug = new StreamWriter(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "debug.txt"));

            string[] airportsArrN = new string[airports.Count];
            double[] airportsArrPN = new double[airports.Count];
            SetAirportP(ref airportsArrN, ref airportsArrPN, ref airports, nflights);

            string[] airportsArrC = new string[airportsC.Count];
            double[] airportsArrPC = new double[airportsC.Count];
            SetAirportP(ref airportsArrC, ref airportsArrPC, ref airportsC, nflightsC);

            string[] airportsArrR = new string[airportsR.Count];
            double[] airportsArrPR = new double[airportsR.Count];
            SetAirportP(ref airportsArrR, ref airportsArrPR, ref airportsR, nflightsR);

            string[] airportsArrGA = new string[airportsga.Count];
            double[] airportsArrPGA = new double[airportsga.Count];
            SetAirportP(ref airportsArrGA, ref airportsArrPGA, ref airportsga, nflightsga);

            //fwdebug.Close();

            Random rnd = new Random();

            int ndeps = 0;
            int ndepsR = 0;
            int ndepsC = 0;
            int narrs = 0;
            int narrsR = 0;
            int narrsC = 0;

            int ndepsga = 0;
            int narrsga = 0;

            fnerror = "";

            for (int hourindx = 0; hourindx < int.Parse(nhr.Text); hourindx++)
            {
                double rndperhrNum = 1.0 + (2 * rnd.NextDouble() - 1) * (double.Parse(rndperhr.Text) / 100);

                int flightsperhrNum = (int)Math.Round(double.Parse(flightsperhr.Text)* rndperhrNum);
                double arrdeploc = 1.0 - (double)(arrdepratio.SelectedIndex + 1) * 0.1;
                if(rndarrdepbox.Checked)
                {
                    arrdeploc = arrdeploc * (1.0 + (2 * rnd.NextDouble() - 1) * (double.Parse(rndperhr.Text) / 100));
                }
                int ndeps1 = (int)((double)flightsperhrNum * arrdeploc);
                int narrs1 = flightsperhrNum - ndeps1;
                if (flightsprofile != null)
                {
                    flightsperhrNum = (int)Math.Round((double)flightsprofile[hourindx]* rndperhrNum);
                    arrdeploc = 1.0 - (arrdepprofile[hourindx] + 1) * 0.1;
                    if (rndarrdepbox.Checked)
                    {
                        arrdeploc = arrdeploc * (1.0 + (2 * rnd.NextDouble() - 1) * (double.Parse(rndperhr.Text) / 100));
                    }
                    ndeps1 = (int)((double)flightsperhrNum * arrdeploc);
                    narrs1 = flightsperhrNum - ndeps1;
                }
                if (narrs1 < 0) narrs1 = 0;

                int gaflightsperhrNum = (int)Math.Round(double.Parse(gaflightsperhr.Text) * rndperhrNum);

                int ndepsga1 = (int)((double)gaflightsperhrNum * ((1.0 - (arrdepratio.SelectedIndex + 1) * 0.1)));
                int narrsga1 = gaflightsperhrNum - ndepsga1;
                if (flightsprofile != null)
                {
                    gaflightsperhrNum = gaflightsprofile[hourindx];
                    arrdeploc = 1.0 - (arrdepprofile[hourindx] + 1) * 0.1;
                    if (rndarrdepbox.Checked)
                    {
                        arrdeploc = arrdeploc * (1.0 + (2 * rnd.NextDouble() - 1) * (double.Parse(rndperhr.Text) / 100));
                    }
                    ndepsga1 = (int)((double)gaflightsperhrNum * arrdeploc);
                    narrsga1 = gaflightsperhrNum - ndepsga1;
                }
                if (narrsga1 < 0) narrsga1 = 0;

                ndeps += ndeps1;
                ndepsga += ndepsga1;
                narrs += narrs1;
                narrsga += narrsga1;

                int ndeps1R = (int)Math.Round(ndeps1 * double.Parse(regionalperhr.Text)/100.0);
                int ndeps1C = (int)Math.Round(ndeps1 * double.Parse(cargoperhr.Text)/100.0);
                if (flightsprofile != null)
                {
                    ndeps1R = (int)Math.Round(ndeps1 * ((double)Rflightsprofile[hourindx] / flightsprofile[hourindx]));
                    ndeps1C = (int)Math.Round(ndeps1 * ((double)Cflightsprofile[hourindx] / flightsprofile[hourindx])); 
                }
                int ndeps1N = ndeps1 - ndeps1R - ndeps1C;

                ndepsC += ndeps1C;
                ndepsR += ndeps1R;

                int narrs1R = (int)Math.Round(narrs1 * double.Parse(regionalperhr.Text)/100.0);
                int narrs1C = (int)Math.Round(narrs1 * double.Parse(cargoperhr.Text)/100.0);
                if (flightsprofile != null)
                {
                    narrs1R = (int)Math.Round(narrs1 * ((double)Rflightsprofile[hourindx] / flightsprofile[hourindx]));
                    narrs1C = (int)Math.Round(narrs1 * ((double)Cflightsprofile[hourindx] / flightsprofile[hourindx]));
                }
                int narrs1N = narrs1 - narrs1R - narrs1C;

                narrsC += narrs1C;
                narrsR += narrs1R;

                for (int ii = 0; ii < ndeps1R; ii++)
                {
                    createff(ref deps, ref rnd, ref airportsArrR, ref airportsArrPR, ref fids, ref tmes, hourindx, tstart.SelectedIndex, ref flightsR);
                }
                for (int ii = 0; ii < ndeps1C; ii++)
                {
                    createff(ref deps, ref rnd, ref airportsArrC, ref airportsArrPC, ref fids, ref tmes, hourindx, tstart.SelectedIndex, ref flightsC);
                }
                for (int ii = 0; ii < ndeps1N; ii++)
                {
                    createff(ref deps, ref rnd, ref airportsArrN, ref airportsArrPN, ref fids, ref tmes, hourindx, tstart.SelectedIndex, ref flights);
                }
                for (int ii = 0; ii < narrs1R; ii++)
                {
                    createff(ref arrs, ref rnd, ref airportsArrR, ref airportsArrPR, ref fids, ref tmes, hourindx, tstart.SelectedIndex, ref flightsR);
                }
                for (int ii = 0; ii < narrs1C; ii++)
                {
                    createff(ref arrs, ref rnd, ref airportsArrC, ref airportsArrPC, ref fids, ref tmes, hourindx, tstart.SelectedIndex, ref flightsC);
                }
                for (int ii = 0; ii < narrs1N; ii++)
                {
                    createff(ref arrs, ref rnd, ref airportsArrN, ref airportsArrPN, ref fids, ref tmes, hourindx, tstart.SelectedIndex, ref flights);
                }
                for (int ii = 0; ii < ndepsga1; ii++)
                {
                    createffGA(ref depsga, ref rnd, ref airportsArrGA, ref airportsArrPGA, ref fids, ref tmes, hourindx, tstart.SelectedIndex);
                }
                for (int ii = 0; ii < narrsga1; ii++)
                {
                    createffGA(ref arrsga, ref rnd, ref airportsArrGA, ref airportsArrPGA, ref fids, ref tmes, hourindx, tstart.SelectedIndex);
                }
            }

            StreamWriter fw;
            try
            {
                fw = new StreamWriter(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "schedule.csv"));
            }
            catch
            {
                string tmptxt = "I can't open file '" + Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "schedule.csv") + "' for writing.\n\n";
                tmptxt += "The most likely reason is that the file is open in Excel or Tower Simulator 3 is running.You need to close other applications first.\n\n";
                tmptxt += "It may also happen when Tower Simulator 3 is installed in a system folder like 'Program Files'.\n\n";
                tmptxt += "In this case the solution is to run T3Scheduler as administrator, for example you can right-click on it and select 'Run as administrator'\n\n";
                tmptxt += "You can also right-click on it, select 'Properties', then 'Compatibility' and then check checkbox 'Run this program as administrator'.";
                MessageBox.Show(tmptxt, "ERROR", MessageBoxButtons.OK);
                return;
            }
            fw.WriteLine(header);
            ArrayList tmpk = new ArrayList(arrs.Keys);
            tmpk.Sort();
            foreach(var kvp in tmpk)
            {
                flight ff = (flight)arrs[kvp];
                fw.WriteLine(ff.info + "," + ff.fid + "," + ff.aplane + "," + ff.arp + "," + airport.SelectedItem + "," + ((string)kvp).Substring(0,5) + ",,," + ff.ftype);
            }
            tmpk = new ArrayList(deps.Keys);
            tmpk.Sort();
            foreach (var kvp in tmpk)
            {
                flight ff = (flight)deps[kvp];
                fw.WriteLine(ff.info + "," + ff.fid + "," + ff.aplane + "," + airport.SelectedItem + "," + ff.arp + ",," + ((string)kvp).Substring(0, 5) + ",," + ff.ftype);
            }
            fw.Close();

            fw = new StreamWriter(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "ga.csv"));
            fw.WriteLine(headerga);
            tmpk = new ArrayList(arrsga.Keys);
            tmpk.Sort();
            foreach (var kvp in tmpk)
            {
                flight ff = (flight)arrsga[kvp];
                fw.WriteLine(ff.info + "," + ff.ftype + "," + ff.aplane + "," + ff.arp + "," + airport.SelectedItem + "," + ((string)kvp).Substring(0, 5) + ",,,0,0,0,Default,");
            }
            tmpk = new ArrayList(depsga.Keys);
            tmpk.Sort();
            foreach (var kvp in tmpk)
            {
                flight ff = (flight)depsga[kvp];
                fw.WriteLine(ff.info + "," + ff.ftype + "," + ff.aplane + "," + airport.SelectedItem + "," + ff.arp + ",," + ((string)kvp).Substring(0, 5) + ",,0,0,0,Default,");
            }
            fw.Close();

            label11.Text = "New schedule created: arr " + narrs + " [" + narrsR + " regional, " + narrsC + " cargo], dep " + ndeps + " [" + ndepsR + " regional, " + ndepsC + " cargo], GA arr " + narrsga + ", GA dep " + ndepsga;
            label11.Text += "  => TOTAL flights " + (narrs + ndeps + narrsga + ndepsga).ToString() + " [" + (narrsR + ndepsR).ToString() + " regional, " + (narrsC + narrsR).ToString() + " cargo, " + (narrsga + ndepsga).ToString() + " GA]";

            if (fnerror != "") MessageBox.Show(fnerror);

        }

        private void gaflightsperhr_TextChanged(object sender, EventArgs e)
        {

        }

        public bool form6applied;
        private void button6_Click(object sender, EventArgs e)
        {
            Form ff6 = new Form6(T3location.Text, (string)airport.SelectedItem, (string)database.SelectedItem, this);
            form6applied = false;
            ff6.ShowDialog();
            if(form6applied)
            {
                database_SelectedIndexChanged(database, e);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2(T3location.Text, (string)airport.SelectedItem, (string)database.SelectedItem, this);
            frm2.ShowDialog();
            //label11.Text = form2share;
            button8.Text = "Plot New Schedule";
            button8_Click(sender, e);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "backup");
            if (button8.Text == "Plot New Schedule")
            {
                path = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem);
            }
            int flights = 0;
            int flightsGA = 0;
            int flightsR = 0;
            int flightsC = 0;
            Hashtable airlines1 = new Hashtable();
            Hashtable airports = new Hashtable();
            Hashtable airplanes = new Hashtable();
            StreamReader ff = new StreamReader(Path.Combine(path, "schedule.csv"));
            string txt = ff.ReadLine();
            int[] arrivals = new int[24];
            int[] departures = new int[24];
            int[] gaarrivals = new int[24];
            int[] gadepartures = new int[24];
            string[] xlabels = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            for (int i = 0; i < 24; i++)
            {
                arrivals[i] = 0;
                departures[i] = 0;
                gaarrivals[i] = 0;
                gadepartures[i] = 0;
            }
            while ((txt = ff.ReadLine()) != null)
            {
                string[] arr = Form2.splitCSV(txt);
                for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Trim();
                airlines1[arr[0]] = "1";
                airplanes[arr[3]] = "1";
                if(arr[9] != "cargo" && arr[9] != "regional") flights++;
                if (arr[9] == "cargo") flightsC++;
                if (arr[9] == "regional")flightsR++; 
                if (arr[7] == "")
                {
                    string[] arr1 = arr[6].Split(":".ToCharArray());
                    int hr = int.Parse(arr1[0]);
                    arrivals[hr]++;
                    airports[arr[4]] = "1";
                }
                else
                {
                    string[] arr1 = arr[7].Split(":".ToCharArray());
                    int hr = int.Parse(arr1[0]);
                    departures[hr]++;
                    airports[arr[5]] = "1";
                }
            }
            ff.Close();
            ff = new StreamReader(Path.Combine(path, "ga.csv"));
            txt = ff.ReadLine();
            while ((txt = ff.ReadLine()) != null)
            {
                string[] arr = Form2.splitCSV(txt);
                for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Trim();
                airplanes[arr[2]] = "1";
                flightsGA++;
                if (arr[6] == "")
                {
                    string[] arr1 = arr[5].Split(":".ToCharArray());
                    int hr = int.Parse(arr1[0]);
                    gaarrivals[hr]++;
                    airports[arr[3]] = "1";
                }
                else
                {
                    string[] arr1 = arr[6].Split(":".ToCharArray());
                    int hr = int.Parse(arr1[0]);
                    gadepartures[hr]++;
                    airports[arr[4]] = "1";
                }
            }
            ff.Close();
            chart1.Series["dep"].Points.Clear();
            chart1.Series["arr"].Points.Clear();
            chart1.Series["ga dep"].Points.Clear();
            chart1.Series["ga arr"].Points.Clear();
            for (int i = 0; i < 24; i++)
            {
                chart1.Series["dep"].Points.AddXY(xlabels[i], departures[i]);
                chart1.Series["arr"].Points.AddXY(xlabels[i], arrivals[i]);
                chart1.Series["ga dep"].Points.AddXY(xlabels[i], gadepartures[i]);
                chart1.Series["ga arr"].Points.AddXY(xlabels[i], gaarrivals[i]);
            }
            chart1.ChartAreas[0].AxisX.Interval = 2;
            if (button8.Text == "Plot New Schedule")
            {
                chart1.Titles.Clear();
                chart1.Titles.Add("New Schedule - " + (string)airport.SelectedItem + " - " + (string)database.SelectedItem + " [T3Scheduler " + VERSION + "]");
                button8.Text = "Plot Original Schedule";
                label11.Text = airports.Count + " airports, " + airplanes.Count + " airplane types, " + (flights + flightsGA + flightsR+ flightsC).ToString() + " total flights ";
                label11.Text += "[" + flights.ToString() + " regular, " + flightsR + " regional, " + flightsC + " cargo, " + flightsGA + " GA]";
            }
            else
            {
                chart1.Titles.Clear();
                chart1.Titles.Add("Original Schedule - " + (string)airport.SelectedItem + " - " + (string)database.SelectedItem + " [T3Scheduler " + VERSION + "]");
                button8.Text = "Plot New Schedule";
                label11.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                button9.Visible = false;
                flightsprofile = null;
                Cflightsprofile = null;
                Rflightsprofile = null;
                gaflightsprofile = null;
                arrdepprofile = null;
                flightsperhr.Enabled = true;
                arrdepratio.Enabled = true;
                if (flightsC != null)
                {
                    if (flightsC.Count > 0)
                    {
                        cargoperhr.Enabled = true;
                    }
                    else
                    {
                        cargoperhr.Text = "0.0";
                        cargoperhr.Enabled = false;
                    }
                    if (flightsR.Count > 0)
                    {
                        regionalperhr.Enabled = true;
                    }
                    else
                    {
                        regionalperhr.Text = "0.0";
                        regionalperhr.Enabled = false;
                    }
                    if (flightsga.Count > 0)
                    {
                        gaflightsperhr.Enabled = true;
                    }
                    else
                    {
                        gaflightsperhr.Text = "0";
                        gaflightsperhr.Enabled = false;
                    }
                }
                else
                {
                    regionalperhr.Enabled = true;
                    cargoperhr.Enabled = true;
                    gaflightsperhr.Enabled = true;
                }
                //panel2.Visible = true;
            }
            else
            {
                button9.Visible = true;
                flightsperhr.Enabled = false;
                gaflightsperhr.Enabled = false;
                arrdepratio.Enabled = false;
                cargoperhr.Enabled = false;
                regionalperhr.Enabled = false;
                //panel2.Visible = false; 
            }
        }

        public bool form3applied;
        private void button9_Click(object sender, EventArgs e)
        {
            if(flightsprofile == null)
            {
                flightsprofile = new int[int.Parse(nhr.Text)];
                Cflightsprofile = new int[int.Parse(nhr.Text)];
                Rflightsprofile = new int[int.Parse(nhr.Text)];
                gaflightsprofile = new int[int.Parse(nhr.Text)];
                arrdepprofile = new int[int.Parse(nhr.Text)];
                for(int i=0; i< int.Parse(nhr.Text); i++)
                {
                    flightsprofile[i] = int.Parse(flightsperhr.Text);
                    Cflightsprofile[i] = (int)Math.Round(int.Parse(flightsperhr.Text) * double.Parse(cargoperhr.Text) / 100.0);
                    Rflightsprofile[i] = (int)Math.Round(int.Parse(flightsperhr.Text) * double.Parse(regionalperhr.Text) / 100.0);
                    gaflightsprofile[i] = int.Parse(gaflightsperhr.Text);
                    arrdepprofile[i] = arrdepratio.SelectedIndex;
                }
            }
            string root = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem);
            Form3 ff3 = new Form3(int.Parse(flightsperhr.Text), int.Parse(gaflightsperhr.Text), double.Parse(cargoperhr.Text), double.Parse(regionalperhr.Text), arrdepratio.SelectedIndex, int.Parse(nhr.Text), tstart.SelectedIndex, root, this);
            form3applied = false;
            ff3.ShowDialog();
            if(form3applied)
            {
                button3_Click(sender, e);
                button8.Text = "Plot New Schedule";
                button8_Click(sender, e);
            }
        }

        private void nhr_TextChanged(object sender, EventArgs e)
        {
            flightsprofile = null;
            Cflightsprofile = null;
            Rflightsprofile = null;
            gaflightsprofile = null;
            arrdepprofile = null;
        }

        private void nhr_DeFocus(object sender, EventArgs e)
        {
            int ih = 0;
            try
            {
                ih = int.Parse(nhr.Text);
            }
            catch
            {
                if (nhr.Text == "")
                {
                    ih = 0;
                }
                else
                {
                    MessageBox.Show("Invalid integer");
                    nhr.Text = "2";
                    return;
                }
            }
            if (ih <= 0) { nhr.Text = "2"; return; }
            if (tstart.SelectedIndex + ih > 24)
            {
                nhr.Text = (24 - tstart.SelectedIndex).ToString();
            }
        }

        private void rndperhr_DeFocus(object sender, EventArgs e)
        {
            double p = 0.0;
            try
            {
                p = double.Parse(rndperhr.Text);
            }
            catch
            {
                rndperhr.Text = "0";
                MessageBox.Show("Invalid randomize per hour (%) number");
            }
            if(p>100.0 || p<0.0)
            {
                rndperhr.Text = "0";
                MessageBox.Show("Invalid randomize per hour (%) number");
            }
        }

        private void cargoperhr_DeFocus(object sender, EventArgs e)
        {
            double p = 0.0;
            try
            {
                p = double.Parse(cargoperhr.Text);
            }
            catch
            {
                cargoperhr.Text = "0";
                MessageBox.Show("Invalid cargo (%) number");
            }
            if (p > 100.0 || p < 0.0)
            {
                cargoperhr.Text = "0";
                MessageBox.Show("Invalid cargo (%) number");
            }
            if (p + double.Parse(regionalperhr.Text) > 100.0)
            {
                cargoperhr.Text = "0";
                MessageBox.Show("cargo (%) plus regional (%) cannot exceed 100%");
            }
        }

        private void regionalperhr_DeFocus(object sender, EventArgs e)
        {
            double p = 0.0;
            try
            {
                p = double.Parse(regionalperhr.Text);
            }
            catch
            {
                regionalperhr.Text = "0";
                MessageBox.Show("Invalid regional (%) number");
            }
            if (p > 100.0 || p < 0.0)
            {
                regionalperhr.Text = "0";
                MessageBox.Show("Invalid regional (%) number");
            }
            if (p + double.Parse(cargoperhr.Text) > 100.0)
            {
                regionalperhr.Text = "0";
                MessageBox.Show("cargo (%) plus regional (%) cannot exceed 100%");
            }
        }

        private void tstart_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ih = int.Parse(nhr.Text);
            if (tstart.SelectedIndex + ih > 24)
            {
                nhr.Text = (24 - tstart.SelectedIndex).ToString();
            }
        }

        private void flightsperhr_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex == 0)
            {
                chart1.Series["dep"].ChartType = SeriesChartType.StackedColumn;
                chart1.Series["arr"].ChartType = SeriesChartType.StackedColumn;
                chart1.Series["ga dep"].ChartType = SeriesChartType.StackedColumn;
                chart1.Series["ga arr"].ChartType = SeriesChartType.StackedColumn;
            }
            else
            {
                chart1.Series["dep"].ChartType = SeriesChartType.Column;
                chart1.Series["arr"].ChartType = SeriesChartType.Column;
                chart1.Series["ga dep"].ChartType = SeriesChartType.Column;
                chart1.Series["ga arr"].ChartType = SeriesChartType.Column;
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            form5applied = false;
            Form5 ff = new Form5(this, (string)airport.SelectedItem, (string)database.SelectedItem, T3location.Text); 
            ff.ShowDialog();
            if (form5applied)
            {
                StreamWriter fw = new StreamWriter(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "terminals.csv"));
                fw.WriteLine(headerT);
                foreach (TerminalData td in T3TerminalsData)
                {
                    string txt = td.name + ",";
                    if (td.airlines.Contains(","))
                    {
                        txt += "\"" + td.airlines + "\",";
                    }
                    else
                    {
                        txt += td.airlines + ",";
                    }
                    if (td.gates.Contains(","))
                    {
                        txt += "\"" + td.gates + "\"";
                    }
                    else
                    {
                        txt += td.gates;
                    }
                    fw.WriteLine(txt);
                }
                fw.Close();
                label11.Text = "After changing statistics you need to genereate new schedule in order to see plot";
                button11.BackColor = Color.LightGreen;
                if (flights.Count > 0)
                {
                    flightsperhr.Enabled = true;
                }
                else
                {
                    flightsperhr.Text = "0";
                    flightsperhr.Enabled = false;
                }
                if (flightsC.Count > 0)
                {
                    cargoperhr.Enabled = true;
                }
                else
                {
                    cargoperhr.Text = "0.0";
                    cargoperhr.Enabled = false;
                }
                if (flightsR.Count > 0)
                {
                    regionalperhr.Enabled = true;
                }
                else
                {
                    regionalperhr.Text = "0.0";
                    regionalperhr.Enabled = false;
                }
                if(flightsga.Count> 0)
                {
                    gaflightsperhr.Enabled= true;
                }
                else
                {
                    gaflightsperhr.Text = "0";
                    gaflightsperhr.Enabled= false;
                }
                DumpStatInfo("T3Scheduler1.txt");
            }
            else
            {
                //button11.BackColor = button11DefColor;
            }
        }

        private void rndperhr_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            string tmptxt = "Optimization takes into account number of gates, airline assigments and runways and makes sure ";
            tmptxt += "flights are arranged in such a way that gates and separations are acceptabale. This prevents flight cancellations.\n\n";
            tmptxt += "It will also prevent creating more flights that can be handled with current airport and parameters.\n\n";
            tmptxt += "NOT YET FUNCTIONAL";
            MessageBox.Show(tmptxt, "Optomization Help", MessageBoxButtons.OK);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            SaveFileDialog folderBrowserDialog1 = new SaveFileDialog();
            folderBrowserDialog1.Filter = "Png Image (.png)|*.png";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                chart1.SaveImage(folderBrowserDialog1.FileName, ChartImageFormat.Png);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "backup");
            if (button8.Text == "Plot Original Schedule")
            {
                path = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem);
            }
            SaveFileDialog folderBrowserDialog1 = new SaveFileDialog();
            folderBrowserDialog1.Filter = "HTML (.html)|*.html";
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            Hashtable flights = new Hashtable();
            StreamReader ff = new StreamReader(Path.Combine(path, "schedule.csv"));
            string txt = ff.ReadLine();
            //time type flight plane orig dest comment
            while((txt = ff.ReadLine()) != null)
            {
                string[] arr = txt.Split(new char[] { ',' });
                for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Trim();
                string row = "";
                int seq = 0;
                if (arr[6] == "")
                {
                    //departure
                    row = "<tr style=\"background-color:#ffffdd\">";
                    row += "<td>" + arr[7] + "</td><td>DEPARTURE</td><td>" + arr[1] + arr[2] + "</td><td>" + arr[3] + "</td><td>" + arr[4] + "</td><td>" + arr[5] + "</td><td>" + arr[9] + "</td></tr>";
                    seq = int.Parse(arr[7].Substring(0, 2)) * 10000 + int.Parse(arr[7].Substring(3)) * 100;
                }
                else
                {
                    //arrival
                    row = "<tr style=\"background-color:#ffddff\">";
                    row += "<td>" + arr[6] + "</td><td>ARRIVAL</td><td>" + arr[1] + arr[2] + "</td><td>" + arr[3] + "</td><td>" + arr[4] + "</td><td>" + arr[5] + "</td><td>" + arr[9] + "</td></tr>";
                    seq = int.Parse(arr[6].Substring(0, 2)) * 10000 + int.Parse(arr[6].Substring(3)) * 100;
                }
                while(flights.ContainsKey(seq.ToString("D6")))
                {
                    seq++;
                }
                flights.Add(seq.ToString("D6"), row);
            }
            ff.Close();
            ff = new StreamReader(Path.Combine(path, "ga.csv"));
            txt = ff.ReadLine();
            //time type flight plane orig dest comment
            while ((txt = ff.ReadLine()) != null)
            {
                string[] arr = txt.Split(new char[] { ',' });
                for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Trim();
                string row = "";
                int seq = 0;
                if (arr[5] == "")
                {
                    //departure
                    row = "<tr style=\"background-color:#ffffdd\">";
                    row += "<td>" + arr[6] + "</td><td>DEPARTURE</td><td>" + arr[0] + "</td><td>" + arr[2] + "</td><td>" + arr[3] + "</td><td>" + arr[4] + "</td><td></td></tr>";
                    seq = int.Parse(arr[6].Substring(0, 2)) * 10000 + int.Parse(arr[6].Substring(3)) * 100;
                }
                else
                {
                    //arrival
                    row = "<tr style=\"background-color:#ffddff\">";
                    row += "<td>" + arr[5] + "</td><td>ARRIVAL</td><td>" + arr[0] + "</td><td>" + arr[2] + "</td><td>" + arr[3] + "</td><td>" + arr[4] + "</td><td></td></tr>";
                    seq = int.Parse(arr[5].Substring(0, 2)) * 10000 + int.Parse(arr[5].Substring(3)) * 100;
                }
                while (flights.ContainsKey(seq.ToString("D6")))
                {
                    seq++;
                }
                flights.Add(seq.ToString("D6"), row);
            }
            ff.Close();


            StreamWriter fw = new StreamWriter(folderBrowserDialog1.FileName);
            fw.WriteLine("<html>");
            fw.WriteLine("<body>");
            fw.WriteLine("<table>");
            fw.WriteLine("<tr style=\"background-color:#ccffff\"><td>time</td><td></td><td>flight</td><td>plane</td><td>orig</td><td>dest</td><td></td></tr>");
            ArrayList tmpk = new ArrayList(flights.Keys);
            tmpk.Sort();
            foreach (var kvp in tmpk)
            {
                fw.WriteLine((string)flights[(string)kvp]);
            }
            fw.WriteLine("</table>");
            fw.WriteLine("</body>");
            fw.WriteLine("</html>");
            fw.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            StreamReader ff = new StreamReader(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "schedule.csv"));
            ArrayList data = new ArrayList();
            string txt = "";
            while((txt=ff.ReadLine())!=null)
            {
                data.Add(txt);
            }
            ff.Close();
            Hashtable fids = new Hashtable();
            Random rnd = new Random();
            StreamWriter fw = new StreamWriter(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "schedule.csv"));
            fw.WriteLine(data[0]);
            for(int i=1; i<data.Count; i++)
            {
                string[] arr = Form2.splitCSV((string)data[i]);
                if (arr[9] == "regional" && !airlinesR.ContainsKey(arr[0]))
                {
                    continue;
                }
                if (arr[9] == "cargo" && !airlinesC.ContainsKey(arr[0])) 
                {
                    continue;
                }
                
                if (arr[9] != "regional" && arr[9] != "cargo" && !airlines.ContainsKey(arr[0]))
                {
                    continue;
                }
                arr[2] = generateFlightNumber(ref fids, arr[0], arr[1], ref rnd).ToString();
                for (int j = 0; j < arr.Length; j++)
                {
                    if (j > 0) fw.Write(",");
                    fw.Write(arr[j]);
                }
                fw.WriteLine();
            }
            fw.Close();

            button8.Text = "Plot New Schedule";
            button8_Click(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            form4applied = false;
            Form4 ff = new Form4(this, Path.Combine(T3location.Text,"Airports",(string)airport.SelectedItem,"databases", (string)database.SelectedItem), (string)database.SelectedItem);
            ff.ShowDialog();
            initFLightNumberRanges();
            if (form4applied)
            {
                button10.BackColor = Color.LightGreen;
            }
            else
            {
                label11.Text = "";
                button10.BackColor = button10DefColor;
            }
        }

        System.Windows.Forms.TextBox editclone;
        bool textProcessed;
        private void button5_Click(object sender, EventArgs e)
        {
            textProcessed = false;
            string newname = (string)database.SelectedItem;
            int n = 1;
            string path = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", newname + n.ToString());
            while (Directory.Exists(path))
            {
                n++;
                path = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", newname + n.ToString());
            }
            var tmpobj = new System.Windows.Forms.TextBox();
            tmpobj.Text = newname+n.ToString();
            tmpobj.KeyDown += OnSubActionD;
            tmpobj.Top = button5.Top;
            tmpobj.Left = button5.Left;
            tmpobj.Parent = this;
            tmpobj.Width = database.Width + 20; ;
            tmpobj.BringToFront();
            tmpobj.MouseHover += textbox_MouseHover;
            tmpobj.LostFocus += textbox_LostFocus;
            tmpobj.Focus();
            editclone = tmpobj;
        }

        void OnSubActionD(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var tmpobj = (System.Windows.Forms.TextBox)sender;
            if (e.KeyCode == Keys.Escape)
            {
                tmpobj.Dispose();
                return;
            }
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            string path = Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", tmpobj.Text);
            if (Directory.Exists(path))
            {
                MessageBox.Show("Database directory '" + tmpobj.Text + "' already exists!", "ERROR");
                return;
            }
            textProcessed = true;
            //copy directory and subdirs
            DirectoryInfo dst = new DirectoryInfo(path);
            DirectoryInfo src = new DirectoryInfo(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem));
            CopyAll(src, dst);
            //refresh databases
            string[] arr = (string[])airportdbs[(string)airport.SelectedItem];
            string[] arr1 = arr.Concat(new string[1] { path }).ToArray();
            airportdbs[(string)airport.SelectedItem] = arr1;
            tmpobj.Dispose();
            airport_SelectedIndexChanged_1(airport, new EventArgs());
        }

        private void textbox_MouseHover(object sender, EventArgs e)
        {
            var tmpobj = (System.Windows.Forms.TextBox)sender;
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(tmpobj, "ENTER to change, ESC to cancel");
        }

        private void textbox_LostFocus(object sender, EventArgs e)
        {
            if (textProcessed) return;
            var tmpobj = (System.Windows.Forms.TextBox)sender;
            if (MessageBox.Show("Are you sure to cancel cloning database directory?","Message", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                tmpobj.Focus();
                return;
            }
            tmpobj.Dispose();
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Current schedule will become the base for any generated schedule until you read original schedule.\nAre you sure?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            ReadScheduleStat(false, false, "T3Scheduler0.txt");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SaveFileDialog folderBrowserDialog1 = new SaveFileDialog();
            folderBrowserDialog1.Filter = "Text (.txt)|*.txt";
            folderBrowserDialog1.FileName = "T3Scheduler_" + (string)airport.SelectedItem + "_" + (string)database.SelectedItem + "_STATDETAILS.txt";
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            DumpStatInfo(folderBrowserDialog1.FileName);
        }

        private void adatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (database.SelectedIndex == -1 || adatabase.Items.Count == 0) return;
            //MessageBox.Show("adatabase " + adatabase.SelectedIndex);
            if (adatabase.SelectedIndex == -1)
            {
                if (!File.Exists(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "package.txt")))
                {
                    MessageBox.Show("File package.txt is missing in your airport database directory!\nCannot set airplane database automatically, please select manually now.", "WARNING");
                    adatabase.SelectedIndex = -1;
                    return;
                }
                //get root database name from package.txt
                rootdatabase = "default";
                StreamReader ff = new StreamReader(Path.Combine(T3location.Text, "Airports", (string)airport.SelectedItem, "databases", (string)database.SelectedItem, "package.txt"));
                string txt = "";
                while ((txt = ff.ReadLine()) != null)
                {
                    if (txt.StartsWith("COMPATIBLE:"))
                    {
                        rootdatabase = txt.Substring(12);
                        break;
                    }
                }
                ff.Close();
                //MessageBox.Show("adatabase " + adatabase.SelectedIndex + " " + rootdatabase + " " + adatabase.Items.Count);
                for (int i=0; i<adatabase.Items.Count; i++)
                {
                    if ((string)adatabase.Items[i] == rootdatabase)
                    {
                        adatabase.SelectedIndex = i;
                    }
                }
                if (adatabase.SelectedIndex == -1)
                {
                    MessageBox.Show("Cannot set airplane database automatically, please select manually now.", "WARNING");
                    return;
                }
            }
            else
            {
                rootdatabase = (string)adatabase.SelectedItem;
            }
                    
            readAirlinesAndAirplanes();

            panel1.Visible = false;
            label3.Text = "";
            label11.Text = "";
            if ((string)database.SelectedItem == "default")
                button7.Enabled = false;
            else
                button7.Enabled = true;
            comboBox1.SelectedIndex = 0;
            label3.Text = "Airplane database: " + rootdatabase + " " + T3AirlinesData.Count + " airlines  " + TS3AirplaneData.Count + " airplanes";
        }
    }
}
