using System.Collections;
using Vrac.SMA.Agents;

namespace Vrac.SMA.Evenements
{
    public class Evt_Mort : Evenement
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="emetteur">L'emetteur de l'Evenement.</param>
        public Evt_Mort(Agent emetteur)
            : base(emetteur, new ArrayList())
        {
        }

        #endregion --> Constructeurs
    }
}
