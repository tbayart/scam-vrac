using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Vrac;
using Vrac.GenerateurCarte;
using Vrac.SMA;
using Vrac.SMA.Evenements;

namespace ConsoleTest
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Heure début : {0}", DateTime.Now.ToLongTimeString());

        //    //CarteV2 map = CarteV2.GetCarteTest(1500000);

        //    for (int i = 0; i < 3; i++)
        //    {
        //        Carte map = Carte.GetCarteTest(750, 750);

        //        File.Copy("./Temp/Finale.bmp", "./Temp/Finale" + i + ".bmp");

        //        Console.WriteLine("ok {1} : {0}", DateTime.Now.ToLongTimeString(), i);
        //    }

        //    Console.WriteLine("Heure fin   : {0}", DateTime.Now.ToLongTimeString());
        //    Console.WriteLine("Terminé !");
        //    Console.ReadLine();
        //}



        static void Main(string[] args)
        {
            Console.WriteLine("Heure début : {0}", DateTime.Now.ToLongTimeString());

            //int test = 1;

            //if (test == 1) // On utilise le nouveau code
            {
                //ThreadStart tsMgrEvts = ManagerEvenements.Start;
                //Thread tMgrEvts = new Thread(tsMgrEvts);
                //tMgrEvts.Start();

                int nbSec = 2;
                Kernel.Init();
                Kernel.InitCitizen();
                Kernel.Start();

                int precedenteValeur = 0;
                for (int i = 0; i < nbSec; i++)
                {
                    Thread.Sleep(1000);
                    int count = Kernel.managerEvenements.count;
                    Console.WriteLine(count- precedenteValeur + " evt");
                    precedenteValeur = count;
                }
                Kernel.KillAll();

                Kernel.Draw().Save(@"./Temp/after.bmp");

                Kernel.managerEvenements.Shutdown(true);
                Console.WriteLine(Kernel.managerEvenements.count / nbSec + " evt à la seconde pendant " + nbSec + " sec");

                //ManagerEvenements.Stop();
            }
            //else // On utilise le code de VracAgent.cs
            //{
            //    ThreadStart tsMgrEvts = VracAgent.EvtDispatcher.Start;
            //    Thread tMgrEvts = new Thread(tsMgrEvts);
            //    tMgrEvts.Start();

            //    VracAgent.Kernel.Init();

            //    VracAgent.EvtDispatcher.Stop();
            //}

            Console.WriteLine("Heure fin   : {0}", DateTime.Now.ToLongTimeString());
            Console.WriteLine("Terminé !");
            Console.ReadLine();
        }
    }
}
