using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vrac.SMA;

namespace WinFormTest
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ThreadStart kst = Kernel.Init;
            //Thread kThrd = new Thread(kst);
            //kThrd.Start();

            Kernel.Init(1024,100);
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
