using Tools;
using ScamCarte.Cartes;

namespace SMA.Actions
{
    public partial class CatalogueActions
    {
        public static Action Planter = new Action()
        {
            doIt = (acteur, cible, coord) =>
            {
                Coordonnees c = ((Coordonnees)coord);
                acteur.World.Map.Elements[c.X][c.Y].ElementBiome = TypeElementBiome.Arbre;
            }
        };
    }
}
