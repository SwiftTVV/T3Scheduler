using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T3Scheduler
{
    public class AirplaneData
    {
        public string ICAO { get; set; }
        public string name { get; set; }
        public string wclass { get; set; }
        public string sclass { get; set; }
        public string atype { get; set; }
        public string[] airlines { get; set; }
    }

    public class AirlineData
    {
        public string ICAO { get; set; }
        public string[] airplanes { get; set; }
        public string airplanesStr { get; set; }
    }

    public class TerminalData
    {
        public string name { get; set; }
        public string airlines { get; set; }
        public string gates { get; set;}
    }
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(GlobalThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(new Form1());
        }

        static void GlobalThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show("This information was logged in file \n" +
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "T3Scheduler.debug.log") +
                "\n" + Form1.VERSION + "\n------------------\n" + e.Exception.Message + "\n" + e.Exception.StackTrace, "Unhandled Exception");
            StreamWriter fw = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "T3Scheduler.debug.log"), true);
            fw.WriteLine(DateTime.Now.ToString());
            fw.WriteLine(Form1.VERSION);
            fw.WriteLine(e.Exception.Message);
            fw.WriteLine(e.Exception.StackTrace);
            fw.Close();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                MessageBox.Show("This information was logged in file \n" +
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "T3Scheduler.debug.log") +
                "\n" + Form1.VERSION + "\n------------------\n" + ex.Message + "\n" + ex.StackTrace, "Unhandled Exception");
                StreamWriter fw = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "T3Scheduler.debug.log"), true);
                fw.WriteLine(DateTime.Now.ToString());
                fw.WriteLine(Form1.VERSION);
                fw.WriteLine(ex.Message);
                fw.WriteLine(ex.StackTrace);
                fw.Close();
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal Non-UI Error",
                        "Fatal Non-UI Error. Could not write the error to the event log. Reason: "
                        + exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }
    }
}
