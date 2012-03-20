﻿using System.Collections.Generic;
using Vrac.SMA.Actions;
using Vrac.SMA.Resultats;
using Vrac.Tools;

namespace Vrac.SMA.Capacites
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
                    { Resultat.Critique, 0.0001d }, 
                    { Resultat.Echec, 0.01d }, 
                    { Resultat.Succes, 1.0d - 0.01d - 0.0001d } 
                } 
            };

            this.Actions = new Dictionary<Resultat, Vrac.SMA.Actions.Action>() 
            { 
                { Resultat.Critique, CatalogueActions.Mourir }, 
                { Resultat.Echec, CatalogueActions.None }, 
                { Resultat.Succes, CatalogueActions.Planter } 
            };
        }

        #endregion --> Constructeurs
    }
}