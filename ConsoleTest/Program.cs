using System;
using System.Collections.Generic;
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
            
            Carte map = Carte.GetCarteTest();

            Console.WriteLine("Heure fin   : {0}", DateTime.Now.ToLongTimeString());
            Console.WriteLine("Terminé !");
            Console.ReadLine();
        }
    }
}
