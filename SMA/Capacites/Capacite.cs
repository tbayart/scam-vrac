using System.Collections.Generic;
using SMA.Agents;
using SMA.Resultats;
using Tools;

namespace SMA.Capacites
{
    public class Capacite
    {
        #region --> Attributs

        // Dictionnaire des Action à effectuer pour chaque Resultat possible.
        public Dictionary<Resultat, Actions.Action> Actions;

        // La distribution des Resultat possibles.
        public Distribution<Resultat> Proba;

        #endregion --> Attributs

        #region --> Méthodes d'instance

        public Resultat doIt(IAgent acteur, IAgent cible, object param)
        {
            Resultat res = this.Proba.get();
            this.Actions[res].doIt(acteur, cible, param);

            return res;
        }

        #endregion --> Méthodes d'instance
    }
}
