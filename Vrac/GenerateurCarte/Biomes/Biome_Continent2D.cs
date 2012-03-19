using System.Collections.Generic;

namespace Vrac.GenerateurCarte.Biomes
{
    public class Biome_Continent2D : IBiome_Param
    {
        #region -> Attributs statiques

        /// <summary>
        /// Les distributions pour les voisins d'un type d'élément du biome.
        /// </summary>
        //public static Dictionary<TypeElementBiome, Distribution<TypeElementBiome>[][]> S_DistributionVoisins;

        private static Dictionary<TypeElementBiome, Distribution<TypeElementBiome>> S_Distributions;

        #endregion -> Attributs statiques

        #region -> Constructeurs statique

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static Biome_Continent2D()
        {
            //S_DistributionVoisins = new Dictionary<TypeElementBiome, Distribution<TypeElementBiome>[][]>();
            S_Distributions = new Dictionary<TypeElementBiome, Distribution<TypeElementBiome>>();

            // On définit les différentes distributions possibles.

            Distribution<TypeElementBiome> bcp_Terre = new Distribution<TypeElementBiome>();
            bcp_Terre.dicoSeuils[TypeElementBiome.Terre] = 0.9997d;
            bcp_Terre.dicoSeuils[TypeElementBiome.Pierre] = 0.0003d;
            bcp_Terre.dicoSeuils[TypeElementBiome.Eau] = 0.0d;
            bcp_Terre.dicoSeuils[TypeElementBiome.Sable] = 0.0d;
            S_Distributions[TypeElementBiome.Terre] = bcp_Terre;

            Distribution<TypeElementBiome> bcp_Eau = new Distribution<TypeElementBiome>();
            bcp_Eau.dicoSeuils[TypeElementBiome.Eau] = 0.9995d;
            bcp_Eau.dicoSeuils[TypeElementBiome.Sable] = 0.0005d;
            bcp_Eau.dicoSeuils[TypeElementBiome.Terre] = 0.0d;
            bcp_Eau.dicoSeuils[TypeElementBiome.Pierre] = 0.0d;
            S_Distributions[TypeElementBiome.Eau] = bcp_Eau;

            Distribution<TypeElementBiome> bcp_Sable = new Distribution<TypeElementBiome>();
            bcp_Sable.dicoSeuils[TypeElementBiome.Eau] = 0.02d;
            bcp_Sable.dicoSeuils[TypeElementBiome.Terre] = 0.02d;
            bcp_Sable.dicoSeuils[TypeElementBiome.Sable] = 0.96d;
            bcp_Sable.dicoSeuils[TypeElementBiome.Pierre] = 0.0d;
            S_Distributions[TypeElementBiome.Sable] = bcp_Sable;

            Distribution<TypeElementBiome> bcp_Pierre = new Distribution<TypeElementBiome>();
            bcp_Pierre.dicoSeuils[TypeElementBiome.Eau] = 0.000001d;
            bcp_Pierre.dicoSeuils[TypeElementBiome.Terre] = 0.000499d;
            bcp_Pierre.dicoSeuils[TypeElementBiome.Pierre] = 0.9995d;
            bcp_Pierre.dicoSeuils[TypeElementBiome.Sable] = 0.0d;
            S_Distributions[TypeElementBiome.Pierre] = bcp_Pierre;

            // On définit le dictionnaire des distributions des voisins.

            //S_DistributionVoisins[TypeElementBiome.Terre] = new[]
            //    {
            //        new [] {bcp_Terre, bcp_Terre, bcp_Terre},
            //        new [] {bcp_Terre, null,      bcp_Terre},
            //        new [] {bcp_Terre, bcp_Terre, bcp_Terre},
            //    };

            //S_DistributionVoisins[TypeElementBiome.Eau] = new[]
            //    {
            //        new [] {bcp_Eau, bcp_Eau, bcp_Eau},
            //        new [] {bcp_Eau, null,    bcp_Eau},
            //        new [] {bcp_Eau, bcp_Eau, bcp_Eau},
            //    };

            //S_DistributionVoisins[TypeElementBiome.Sable] = new[]
            //    {
            //        new [] {bcp_Sable, bcp_Sable, bcp_Sable},
            //        new [] {bcp_Sable, null,      bcp_Sable},
            //        new [] {bcp_Sable, bcp_Sable, bcp_Sable},
            //    };

            //S_DistributionVoisins[TypeElementBiome.Pierre] = new[]
            //    {
            //        new [] {bcp_Pierre, bcp_Pierre, bcp_Pierre},
            //        new [] {bcp_Pierre, null,       bcp_Pierre},
            //        new [] {bcp_Pierre, bcp_Pierre, bcp_Pierre},
            //    };
        }

        #endregion -> Constructeurs statique

        public static Distribution<TypeElementBiome>[][] GetDistributionVoisins(TypeElementBiome[][] region)
        {
            Distribution<TypeElementBiome> dist = new Distribution<TypeElementBiome>();
            dist.dicoSeuils[TypeElementBiome.Eau] = 0;
            dist.dicoSeuils[TypeElementBiome.Terre] = 0;
            dist.dicoSeuils[TypeElementBiome.Sable] = 0;
            dist.dicoSeuils[TypeElementBiome.Pierre] = 0;
            int nbElements = 0;

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    TypeElementBiome e = region[i][j];

                    if (e != TypeElementBiome.Vide)
                    {
                        nbElements++;
                        dist.dicoSeuils[TypeElementBiome.Eau] += S_Distributions[e].dicoSeuils[TypeElementBiome.Eau];
                        dist.dicoSeuils[TypeElementBiome.Terre] += S_Distributions[e].dicoSeuils[TypeElementBiome.Terre];
                        dist.dicoSeuils[TypeElementBiome.Sable] += S_Distributions[e].dicoSeuils[TypeElementBiome.Sable];
                        dist.dicoSeuils[TypeElementBiome.Pierre] += S_Distributions[e].dicoSeuils[TypeElementBiome.Pierre];
                    }
                }

            dist.dicoSeuils[TypeElementBiome.Eau] /= nbElements;
            dist.dicoSeuils[TypeElementBiome.Terre] /= nbElements;
            dist.dicoSeuils[TypeElementBiome.Sable] /= nbElements;
            dist.dicoSeuils[TypeElementBiome.Pierre] /= nbElements;

            return new[]
                {
                    new [] {dist, dist, dist},
                    new [] {dist, null, dist},
                    new [] {dist, dist, dist}
                };
        }
    }
}
