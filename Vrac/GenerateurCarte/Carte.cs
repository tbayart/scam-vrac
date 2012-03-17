using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Vrac.GenerateurCarte.Biomes;
using Vrac.Tools;

namespace Vrac.GenerateurCarte
{
    public class Carte
    {
        #region -> Attributs

        // Représente la carte.
        private TypeElementBiome[][] _carte;

        #endregion -> Attributs

        #region -> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Carte()
        {
        }

        #endregion -> Constructeurs

        #region -> Méthodes statiques

        public static Carte GetCarteTest()
        {
            // Pour un élément _carte[x][y] :
            //   x représente la colonne
            //   y représente la ligne
            //
            //    +----> x
            //    |
            //    V
            //    y

            Carte map = new Carte();

            int largeur = 1024;
            int hauteur = 1024;

            // ----- POUR DEBUG -----
            int nbTotalElements = largeur * hauteur;
            int nbElementsActuel = 0;
            int etape = 1;
            int limiteEtape = nbTotalElements / 10;
            // ----------------------

            List<Coordonnees> lstElementsAEtendre = new List<Coordonnees>();
            
            // Initialisation de la carte avec des éléments Vide.

            map._carte = new TypeElementBiome[largeur][];

            for (int i = 0; i < largeur; i++)
            {
                map._carte[i] = new TypeElementBiome[hauteur];

                for (int j = 0; j < hauteur; j++)
                {
                    map._carte[i][j] = TypeElementBiome.Vide;
                }
            }

            // On va placer aléatoirement N éléments.

            Distribution<TypeElementBiome> distribElt = new Distribution<TypeElementBiome>
            {
                seuils = new List<double>() { 0.1d, 1d },
                results = new List<TypeElementBiome>() { TypeElementBiome.Eau, TypeElementBiome.Terre }
            };

            int nbEltsAPlacer = (largeur * hauteur) / (128 * 128);

            for (int i = 0; i < nbEltsAPlacer; i++)
            {
                int x = Randomizer.Next(0, largeur);
                int y = Randomizer.Next(0, hauteur);

                map._carte[x][y] = distribElt.get();

                lstElementsAEtendre.Add(new Coordonnees(x, y));
            }

            // ----- POUR DEBUG -----
            nbElementsActuel = nbEltsAPlacer;

            if (!Directory.Exists(@".\Temp\"))
                Directory.CreateDirectory(@".\Temp\");

            map.ecrire(@".\Temp\Etape_0.bmp");
            // ----------------------

            // On étend la carte tant qu'il y a de la place.

            while (lstElementsAEtendre.Count > 0)
            {
                int i = Randomizer.Next(0, lstElementsAEtendre.Count);
                
                Coordonnees coord = lstElementsAEtendre[i];
                lstElementsAEtendre.RemoveAt(i);

                List<Coordonnees> lstTemp = map.etendre(coord);
                lstElementsAEtendre.AddRange(lstTemp);

                // ----- POUR DEBUG -----
                nbElementsActuel += lstTemp.Count;

                if (etape * limiteEtape < nbElementsActuel)
                {
                    map.ecrire(String.Format(@".\Temp\Etape_{0}.bmp", etape));
                    etape++;
                }
                // ----------------------
            }

            // ----- POUR DEBUG -----
            map.ecrire(@".\Temp\Finale.bmp");
            // ----------------------

            return map;
        }

        #endregion -> Méthodes statiques

        #region -> Méthodes d'instance

        #region -> Privées

        /// <summary>
        /// Retourne le nombre d'éléments non vide de la carte.
        /// </summary>
        /// <returns>Le nombre d'éléments non vide de la carte.</returns>
        private int calculerNbElements()
        {
            int nb = 0;

            int l = this._carte.Length;

            for (int i = 0; i < l; i++)
                for (int j = 0; j < l; j++)
                    if (this._carte[i][j] != TypeElementBiome.Vide)
                        nb++;

            return nb;
        }

        /// <summary>
        /// Définit les types des éléments voisins d'un point donné de la carte.
        /// </summary>
        /// <param name="coord">Les coordonnées d'un point de la carte.</param>
        /// <returns>La liste des coordonnées des voisins nouvellement définis.</returns>
        private List<Coordonnees> etendre(Coordonnees coord)
        {
            Distribution<TypeElementBiome>[][] distrib = Biome_Continent2D.S_DistributionVoisins[this._carte[coord.X][coord.Y]];

            List<Coordonnees> lstNouveauxVoisins = new List<Coordonnees>();

            for (int offset_x = -1; offset_x <= 1; offset_x++)
            {
                for (int offset_y = -1; offset_y <= 1; offset_y++)
                {
                    if (offset_y == 0 && offset_x == 0)
                        continue;

                    if (coord.X + offset_x > -1 && coord.X + offset_x < this._carte.Length
                        && coord.Y + offset_y > -1 && coord.Y + offset_y < this._carte[coord.X].Length)
                    {
                        if (this._carte[coord.X + offset_x][coord.Y + offset_y] == TypeElementBiome.Vide)
                            lstNouveauxVoisins.Add(new Coordonnees(coord.X + offset_x, coord.Y + offset_y));

                        this._carte[coord.X + offset_x][coord.Y + offset_y] = distrib[offset_x + 1][offset_y + 1].get();
                    }
                }
            }

            return lstNouveauxVoisins;
        }

        private void ecrire(string cheminFichier)
        {
            if (this._carte != null && this._carte.Length > 0 && this._carte[0] != null)
            {
                int hauteur = this._carte.Length;
                int largeur = this._carte[0].Length;

                Bitmap image = new Bitmap(largeur, hauteur);
                for (int i = 0; i < largeur; i++)
                    for (int j = 0; j < hauteur; j++)
                    {
                        image.SetPixel(i, j, this.getColorElement(i, j));
                    }

                //using (Graphics g = Graphics.FromImage(image))
                //{
                //    g.DrawPolygon(Pens.Red, p.getPoints());
                //}

                image.Save(String.Format(cheminFichier));
            }
        }

        private Color getColorElement(int x, int y)
        {
            TypeElementBiome t = _carte[x][y];

            switch (t)
            {
                case TypeElementBiome.Vide:
                    return Color.Black; //Transparent;
                case TypeElementBiome.Terre:
                    return Color.Brown;
                case TypeElementBiome.Eau:
                    return Color.Blue;
                case TypeElementBiome.Sable:
                    return Color.SandyBrown;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion -> Privées

        #endregion -> Méthodes d'instance
    }
}
