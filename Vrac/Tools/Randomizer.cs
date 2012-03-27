using System;

namespace Vrac.Tools
{
    public static class Randomizer
    {
        #region -> Attributs statiques

        public static Random S_random;

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

    }
}
