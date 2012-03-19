using System.Collections.Generic;

namespace Vrac.GenerateurCarte.Biomes
{
    public class Biome_Continent2D : IBiome_Param
    {
        #region -> Attributs statiques

        /// <summary>
        /// Les distributions pour les voisins d'un type d'élément du biome.
        /// </summary>
        public static Dictionary<TypeElementBiome, Distribution<TypeElementBiome>[][]> S_DistributionVoisins;

        #endregion -> Attributs statiques

        #region -> Constructeurs statique

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static Biome_Continent2D()
        {
            S_DistributionVoisins = new Dictionary<TypeElementBiome, Distribution<TypeElementBiome>[][]>();

            // On définit les différentes distributions possibles.

            Distribution<TypeElementBiome> bcp_Terre = new Distribution<TypeElementBiome>
            {
                seuils = new List<double>() { 0.0d, 0.0003d, 1.0d },
                results = new List<TypeElementBiome>() { TypeElementBiome.Eau, TypeElementBiome.Sable, TypeElementBiome.Terre }
            };

            Distribution<TypeElementBiome> bcp_Eau = new Distribution<TypeElementBiome>
            {
                seuils = new List<double>() { 0.0d, 0.0005d, 1.0d },
                results = new List<TypeElementBiome>() { TypeElementBiome.Terre, TypeElementBiome.Sable, TypeElementBiome.Eau }
            };

            Distribution<TypeElementBiome> bcp_Sable = new Distribution<TypeElementBiome>
            {
                seuils = new List<double>() { 0.02d, 0.04d, 1.0d },
                results = new List<TypeElementBiome>() { TypeElementBiome.Eau, TypeElementBiome.Terre, TypeElementBiome.Sable }
            };

            // On définit le dictionnaire des distributions des voisins.

            S_DistributionVoisins[TypeElementBiome.Terre] = new[]
                {
                    new [] {bcp_Terre, bcp_Terre, bcp_Terre},
                    new [] {bcp_Terre, null,      bcp_Terre},
                    new [] {bcp_Terre, bcp_Terre, bcp_Terre},
                };

            S_DistributionVoisins[TypeElementBiome.Eau] = new[]
                {
                    new [] {bcp_Eau, bcp_Eau, bcp_Eau},
                    new [] {bcp_Eau, null,    bcp_Eau},
                    new [] {bcp_Eau, bcp_Eau, bcp_Eau},
                };

            S_DistributionVoisins[TypeElementBiome.Sable] = new[]
                {
                    new [] {bcp_Sable, bcp_Sable, bcp_Sable},
                    new [] {bcp_Sable, null,      bcp_Sable},
                    new [] {bcp_Sable, bcp_Sable, bcp_Sable},
                };
        }

        #endregion -> Constructeurs statique
    }
}
