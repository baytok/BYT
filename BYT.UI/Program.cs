﻿using System;
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
            string arg1 = "fc229c1e-3572-44c3-bc4f-df72015fed90";
            string arg2 = "";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BYTIslemler(arg1, arg2));
          
        }
    }
}
