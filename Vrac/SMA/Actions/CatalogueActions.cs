using System;
using ScamCarte.Cartes;
using Tools;
using Vrac.SMA.Evenements;

namespace Vrac.SMA.Actions
{
    public class CatalogueActions
    {
        // TODO: Voir pour ne pas référencer directement Vrac.GenerateurCarte.TypeElementBiome.
        // --> Déplacer TypeElementBiome ?

        public static Action Teleporter;
        public static Action Planter;
        public static Action Mourir;
        public static Action None;
        public static Action Ecouter;
        public static Action Parler;
        public static Action Construire;
        public static Action ConstruireRoute;

        #region --> Constructeurs

        /// <summary>
        /// Constructeur statique.
        /// </summary>
        static CatalogueActions()
        {
            // Initialisation des Action statiques.
            Teleporter = new Action()
            {
                doIt = (acteur, cible, coord) =>
                {
                    Coordonnees c = ((Coordonnees)coord);
                    cible.Coord.X = c.X;
                    cible.Coord.Y = c.Y;
                }
            };

            Planter = new Action()
            {
                doIt = (acteur, cible, coord) =>
                {
                    Coordonnees c = ((Coordonnees)coord);
                    Kernel.CarteManipulee.Elements[c.X][c.Y].ElementBiome = TypeElementBiome.Arbre;
                }
            };

            ConstruireRoute = new Action()
            {
                doIt = (acteur, cible, coord) =>
                {
                    Coordonnees c = ((Coordonnees)coord);
                    try
                    {
                        Kernel.CarteManipulee.Elements[c.X][c.Y].ElementBiome = TypeElementBiome.Route;
                        
                    }
                    catch (Exception ex)
                    {
                    }
                }
            };

            Construire = new Action()
            {
                doIt = (acteur, cible, coord) =>
                {
                    Coordonnees c = ((Coordonnees)coord);
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (c.X + i > 0
                                && c.Y + j > 0
                                && c.X + i < Kernel.CarteManipulee.Largeur
                                && c.Y + j < Kernel.CarteManipulee.Hauteur
                                && (i!=0 && j!=0)
                                )
                                Kernel.CarteManipulee.Elements[c.X + i][c.Y + j].ElementBiome = TypeElementBiome.Maison;
                        }
                    }
                }
            };

            Mourir = new Action()
            {
                doIt = (acteur, cible, coord) => { }
            };

            None = new Action()
            {
                doIt = (acteur, cible, coord) => { }
            };

            Ecouter = new Action()
            {
                doIt = (acteur, cible, evt) => acteur.Recevoir((Evenement)evt)
            };

            Parler = new Action()
            {
                doIt = (acteur, cible, evt) => acteur.Envoyer((Evenement)evt)
            };
        }

        #endregion --> Constructeurs
    }
}
