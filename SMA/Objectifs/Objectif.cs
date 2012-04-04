using SMA.Comportements;

namespace SMA.Objectifs
{
    public class Objectif
    {
        /// <summary>
        /// Le type d'objectif.
        /// </summary>
        public TypeObjectif TypeObj { get; private set; }

        /// <summary>
        /// La chaîne de comportements utilisée pour la réalisation de l'objectif.
        /// </summary>
        public Comportement RealisationObjectif { get; private set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="type">Le type d'objectif.</param>
        /// <param name="chaineRealisation">La chaîne de comportements utilisée pour la réalisation de l'objectif.</param>
        public Objectif(TypeObjectif type, Comportement chaineRealisation)
        {
            this.TypeObj = type;
            this.RealisationObjectif = chaineRealisation;
        }
    }
}
