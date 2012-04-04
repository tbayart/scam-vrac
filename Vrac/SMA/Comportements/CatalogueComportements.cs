using System;
using ScamCarte.Cartes;
using Tools;
using Vrac.SMA.Actions;
using Vrac.SMA.Caracteristiques;
using Vrac.SMA.Evenements;
using Vrac.SMA.Resultats;

namespace Vrac.SMA.Comportements
{
    public class CatalogueComportements
    {
        public static Comportement CitizenTurn;

        public static Comportement CitizenBuildRoad;

        public static Comportement DryadTurn;

        public static Comportement NearTo;

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
                    //ThreadPool.QueueUserWorkItem(HighPerfTimer.timePoint,agent.id.ToString());
                    if (agent.caracteristiques[LesCaracteristiques.Solitude].valeur > 20)
                        agent.caracteristiques[LesCaracteristiques.DistanceDeDeplacement].valeur = 8;

                    int dist = agent.caracteristiques[LesCaracteristiques.DistanceDeDeplacement].valeur;

                    if (Kernel.CarteManipulee.Elements[agent.Coord.X][agent.Coord.Y].ElementBiome == TypeElementBiome.Eau
                        || Kernel.CarteManipulee.Elements[agent.Coord.X][agent.Coord.Y].ElementBiome == TypeElementBiome.Pierre)
                        dist *= 4;


                    //Coordonnees oldCoord = new Coordonnees(agent.Coord.X, agent.Coord.Y); 
                    Coordonnees newCoord = 
                        new Coordonnees(
                            agent.Coord.X + Randomizer.Next(-dist, dist + 1), 
                            agent.Coord.Y + Randomizer.Next(-dist, dist + 1)
                            );


                    newCoord.X = Math.Max(0, newCoord.X);
                    newCoord.X = Math.Min(Kernel.CarteManipulee.Largeur-1, newCoord.X);
                    newCoord.Y = Math.Max(0, newCoord.Y);
                    newCoord.Y = Math.Min(Kernel.CarteManipulee.Hauteur-1, newCoord.Y);
                    
                    Resultat res = agent.Do(NomAction.Teleporter, agent, newCoord);
                    
                    if (res == Resultat.Succes)
                        agent.Do(NomAction.Parler, agent, new Evt_Deplace(agent, agent.Coord.X, agent.Coord.Y));

                    agent.caracteristiques[LesCaracteristiques.Solitude].valeur++;

                    //Thread.Sleep(agent.caracteristiques[LesCaracteristiques.LenteurEsprit].valeur);
                    agent.Envoyer(new Evenement(agent, null, 0));
                }
            };

            NearTo = new Comportement()
            {
                IsForwarder = (e, agent) => false,
                IsHandler = (e, agent) => e is Evt_Deplace,
                Handle = (e, agent) =>
                {
                    if (Kernel.CarteManipulee.Elements[agent.Coord.X][agent.Coord.Y].ElementBiome == TypeElementBiome.Eau
                        || Kernel.CarteManipulee.Elements[agent.Coord.X][agent.Coord.Y].ElementBiome == TypeElementBiome.Pierre)
                        return;

                    agent.caracteristiques[LesCaracteristiques.DistanceDeDeplacement].valeur = 1;
                    agent.caracteristiques[LesCaracteristiques.Solitude].valeur = 3;
                    Resultat res = agent.Do(NomAction.Planter, agent, new Coordonnees(agent.Coord.X, agent.Coord.Y));

                    //Thread.Sleep(agent.caracteristiques[LesCaracteristiques.LenteurEsprit].valeur);
                    //agent.Envoyer(new Evenement(agent, null, 0));

                    if (res != Resultat.Succes)
                        return;

                    Resultat resMort = agent.Do(NomAction.Mourir, agent, null);

                    if (resMort == Resultat.Succes)
                        agent.Do(NomAction.Parler, agent, new Evt_Mort(agent));
                }
            };

            DryadTurn.Next = NearTo;

            
            // Initialisation des Comportement statiques.
            CitizenTurn = new Comportement()
                              {
                                  IsForwarder = (e, agent) => false,
                                  IsHandler = (e, agent) => e == Evenement.FirstTurn || e.Portee == 0,
                                  Handle = (e, agent) =>
                                               {
                                                   int dist = agent.caracteristiques[LesCaracteristiques.DistanceDeDeplacement].valeur;

                                                   //Coordonnees oldCoord = new Coordonnees(agent.Coord.X, agent.Coord.Y); 
                                                   Coordonnees newCoord = new Coordonnees(agent.Coord.X + Randomizer.Next(-dist, dist + 1), agent.Coord.Y + Randomizer.Next(-dist, dist + 1));

                                                   newCoord.X = Math.Max(0, newCoord.X);
                                                   newCoord.X = Math.Min(Kernel.CarteManipulee.Largeur - 1, newCoord.X);
                                                   newCoord.Y = Math.Max(0, newCoord.Y);
                                                   newCoord.Y = Math.Min(Kernel.CarteManipulee.Hauteur - 1, newCoord.Y);

                                                   Resultat res = agent.Do(NomAction.Teleporter, agent, newCoord);

                                                   if (res == Resultat.Succes)
                                                       agent.Do(NomAction.Construire, agent, agent.Coord);

                                                   // MaisonAutour ? 
                                                   agent.caracteristiques[LesCaracteristiques.Maison_X] = CatalogueCaracteristique.Maison_X(Kernel.CarteManipulee.Largeur / 2);
                                                   agent.caracteristiques[LesCaracteristiques.Maison_Y] = CatalogueCaracteristique.Maison_Y(Kernel.CarteManipulee.Hauteur / 2);

                                                   //Thread.Sleep(agent.caracteristiques[LesCaracteristiques.LenteurEsprit].valeur);
                                                   agent.Envoyer(new Evenement(agent, null, 0));
                                               }
                              };

            CitizenBuildRoad = new Comportement()
            {
                IsForwarder = (e, agent) => false,
                IsHandler = (e, agent) =>
                                {
                                    Caracteristique Maison_X=null;
                                    Caracteristique Maison_Y=null;
                                    agent.caracteristiques.TryGetValue(LesCaracteristiques.Maison_X, out Maison_X);
                                    agent.caracteristiques.TryGetValue(LesCaracteristiques.Maison_Y, out Maison_Y);
                                    return (e == Evenement.FirstTurn || e.Portee == 0)
                                           && (Maison_X!=null && Maison_X.valeur >= 0)
                                           && (Maison_Y!=null && Maison_Y.valeur >= 0);
                                },
                Handle = (e, agent) =>
                             {
                                 Resultat res = agent.Do(NomAction.ConstruireRoute, agent, agent.Coord);
                                 if (res == Resultat.Succes)
                                 {
                                     int offset_x = agent.caracteristiques[LesCaracteristiques.Maison_X].valeur - agent.Coord.X;
                                     int offset_y = agent.caracteristiques[LesCaracteristiques.Maison_Y].valeur - agent.Coord.Y;

                                     agent.Do(NomAction.Teleporter, agent, new Coordonnees(agent.Coord.X + (offset_x == 0 ? offset_x : (offset_x / Math.Abs(offset_x))), agent.Coord.Y + (offset_y == 0 ? offset_y : (offset_y / Math.Abs(offset_y)))));
                                 }

                                //Thread.Sleep(agent.caracteristiques[LesCaracteristiques.LenteurEsprit].valeur);
                                agent.Envoyer(new Evenement(agent, null, 0));
                             }
            };

            CitizenBuildRoad.Next = CitizenTurn;
        }

        #endregion --> Constructeurs
    }
}
