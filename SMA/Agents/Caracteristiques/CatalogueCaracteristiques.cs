namespace SMA.Agents.Caracteristiques
{
    public class CatalogueCaracteristiques
    {
        public static Caracteristique Faim(int val)
        {
            return new Caracteristique {Valeur = val, Nom = NomCaracteristique.Faim.ToString() };
        }

        public static Caracteristique Sommeil(int val)
        {
            return new Caracteristique { Valeur = val, Nom = NomCaracteristique.Sommeil.ToString() };
        }

        public static Caracteristique DistanceDeDeplacement(int val)
        {
            return new Caracteristique { Valeur = val, Nom = NomCaracteristique.DistanceDeDeplacement.ToString() };
        }

        public static Caracteristique Solitude(int val)
        {
            return new Caracteristique { Valeur = val, Nom = NomCaracteristique.Solitude.ToString() };
        }

        public static Caracteristique LenteurEsprit(int val)
        {
            return new Caracteristique { Valeur = val, Nom = NomCaracteristique.LenteurEsprit.ToString() };
        }

        public static Caracteristique Maison_X(int val)
        {
            return new Caracteristique { Valeur = val, Nom = NomCaracteristique.Maison_X.ToString() };
        }

        public static Caracteristique Maison_Y(int val)
        {
            return new Caracteristique { Valeur = val, Nom = NomCaracteristique.Maison_Y.ToString() };
        }
    }
}
