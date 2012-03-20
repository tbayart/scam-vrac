using System;
using Vrac.SMA.Actions;
using Vrac.SMA.Evenements;
using Vrac.SMA.Resultats;
using Vrac.Tools;

namespace Vrac.SMA.Comportements
{
    public class CatalogueComportements
    {
        public static Comportement DryadTurn;

        private static Comportement NearTo;

        #region --> Constructeurs

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static CatalogueComportements()
        {
            // Initialisation des Comportement statiques.
            DryadTurn = new Comportement()
            {
                IsForwarder = (e, agent) => false,
                IsHandler = (e, agent) => e == Evenement.NewTurn,
                Handle = (e, agent) =>
                {
                    Coordonnees newCoord = new Coordonnees(agent.Coord.X + Randomizer.Next(-3, 3), agent.Coord.Y + Randomizer.Next(-3, 3));

                    newCoord.X = Math.Max(0, newCoord.X);
                    newCoord.X = Math.Min(Kernel.CarteManipulee._carte.Length-1, newCoord.X);
                    newCoord.Y = Math.Max(0, newCoord.Y);
                    newCoord.Y = Math.Min(Kernel.CarteManipulee._carte[0].Length-1, newCoord.Y);
                    
                    Resultat res = agent.Do(NomAction.Teleporter, agent, newCoord);
                    
                    if (res == Resultat.Succes)
                    {
                        agent.Do(NomAction.Parler, agent, new Evt_Deplace(agent, agent.Coord.X, agent.Coord.Y));
                    }
                }
            };

            NearTo = new Comportement()
            {
                IsForwarder = (e, agent) => false,
                IsHandler = (e, agent) => e is Evt_Deplace,
                Handle = (e, agent) =>
                {
                    Resultat res = agent.Do(NomAction.Planter, agent, new Coordonnees(agent.Coord.X, agent.Coord.Y));

                    if (res == Resultat.Succes)
                    {
                        Resultat resMort = agent.Do(NomAction.Mourir, agent, null);

                        if (resMort == Resultat.Succes)
                            agent.Do(NomAction.Parler, agent, new Evt_Mort(agent));
                    }
                }
            };

            DryadTurn.Next = NearTo;
        }

        #endregion --> Constructeurs
    }
}
