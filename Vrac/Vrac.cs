using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;

namespace Vrac
{
    public enum TypeElementBiome
    { 
        Vide, 
        Terre, 
        Sable,
        Eau 
    }

    public class Distribution<T>
    {
        public static Random R = new Random();

        public List<double> seuils;
        public List<T> results;
        
        public T get()
        {
            double d = R.NextDouble();
            int i = 0;

            while (this.seuils[i] < d) 
                i++;

            //for (int k = 0; k < seuils.Count; k++)
            //{
            //    if (k == i)
            //        seuils[k] *= 1.01;
            //    else
            //        seuils[k] *= 0.99;
            //}

            return this.results[i];
        }
    }

    public interface IBiome_Param
    {
    }

    public class Biome_Continent2D : IBiome_Param
    {
        public static Dictionary<TypeElementBiome, Distribution<TypeElementBiome>[][]> S_Voisins;

        static Biome_Continent2D()
        {
            S_Voisins = new Dictionary<TypeElementBiome, Distribution<TypeElementBiome>[][]>();

            Distribution<TypeElementBiome> bcp_Terre = new Distribution<TypeElementBiome>
            {
                seuils = new List<double>() { 0.0d, 0.0015d, 1.0d },
                results = new List<TypeElementBiome>() { TypeElementBiome.Eau, TypeElementBiome.Sable, TypeElementBiome.Terre } 
            };

            Distribution<TypeElementBiome> bcp_Eau = new Distribution<TypeElementBiome> 
            {
                seuils = new List<double>() { 0.0d, 0.0015d, 1.0d },
                results = new List<TypeElementBiome>() { TypeElementBiome.Terre, TypeElementBiome.Sable, TypeElementBiome.Eau } 
            };

            Distribution<TypeElementBiome> bcp_Sable = new Distribution<TypeElementBiome>
            {
                seuils = new List<double>() { 0.05d, 0.05d, 1.0d },
                results = new List<TypeElementBiome>() { TypeElementBiome.Eau, TypeElementBiome.Terre, TypeElementBiome.Sable }
            };

            S_Voisins[TypeElementBiome.Terre] = new[]
                {
                    new [] {bcp_Terre, bcp_Terre, bcp_Terre},
                    new [] {bcp_Terre, null,      bcp_Terre},
                    new [] {bcp_Terre, bcp_Terre, bcp_Terre},
                };

            S_Voisins[TypeElementBiome.Eau] = new[]
                {
                    new [] {bcp_Eau, bcp_Eau, bcp_Eau},
                    new [] {bcp_Eau, null,    bcp_Eau},
                    new [] {bcp_Eau, bcp_Eau, bcp_Eau},
                };

            S_Voisins[TypeElementBiome.Sable] = new[]
                {
                    new [] {bcp_Sable, bcp_Sable, bcp_Sable},
                    new [] {bcp_Sable, null,      bcp_Sable},
                    new [] {bcp_Sable, bcp_Sable, bcp_Sable},
                };
        }
    }

    public class World
    {
        private TypeElementBiome[][] _world;
            Random r = new Random();

        public World()
        {
            int l = 8192;
            this._world = new TypeElementBiome[l][];
            List<int[]> lstNouveauxVoisins = new List<int[]>();

            for (int i = 0; i < l; i++)
            {
                this._world[i] = new TypeElementBiome[l];

                for (int j = 0; j < l; j++)
                {
                    this._world[i][j] = TypeElementBiome.Vide;
                }

            }

            Distribution<TypeElementBiome> d = new Distribution<TypeElementBiome>
            {
                seuils = new List<double>() { 0.1d, 1d },
                results = new List<TypeElementBiome>() { TypeElementBiome.Eau, TypeElementBiome.Terre }
            };

            for (int i = 0; i < 64; i++)
            {
                int x = r.Next(0, l);
                int y = r.Next(0, l);

                this._world[x][y] = d.get();

                lstNouveauxVoisins.Add(new int[] {x , y});
            }

            while (lstNouveauxVoisins.Count > 0)
            {
                int i = r.Next(0, lstNouveauxVoisins.Count);
                
                int[] coord = lstNouveauxVoisins[i];
                lstNouveauxVoisins.RemoveAt(i);

                lstNouveauxVoisins.AddRange(this.etendre(coord[0], coord[1]));
            }

            // Là , il faut faire un peu de récursif pour que le spread ce passe comme ça :
            // le chiffre indique l'ordre de génération du contenu :
            // 33333
            // 32223
            // 32123
            // 32223
            // 33333
        }

        private int calculerNbElements()
        {
            int nb = 0;

            int l = this._world.Length;

            for (int i = 0; i < l; i++)
                for (int j = 0; j < l; j++)
                    if (this._world[i][j] != TypeElementBiome.Vide)
                        nb++;

            return nb;
        }

        private List<int[]> etendre(int x, int y)
        {
            Distribution<TypeElementBiome>[][] distrib = Biome_Continent2D.S_Voisins[this._world[x][y]];
            List<int[]> lstNouveauxVoisins = new List<int[]>();

            for (int offset_x = -1; offset_x <= 1; offset_x++)
            {
                for (int offset_y = -1; offset_y <= 1; offset_y++)
                {
                    if (offset_y == 0 && offset_x == 0)
                        continue;

                    //if (x + offset_x > -1 && x + offset_x < this._world.Length
                    //    && y + offset_y > -1 && y + offset_y < this._world[x].Length
                    //    && this._world[x + offset_x][y + offset_y] == TypeElementBiome.Vide)
                    //{
                    //    this._world[x + offset_x][y + offset_y] = distrib[offset_x + 1][offset_y + 1].get();
                    //    lstNouveauxVoisins.Add(new int[] { x + offset_x, y + offset_y });

                    //    // DEBUG
                    //    File.AppendAllText(@"Debug.txt", (x + offset_x) + " ; " + (y + offset_y) + Environment.NewLine);
                    //}

                    if (x + offset_x > -1 && x + offset_x < this._world.Length
                        && y + offset_y > -1 && y + offset_y < this._world[x].Length)
                    {
                        if (this._world[x + offset_x][y + offset_y] == TypeElementBiome.Vide)
                            lstNouveauxVoisins.Add(new int[] { x + offset_x, y + offset_y });

                        this._world[x + offset_x][y + offset_y] = distrib[offset_x + 1][offset_y + 1].get();
                    }
                }
            }

            return lstNouveauxVoisins;
        }

        //private void etendreVoisinsDe(int x, int y)
        //{
        //    List<int[]> lst = new List<int[]>();

        //    for (int offset_x = -1; offset_x <= 1; offset_x++)
        //    {
        //        for (int offset_y = -1; offset_y <= 1; offset_y++)
        //        {
        //            if (offset_y == 0 && offset_x == 0)
        //                continue;

        //            if (x + offset_x > -1 && x + offset_x < this._world.Length
        //                && y + offset_y > -1 && y + offset_y < this._world[x].Length
        //                && this._world[x + offset_x][y + offset_y] != TypeElementBiome.Vide)
        //            {
        //                this.etendre(x + offset_x, y + offset_y);
        //                //lst.Add(new int[] { x + offset_x, y + offset_y });
        //            }
        //        }
        //    }

        //    //Parallel.ForEach(lst, coord =>
        //    //{
        //    //    this.etendre(coord[0], coord[1]);
        //    //});
        //}

        public void ecrire()
        {
            if (this._world != null && this._world.Length > 0 && this._world[0] != null)
            {
                int hauteur = this._world.Length;
                int largeur = this._world[0].Length;

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
                image.Save(String.Format(@"Test.bmp"));
            }
        }

        private Color getColorElement(int x, int y)
        {
            TypeElementBiome t = _world[x][y];

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
    } 
}
