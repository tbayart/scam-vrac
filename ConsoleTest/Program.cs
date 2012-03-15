using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vrac;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            World w = new World();

            w.ecrire();

            Console.WriteLine("Terminé !");
            Console.ReadLine();
        }
    }
}
