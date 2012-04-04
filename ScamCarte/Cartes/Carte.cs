using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tools;
using System.Drawing;

namespace ScamCarte.Cartes
{
    public class Carte
    {
        #region -> Propriétés

        /// <summary>
        /// Obtient ou définit les éléments composant la carte.
        /// </summary>
        public ElementCarte[][] Elements { get; set; }

        /// <summary>
        /// Obtient la superficie des éléments autres que Eau.
        /// </summary>
        public int Superficie { get; internal set; }

        /// <summary>
        /// Obtient la largeur de la carte.
        /// </summary>
        public int Largeur
        {
            get { return Elements.Length; }
        }

        /// <summary>
        /// Obtient la hauteur de la carte.
        /// </summary>
        public int Hauteur
        {
            get { return Elements[0].Length; }
        }

        #endregion -> Propriétés

        #region -> Gestion sauvegarde

        /// <summary>
        /// Retourne un tableau de strings représentant l'instance de la Carte.
        /// </summary>
        /// <returns>Un tableau de strings représentant l'instance de la Carte.</returns>
        public string[] getDonneeSauvegarde()
        {
            List<string> lignes = new List<string>();

            // 1ère ligne : Superficie / Largeur / Hauteur
            lignes.Add(String.Format("{0};{1};{2}", this.Superficie, this.Largeur, this.Hauteur));

            // Lignes suivantes : Eléments
            for (int i = 0; i < this.Largeur; i++)
            {
                string ligne = String.Empty;

                for (int j = 0; j < this.Hauteur; j++)
                {
                    ligne += String.Format("{0};", this.Elements[i][j].getDonneeSauvegarde());
                }

                lignes.Add(ligne);
            }

            return lignes.ToArray();
        }

        /// <summary>
        /// Instancie une Carte à partir d'un tableau de strings.
        /// </summary>
        /// <param name="donnees">Un tableau de strings représentant les valeurs des attributs de la Carte.</param>
        /// <returns>Une Carte.</returns>
        public static Carte ChargerDonneesSauvegarde(string[] donnees)
        {
            Carte c = new Carte();

            string[] donneesGenerales = donnees[0].Split(';');
            
            int largeur = int.Parse(donneesGenerales[1]);
            int hauteur = int.Parse(donneesGenerales[2]);

            c.Superficie = int.Parse(donneesGenerales[0]);
            c.Elements = new ElementCarte[largeur][];
            
            for (int i = 0; i < largeur; i++)
            {
                c.Elements[i] = new ElementCarte[hauteur];

                string[] donneesColonne = donnees[i + 1].Split(';');

                for (int j = 0; j < hauteur; j++)
                {
                    c.Elements[i][j] = ElementCarte.ChargerDonneesSauvegarde(donneesColonne[j].Split(':'));
                }
            }

            return c;
        }

        #endregion -> Gestion sauvegarde

        public Coordonnees getPostionElement(TypeElementBiome typeElement, Coordonnees centre)
        {
            if (this.Elements[centre.X][centre.Y].ElementBiome == typeElement)
                return centre;

            int rayonMax = Math.Max(this.Hauteur / 2, this.Largeur / 2);
            int rayonCarre;
            int y;
            int x1, x2, y1, y2;

            Coordonnees res = null;

            // DEBUG
            //StreamWriter sw = new StreamWriter(@".\Debug_getPostionElement.txt", false);
            //double tempsTotal = 0;
            //double tempsCalculRayonCarre = 0;
            //double tempsCalculY = 0;
            //double tempsRechercheDansTableauElements = 0;

            //DateTime debutGlobal, debutCalculRayonCarre, debutCalculY, debutRechercheTableau;
            
            //sw.WriteLine("Rayon;Etape;Temps");
            
            //debutGlobal = DateTime.Now;
            // -----

            for (int rayon = 1; rayon < rayonMax; rayon++)
            {
                // DEBUG
                //debutCalculRayonCarre = DateTime.Now;
                // -----

                rayonCarre = rayon * rayon;

                // DEBUG
                //tempsCalculRayonCarre = DateTime.Now.Subtract(debutCalculRayonCarre).TotalMilliseconds;
                //sw.WriteLine("{0};Calcul R²;{1:0.000000}", rayon, tempsCalculRayonCarre);
                // -----

                for (int x = 0; x <= rayon; x++)
                {
                    // DEBUG
                    //debutCalculY = DateTime.Now;
                    // -----

                    // x² + y² = R²
                    y = (int)Math.Sqrt(rayonCarre - (x * x));

                    // DEBUG
                    //tempsCalculY = DateTime.Now.Subtract(debutCalculY).TotalMilliseconds;
                    //sw.WriteLine(";Calcul Y;;{0:0.000000}", tempsCalculY);
                    //debutRechercheTableau = DateTime.Now;
                    // -----

                    x1 = centre.X - x;
                    x2 = centre.X + x;
                    y1 = centre.Y - y;
                    y2 = centre.Y + y;

                    if (x1 > 0 && y1 > 0 && this.Elements[x1][y1].ElementBiome == typeElement)
                        res = new Coordonnees(x1, y1);
                    else if (x1 > 0 && y2 < this.Hauteur && this.Elements[x1][y2].ElementBiome == typeElement)
                        res = new Coordonnees(x1, y2);
                    else if (x2 < this.Largeur && y1 > 0 && this.Elements[x2][y1].ElementBiome == typeElement)
                        res = new Coordonnees(x2, y1);
                    else if (x2 < this.Largeur && y2 < this.Hauteur && this.Elements[x2][y2].ElementBiome == typeElement)
                        res = new Coordonnees(x2, y2);
                        
                    // DEBUG
                    //tempsRechercheDansTableauElements = DateTime.Now.Subtract(debutRechercheTableau).TotalMilliseconds;
                    //sw.WriteLine(";Recherche;;{0:0.000000}", tempsRechercheDansTableauElements);
                    // -----

                    if (res != null)
                        break;
                }

                // DEBUG
                //sw.WriteLine("");
                //sw.Flush();
                // -----

                if (res != null)
                    break;
            }

            // DEBUG
            //tempsTotal = DateTime.Now.Subtract(debutGlobal).TotalMilliseconds;
            //sw.WriteLine("TOTAL;{0:0.000}", tempsTotal);
            //sw.WriteLine();
            //if (res != null)
            //    sw.WriteLine("Point trouvé;{0}:{1}", res.X, res.Y);
            //else
            //    sw.WriteLine("Rien trouvé", tempsTotal);
            
            //sw.Flush();
            //sw.Close();
            // -----

            return res;
        }



        // Méthodes venant de _OLD_Vrac

        public Bitmap getBitmap()
        {
            if (this.Elements == null || this.Elements.Length <= 0 || this.Elements[0] == null)
                return null;

            int largeur = this.Largeur;
            int hauteur = this.Hauteur;

            Bitmap image = new Bitmap(largeur, hauteur);

            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    image.SetPixel(i, j, this.getColorElement(i, j));
                }
            }

            //using (Graphics g = Graphics.FromImage(image))
            //{
            //    g.DrawPolygon(Pens.Red, p.getPoints());
            //}

            return image;
        }

        private Color getColorElement(int x, int y)
        {
            TypeElementBiome t = this.Elements[x][y].ElementBiome;

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
                case TypeElementBiome.Pierre:
                    return Color.DarkGray;
                case TypeElementBiome.Arbre:
                    return Color.Green;
                case TypeElementBiome.Maison:
                case TypeElementBiome.Route:
                    return Color.White;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
