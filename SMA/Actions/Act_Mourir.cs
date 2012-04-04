using System;

namespace SMA.Actions
{
    public partial class CatalogueActions
    {
        public static Action Mourir = new Action()
        {
            doIt = (acteur, cible, coord) => { }
        };
    }
}
