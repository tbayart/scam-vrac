using System.Collections.Generic;
using Vrac.SMA.Actions;
using Vrac.SMA.Resultats;
using Vrac.Tools;

namespace Vrac.SMA.Capacites
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

            this.Actions = new Dictionary<Resultat, Vrac.SMA.Actions.Action>() 
            { 
                { Resultat.Succes, CatalogueActions.ConstruireRoute } 
            };
        }

        #endregion --> Constructeurs
    }
}
