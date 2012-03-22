using System.Collections;
using Vrac.SMA.Agents;

namespace Vrac.SMA.Evenements
{
    public class Evt_Deplace : Evenement
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="emetteur">L'emetteur de l'Evenement.</param>
        /// <param name="x">La coordonnée x du déplacement.</param>
        /// <param name="y">La coordonnée y du déplacement.</param>
        public Evt_Deplace(Agent emetteur, int x, int y)
            : base(emetteur, new ArrayList() { x, y })
        {
		    this.Portee = 3;
        }

        #endregion --> Constructeurs
    }
}
