using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace SMA.Actions
{
    public partial class CatalogueActions
    {
        public static Action ConstruireRoute = new Action()
        {
            doIt = (acteur, cible, coord) =>
            {
                Coordonnees c = ((Coordonnees)coord);
                try
                {
                    //Kernel.CarteManipulee.Elements[c.X][c.Y].ElementBiome = TypeElementBiome.Route;

                }
                catch (Exception ex)
                {
                }
            }
        };
    }
}
