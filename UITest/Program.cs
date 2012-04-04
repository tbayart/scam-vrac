using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SMA;

namespace UITest
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Démarrage du SMA avec une nouvelle carte générée aléatoirement.
            Kernel.Start();

            // Démarrage de l'UI.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
