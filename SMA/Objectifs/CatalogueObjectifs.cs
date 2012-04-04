using SMA.Comportements;

namespace SMA.Objectifs
{
    public class CatalogueObjectifs
    {
        public static Objectif Objectif_Test = new Objectif(
            TypeObjectif.RejoindreTerre, 
            CatalogueComportements.Comportement_AtteindreObjectifTest);

        public static Objectif Objectif_Dryad = new Objectif(
            TypeObjectif.Dryad,
            CatalogueComportements.Comportement_Dryad);
    }
}
