using SMA.Agents;

namespace SMA.Actions
{
    public class Action
    {
        #region --> Delegates

        public delegate void DoIt(IAgent acteur, IAgent cible, object param);

        #endregion --> Delegates

        #region --> Attributs

        // La méthode appelée par l'Action.
        public DoIt doIt;

        #endregion --> Attributs
    }
}
