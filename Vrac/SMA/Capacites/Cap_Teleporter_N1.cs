using System.Collections.Generic;
using Tools;
using Vrac.SMA.Actions;
using Vrac.SMA.Resultats;

namespace Vrac.SMA.Capacites
{
    public class Cap_Teleporter_N1 : Capacite
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Cap_Teleporter_N1()
        {
            this.Proba = new Distribution<Resultat>() 
            { 
                DicoSeuils = new Dictionary<Resultat, double>() 
                { 
                    { Resultat.Succes, 0.8d },
                    { Resultat.Echec, 0.2d }
                }
            };

            this.Actions = new Dictionary<Resultat, Vrac.SMA.Actions.Action>() 
            { 
                { Resultat.Echec, CatalogueActions.None }, 
                { Resultat.Succes, CatalogueActions.Teleporter } 
            };
        }

        #endregion --> Constructeurs
    }
}
