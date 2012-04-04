using System.Collections.Generic;
using Tools;
using Vrac.SMA.Agents;
using Vrac.SMA.Resultats;

namespace Vrac.SMA.Capacites
{
    ///// <summary>
    ///// La liste des Capacite existantes.
    ///// </summary>
    //public enum NomCapacite
    //{
    //    Ecouter,
    //    Parler,
    //    Planter,
    //    Retirer,
    //    Teleporter_N1,
    //    Teleporter_N2
    //}

    public class Capacite
    {
        #region --> Attributs

        // Dictionnaire des Action à effectuer pour chaque Resultat possible.
        public Dictionary<Resultat, Actions.Action> Actions;

        // La distribution des Resultat possibles.
        public Distribution<Resultat> Proba;

        #endregion --> Attributs

        #region --> Méthodes d'instance

        public Resultat doIt(Agent acteur, Agent cible, object param)
        {
            Resultat res = this.Proba.get();
            this.Actions[res]
                .doIt(acteur, cible, param);

            return res;
        }

        #endregion --> Méthodes d'instance
    }
}
