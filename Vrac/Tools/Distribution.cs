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

            foreach (T cle in this.DicoSeuils.Keys)
            {
                sommeSeuils += this.DicoSeuils[cle];

                if (d < sommeSeuils)
                    return cle;
            }

            throw new Exception("Erreur !");
        }

        #endregion -> Méthodes d'instance
    }
}
