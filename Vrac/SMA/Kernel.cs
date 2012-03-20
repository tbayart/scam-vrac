using System;
using System.Drawing;
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
            CarteManipulee = Vrac.GenerateurCarte.Carte.GetCarteTest(256, 256);

            for (int i = 0; i < 50; i++)
            {
                Dryad d = Creer<Dryad>();
                d.Coord = new Coordonnees(Randomizer.Next(5, 250), Randomizer.Next(5, 250));
            }

            Start();
        }

        private static int nbIter = 20;

        public static void Start()
        {
            while (!S_Stop && --nbIter > 0)
            {
                NewTurn();
                Thread.Sleep(0);

                Bitmap bmp = CarteManipulee.getBitmap();

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    PagesBlanches.Agents.ForEach(a =>
                        g.DrawEllipse(Pens.White, a.Coord.X - 1, a.Coord.Y - 1, 2, 2)
                        );
                }

                bmp.Save(@"./Temp/AgentEtape" + String.Format("{0:000}", (20 - nbIter)) + ".bmp");
            }
        }

        public static void Stop()
        {
            S_Stop = true;
        }

        public static T Creer<T>() where T : Agent, new()
        {
            T agt = new T();
            
            PagesBlanches.add(agt);

            return agt;
        }

        public static void Receive(Evenement evt)
        {
            if (evt is Evt_Mort)
            {
                PagesBlanches.del(evt.Emetteur);
            }
        }

        public static void NewTurn()
        {
            ManagerEvenements.Poster(Evenement.NewTurn);
        }

        #endregion --> Méthodes statiques
    }
}
