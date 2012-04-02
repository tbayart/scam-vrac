using System;
using System.Threading;

namespace Vrac.Tools
{
    public static class Randomizer
    {
        #region -> Attributs statiques

        readonly static object _locker = new object();
        private static bool locked;
        private static Random S_random;

        #endregion -> Attributs statiques


        static T underLock<T>(Func<T> func)
        {

            lock (S_random)
            {
                while (locked) Monitor.Wait(_locker);
                locked = true;
                T r = func();
                locked = false;
                Monitor.Pulse(_locker);
                return r;
            }
        }

        static T underLock<P1, T>(Func<P1, T> func, P1 param)
        {

            lock (S_random)
            {
                while (locked) Monitor.Wait(_locker);
                locked = true;
                T r = func(param);
                locked = false;
                Monitor.Pulse(_locker);
                return r;
            }
        }

        static T underLock<P1, P2, T>(Func<P1, P2, T> func, P1 param1, P2 param2)
        {

            lock (S_random)
            {
                while (locked) Monitor.Wait(_locker);
                locked = true;
                T r = func(param1,param2);
                locked = false;
                Monitor.Pulse(_locker);
                return r;
            }
        }

        #region -> Publiques

        /// <summary>
        /// Retourne un nombre aléatoire non négatif.
        /// </summary>
        /// <returns>Un nombre aléatoire non négatif.</returns>
        public static int Next()
        {
            return underLock(S_random.Next);
        }

        /// <summary>
        /// Retourne un nombre aléatoire non négatif, inférieur au nombre maximal spécifié.
        /// </summary>
        /// <param name="p_maxValue">Limite supérieure (exclusive) du nom aléatoire à générer.</param>
        /// <returns>Un nombre aléatoire non négatif, inférieur au nombre maximal spécifié.</returns>
        public static int Next(int p_maxValue)
        {

            return underLock(S_random.Next, p_maxValue);
        }

        /// <summary>
        /// Retourne un nombre aléatoire figurant dans la plage spécifiée.
        /// </summary>
        /// <param name="p_minValue">Limite inférieure (incluse) du nom aléatoire à générer.</param>
        /// <param name="p_maxValue">Limite supérieure (exclusive) du nom aléatoire à générer.</param>
        /// <returns>Un nombre aléatoire figurant dans la plage spécifiée.</returns>
        public static int Next(int p_minValue, int p_maxValue)
        {

            return underLock(S_random.Next, p_minValue, p_maxValue);
        }

        /// <summary>
        /// Retourne un nombre aléatoire compris entre 0,0 et 1,0.
        /// </summary>
        /// <returns>Un nombre aléatoire compris entre 0,0 et 1,0.</returns>
        public static double NextDouble()
        {
            
            return underLock(S_random.NextDouble);
        }

        #endregion -> Publiques


        #region -> Constructeurs statique

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static Randomizer()
        {
            S_random = new Random();
        }



        #endregion -> Constructeurs statique

    }
}
