using System.Collections.Generic;
using SMA.Actions;
using SMA.Resultats;
using Tools;

namespace SMA.Capacites
{
    public class Cap_ConstruireRoute : Capacite
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Cap_ConstruireRoute()
        {
            this.Proba = new Distribution<Resultat>() 
            { 
                DicoSeuils = new Dictionary<Resultat, double>() 
                { 
                    { Resultat.Succes, 1d }, 
                } 
            };

            this.Actions = new Dictionary<Resultat, Action>() 
            { 
                { Resultat.Succes, CatalogueActions.ConstruireRoute } 
            };
        }

        #endregion --> Constructeurs
    }
}
