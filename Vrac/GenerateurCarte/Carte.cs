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

        private static Carte GetCarteVide(int largeur, int hauteur)
        {
            Carte map = new Carte();

            map._carte = new TypeElementBiome[largeur][];

            for (int i = 0; i < largeur; i++)
            {
                map._carte[i] = new TypeElementBiome[hauteur];

                for (int j = 0; j < hauteur; j++)
                {
                    map._carte[i][j] = TypeElementBiome.Vide;
                }
            }

            return map;
        }

        public static Carte GetCarteTest(int largeur, int hauteur)
        {
            // Pour un élément _carte[x][y] :
            //   x représente la colonne
            //   y représente la ligne
            //
            //    +----> x
            //    |
            //    V
            //    y

            // ----- POUR DEBUG -----
            int nbTotalElements = largeur * hauteur;
            int nbElementsActuel = 0;
            int etape = 1;
            int limiteEtape = nbTotalElements / 10;
            // ----------------------

            List<Coordonnees> lstElementsAEtendre = new List<Coordonnees>();
            
            // Initialisation de la carte avec des éléments Vide.
            Carte map = GetCarteVide(largeur, hauteur);

            // On va placer aléatoirement N éléments.
            lstElementsAEtendre.AddRange(map.placerElementsAleatoirement());

            // ----- POUR DEBUG -----
            nbElementsActuel = lstElementsAEtendre.Count;

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

                List<Coordonnees> lstTemp = map.etendreV2(coord);
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
            map.ecrire(@".\Temp\Etape_Avant_Lissage_1.bmp");
            // ----------------------

            // Lissage de la carte.
            map.lisser();

            // ----- POUR DEBUG -----
            map.ecrire(@".\Temp\Etape_Avant_Lissage_2.bmp");
            // ----------------------

            map.lisser();

            // ----- POUR DEBUG -----
            map.ecrire(@".\Temp\Finale.bmp");
            // ----------------------

            return map;
        }

        private static void Smooth(Carte map)
        {
            int x_max = map._carte.Length;
            int y_max = map._carte[0].Length;

            for (int x = 1; x < x_max - 1; x++)
                for (int y = 1; y < y_max - 1; y++)
                    Smooth(map, x, y);
        }

        private static void Smooth(Carte map, int x, int y)
        {
            int T = -1;
            int S = 0;
            int E = 1;

            int score = 0;
            for (int a = -1; a < 2; a++)
                for (int b = -1; b < 2; b++)
                    if (map._carte[x + a][y + b] == TypeElementBiome.Eau)
                        score++;
                    else if (map._carte[x + a][y + b] == TypeElementBiome.Terre)
                        score--;

            if (score < -5)
                map._carte[x][y] = TypeElementBiome.Terre;
            if (score > 5)
                map._carte[x][y] = TypeElementBiome.Eau;


            //// Cas 1 : point isolé
            //    if (map._carte[x - 1][y] == map._carte[x + 1][y]    // à gauche = à droite
            //        && map._carte[x][y - 1] == map._carte[x][y + 1]    // en haut = en bas
            //        && map._carte[x][y - 1] == map._carte[x - 1][y]    // en haut = à gauche
            //        )
            //        map._carte[x][y] = map._carte[x][y - 1];            // set

            // Cas 2 : L , dans chaque sens : 2 colonnes perpendiculaires identiques

            bool Gch_E_HGche = map._carte[x - 1][y] == map._carte[x - 1][y - 1];
            bool Gch_E_BGche = map._carte[x - 1][y] == map._carte[x - 1][y + 1];
            bool H_E_HGche = map._carte[x][y - 1] == map._carte[x - 1][y - 1];
            bool H_E_HDrt = map._carte[x][y - 1] == map._carte[x + 1][y - 1];
            bool Drt_E_HDrt = map._carte[x + 1][y] == map._carte[x + 1][y - 1];
            bool Drt_E_BDrt = map._carte[x + 1][y] == map._carte[x + 1][y + 1];
            bool B_E_BGche = map._carte[x][y + 1] == map._carte[x - 1][y + 1];
            bool B_E_BDrt = map._carte[x][y + 1] == map._carte[x + 1][y + 1];

            if (Gch_E_HGche && Gch_E_BGche // colonne de gauche
                && H_E_HGche && H_E_HDrt   // ligne du haut
                )
                map._carte[x][y] = map._carte[x - 1][y];

            if (Drt_E_HDrt && Drt_E_BDrt // colonne de droite
                && H_E_HGche && H_E_HDrt // ligne du haut
                )
                map._carte[x][y] = map._carte[x + 1][y];

            if (Drt_E_HDrt && Drt_E_BDrt // colonne de droite
                && B_E_BGche && B_E_BDrt // ligne du bas
                )
                map._carte[x][y] = map._carte[x + 1][y];

            if (Gch_E_HGche && Gch_E_BGche // colonne de gauche
                && B_E_BGche && B_E_BDrt   // ligne du bas
                )
                map._carte[x][y] = map._carte[x][y + 1];
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
        /// Place aléatoirement un certain nombre d'éléments sur la carte.
        /// </summary>
        /// <returns>Les coordonnées des éléments placés.</returns>
        private List<Coordonnees> placerElementsAleatoirement()
        {
            Distribution<TypeElementBiome> distribElt = new Distribution<TypeElementBiome>();
            distribElt.dicoSeuils[TypeElementBiome.Eau] = 0.05d;
            distribElt.dicoSeuils[TypeElementBiome.Terre] = 0.9d;
            distribElt.dicoSeuils[TypeElementBiome.Sable] = 0.05d;

            int largeur = this._carte.Length;
            int hauteur = this._carte[0].Length;

            List<Coordonnees> lstElementsPlaces = new List<Coordonnees>();

            int nbEltsAPlacer = (largeur * hauteur) / (128 * 128);

            for (int i = 0; i < nbEltsAPlacer; i++)
            {
                int x = Randomizer.Next(0, largeur);
                int y = Randomizer.Next(0, hauteur);

                this._carte[x][y] = distribElt.get();

                lstElementsPlaces.Add(new Coordonnees(x, y));
            }

            return lstElementsPlaces;
        }

        /// <summary>
        /// Lisse la disposition des éléments sur la carte.
        /// </summary>
        private void lisser()
        {
            int largeur = this._carte.Length;
            int hauteur = this._carte[0].Length;

            for (int i = 1; i < largeur - 1; i++)
                for (int j = 1; j < hauteur - 1; j++)
                {
                    if (this._carte[i - 1][j] == this._carte[i][j - 1]
                        && this._carte[i - 1][j] == this._carte[i + 1][j]
                        && this._carte[i - 1][j] == this._carte[i][j + 1])
                    {
                        this._carte[i][j] = this._carte[i - 1][j];
                        continue;
                    }

                    bool verifCol1 = (this._carte[i - 1][j] == this._carte[i - 1][j - 1] && this._carte[i - 1][j] == this._carte[i - 1][j + 1]);
                    bool verifCol3 = (this._carte[i + 1][j] == this._carte[i + 1][j - 1] && this._carte[i + 1][j] == this._carte[i + 1][j + 1]);
                    bool verifLig1 = (this._carte[i][j - 1] == this._carte[i - 1][j - 1] && this._carte[i][j - 1] == this._carte[i + 1][j - 1]);
                    bool verifLig3 = (this._carte[i][j + 1] == this._carte[i - 1][j + 1] && this._carte[i][j + 1] == this._carte[i + 1][j + 1]);

                    if ((verifCol1 && verifLig1)
                        || (verifCol1 && verifLig3))
                    {
                        this._carte[i][j] = this._carte[i - 1][j];
                        continue;
                    }
                    else if ((verifCol3 && verifLig1)
                        || (verifCol3 && verifLig3))
                    {
                        this._carte[i][j] = this._carte[i + 1][j];
                        continue;
                    }
                }
        }

        /// <summary>
        /// Définit les types des éléments voisins d'un point donné de la carte.
        /// </summary>
        /// <param name="coord">Les coordonnées d'un point de la carte.</param>
        /// <returns>La liste des coordonnées des voisins nouvellement définis.</returns>
        private List<Coordonnees> etendre(Coordonnees coord)
        {
            List<Coordonnees> lstNouveauxVoisins = new List<Coordonnees>();
            
            //Distribution<TypeElementBiome>[][] distrib = Biome_Continent2D.S_DistributionVoisins[this._carte[coord.X][coord.Y]];
            
            //for (int offset_x = -1; offset_x <= 1; offset_x++)
            //{
            //    for (int offset_y = -1; offset_y <= 1; offset_y++)
            //    {
            //        if (offset_y == 0 && offset_x == 0)
            //            continue;

            //        if (coord.X + offset_x > -1 && coord.X + offset_x < this._carte.Length
            //            && coord.Y + offset_y > -1 && coord.Y + offset_y < this._carte[coord.X].Length)
            //        {
            //            if (this._carte[coord.X + offset_x][coord.Y + offset_y] == TypeElementBiome.Vide)
            //                lstNouveauxVoisins.Add(new Coordonnees(coord.X + offset_x, coord.Y + offset_y));

            //            this._carte[coord.X + offset_x][coord.Y + offset_y] = distrib[offset_x + 1][offset_y + 1].get();
            //        }
            //    }
            //}

            return lstNouveauxVoisins;
        }

        private List<Coordonnees> etendreV2(Coordonnees coord)
        {
            List<Coordonnees> lstNouveauxVoisins = new List<Coordonnees>();

            TypeElementBiome[][] region = new TypeElementBiome[5][];

            for (int i = 0; i < 5; i++)
            {
                region[i] = new TypeElementBiome[5];

                for (int j = 0; j < 5; j++)
                {
                    try
                    {
                        region[i][j] = this._carte[coord.X - 2 + i][coord.Y - 2 + j];
                    }
                    catch
                    {
                        region[i][j] = TypeElementBiome.Vide;
                    }
                }
            }

            Distribution<TypeElementBiome>[][] distrib = Biome_Continent2D.GetDistributionVoisins(region);

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
                    return Color.DarkGreen;
                case TypeElementBiome.Eau:
                    return Color.Blue;
                case TypeElementBiome.Sable:
                    return Color.SandyBrown;
                case TypeElementBiome.Pierre:
                    return Color.DarkGray;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion -> Privées

        #endregion -> Méthodes d'instance
    }


    public class CarteV2
    {
        #region -> Attributs

        // Représente la carte.
        private TypeElementBiome[][] _carte;

        #endregion -> Attributs

        public static CarteV2 GetCarteTest(int superficie)
        {
            Distribution<byte> distCoteIni = new Distribution<byte>();
            distCoteIni.dicoSeuils[0] = 0.75d;
            distCoteIni.dicoSeuils[1] = 0.25d;

            Distribution<byte> distDiagIni = new Distribution<byte>();
            distDiagIni.dicoSeuils[0] = 0.95d;
            distDiagIni.dicoSeuils[1] = 0.05d;

            Distribution<byte>[][] distribEtapeInitiale = new[]
                {
                    new [] { distDiagIni, distCoteIni, distDiagIni },
                    new [] { distCoteIni, null,        distCoteIni },
                    new [] { distDiagIni, distCoteIni, distDiagIni }
                };

            Distribution<byte> distCoteN = new Distribution<byte>();
            distCoteN.dicoSeuils[0] = 0.75d;
            distCoteN.dicoSeuils[1] = 0.25d;

            Distribution<byte> distDiagN = new Distribution<byte>();
            distDiagN.dicoSeuils[0] = 0.75d;
            distDiagN.dicoSeuils[1] = 0.25d;

            Distribution<byte>[][] distribEtapeN = new[]
                {
                    new [] { distDiagN, distCoteN, distDiagN },
                    new [] { distCoteN, null,      distCoteN },
                    new [] { distDiagN, distCoteN, distDiagN }
                };

            byte[][] carteTemp = EtapeInitiale(superficie * 60 / 100, 256, distribEtapeInitiale);

            // ----- DEBUG -----
            DessinerCarte(carteTemp, 1, 256);
            // -----------------

            carteTemp = EtapeN(carteTemp, superficie * 20 / 100, 64, distribEtapeN);

            // ----- DEBUG -----
            DessinerCarte(carteTemp, 2, 64);
            // -----------------

            carteTemp = EtapeN(carteTemp, superficie * 10 / 100, 16, distribEtapeN);

            // ----- DEBUG -----
            DessinerCarte(carteTemp, 3, 16);
            // -----------------

            carteTemp = EtapeN(carteTemp, superficie * 7 / 100, 4, distribEtapeN);

            // ----- DEBUG -----
            DessinerCarte(carteTemp, 4, 4);
            // -----------------

            carteTemp = OptimiserCarte(EtapeN(carteTemp, superficie * 3 / 100, 1, distribEtapeN));

            // ----- DEBUG -----
            DessinerCarte(carteTemp, 5, 1);
            // -----------------

            return null;
        }

        private static void DessinerCarte(byte[][] carte, int etape, int largeurBlocs)
        {
            int largeur = carte.Length;
            int hauteur = carte[0].Length;
            Bitmap image = new Bitmap(largeur * largeurBlocs, hauteur * largeurBlocs);

            if (largeurBlocs > 1)
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    for (int i = 0; i < largeur; i++)
                        for (int j = 0; j < hauteur; j++)
                        {
                            g.FillRectangle((carte[i][j] == 1 ? Brushes.Brown : Brushes.Black), i * largeurBlocs, j * largeurBlocs, largeurBlocs, largeurBlocs);
                        }
                }
            }
            else
            {
                for (int i = 0; i < largeur; i++)
                    for (int j = 0; j < hauteur; j++)
                    {
                        image.SetPixel(i, j, (carte[i][j] == 1 ? Color.Brown : Color.Black));
                    }

                int superficieReelle = 0;
                for (int i = 0; i < largeur; i++)
                    for (int j = 0; j < hauteur; j++)
                        superficieReelle += carte[i][j];

                using (Graphics g = Graphics.FromImage(image))
                {
                    g.DrawString(String.Format("Superficie réelle : {0}", superficieReelle), new Font("Arial", 16), Brushes.White, 8, hauteur - 24);
                }
            }

            image.Save(String.Format(@"DebugCarteV2_Etape{0}.bmp", etape));
        }

        private static List<Coordonnees> Etendre(ref byte[][] carte, Coordonnees coord, Distribution<byte>[][] distrib)
        {
            List<Coordonnees> nouveauxElements = new List<Coordonnees>();

            for (int offset_x = -1; offset_x <= 1; offset_x++)
            {
                for (int offset_y = -1; offset_y <= 1; offset_y++)
                {
                    if (offset_y == 0 && offset_x == 0)
                        continue;

                    if (coord.X + offset_x > -1 && coord.X + offset_x < carte.Length
                        && coord.Y + offset_y > -1 && coord.Y + offset_y < carte[coord.X].Length)
                    {
                        if (carte[coord.X + offset_x][coord.Y + offset_y] == 0 && distrib[offset_x + 1][offset_y + 1].get() == 1)
                        {
                            carte[coord.X + offset_x][coord.Y + offset_y] = 1;
                            nouveauxElements.Add(new Coordonnees(coord.X + offset_x, coord.Y + offset_y));
                        }
                    }
                }
            }

            return nouveauxElements;
        }

        private static byte[][] OptimiserCarte(byte[][] carte)
        {
            int largeurCarte = carte.Length;
            int hauteurCarte = carte[0].Length;

            int xMin = largeurCarte - 1;
            int xMax = 0;
            int yMin = hauteurCarte - 1;
            int yMax = 0;

            for (int i = 0; i < largeurCarte; i++)
                for (int j = 0; j < hauteurCarte; j++)
                {
                    if (carte[i][j] == 1)
                    {
                        xMin = Math.Min(xMin, i);
                        xMax = Math.Max(xMax, i);
                        yMin = Math.Min(yMin, j);
                        yMax = Math.Max(yMax, j);
                    }
                }

            int largeurCarteOptimisee = xMax - xMin + 5;
            int hauteurCarteOptimisee = yMax - yMin + 5;

            byte[][] carteOptimisee = new byte[largeurCarteOptimisee][];

            for (int i = 0; i < largeurCarteOptimisee; i++)
            {
                carteOptimisee[i] = new byte[hauteurCarteOptimisee];

                for (int j = 0; j < hauteurCarteOptimisee; j++)
                {
                    carteOptimisee[i][j] = carte[xMin + i - 2][yMin + j - 2];
                }
            }

            return carteOptimisee;
        }

        private static byte[][] CreerCarteEtape(byte[][] carteEtapePrecedente)
        {
            int largeurCarte = carteEtapePrecedente.Length * 4;
            int hauteurCarte = carteEtapePrecedente[0].Length * 4;

            byte[][] carte = new byte[largeurCarte][];
            for (int i = 0; i < largeurCarte; i++)
            {
                carte[i] = new byte[hauteurCarte];

                for (int j = 0; j < hauteurCarte; j++)
                {
                    carte[i][j] = carteEtapePrecedente[i / 4][j / 4];
                }
            }

            return carte;
        }

        private static List<Coordonnees> DeterminerBlocsAEtendre(byte[][] carte)
        {
            int nbVides;
            List<Coordonnees> blocsAEtendre = new List<Coordonnees>();

            int largeurCarte = carte.Length;
            int hauteurCarte = carte[0].Length;

            for (int i = 1; i < largeurCarte - 1; i++)
            {
                for (int j = 1; j < hauteurCarte - 1; j++)
                {
                    if (carte[i][j] != 0)
                    {
                        nbVides = 0;

                        if (carte[i - 1][j - 1] == 0) nbVides++;
                        if (carte[i][j - 1] == 0) nbVides++;
                        if (carte[i + 1][j - 1] == 0) nbVides++;
                        if (carte[i - 1][j] == 0) nbVides++;
                        if (carte[i + 1][j] == 0) nbVides++;
                        if (carte[i - 1][j + 1] == 0) nbVides++;
                        if (carte[i][j + 1] == 0) nbVides++;
                        if (carte[i + 1][j + 1] == 0) nbVides++;

                        if ((nbVides > 2 && nbVides < 5) || nbVides > 6)
                            blocsAEtendre.Add(new Coordonnees(i, j));
                    }
                }
            }

            return blocsAEtendre;
        }
        
        private static void Lisser(ref byte[][] carte)
        {
            int largeur = carte.Length;
            int hauteur = carte[0].Length;

            for (int i = 1; i < largeur - 1; i++)
                for (int j = 1; j < hauteur - 1; j++)
                {
                    if (carte[i][j] == 0)
                    {
                        if (carte[i - 1][j] == 1 && carte[i][j - 1] == 1
                            && carte[i + 1][j] == 1 && carte[i][j + 1] == 1)
                        {
                            carte[i][j] = 1;
                            continue;
                        }

                        bool verifCol1 = (carte[i - 1][j - 1] == 1 && carte[i - 1][j] == 1 && carte[i - 1][j + 1] == 1);
                        bool verifCol3 = (carte[i + 1][j - 1] == 1 && carte[i + 1][j] == 1 && carte[i + 1][j + 1] == 1);
                        bool verifLig1 = (carte[i - 1][j - 1] == 1 && carte[i][j - 1] == 1 && carte[i + 1][j - 1] == 1);
                        bool verifLig3 = (carte[i - 1][j + 1] == 1 && carte[i][j + 1] == 1 && carte[i + 1][j + 1] == 1);

                        if ((verifCol1 && verifLig1)
                            || (verifCol1 && verifLig3)
                            || (verifCol3 && verifLig1)
                            || (verifCol3 && verifLig3))
                        {
                            carte[i][j] = 1;
                            continue;
                        }
                    }
                    else
                    {
                        if ((carte[i - 1][j - 1] == 0 && carte[i][j - 1] == 0 && carte[i + 1][j] == 0 && carte[i + 1][j + 1] == 0)
                           || (carte[i][j - 1] == 0 && carte[i + 1][j - 1] == 0 && carte[i - 1][j] == 0 && carte[i - 1][j + 1] == 0)
                           || (carte[i - 1][j + 1] == 0 && carte[i][j + 1] == 0 && carte[i + 1][j - 1] == 0 && carte[i + 1][j] == 0)
                           || (carte[i][j + 1] == 0 && carte[i + 1][j + 1] == 0 && carte[i - 1][j - 1] == 0 && carte[i - 1][j] == 0))
                        {
                            carte[i][j] = 0;
                            continue;
                        }
                    }
                }
        }

        private static byte[][] EtapeInitiale(int superficieMax, int largeurBlocs, Distribution<byte>[][] distributionVoisins)
        {
            int nbBlocsAPlacer = superficieMax / (largeurBlocs * largeurBlocs);
            int nbBlocsPlaces = 0;
            List<Coordonnees> blocsATraiter = new List<Coordonnees>();

            int coteCarteTemp = (int)Math.Round(4 * Math.Sqrt(nbBlocsAPlacer), 0);

            // On initialise l'objet qui représente la carte temporaire.
            byte[][] carte = new byte[coteCarteTemp][];
            for (int i = 0; i < coteCarteTemp; i++)
            {
                carte[i] = new byte[coteCarteTemp];

                for (int j = 0; j < coteCarteTemp; j++)
                {
                    carte[i][j] = 0;
                }
            }

            // On place un bloc au centre.
            carte[coteCarteTemp / 2][coteCarteTemp / 2] = 1;
            nbBlocsPlaces++;
            blocsATraiter.Add(new Coordonnees(coteCarteTemp / 2, coteCarteTemp / 2));

            // On place les blocs.
            while (nbBlocsPlaces < nbBlocsAPlacer)
            {
                List<Coordonnees> lstTemp = new List<Coordonnees>();

                if (blocsATraiter.Count == 0)
                    blocsATraiter = DeterminerBlocsAEtendre(carte);

                foreach (Coordonnees coord in blocsATraiter)
                {
                    List<Coordonnees> lst = Etendre(ref carte, coord, distributionVoisins);
                    nbBlocsPlaces += lst.Count;

                    lstTemp.AddRange(lst);

                    if (nbBlocsPlaces >= nbBlocsAPlacer)
                        break;
                }

                blocsATraiter = lstTemp;
            }

            // On lisse.
            Lisser(ref carte);

            return OptimiserCarte(carte);
        }

        private static byte[][] EtapeN(byte[][] carteEtapePrecedente, int superficieMax, int largeurBlocs, Distribution<byte>[][] distributionVoisins)
        {
            int nbBlocsAPlacer = superficieMax / (largeurBlocs * largeurBlocs);
            int nbBlocsPlaces = 0;

            // On initialise une nouvelle carte à partir de celle de l'étape 1.
            byte[][] carte = CreerCarteEtape(carteEtapePrecedente);

            // On recherche les blocs à étendre.
            List<Coordonnees> blocsAEtendre = DeterminerBlocsAEtendre(carte);

            // On ajoute les blocs.
            while (nbBlocsPlaces < nbBlocsAPlacer)
            {
                List<Coordonnees> lstTemp = new List<Coordonnees>();

                if (blocsAEtendre.Count == 0)
                    blocsAEtendre = DeterminerBlocsAEtendre(carte);

                foreach (Coordonnees coord in blocsAEtendre)
                {
                    List<Coordonnees> lst = Etendre(ref carte, coord, distributionVoisins);
                    nbBlocsPlaces += lst.Count;

                    lstTemp.AddRange(lst);

                    if (nbBlocsPlaces >= nbBlocsAPlacer)
                        break;
                }

                blocsAEtendre = lstTemp;
            }

            // On lisse la carte.
            Lisser(ref carte);

            return carte;
        }
    }
}
