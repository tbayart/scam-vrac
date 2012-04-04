using System;
using System.Collections.Generic;
using ScamCarte.Cartes;
using SMA.Actions;
using SMA.Agents;
using SMA.Agents.Caracteristiques;
using SMA.Messages;
using SMA.Objectifs;
using SMA.Resultats;
using Tools;

namespace SMA.Comportements
{
    public partial class CatalogueComportements
    {
        private static Comportement DeplacerDryad = new Comportement()
            {
                IsHandler = (msg, agt) => true,
                IsForwarder = (msg, agt) => false,
                Next = null,

                Handle = (msg, agt) =>
                {
                    if (agt.ConnaissancesObjectifs[TypeObjectif.Dryad].ContainsKey("Destination"))
                    {
                        Coordonnees dest = (Coordonnees)agt.ConnaissancesObjectifs[TypeObjectif.Dryad]["Destination"];

                        if (dest.X == agt.Coord.X && dest.Y == agt.Coord.Y)
                        {
                            agt.ConnaissancesObjectifs[TypeObjectif.Dryad].Remove("Destination");
                        }
                        else
                        {
                            int deplacement = agt.Caracteristiques[NomCaracteristique.DistanceDeDeplacement].Valeur;
                            int dist = dest.getDistance(agt.Coord);

                            if (dist < deplacement)
                            {
                                agt.Coord.X = dest.X;
                                agt.Coord.Y = dest.Y;
                            }
                            else
                            {
                                int deltaX = (deplacement * (dest.X - agt.Coord.X)) / dist;
                                int deltaY = (deplacement * (dest.Y - agt.Coord.Y)) / dist;
                                
                                agt.Coord.X += deltaX;
                                agt.Coord.Y += deltaY;
                            }
                        }

                        agt.World.poster(CatalogueMessages.GetMessageRealiserObjectif(TypeObjectif.Dryad, agt));
                    }
                    else
                    {
                        Coordonnees c = agt.World.Map.getPostionElement(TypeElementBiome.Terre, agt.Coord);

                        if (c != null)
                        {
                            agt.ConnaissancesObjectifs[TypeObjectif.Dryad].Add("Destination", c);

                            agt.World.poster(CatalogueMessages.GetMessageRealiserObjectif(TypeObjectif.Dryad, agt));
                        }
                    }
                }
            };

        public static Comportement Comportement_Dryad = new Comportement()
            {
                IsHandler = (msg, agt) => !agt.ConnaissancesObjectifs[TypeObjectif.Dryad].ContainsKey("Destination") && agt.World.Map.Elements[agt.Coord.X][agt.Coord.Y].ElementBiome == TypeElementBiome.Terre,
                IsForwarder = (msg, agt) => false,
                Next = DeplacerDryad,

                Handle = (msg, agt) =>
                {
                    List<Agent> dryadsProches = agt.World.Agents.PagesBlanches(typeof(Dryad), agt.Coord, 4);

                    if (dryadsProches.Count > 0)
                    {
                        Resultat res = agt.FaireAction(NomAction.Planter, agt, new Coordonnees(agt.Coord.X, agt.Coord.Y));

                        if (res != Resultat.Succes)
                            agt.World.poster(CatalogueMessages.GetMessageRealiserObjectif(TypeObjectif.Dryad, agt));

                        Resultat resMort = agt.FaireAction(NomAction.Mourir, agt, null);

                        if (resMort != Resultat.Succes)
                            agt.World.poster(CatalogueMessages.GetMessageRealiserObjectif(TypeObjectif.Dryad, agt));
                        else
                        {
                            //agt.Objectifs.Remove(TypeObjectif.Dryad);
                            //agt.ConnaissancesObjectifs.Remove(TypeObjectif.Dryad);
                        }
                    }
                    else
                    {
                        List<Agent> lst = agt.World.Agents.PagesBlanches(typeof(Dryad));

                        Coordonnees c = lst[Randomizer.Next(lst.Count)].Coord;

                        if (c != null)
                        {
                            agt.ConnaissancesObjectifs[TypeObjectif.Dryad].Add("Destination", c);

                            agt.World.poster(CatalogueMessages.GetMessageRealiserObjectif(TypeObjectif.Dryad, agt));
                        }

                        /*
                        int dist = agt.Caracteristiques[NomCaracteristique.DistanceDeDeplacement].Valeur;

                        Coordonnees newCoord = new Coordonnees(agt.Coord.X + Randomizer.Next(-dist, dist + 1),
                                                               agt.Coord.Y + Randomizer.Next(-dist, dist + 1));

                        newCoord.X = Math.Max(0, newCoord.X);
                        newCoord.X = Math.Min(agt.World.Map.Largeur - 1, newCoord.X);
                        
                        newCoord.Y = Math.Max(0, newCoord.Y);
                        newCoord.Y = Math.Min(agt.World.Map.Hauteur - 1, newCoord.Y);

                        agt.Coord.X = newCoord.X;
                        agt.Coord.Y = newCoord.Y;

                        agt.World.poster(CatalogueMessages.GetMessageRealiserObjectif(TypeObjectif.Dryad, agt));
                        */
                    }
                }
            };
    }
}
