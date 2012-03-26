using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Vrac.SMA.Actions;
using Vrac.SMA.Agents;

namespace Vrac.SMA.Evenements
{
    //public static class ManagerEvenements
    //{
        //#region --> Attributs

        //public static object SyncRoot = new object();
        //public static Queue<Evenement> BAL = new Queue<Evenement>();

        //private static bool S_Stop = false;
        
        //#endregion --> Attributs


        //public static void Start()
        //{
        //    while (!S_Stop)
        //    {
        //        Do();
        //        Thread.Sleep(0);
        //    }
        //}

        //public static void Stop()
        //{
        //    S_Stop = true;
        //}

        //public static void Poster(Evenement evt)
        //{
        //    lock (SyncRoot) 
        //    { 
        //        BAL.Enqueue(evt); 
        //    }
        //}

        //private static void Do()
        //{
        //    Evenement evt = null;

        //    lock (SyncRoot)
        //    {
        //        if (BAL.Count > 0)
        //            evt = BAL.Dequeue();
        //        else
        //            Thread.Sleep(10);
        //    }

        //    if (evt != null)
        //        ThreadPool.QueueUserWorkItem(Do,evt);
        //}
        
        //#region --> Méthodes statiques
        //public static void Poster(Evenement evt)
        //{
        //    ThreadPool.QueueUserWorkItem(Do,evt);
        //}

        //private static void Do(object o)
        //{
        //    Evenement evt = o as Evenement;
        //    if (evt==null)
        //        return;
        //    Kernel.Receive(evt);

        //    if (evt.Portee == 0)
        //        evt.Emetteur.Recevoir(evt);

        //    else
        //    {
        //        List<Agent> lst = null;
        //        lock (Kernel.PagesBlanches)
        //        {
        //            lst = Kernel.PagesBlanches.Agents(null, -1 /*(evt.Emetteur!=null?evt.Emetteur.Coord:null, evt.Portee)*/).Where(
        //                a => evt.Portee == -1 || evt.Emetteur.Coord.getDistance(a.Coord) < evt.Portee && evt.Emetteur != a).ToList();
        //        }

        //        lst.ForEach(
        //            agtConcerne => agtConcerne.Do(NomAction.Ecouter, evt.Emetteur, evt)
        //            );
        //    }
        //}

        //#endregion --> Méthodes statiques
    //}

    public class ManagerEvenements
    {
        readonly object _locker = new object();
        Thread[] _workers;
        Queue<Evenement> _itemQ = new Queue<Evenement>();

        public ManagerEvenements(int workerCount )
        {
            _workers = new Thread[workerCount];

            // Create and start a separate thread for each worker
            for (int i = 0; i < workerCount; i++)
                (_workers[i] = new Thread(Consume)).Start();
        }

        public void Shutdown(bool waitForWorkers)
        {
            // Enqueue one null item per worker to make each exit.
            foreach (Thread worker in _workers)
                Poster(null);

            // Wait for workers to finish
            if (waitForWorkers)
                foreach (Thread worker in _workers)
                    worker.Join();
        }

        public void Poster(Evenement item)
        {
            lock (_locker)
            {
                _itemQ.Enqueue(item);           // We must pulse because we're
                Monitor.Pulse(_locker);         // changing a blocking condition.
            }
        }

        public void Consume()
        {
            while (true)                        // Keep consuming until
            {                                   // told otherwise.
                Evenement item;
                lock (_locker)
                {
                    while (_itemQ.Count == 0) Monitor.Wait(_locker);
                    item = _itemQ.Dequeue();
                }
                if (item == null) return;         // This signals our exit.
                Do (item);                           // Execute item.
            }
        }


        private static void Do(Evenement evt)
        {
            Kernel.Receive(evt);

            if (evt.Portee == 0)
                evt.Emetteur.Recevoir(evt);

            else
            {
                List<Agent> lst = null;
                lock (Kernel.PagesBlanches)
                {
                    lst = Kernel.PagesBlanches
                                .Agents((evt.Emetteur!=null?evt.Emetteur.Coord:null), evt.Portee)
                                .Where(a => evt.Emetteur != a ).ToList();
                }

                lst.ForEach(
                    agtConcerne => agtConcerne.Do(NomAction.Ecouter, evt.Emetteur, evt)
                    );
            }
        }
    }

}
