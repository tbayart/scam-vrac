using System;

namespace Vrac.Tools
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

        //public int getDistance(Coordonnees cible)
        //{
        //    return (int)Math.Round(Math.Sqrt(Math.Pow(this.X - cible.X, 2) + Math.Pow(this.Y - cible.Y, 2)), 0);
        //}
        //public int getDistanceCarree(Coordonnees cible)
        //{
        //    int _X = this.X - cible.X;
        //    int _Y = this.Y - cible.Y;
        //    return _X*_X + _Y*_Y;
        //}

        public static int GetDistanceCarree(Coordonnees source, Coordonnees cible)
        {
            int _X = source.X - cible.X;
            int _Y = source.Y - cible.Y;
            return _X * _X + _Y * _Y;
        }
        #endregion -> publiques

        #endregion -> Méthodes d'instance
    }
}
