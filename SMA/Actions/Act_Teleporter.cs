using Tools;

namespace SMA.Actions
{
    public partial class CatalogueActions
    {
        public static Action Teleporter = new Action()
        {
            doIt = (acteur, cible, coord) =>
            {
                Coordonnees c = ((Coordonnees)coord);
                cible.Coord.X = c.X;
                cible.Coord.Y = c.Y;
            }
        };
    }
}
