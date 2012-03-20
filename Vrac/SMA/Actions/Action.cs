using Vrac.SMA.Agents;

namespace Vrac.SMA.Actions
{
    /// <summary>
    /// La liste des Action possibles.
    /// </summary>
    public enum NomAction
    {
        Ecouter,
        Mourir,
        None,
        Parler,
        Planter,
        Teleporter,
    }

    public class Action
    {
        #region --> Delegates

        public delegate void DoIt(Agent acteur, Agent cible, object param);

        #endregion --> Delegates

        #region --> Attributs

        // La méthode appelée par l'Action.
        public DoIt doIt;

        #endregion --> Attributs
    }
}
