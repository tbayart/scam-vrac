using System;

namespace SMA.Actions
{
    public partial class CatalogueActions
    {
        public static Action None = new Action()
        {
            doIt = (acteur, cible, coord) => { }
        };
    }
}
