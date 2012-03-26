using System;
using System.Collections.Generic;

namespace Vrac.Tools
{
    public class Distribution<T>
    {
        #region -> Attributs

        // Le dictionnaire des seuils.
        public Dictionary<T, double> DicoSeuils { get; set; }

        #endregion -> Attributs

        #region -> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Distribution()
        {
            this.DicoSeuils = new Dictionary<T, double>();
        }

        #endregion -> Constructeurs

        #region -> Méthodes d'instance

        public T get()
        {
            double d = Randomizer.NextDouble();
            double sommeSeuils = 0;

            foreach (var info in this.DicoSeuils)
            {
                sommeSeuils += info.Value;

                if (d < sommeSeuils)
                    return info.Key;
            }

            throw new Exception("Erreur !");
        }

        #endregion -> Méthodes d'instance
    }
}
