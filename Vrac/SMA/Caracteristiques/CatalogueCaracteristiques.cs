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
        Maison_Y,
        Maison_X,
    }

    public class CatalogueCaracteristique
    {
        public static Caracteristique Faim(int val)
        {
            return new Caracteristique {valeur = val, nom = LesCaracteristiques.Faim.ToString() };
        }

        public static Caracteristique Sommeil(int val)
        {
            return new Caracteristique { valeur = val, nom = LesCaracteristiques.Sommeil.ToString() };
        }

        public static Caracteristique DistanceDeDeplacement(int val)
        {
            return new Caracteristique { valeur = val, nom = LesCaracteristiques.DistanceDeDeplacement.ToString() };
        }

        public static Caracteristique Solitude(int val)
        {
            return new Caracteristique { valeur = val, nom = LesCaracteristiques.Solitude.ToString() };
        }

        public static Caracteristique LenteurEsprit(int val)
        {
            return new Caracteristique { valeur = val, nom = LesCaracteristiques.LenteurEsprit.ToString() };
        }

        public static Caracteristique Maison_X(int val)
        {
            return new Caracteristique { valeur = val, nom = LesCaracteristiques.Maison_X.ToString() };
        }

        public static Caracteristique Maison_Y(int val)
        {
            return new Caracteristique { valeur = val, nom = LesCaracteristiques.Maison_Y.ToString() };
        }
    }
}
