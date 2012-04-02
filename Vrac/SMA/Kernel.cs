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

        public static ManagerEvenements managerEvenements;

        public static Annuaire PagesBlanches = new Annuaire();

        public static Vrac.GenerateurCarte.Carte CarteManipulee;

        private static bool S_Stop = false;

        #endregion --> Attributs

        #region --> Méthodes statiques


        public static void Init(int tailleCarte, int nbThread)
        {

            CarteManipulee = Vrac.GenerateurCarte.Carte.GetCarteTerre(tailleCarte, tailleCarte);
            //CarteManipulee = Vrac.GenerateurCarte.CarteV2.GetCarteTest(2000000);

            int taille = Math.Max(CarteManipulee._carte.Length, CarteManipulee._carte[0].Length);
            PagesBlanches.CreerSecteurPrincipal(taille, taille / 2, taille / 2, (int)(Math.Sqrt(2)*taille)+1);
            //PagesBlanches.DrawSecteurs();
            managerEvenements = new ManagerEvenements(nbThread);

            
        }

        public static void InitDryad(int nb, int tailleCarte)
        {
            for (int i = 0; i < nb; i++) //taille * taille / 512
            {
                Creer<Dryad>(tailleCarte);
            }

        }

        public static void InitCitizen(int nb, int tailleCarte)
        {
            for (int i = 0; i < nb; i++) //taille * taille / 512
            {
                Creer<Citizen>(tailleCarte);
            }

        }

        //private static int nbIterMax = 2;
        //private static int nbIter = nbIterMax;

        public static void Start()
        {
            FirstTurn();
            //while (!S_Stop && nbIter-- > 0)
            //{
            //    Thread.Sleep(1000);

            //Draw().Save(@"./Temp/AgentEtape" + String.Format("{0:00000}", (nbIterMax - nbIter)) + ".bmp");
                
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
                PagesBlanches.Agents(new Coordonnees(512,512), 1000).ToList().ForEach(a =>
                                                                g.DrawEllipse(Pens.Yellow, a.Coord.X - 2, a.Coord.Y - 2, 4, 4)
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
                            Coord = new Coordonnees(Randomizer.Next(taille), Randomizer.Next(taille ))
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

        public static void FirstTurn()
        {
            managerEvenements.Poster(Evenement.FirstTurn);

            //lock (semaphore.sema)
            //{
            //    File.AppendAllText("C:/Trace.txt", "----------------------   " + count++ + "   ----------------------" + Environment.NewLine);

            //    PagesBlanches.Agents(null, -1).ToList().ForEach(a => File.AppendAllText("C:/Trace.txt", a.Coord.ToString() + Environment.NewLine));
            //}
        }

        #endregion --> Méthodes statiques

        public static void KillAll()
        {
            PagesBlanches.clAgents.ForEach(a => PagesBlanches.del(a)); // raccourci pour tout virer
        }
    }

    static class semaphore
    {
        public static object sema = new object();
    }
}
