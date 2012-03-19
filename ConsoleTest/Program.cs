using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vrac;
using Vrac.GenerateurCarte;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Heure début : {0}", DateTime.Now.ToLongTimeString());

            for (int i = 0; i < 3; i++)
            {

                Carte map = Carte.GetCarteTest();

                File.Copy("./Temp/Finale.bmp", "./Temp/Finale" + i + ".bmp");

                Console.WriteLine("ok {1}: {0}", DateTime.Now.ToLongTimeString(),i);
            }

            Console.WriteLine("Heure fin   : {0}", DateTime.Now.ToLongTimeString());
            Console.WriteLine("Terminé !");
            Console.ReadLine();
        }
    }
}
