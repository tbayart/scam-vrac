﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Vrac;
using Vrac.GenerateurCarte;
using System.IO;
using VracAgent;

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

            ThreadStart tsDsp = EvtDispatcher.Start;
            Thread t = new Thread(tsDsp);
            t.Start();

            Kernel.Init();

            EvtDispatcher.Stop();

            Console.WriteLine("Heure fin   : {0}", DateTime.Now.ToLongTimeString());
            Console.WriteLine("Terminé !");
            Console.ReadLine();
        }
    }
}
