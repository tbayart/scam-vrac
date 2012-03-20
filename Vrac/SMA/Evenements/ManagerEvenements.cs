using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Vrac.SMA.Actions;

namespace Vrac.SMA.Evenements
{
    public static class ManagerEvenements
    {
        #region --> Attributs

        public static object SyncRoot = new object();
        public static Queue<Evenement> BAL = new Queue<Evenement>();

        private static bool S_Stop = false;
        
        #endregion --> Attributs

        #region --> Méthodes statiques

        public static void Start()
        {
            while (!S_Stop)
            {
                Do();
                Thread.Sleep(0);
            }
        }

        public static void Stop()
        {
            S_Stop = true;
        }

        public static void Poster(Evenement evt)
        {
            lock (SyncRoot) 
            { 
                BAL.Enqueue(evt); 
            }
        }

        private static void Do()
        {
            Evenement evt = null;

            lock (SyncRoot)
            {
                if (BAL.Count > 0)
                    evt = BAL.Dequeue();
                else
                    Thread.Sleep(10);
            }

            if (evt != null)
                Do(evt);
        }

        private static void Do(Evenement evt)
        {
            Kernel.Receive(evt);

            // Récupérer les agents qui sont à portée.
            var q = Kernel.PagesBlanches.Agents
                .Where(
                    a => evt.Portee == -1 || evt.Emetteur.Coord.getDistance(a.Coord) < evt.Portee && evt.Emetteur != a
                );

            q.ToList().ForEach(
                    agtConcerne => agtConcerne.Do(NomAction.Ecouter, evt.Emetteur, evt)
                );
        }

        #endregion --> Méthodes statiques
    }
}
