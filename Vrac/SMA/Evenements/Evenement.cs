using System.Collections;
using Vrac.SMA.Agents;

namespace Vrac.SMA.Evenements
{
    public class Evenement
    {
        public static Evenement FirstTurn;

        #region --> Attributs

        // La distance maximale à laquelle l'Evenement est diffusé.
        public double Portee { get; protected set; }

        // L'emetteur de l'Evenement.
        public Agent Emetteur { get; protected set; }

        // Les paramètres de l'Evenement.
        public ArrayList Parametres { get; protected set; }

        #endregion --> Attributs

        #region --> Constructeurs

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static Evenement()
        {
            // Initialisation des Evenement statiques.
            FirstTurn = new Evenement(null, null, -1);
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="emetteur">L'emetteur de l'Evenement.</param>
        /// <param name="param">Les paramètres de l'Evenement.</param>
        public Evenement(Agent emetteur, ArrayList param)
            :this(emetteur, param, 0)
        {
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="emetteur">L'emetteur de l'Evenement.</param>
        /// <param name="param">Les paramètres de l'Evenement.</param>
        /// <param name="portee">La distance maximale à laquelle l'Evenement est diffusé.</param>
        public Evenement(Agent emetteur, ArrayList param, double portee)
        {
            this.Emetteur = emetteur;
            this.Parametres = param;
            this.Portee = portee;
        }

        #endregion --> Constructeurs
    }
}
