﻿using System.Collections.Generic;
using Tools;
using Vrac.SMA.Actions;
using Vrac.SMA.Resultats;

namespace Vrac.SMA.Capacites
{
    public class Cap_Retirer : Capacite
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Cap_Retirer()
        {
            this.Proba = new Distribution<Resultat>() 
            { 
                DicoSeuils = new Dictionary<Resultat, double>() 
                { 
                    { Resultat.Echec, 0.999d }, 
                    { Resultat.Succes, 0.001d }
                } 
            };
            
            this.Actions = new Dictionary<Resultat, Vrac.SMA.Actions.Action>() 
            { 
                 { Resultat.Echec, CatalogueActions.None }, 
                 { Resultat.Succes, CatalogueActions.Mourir }
            };
        }

        #endregion --> Constructeurs
    }
}
