using System;
using System.IO;

namespace ScamCarte
{
    public class BenchGenerateur
    {
        private static Random R = new Random();

        public static void BenchGenerateurCarte()
        {
            if (!Directory.Exists(@".\BenchCarte\"))
            {
                Directory.CreateDirectory(@".\BenchCarte\");
            }

            StreamWriter sw = new StreamWriter(@".\BenchCarte\BenchCarte.csv", false);
            sw.WriteLine("Etape;Superficie;Temps;");

            int etape = 1;

            DateTime dt = DateTime.Now;

            BenchPallier(ref etape, 45000, 153000, 500, sw);

            BenchPallier(ref etape, 170000, 635000, 300, sw);

            BenchPallier(ref etape, 675000, 2580000, 100, sw);

            BenchPallier(ref etape, 2660000, 10000000, 50, sw);

            BenchCarte(ref etape, 20000000, sw);
            BenchCarte(ref etape, 30000000, sw);
            //BenchCarte(ref etape, 40000000, sw);
            sw.WriteLine();

            double time = DateTime.Now.Subtract(dt).TotalSeconds;

            Console.WriteLine();
            Console.WriteLine("Temps total : {0:0.000000} s", time);
            sw.WriteLine("Total;;{0:0.000000};s", time);

            sw.Flush();
            sw.Close();

            Console.WriteLine("Terminé !");
            Console.ReadLine();
        }

        private static void BenchPallier(ref int etape, int pallierMin, int pallierMax, int nbIter, StreamWriter writerFichierDebug)
        {
            int ecartPalliers = (pallierMax - pallierMin) / 10;
            int x;

            for (int i = 0; i < nbIter; i++)
            {
                x = R.Next(11);
                BenchCarte(ref etape, pallierMin + x * ecartPalliers, writerFichierDebug);
            }
        }

        private static void BenchCarte(ref int etape, int superficie, StreamWriter writerFichierDebug)
        {
            DateTime dt = DateTime.Now;

            GenerateurCarte.GenererNouvelleCarte(superficie);

            double time = DateTime.Now.Subtract(dt).TotalMilliseconds;

            Console.WriteLine("Carte {0} (Superficie : {1:## ### ###}) générée en {2:0.000000} ms", etape, superficie, time);
            writerFichierDebug.WriteLine("{0};{1};{2:0.000000};ms", etape, superficie, time);
            writerFichierDebug.Flush();

            etape++;
        }
    }
}
