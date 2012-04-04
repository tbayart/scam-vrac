using System;

namespace Tools
{
    public class Coordonnees
    {
        #region -> Propriétés

        public int X;
        public int Y;

        #endregion -> Propriétés

        #region -> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Coordonnees()
            : this(0, 0)
        {
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="x">La coordonnée x.</param>
        /// <param name="y">La coordonnée y.</param>
        public Coordonnees(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion -> Constructeurs

        #region -> Méthodes d'instance

        #region -> publiques

        public Coordonnees clone()
        {
            return new Coordonnees(this.X, this.Y);
        }

        public override string ToString()
        {
            return String.Format("({0} ; {1})", this.X, this.Y);
        }

        public int getDistance(Coordonnees cible)
        {
            return (int)Math.Round(Math.Sqrt(Math.Pow(this.X - cible.X, 2) + Math.Pow(this.Y - cible.Y, 2)), 0);
        }

        public int getDistanceCarree(Coordonnees cible)
        {
            int deltaX = this.X - cible.X;
            int deltaY = this.Y - cible.Y;

            return deltaX * deltaX + deltaY * deltaY;
        }

        #endregion -> publiques

        #endregion -> Méthodes d'instance

        #region -> Méthodes statiques publiques

        public static int GetDistanceCarree(Coordonnees a, Coordonnees b)
        {
            int deltaX = a.X - b.X;
            int deltaY = a.Y - b.Y;

            return deltaX * deltaX + deltaY * deltaY;
        }

        #endregion -> Méthodes statiques publiques
    }
}
