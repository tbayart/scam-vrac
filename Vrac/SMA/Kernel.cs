using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Vrac.SMA.Agents;
using Vrac.SMA.Evenements;
using Vrac.Tools;

namespace Vrac.SMA
{
    public static class Kernel
    {
        // TODO: Voir pour ne pas référencer directement Vrac.GenerateurCarte.Carte.
        // --> Déplacer Carte ?

        #region --> Attributs

        public static Annuaire PagesBlanches = new Annuaire();

        public static Vrac.GenerateurCarte.Carte CarteManipulee;

        private static bool S_Stop = false;

        #endregion --> Attributs

        #region --> Méthodes statiques

        public static void Init()
        {
            int taille = 512;
            CarteManipulee = Vrac.GenerateurCarte.Carte.GetCarteTest(taille, taille);
            PagesBlanches.CreerSecteurs(taille, taille, 256);
            for (int i = 0; i < taille*taille/512; i++)
            {
                Creer<Dryad>(taille);
            }

            Start();
        }

        private static int nbIterMax = 2000;
        private static int nbIter = nbIterMax;

        public static void Start()
        {
            while (!S_Stop && nbIter-- > 0)
            {
                int count;
                int countAsync;
                int max;
                int maxAsync;
                ThreadPool.GetAvailableThreads(out count, out countAsync);
                ThreadPool.GetMaxThreads(out max, out maxAsync);
                while (countAsync < maxAsync &&  count < max )
                {
                    Thread.Sleep(100);
                    ThreadPool.GetAvailableThreads(out count, out countAsync);
                }

                NewTurn();
                Thread.Sleep(0);

                    Bitmap bmp = CarteManipulee.getBitmap();

                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            PagesBlanches.Agents(null, -1).ToList().ForEach(a =>
                                                                            g.DrawEllipse(Pens.White, a.Coord.X - 1, a.Coord.Y - 1, 2, 2)
                                );
                        }

                if (nbIter % 100 == 0)
                {
                    bmp.Save(@"./Temp/AgentEtape" + String.Format("{0:00000}", (nbIterMax - nbIter)) + ".bmp");
                }
            }
        }

        public static void Stop()
        {
            S_Stop = true;
        }

        public static T Creer<T>(int taille) where T : Agent, new()
        {
            T agt = new T
                        {
                            Coord = new Coordonnees(Randomizer.Next((int) (taille*0.05), (int) (taille*0.95)), Randomizer.Next((int) (taille*0.05), (int) (taille*0.95)))
                        };

            PagesBlanches.add(agt);

            return agt;
        }

        public static void Receive(Evenement evt)
        {
            if (evt is Evt_Mort)
            {
                lock (PagesBlanches)
                {
                    PagesBlanches.del(evt.Emetteur);
                }
            }
            if (evt is Evt_Deplace)
            {
                lock (PagesBlanches)
                {
                    PagesBlanches.majAgent(evt.Emetteur);
                }
            }
        }

        private static int count = 0;

        public static void NewTurn()
        {
            ManagerEvenements.Poster(Evenement.NewTurn);

            //lock (semaphore.sema)
            //{
            //    File.AppendAllText("C:/Trace.txt", "----------------------   " + count++ + "   ----------------------" + Environment.NewLine);

            //    PagesBlanches.Agents(null, -1).ToList().ForEach(a => File.AppendAllText("C:/Trace.txt", a.Coord.ToString() + Environment.NewLine));
            //}
        }

        #endregion --> Méthodes statiques
    }

    static class semaphore
    {
        public static object sema = new object();
    }
}
