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

namespace T3Scheduler
{
    public partial class Form4 : Form
    {
        Form1 parent;
        string basepath;
        string database;
        Hashtable airlines;
        public Form4(Form1 parent, string basepath, string database)
        {
            InitializeComponent();
            this.Text = "Flight number ranges";
            this.parent = parent;
            this.basepath = basepath;
            this.database = database;

            textBox1.Text = "";

            ArrayList tmpk = new ArrayList(parent.airlineFnumbers.Keys);
            tmpk.Sort();
            for (int i = 0; i < parent.airlineFnumbers.Count; i++)
            {
                if (i > 0) textBox1.Text += "\r\n";
                string[] sss = ((string)tmpk[i]).Split('-');
                textBox1.Text += sss[0] + " " + sss[1] + " " + parent.airlineFnumbers[tmpk[i]];
            }

            airlines = new Hashtable();
            foreach(AirlineData ad in parent.T3AirlinesData)
            {
                airlines[ad.ICAO] = "1";
            }

            label2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sgf = "";
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            folderBrowserDialog1.FileName = "TS3AirlineRanges_" + database + ".txt";
            folderBrowserDialog1.CheckFileExists = true;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                sgf = folderBrowserDialog1.FileName;
            }
            else
            {
                return;
            }
            textBox1.Text = "";
            StreamReader fpr = new StreamReader(sgf);
            textBox1.Text = fpr.ReadToEnd();
            fpr.Close();
            label2.Text = textBox1.Text.Length + " characters read from " + sgf;
        }

        private string validateLiveries(string airline, string oper)
        {
            string extra = "";
            if (airline != oper) extra = "_" + oper;
            bool isOK = false;
            foreach (AirlineData adta in parent.T3AirlinesData)
            {

                if (adta.ICAO == airline + extra) isOK = true;
            }
            if (isOK)
                return "";
            else
                return "no livery for " + airline + "_" + oper;
        }

        private string validateRanges(string ranges)
        {
            string[] rrr = ranges.Split(',');
            foreach (string rng in rrr)
            {
                string[] xxx = rng.Split('-');
                if (xxx.Length != 2)
                {
                    return "ERROR: Invalid range " + rng;
                }
                try
                {
                    if (int.Parse(xxx[0]) >= int.Parse(xxx[1]))
                    {
                        return "ERROR: range " + rng + " not inorder";
                    }
                    if (int.Parse(xxx[0])<1 || int.Parse(xxx[0]) > 9999)
                    {
                        return "ERROR: " + xxx[0] + " is out of range";
                    }
                    if (int.Parse(xxx[1]) < 1 || int.Parse(xxx[1]) > 9999)
                    {
                        return "ERROR: " + xxx[1] + " is out of range";
                    }
                }
                catch
                {
                    return "ERROR: Invalid range " + rng + " cannot parse numbers";
                }
            }
            return "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sgf = "";
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            folderBrowserDialog1.FileName = "TS3AirlineRanges_" + database + ".txt";
            folderBrowserDialog1.CheckFileExists = false;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                sgf = folderBrowserDialog1.FileName;
            }
            else
            {
                return;
            }
            StreamWriter fpw = new StreamWriter(sgf);
            fpw.Write(textBox1.Text);
            fpw.Close();
            label2.Text = textBox1.Text.Length + " characters written to " + sgf;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] rrr = textBox1.Text.Split('\n');
            string warning = "";
            Hashtable tt = new Hashtable(); 
            foreach (string lll1 in rrr)
            {
                string lll = lll1.Replace("\r", "");
                if (lll == "") continue;
                string[] sss = lll.Split(' ');
                if (!airlines.ContainsKey(sss[0]))
                {
                    warning += sss[0] + " is not a valid TS3 airline\n";
                }
                if (!airlines.ContainsKey(sss[1]))
                {
                    warning += sss[1] + " is not a valid TS3 airline\n";
                }
                if (tt.ContainsKey(sss[0] + "-" + sss[1]))
                {
                    MessageBox.Show(sss[0] + "-" + sss[1] + " occurs twice, i.e. in two lines. One airline & operator per line please!", "ERROR");
                    return;
                }
                tt[sss[0] + "-" + sss[1]] = "1";
                string res = validateRanges(sss[2]);
                if(res != "")
                {
                    label2.Text = "ERROR: invalid ranges";
                    MessageBox.Show(res, "ERROR");
                    return;
                }
                string txt = validateLiveries(sss[0], sss[1]);
                if (txt != "") warning += txt + "\n";
            }
            if(warning != "")
            {
                MessageBox.Show(warning, "WARNING");
            }
            parent.airlineFnumbers = new Hashtable();
            foreach (string lll in rrr)
            {
                if (lll == "") continue;
                string[] sss = lll.Split(' ');
                parent.airlineFnumbers[sss[0] + "-" + sss[1]] = sss[2];
            }
            label2.Text = "Applied!";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] rrr = textBox1.Text.Split('\n');
            string warning = "";
            Hashtable tt= new Hashtable();
            foreach (string lll in rrr)
            {
                if (lll == "") continue;
                string[] sss = lll.Split(' ');
                if (!airlines.ContainsKey(sss[0]))
                {
                    warning += sss[0] + " is not a valid TS3 airline\n";
                }
                if (!airlines.ContainsKey(sss[1]))
                {
                    warning += sss[1] + " is not a valid TS3 airline\n";
                }
                if(tt.ContainsKey(sss[0] + "-" + sss[1]))
                {
                    MessageBox.Show(sss[0] + "-" + sss[1] + " occurs twice, i.e. in two lines. One airline & operator per line please!", "ERROR");
                    return;
                }
                tt[sss[0] + "-" + sss[1]] = "1";
                string res = validateRanges(sss[2]);
                if (res != "")
                {
                    warning += "ERROR: invalid ranges\n";
                }
                string txt = validateLiveries(sss[0], sss[1]);
                if (txt != "") warning += txt + "\n";
            }
            if (warning != "")
            {
                MessageBox.Show(warning, "WARNING");
            }
            else
            {
                MessageBox.Show("Ranges are OK", "MESSAGE");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            StreamReader fpr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FlightRangesDefault.txt"));
            textBox1.Text = fpr.ReadToEnd();
            fpr.Close();
            label2.Text = "Default flight number ranges loaded";
        }
    }
}
