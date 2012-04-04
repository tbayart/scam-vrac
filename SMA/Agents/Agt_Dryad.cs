using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using SMA.Actions;
using SMA.Capacites;
using SMA.Agents.Caracteristiques;

namespace SMA.Agents
{
    public class Dryad : Agent
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="id">L'id de l'agent.</param>
        /// <param name="coord">Les coordonnées de l'agent.</param>
        /// <param name="monde">Le monde auquel appartient l'agent.</param>
        public Dryad(string id, Coordonnees coord, Monde monde)
            : base(id, coord, monde)
        {
            // Initialisation des capacités d'une dryad.
            this.Capacites[NomAction.Teleporter] = new Cap_Teleporter_N1();
            this.Capacites[NomAction.Planter] = new Cap_Planter();
            this.Capacites[NomAction.Mourir] = new Cap_Retirer();
            this.Capacites[NomAction.Ecouter] = new Cap_Ecouter();
            this.Capacites[NomAction.Parler] = new Cap_Parler();

            // Initialisation des caractéristiques d'une dryad.
            this.Caracteristiques[NomCaracteristique.DistanceDeDeplacement] = CatalogueCaracteristiques.DistanceDeDeplacement(6);
            this.Caracteristiques[NomCaracteristique.Solitude] = CatalogueCaracteristiques.Solitude(1);
            this.Caracteristiques[NomCaracteristique.LenteurEsprit] = CatalogueCaracteristiques.LenteurEsprit(100);
        }
    }
}
