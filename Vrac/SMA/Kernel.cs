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
            int taille = 256;
            CarteManipulee = Vrac.GenerateurCarte.Carte.GetCarteTest(taille, taille);
            PagesBlanches.CreerSecteurPrincipal(taille, taille / 2, taille / 2, (int)(Math.Sqrt(2)*taille)+1);

            PagesBlanches.DrawSecteurs();

            for (int i = 0; i < taille * taille / 512; i++)
            {
                Creer<Dryad>(taille);
            }

            Start();
        }

        private static int nbIterMax = 10;
        private static int nbIter = nbIterMax;

        public static void Start()
        {
            FirstTurn();
            //while (!S_Stop /*&& nbIter-- > 0*/)
            //{
            //    Thread.Sleep(1);

            //    //Draw().Save(@"./Temp/AgentEtape" + String.Format("{0:00000}", (nbIterMax - nbIter)) + ".bmp");
                
            //}
        }

        public static Bitmap Draw()
        {
            Bitmap bmp = CarteManipulee.getBitmap();

            using (Graphics g = Graphics.FromImage(bmp))
            {
                PagesBlanches.Agents(null, -1).ToList().ForEach(a =>
                                                                g.DrawEllipse(Pens.White, a.Coord.X - 1, a.Coord.Y - 1, 2, 2)
                    );
            }
            return bmp;
        }

        public static void Stop()
        {
            S_Stop = true;
        }

        public static T Creer<T>(int taille) where T : Agent, new()
        {
            T agt = new T
                        {
                            Coord = new Coordonnees(Randomizer.Next((int) (taille*0.001), (int) (taille*0.999)), Randomizer.Next((int) (taille*0.001), (int) (taille*0.999)))
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
            else if (evt is Evt_Deplace)
            {
                lock (PagesBlanches)
                {
                    PagesBlanches.majAgent(evt.Emetteur);
                }
            }
        }

        private static int count = 0;

        public static void FirstTurn()
        {
            ManagerEvenements.Poster(Evenement.FirstTurn);

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
