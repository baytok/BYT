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
            string arg1 = "BYT:5624922e-e1fa-46cd-8704-9e04fb0603ef";
            string arg2 = "";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BYTIslemler(arg1, arg2));
          
        }
    }
}
