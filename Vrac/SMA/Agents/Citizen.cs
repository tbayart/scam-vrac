using System.Collections.Generic;
using Vrac.SMA.Actions;
using Vrac.SMA.Capacites;
using Vrac.SMA.Caracteristiques;
using Vrac.SMA.Comportements;

namespace Vrac.SMA.Agents
{
    public class Citizen : Agent
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Citizen()
        {
            this.Chaine = CatalogueComportements.CitizenBuildRoad;

            this.Capacites = new Dictionary<NomAction, Capacite>();
            this.Capacites[NomAction.Teleporter] = new Cap_Teleporter_N2();
            this.Capacites[NomAction.Mourir] = new Cap_Retirer();
            this.Capacites[NomAction.Ecouter] = new Cap_Ecouter();
            this.Capacites[NomAction.Parler] = new Cap_Parler();
            this.Capacites[NomAction.Construire] = new Cap_Construire();
            this.Capacites[NomAction.ConstruireRoute] = new Cap_ConstruireRoute();

            caracteristiques = new Dictionary<LesCaracteristiques, Caracteristique>();

            caracteristiques[LesCaracteristiques.DistanceDeDeplacement] = CatalogueCaracteristique.DistanceDeDeplacement(1);
            caracteristiques[LesCaracteristiques.Solitude] = CatalogueCaracteristique.Solitude(1);
            caracteristiques[LesCaracteristiques.LenteurEsprit] = CatalogueCaracteristique.LenteurEsprit(1);
            
        }

        #endregion --> Constructeurs
    }
}
