using System.Collections.Generic;
using SMA.Actions;
using SMA.Resultats;
using Tools;

namespace SMA.Capacites
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
            
            this.Actions = new Dictionary<Resultat, Action>() 
            { 
                 { Resultat.Echec, CatalogueActions.None }, 
                 { Resultat.Succes, CatalogueActions.Mourir }
            };
        }

        #endregion --> Constructeurs
    }
}
