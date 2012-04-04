using System.Collections.Generic;
using SMA.Actions;
using SMA.Resultats;
using Tools;

namespace SMA.Capacites
{
    public class Cap_Teleporter_N2 : Capacite
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Cap_Teleporter_N2()
        {
            this.Proba = new Distribution<Resultat>() 
            { 
                DicoSeuils = new Dictionary<Resultat, double>() 
                { 
                    { Resultat.Succes, 1.0d} ,
                } 
            };

            this.Actions = new Dictionary<Resultat, Action>() 
            { 
                { Resultat.Succes, CatalogueActions.Teleporter } 
            };
        }

        #endregion --> Constructeurs
    }
}
