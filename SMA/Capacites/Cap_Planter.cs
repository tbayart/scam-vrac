﻿using System.Collections.Generic;
using SMA.Actions;
using SMA.Resultats;
using Tools;

namespace SMA.Capacites
{
    public class Cap_Planter : Capacite
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Cap_Planter()
        {
            this.Proba = new Distribution<Resultat>() 
            { 
                DicoSeuils = new Dictionary<Resultat, double>() 
                { 
                    { Resultat.Succes, 0.9899d } ,
                    { Resultat.Echec, 0.01d }, 
                    { Resultat.Critique, 0.0001d }
                } 
            };

            this.Actions = new Dictionary<Resultat, Action>() 
            { 
                { Resultat.Critique, CatalogueActions.Mourir }, 
                { Resultat.Echec, CatalogueActions.None }, 
                { Resultat.Succes, CatalogueActions.Planter } 
            };
        }

        #endregion --> Constructeurs
    }
}
