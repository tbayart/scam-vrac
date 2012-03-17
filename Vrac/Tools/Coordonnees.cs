using System;

namespace Vrac.Tools
{
    public class Coordonnees
    {
        #region -> Attributs

        // La coordonnée x.
        private int _x;

        // La coordonnée y.
        private int _y;

        #endregion -> Attributs

        #region -> Propriétés

        public int X
        {
            get { return this._x; }
            set { this._x = value; }
        }

        public int Y
        {
            get { return this._y; }
            set { this._y = value; }
        }

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
            this._x = x;
            this._y = y;
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
            return String.Format("({0} ; {1})", this._x, this._y);
        }

        public int getDistance(Coordonnees cible)
        {
            return (int)Math.Round(Math.Sqrt(Math.Pow(this.X - cible.X, 2) + Math.Pow(this.Y - cible.Y, 2)), 0);
        }

        #endregion -> publiques

        #endregion -> Méthodes d'instance
    }
}
