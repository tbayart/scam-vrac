using System.Threading;
using ScamCarte.Cartes;
using SMA.Messages;
using SMA.Objectifs;
using Tools;

namespace SMA.Comportements
{
    public partial class CatalogueComportements
    {
        public static Comportement Comportement_AtteindreObjectifTest = new Comportement()
            {
                IsHandler = (msg, agt) => { return true; },
                IsForwarder = (msg, agt) => { return false; },
                Next = null,

                Handle = (msg, agt) =>
                {
                    if (agt.World.Map.Elements[agt.Coord.X][agt.Coord.Y].ElementBiome == ScamCarte.Cartes.TypeElementBiome.Terre)
                    {
                        agt.Objectifs.Remove(TypeObjectif.RejoindreTerre);
                        agt.ConnaissancesObjectifs.Remove(TypeObjectif.RejoindreTerre);
                    }
                    else if (agt.ConnaissancesObjectifs[TypeObjectif.RejoindreTerre].ContainsKey("Destination"))
                    {
                        Coordonnees dest = (Coordonnees)agt.ConnaissancesObjectifs[TypeObjectif.RejoindreTerre]["Destination"];

                        if (dest.X == agt.Coord.X && dest.Y == agt.Coord.Y)
                        {
                            agt.ConnaissancesObjectifs[TypeObjectif.RejoindreTerre].Remove("Destination");
                        }
                        else
                        {
                            int deltaX = (dest.X - agt.Coord.X);
                            int deltaY = (dest.Y - agt.Coord.Y);
                            int distance = dest.getDistance(agt.Coord) / 2;

                            if (distance > 0)
                            {
                                deltaX /= distance;
                                deltaY /= distance;
                            }
                            
                            agt.Coord.X += deltaX;
                            agt.Coord.Y += deltaY;

                            // POUR DEBUG
                            Thread.Sleep(5);
                        }

                        agt.World.poster(CatalogueMessages.GetMessageRealiserObjectif(TypeObjectif.RejoindreTerre, agt));
                    }
                    else
                    {
                        Coordonnees c = agt.World.Map.getPostionElement(TypeElementBiome.Terre, agt.Coord);

                        if (c != null)
                        {
                            agt.ConnaissancesObjectifs[TypeObjectif.RejoindreTerre].Add("Destination", c);

                            //((Agent)agt).setPosition(c);

                            agt.World.poster(CatalogueMessages.GetMessageRealiserObjectif(TypeObjectif.RejoindreTerre, agt));
                        }
                    }
                }
            };
    }
}
