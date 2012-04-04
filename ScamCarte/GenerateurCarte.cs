using System;
using System.Collections.Generic;
using System.Drawing;
using ScamCarte.Cartes;
using Tools;

namespace ScamCarte
{
    public class GenerateurCarte
    {
        #region -> Attributs statiques

        private static Distribution<byte>[][] S_DistribEtapeInitiale;
        private static Distribution<byte>[][] S_DistribEtapeN;

        #endregion -> Attributs statiques

        #region -> Constructeur statique

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static GenerateurCarte()
        {
            Distribution<byte> distCoteIni = new Distribution<byte>();
            distCoteIni.DicoSeuils[0] = 0.75d;
            distCoteIni.DicoSeuils[1] = 0.25d;

            Distribution<byte> distDiagIni = new Distribution<byte>();
            distDiagIni.DicoSeuils[0] = 0.95d;
            distDiagIni.DicoSeuils[1] = 0.05d;

            S_DistribEtapeInitiale = new[]
                {
                    new [] { distDiagIni, distCoteIni, distDiagIni },
                    new [] { distCoteIni, null,        distCoteIni },
                    new [] { distDiagIni, distCoteIni, distDiagIni }
                };

            Distribution<byte> distCoteN = new Distribution<byte>();
            distCoteN.DicoSeuils[0] = 0.75d;
            distCoteN.DicoSeuils[1] = 0.25d;

            Distribution<byte> distDiagN = new Distribution<byte>();
            distDiagN.DicoSeuils[0] = 0.75d;
            distDiagN.DicoSeuils[1] = 0.25d;

            S_DistribEtapeN = new[]
                {
                    new [] { distDiagN, distCoteN, distDiagN },
                    new [] { distCoteN, null,      distCoteN },
                    new [] { distDiagN, distCoteN, distDiagN }
                };
        }

        #endregion -> Constructeur statique

        #region -> Méthodes statiques

        #region -> Public

        /// <summary>
        /// Génère une nouvelle carte.
        /// </summary>
        /// <param name="superficie">La superficie souhaitée de la carte.</param>
        /// <returns>Une carte générée aléatoirement.</returns>
        public static Carte GenererNouvelleCarte(int superficie)
        {
            int largeurBlocs;
            byte superficieInitiale;
            int superficieTraitee;
            int superficieRestante = superficie;

            // --> POUR DEBUG <--
            //int etape = 1;
            // ------------------

            // On détermine le % de la superficie à traiter et la largeur des blocs pour l'étape initiale.
            DeterminerDonneesInitiales(superficie, out largeurBlocs, out superficieInitiale);

            // Etape initiale.
            byte[][] carteTemp = EtapeInitiale(superficie * superficieInitiale / 100, largeurBlocs, out superficieTraitee);

            // --> POUR DEBUG <--
            //DessinerCarte(carteTemp, etape, largeurBlocs, superficie.ToString());
            //etape++;
            // ------------------

            superficieRestante -= superficieTraitee;
            largeurBlocs /= 4;

            // Etape suivantes.
            while (largeurBlocs > 2)
            {
                carteTemp = EtapeN(carteTemp, superficieRestante * 70 / 100, largeurBlocs, out superficieTraitee);

                // --> POUR DEBUG <--
                //DessinerCarte(carteTemp, etape, largeurBlocs, superficie.ToString());
                //etape++;
                // ------------------

                superficieRestante -= superficieTraitee;
                largeurBlocs /= 4;
            }

            if (largeurBlocs == 2)
            {
                carteTemp = CreerCarteEtape(carteTemp, 2);
            }

            // Etape finale.
            carteTemp = EtapeN(carteTemp, superficieRestante, 1, out superficieTraitee);
            Lisser(ref carteTemp);
            carteTemp = OptimiserCarte(carteTemp);

            // --> POUR DEBUG <--
            //DessinerCarte(carteTemp, etape, 1, superficie.ToString());
            //etape++;
            // ------------------

            // 
            Carte carte = new Carte();
            carte.Elements = new ElementCarte[carteTemp.Length][];
            carte.Superficie = 0;

            for (int i = 0; i < carteTemp.Length; i++)
            {
                carte.Elements[i] = new ElementCarte[carteTemp[i].Length];

                for (int j = 0; j < carteTemp[0].Length; j++)
                {
                    byte b = carteTemp[i][j];

                    carte.Elements[i][j] = new ElementCarte((TypeElementBiome)b);

                    if (b > 0)
                        carte.Superficie++;
                }
            }

            return carte;
        }

        #endregion -> Public

        #region -> Private

        private static void DeterminerDonneesInitiales(int superficie, out int largeurBlocs, out byte superficieInitiale)
        {
            /*
            Largeur bloc initial = 64       50 % carte  = [ 20 480 - 81 920 [
            (entre 5 et 20 blocs)           100 % carte = [ 40 960 - 163 840 [
       
            Largeur bloc initial = 128      50 % carte  = [ 81 920 - 327 680 [
            (entre 5 et 20 blocs)           100 % carte = [ 163 840 - 655 360 [
       
            Largeur bloc initial = 256      50 % carte  = [ 327 680 - 1 310 720 [
            (entre 5 et 20 blocs)           100 % carte = [ 655 360 - 2 621 440 [
       
            Largeur bloc initial = 512      50 % carte  = [ 1 310 720 - 5 242 880 [
            (entre 5 et 20 blocs)           100 % carte = [ 2 621 440 - 10 485 760 [
       
            Largeur bloc initial = 1024     50 % carte  = [ 5 242 880 - 20 971 520 [
            (entre 5 et 20 blocs)           100 % carte = [ 10 485 760 - 41 943 040 [
            */

            superficieInitiale = 50;

            if (superficie < 40960)
            {
                throw new Exception("La superficie souhaitée est trop petite.");
            }
            else if (superficie > 40000000)
            {
                throw new Exception("La superficie souhaitée est trop grande.");
            }
            else if (superficie < 163840)
            {
                largeurBlocs = 64;
            }
            else if (superficie < 655360)
            {
                largeurBlocs = 128;
            }
            else if (superficie < 2621440)
            {
                largeurBlocs = 256;
            }
            else if (superficie < 10485760)
            {
                largeurBlocs = 512;
            }
            else
            {
                largeurBlocs = 1024;
            }
        }

        private static byte[][] EtapeInitiale(int superficieMax, int largeurBlocs, out int superficieTraitee)
        {
            int nbBlocsAPlacer = superficieMax / (largeurBlocs * largeurBlocs);
            int nbBlocsPlaces = 0;
            List<Coordonnees> blocsATraiter = new List<Coordonnees>();

            int coteCarteTemp = (int)Math.Round(4 * Math.Sqrt(nbBlocsAPlacer), 0);
            int coteCarteTempSurDeux = coteCarteTemp / 2;

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
            carte[coteCarteTempSurDeux][coteCarteTempSurDeux] = 1;
            nbBlocsPlaces++;
            blocsATraiter.Add(new Coordonnees(coteCarteTempSurDeux, coteCarteTempSurDeux));

            // On place les blocs.
            while (nbBlocsPlaces < nbBlocsAPlacer)
            {
                List<Coordonnees> lstTemp = new List<Coordonnees>();

                if (blocsATraiter.Count == 0)
                    blocsATraiter = DeterminerBlocsAEtendre(carte);

                foreach (Coordonnees coord in blocsATraiter)
                {
                    List<Coordonnees> lst = Etendre(ref carte, coord, S_DistribEtapeInitiale);
                    nbBlocsPlaces += lst.Count;

                    lstTemp.AddRange(lst);

                    if (nbBlocsPlaces >= nbBlocsAPlacer)
                        break;
                }

                blocsATraiter = lstTemp;
            }

            superficieTraitee = nbBlocsPlaces * largeurBlocs * largeurBlocs;

            return OptimiserCarte(carte);
        }

        private static byte[][] EtapeN(byte[][] carteEtapePrecedente, int superficieMax, int largeurBlocs, out int superficieTraitee)
        {
            int nbBlocsAPlacer = superficieMax / (largeurBlocs * largeurBlocs);
            int nbBlocsPlaces = 0;

            // On initialise une nouvelle carte à partir de celle de l'étape 1.
            byte[][] carte = CreerCarteEtape(carteEtapePrecedente, 4);

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
                    List<Coordonnees> lst = Etendre(ref carte, coord, S_DistribEtapeN);
                    nbBlocsPlaces += lst.Count;

                    lstTemp.AddRange(lst);

                    if (nbBlocsPlaces >= nbBlocsAPlacer)
                        break;
                }

                blocsAEtendre = lstTemp;
            }

            // On lisse la carte.
            //Lisser(ref carte);

            superficieTraitee = nbBlocsPlaces * largeurBlocs * largeurBlocs;

            return carte;
        }

        private static void DessinerCarte(byte[][] carte, int etape, int largeurBlocs, string prefixeNomFichier)
        {
            int largeur = carte.Length;
            int hauteur = carte[0].Length;
            Bitmap image = new Bitmap(largeur * largeurBlocs, hauteur * largeurBlocs);
            
            int superficieReelle = 0;
            int superficieBloc = largeurBlocs * largeurBlocs;

            if (largeurBlocs > 1)
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    for (int i = 0; i < largeur; i++)
                        for (int j = 0; j < hauteur; j++)
                        {
                            g.FillRectangle((carte[i][j] == 1 ? Brushes.Brown : Brushes.Black), i * largeurBlocs, j * largeurBlocs, largeurBlocs, largeurBlocs);
                            superficieReelle += (carte[i][j] * superficieBloc);
                        }

                    g.DrawString(String.Format("Superficie actuelle : {0}", superficieReelle), new Font("Arial", 16), Brushes.White, 8, (hauteur * largeurBlocs) - 44);
                    g.DrawString(String.Format("{0} blocs {1} x {1}", superficieReelle / superficieBloc, largeurBlocs), new Font("Arial", 16), Brushes.White, 8, (hauteur * largeurBlocs) - 24);
                }
            }
            else
            {
                for (int i = 0; i < largeur; i++)
                    for (int j = 0; j < hauteur; j++)
                    {
                        image.SetPixel(i, j, (carte[i][j] == 1 ? Color.Brown : Color.Black));
                        superficieReelle += carte[i][j];
                    }

                using (Graphics g = Graphics.FromImage(image))
                {
                    g.DrawString(String.Format("Superficie réelle : {0}", superficieReelle), new Font("Arial", 16), Brushes.White, 8, hauteur - 24);
                }
            }

            image.Save(String.Format(@".\DebugImages\{0}_Carte_Etape_{1:00}.bmp", prefixeNomFichier, etape));
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
                    try
                    {
                        carteOptimisee[i][j] = carte[xMin + i - 2][yMin + j - 2];
                    }
                    catch
                    {
                    }
                }
            }

            return carteOptimisee;
        }

        private static byte[][] CreerCarteEtape(byte[][] carteEtapePrecedente, int multiplicateur)
        {
            int largeurCarte = carteEtapePrecedente.Length * multiplicateur;
            int hauteurCarte = carteEtapePrecedente[0].Length * multiplicateur;

            byte[][] carte = new byte[largeurCarte][];
            for (int i = 0; i < largeurCarte; i++)
            {
                carte[i] = new byte[hauteurCarte];

                for (int j = 0; j < hauteurCarte; j++)
                {
                    carte[i][j] = carteEtapePrecedente[i / multiplicateur][j / multiplicateur];
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
                for (int j = 1; j < hauteurCarte - 1; j++)
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

            // S'il n'y a aucun bloc à étendre, on assouplit la règle.
            if (blocsAEtendre.Count == 0)
            {
                for (int i = 1; i < largeurCarte - 1; i++)
                    for (int j = 1; j < hauteurCarte - 1; j++)
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

                            if (nbVides > 2)
                                blocsAEtendre.Add(new Coordonnees(i, j));
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

        #endregion -> Private

        #endregion -> Méthodes statiques
    }
}
