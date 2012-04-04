using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScamCarte.Cartes
{
    public class ElementCarte
    {
        #region -> Attributs

        // Le type d'élément biome représenté.
        public TypeElementBiome ElementBiome;

        // L'altitude de l'élément.
        public int Altitude;

        #endregion -> Attributs

        #region -> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        private ElementCarte()
        {
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="elementBiome">Le type d'élément biome représenté.</param>
        public ElementCarte(TypeElementBiome elementBiome)
        {
            this.ElementBiome = elementBiome;
            this.Altitude = 0;
        }

        #endregion -> Constructeurs

        #region -> Gestion sauvegarde

        /// <summary>
        /// Retourne une string représentant l'instance de l'ElementCarte.
        /// </summary>
        /// <returns>Une chaine représentant l'instance de l'ElementCarte.</returns>
        public string getDonneeSauvegarde()
        {
            return String.Format("{0}:{1}", (int)this.ElementBiome, this.Altitude);
        }

        /// <summary>
        /// Instancie un ElementCarte à partir d'un tableau de strings.
        /// </summary>
        /// <param name="donnees">Un tableau de strings représentant les valeurs des attributs du ElementCarte.</param>
        /// <returns>Un ElementCarte.</returns>
        public static ElementCarte ChargerDonneesSauvegarde(string[] donnees)
        {
            ElementCarte e = new ElementCarte();

            e.ElementBiome = (TypeElementBiome)int.Parse(donnees[0]);
            e.Altitude = int.Parse(donnees[1]);

            return e;
        }

        #endregion -> Gestion sauvegarde
    }
}
