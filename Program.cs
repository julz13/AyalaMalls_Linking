
using AyalaMalls_Linking.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyalaMalls_Linking
{
    static class Program
    {
        public static string DatabaseFile { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            //Application.Run(new Form1());
            //Application.Run(new frmConfig());
            //Application.Run(new frmDateFilter());


        }
    }
}
