using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Vrac;
using Vrac.SMA;
using Vrac.SMA.Agents;
using Vrac.SMA.Evenements;
using Tools;

namespace ConsoleTest
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    Kernel.Init();
        //    Kernel.InitDryad();
        //    for (int i = 0; i < 10000; i++)
        //    {
             
        //    Coordonnees c1;
        //    c1 = new Coordonnees(Randomizer.Next(1024),Randomizer.Next(1024));
        //    HighPerfTimer.time(() => { var v = Kernel.PagesBlanches.Agents(c1,Randomizer.NextDouble()*100); }, "get agent");

        //    }
        //}

        static void Main(string[] args)
        {
            if (Directory.Exists(@".\Temp\"))
                Directory.Delete(@".\Temp\", true);

            Directory.CreateDirectory(@".\Temp\");

            Console.WriteLine("Heure début : {0}", DateTime.Now.ToLongTimeString());

            int nbSec = 10;
            Kernel.Init(500000,100);

            Kernel.PagesBlanches.DrawSecteurs().Save(@".\Temp\debugS.bmp");

            Kernel.InitDryad(1000);
            Kernel.Start();

            for (int i = 0; i < nbSec; i++)
            {

                Kernel.Draw().Save(@".\Temp\debug"+i+".bmp");  
            }
            
            Kernel.KillAll();

            //Kernel.InitCitizen(100,1024);
            //Kernel.Start();
            //Thread.Sleep(nbSec * 1000);

            Kernel.managerEvenements.Shutdown(true);
            Console.WriteLine(Kernel.managerEvenements.count / nbSec + " evts à la seconde pendant " + nbSec + " sec");

            Kernel.Draw().Save(@".\Temp\debugF.bmp");

            Console.WriteLine("Heure fin   : {0}", DateTime.Now.ToLongTimeString());
            Console.WriteLine("Terminé !");
            Console.ReadLine();
        }
    }
}
