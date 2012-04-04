using System.Collections.Generic;
using SMA.Actions;
using SMA.Resultats;
using Tools;

namespace SMA.Capacites
{
    public class Cap_Construire : Capacite
    {
        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Cap_Construire()
        {
            this.Proba = new Distribution<Resultat>() 
            { 
                DicoSeuils = new Dictionary<Resultat, double>() 
                { 
                    { Resultat.Succes, 0.8d }, 
                    { Resultat.Echec, 0.2 } ,
                } 
            };

            this.Actions = new Dictionary<Resultat, Action>() 
            { 
                { Resultat.Echec, CatalogueActions.None }, 
                { Resultat.Succes, CatalogueActions.Construire } 
            };
        }

        #endregion --> Constructeurs
    }
}
