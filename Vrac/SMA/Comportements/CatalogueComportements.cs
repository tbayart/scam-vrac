using System;
using System.IO;
using System.Threading;
using Vrac.GenerateurCarte;
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
                IsHandler = (e, agent) => e == Evenement.FirstTurn || e.Portee==0,
                Handle = (e, agent) =>
                {
                    ThreadPool.QueueUserWorkItem(HighPerfTimer.timePoint,agent.id.ToString());
                    if (agent.caracteristiques[LesCaracteristiques.Solitude].valeur > 20)
                        agent.caracteristiques[LesCaracteristiques.DistanceDeDeplacement].valeur = 6;

                    if (Kernel.CarteManipulee._carte[agent.Coord.X][agent.Coord.Y] == TypeElementBiome.Eau || Kernel.CarteManipulee._carte[agent.Coord.X][agent.Coord.Y] == TypeElementBiome.Pierre)
                        agent.caracteristiques[LesCaracteristiques.DistanceDeDeplacement].valeur = 10;

                    int dist = agent.caracteristiques[LesCaracteristiques.DistanceDeDeplacement].valeur;

                    Coordonnees oldCoord = new Coordonnees(agent.Coord.X, agent.Coord.Y); 
                    Coordonnees newCoord = new Coordonnees(agent.Coord.X + Randomizer.Next(-dist, dist+1), agent.Coord.Y + Randomizer.Next(-dist, dist+1));


                    newCoord.X = Math.Max(0, newCoord.X);
                    newCoord.X = Math.Min(Kernel.CarteManipulee._carte.Length-1, newCoord.X);
                    newCoord.Y = Math.Max(0, newCoord.Y);
                    newCoord.Y = Math.Min(Kernel.CarteManipulee._carte[0].Length-1, newCoord.Y);
                    
                    Resultat res = agent.Do(NomAction.Teleporter, agent, newCoord);
                    
                    if (res == Resultat.Succes)
                    {
                        //lock (semaphore.sema)
                        //{
                        //    File.AppendAllText("C:/Trace.txt", agent.id + "(" + oldCoord + ") move by " + dist + " to " + newCoord + Environment.NewLine);
                        //}

                        agent.Do(NomAction.Parler, agent, new Evt_Deplace(agent, agent.Coord.X, agent.Coord.Y));
                    }

                    agent.caracteristiques[LesCaracteristiques.Solitude].valeur++;

                    Thread.Sleep(agent.caracteristiques[LesCaracteristiques.LenteurEsprit].valeur);
                    agent.Envoyer(new Evenement(agent, null, 0));
                }
            };

            NearTo = new Comportement()
            {
                IsForwarder = (e, agent) => false,
                IsHandler = (e, agent) => e is Evt_Deplace,
                Handle = (e, agent) =>
                {
                    if (Kernel.CarteManipulee._carte[agent.Coord.X][agent.Coord.Y] == TypeElementBiome.Eau || Kernel.CarteManipulee._carte[agent.Coord.X][agent.Coord.Y] == TypeElementBiome.Pierre)
                        return;

                    agent.caracteristiques[LesCaracteristiques.DistanceDeDeplacement].valeur = 1;
                    agent.caracteristiques[LesCaracteristiques.Solitude].valeur /= 10;
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
