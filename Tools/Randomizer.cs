using System;
using System.Threading;

namespace Tools
{
    public static class Randomizer
    {
        #region -> Attributs statiques

        private static readonly object S_locker = new object();
        private static bool S_locked;

        private static Random S_random;

        #endregion -> Attributs statiques

        #region -> Constructeurs statique

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static Randomizer()
        {
            S_random = new Random();
        }


        #endregion -> Constructeurs statique

        #region -> Méthodes statiques

        #region -> Privées

        private static T UnderLock<T>(Func<T> func)
        {
            lock (S_locker)
            {
                while (S_locked) 
                    Monitor.Wait(S_locker);

                S_locked = true;
                T r = func();
                S_locked = false;
                
                Monitor.Pulse(S_locker);
                
                return r;
            }
        }

        private static T UnderLock<P1, T>(Func<P1, T> func, P1 param)
        {
            lock (S_locker)
            {
                while (S_locked) 
                    Monitor.Wait(S_locker);
                
                S_locked = true;
                T r = func(param);
                S_locked = false;
                
                Monitor.Pulse(S_locker);
                
                return r;
            }
        }

        private static T UnderLock<P1, P2, T>(Func<P1, P2, T> func, P1 param1, P2 param2)
        {
            lock (S_locker)
            {
                while (S_locked) 
                    Monitor.Wait(S_locker);
                
                S_locked = true;
                T r = func(param1, param2);
                S_locked = false;
                
                Monitor.Pulse(S_locker);
                
                return r;
            }
        }

        #endregion -> Privées

        #region -> Publiques

        /// <summary>
        /// Retourne un nombre aléatoire non négatif.
        /// </summary>
        /// <returns>Un nombre aléatoire non négatif.</returns>
        public static int Next()
        {
            //return S_random.Next();
            return UnderLock(S_random.Next);
        }

        /// <summary>
        /// Retourne un nombre aléatoire non négatif, inférieur au nombre maximal spécifié.
        /// </summary>
        /// <param name="p_maxValue">Limite supérieure (exclusive) du nom aléatoire à générer.</param>
        /// <returns>Un nombre aléatoire non négatif, inférieur au nombre maximal spécifié.</returns>
        public static int Next(int p_maxValue)
        {
            //return S_random.Next(p_maxValue);
            return UnderLock(S_random.Next, p_maxValue);
        }

        /// <summary>
        /// Retourne un nombre aléatoire figurant dans la plage spécifiée.
        /// </summary>
        /// <param name="p_minValue">Limite inférieure (incluse) du nom aléatoire à générer.</param>
        /// <param name="p_maxValue">Limite supérieure (exclusive) du nom aléatoire à générer.</param>
        /// <returns>Un nombre aléatoire figurant dans la plage spécifiée.</returns>
        public static int Next(int p_minValue, int p_maxValue)
        {
            //return S_random.Next(p_minValue, p_maxValue);
            return UnderLock(S_random.Next, p_minValue, p_maxValue);
        }

        /// <summary>
        /// Retourne un nombre aléatoire compris entre 0,0 et 1,0.
        /// </summary>
        /// <returns>Un nombre aléatoire compris entre 0,0 et 1,0.</returns>
        public static double NextDouble()
        {
            //return S_random.NextDouble();
            return UnderLock(S_random.NextDouble);
        }

        #endregion -> Publiques

        #endregion -> Méthodes statiques
    }
}
