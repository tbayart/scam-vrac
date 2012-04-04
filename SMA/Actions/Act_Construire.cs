using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace SMA.Actions
{
    public partial class CatalogueActions
    {
        public static Action Construire = new Action()
        {
            doIt = (acteur, cible, coord) =>
            {
                Coordonnees c = ((Coordonnees)coord);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //if (c.X + i > 0
                        //    && c.Y + j > 0
                        //    && c.X + i < Kernel.CarteManipulee.Largeur
                        //    && c.Y + j < Kernel.CarteManipulee.Hauteur
                        //    && (i != 0 && j != 0)
                        //    )
                        //    Kernel.CarteManipulee.Elements[c.X + i][c.Y + j].ElementBiome = TypeElementBiome.Maison;
                    }
                }
            }
        };
    }
}
