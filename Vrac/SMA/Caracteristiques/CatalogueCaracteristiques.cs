using System;
using Vrac.SMA.Actions;
using Vrac.SMA.Caracteristiques;
using Vrac.SMA.Evenements;
using Vrac.SMA.Resultats;
using Vrac.Tools;

namespace Vrac.SMA.Comportements
{
    public enum LesCaracteristiques
    {
        Faim,
        Sommeil,
        DistanceDeDeplacement,
        Solitude,
        LenteurEsprit,
    }

    public class CatalogueCaracteristique
    {
        public static Caracteristique Faim()
        {
            return new Caracteristique { nom = LesCaracteristiques.Faim.ToString() };
        }

        public static Caracteristique Sommeil()
        {
            return new Caracteristique { nom = LesCaracteristiques.Sommeil.ToString() };
        }

        public static Caracteristique DistanceDeDeplacement()
        {
            return new Caracteristique { nom = LesCaracteristiques.DistanceDeDeplacement.ToString() };
        }

        public static Caracteristique Solitude()
        {
            return new Caracteristique { nom = LesCaracteristiques.Solitude.ToString() };
        }

        public static Caracteristique LenteurEsprit()
        {
            return new Caracteristique { nom = LesCaracteristiques.LenteurEsprit.ToString() };
        }
    }
}
