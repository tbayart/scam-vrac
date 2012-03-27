namespace Vrac.SMA.Resultats
{
    public class Resultat
    {
        public static Resultat Critique;
        public static Resultat Echec;
        public static Resultat Succes;
        public static Resultat Epic;

        #region --> Attributs
        
        // L'id du Resultat.
        private int _id;
        
        #endregion --> Attributs

        #region --> Constructeurs

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static Resultat()
        {
            // Initialisation des Resultat statiques.
            Critique = new Resultat(1);
            Echec = new Resultat(2);
            Succes = new Resultat(3);
            Epic = new Resultat(4);
        }

        /// <summary>
        /// Constructeur privé.
        /// </summary>
        /// <param name="id">L'id du Resultat.</param>
        private Resultat(int id)
        {
            this._id = id;
        }

        #endregion --> Constructeurs

        #region --> Méthodes d'instance

        public override bool Equals(object obj)
        {
            return (this._id == (obj as Resultat)._id);
        }

        #endregion --> Méthodes d'instance

        public bool Equals(Resultat other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other._id.Equals(this._id);
        }

        public override int GetHashCode()
        {
            return this._id;
        }
    }
}
