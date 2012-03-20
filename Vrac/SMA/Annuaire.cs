using System.Collections.Generic;
using Vrac.SMA.Agents;

namespace Vrac.SMA
{
    public class Annuaire
    {
        #region --> Attributs

        // La liste de tous les Agent instanciés.
        public List<Agent> Agents { get; set; }

        #endregion --> Attributs

        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Annuaire()
        {
            this.Agents = new List<Agent>();
        }
        
        #endregion --> Constructeurs

        #region --> Méthodes d'instance

        public void add(Agent agent) 
        { 
            this.Agents.Add(agent); 
        }

        public void del(Agent agent) 
        { 
            this.Agents.Remove(agent); 
        }

        #endregion --> Méthodes d'instance
    }
}
