using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BYT.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //string arg1 = args[0];
            //string arg2 = args[1];
            string arg1 = "BYT:ca433f52-80e3-46d3-8e85-1356fd436e6f";
            string arg2 = "";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BYTIslemler(arg1, arg2));
          
        }
    }
}
