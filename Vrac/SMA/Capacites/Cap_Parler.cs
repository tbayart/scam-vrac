using System.Collections.Generic;
using Tools;
using Vrac.SMA.Actions;
using Vrac.SMA.Resultats;

namespace Vrac.SMA.Capacites
{
    public class Cap_Parler : Capacite
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Cap_Parler()
        {
            this.Proba = new Distribution<Resultat>() 
            { 
                DicoSeuils = new Dictionary<Resultat, double>() 
                { 
                    { Resultat.Succes, 0.99d } ,
                    { Resultat.Echec, 0.01d }
                } 
            };
            
            this.Actions = new Dictionary<Resultat, Vrac.SMA.Actions.Action>() 
            { 
                { Resultat.Echec, CatalogueActions.None }, 
                { Resultat.Succes, CatalogueActions.Parler } 
            };
        }

        #endregion --> Constructeurs
    }
}
