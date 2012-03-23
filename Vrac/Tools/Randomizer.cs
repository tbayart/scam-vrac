using System;

namespace Vrac.Tools
{
    public static class Randomizer
    {
        #region -> Attributs statiques

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

        #region -> Publiques

        /// <summary>
        /// Retourne un nombre aléatoire non négatif.
        /// </summary>
        /// <returns>Un nombre aléatoire non négatif.</returns>
        public static int Next()
        {
            return S_random.Next();
        }

        /// <summary>
        /// Retourne un nombre aléatoire non négatif, inférieur au nombre maximal spécifié.
        /// </summary>
        /// <param name="p_maxValue">Limite supérieure (exclusive) du nom aléatoire à générer.</param>
        /// <returns>Un nombre aléatoire non négatif, inférieur au nombre maximal spécifié.</returns>
        public static int Next(int p_maxValue)
        {
            return S_random.Next(p_maxValue);
        }

        /// <summary>
        /// Retourne un nombre aléatoire figurant dans la plage spécifiée.
        /// </summary>
        /// <param name="p_minValue">Limite inférieure (incluse) du nom aléatoire à générer.</param>
        /// <param name="p_maxValue">Limite supérieure (exclusive) du nom aléatoire à générer.</param>
        /// <returns>Un nombre aléatoire figurant dans la plage spécifiée.</returns>
        public static int Next(int p_minValue, int p_maxValue)
        {
            return S_random.Next(p_minValue, p_maxValue);
        }

        /// <summary>
        /// Retourne un nombre aléatoire compris entre 0,0 et 1,0.
        /// </summary>
        /// <returns>Un nombre aléatoire compris entre 0,0 et 1,0.</returns>
        public static double NextDouble()
        {
            return S_random.NextDouble();
        }

        #endregion -> Publiques

        #endregion -> Méthodes statiques
    }
}
