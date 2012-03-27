using System.Collections.Generic;
using Vrac.SMA.Actions;
using Vrac.SMA.Capacites;
using Vrac.SMA.Caracteristiques;
using Vrac.SMA.Comportements;

namespace Vrac.SMA.Agents
{
    public class Dryad : Agent
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Dryad()
        {
            this.Chaine = CatalogueComportements.DryadTurn;

            this.Capacites = new Dictionary<NomAction, Capacite>();
            this.Capacites[NomAction.Teleporter] = new Cap_Teleporter_N1();
            this.Capacites[NomAction.Planter] = new Cap_Planter();
            this.Capacites[NomAction.Mourir] = new Cap_Retirer();
            this.Capacites[NomAction.Ecouter] = new Cap_Ecouter();
            this.Capacites[NomAction.Parler] = new Cap_Parler();

            caracteristiques = new Dictionary<LesCaracteristiques, Caracteristique>();

            caracteristiques[LesCaracteristiques.DistanceDeDeplacement] = CatalogueCaracteristique.DistanceDeDeplacement(6);
            caracteristiques[LesCaracteristiques.Solitude] = CatalogueCaracteristique.Solitude(1);
            caracteristiques[LesCaracteristiques.LenteurEsprit] = CatalogueCaracteristique.LenteurEsprit(1);
        }

        #endregion --> Constructeurs
    }
}
