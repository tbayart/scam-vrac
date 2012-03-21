using System;
using Vrac.SMA.Agents;
using Vrac.SMA.Evenements;

namespace Vrac.SMA.Comportements
{
    public class Comportement
    {
        #region --> Attributs

        public Comportement Next { get; set; }

        public Action<Evenement, Agent> Handle { get; set; }
        public Func<Evenement, Agent, bool> IsHandler { get; set; }
        public Func<Evenement, Agent, bool> IsForwarder { get; set; }

        #endregion --> Attributs

        #region --> Méthodes d'instance

        public void handleOrLetTheNextDoIt(Evenement evt, Agent agent)
        {
            if (this.IsHandler(evt, agent))
            {
                this.Handle(evt, agent);

                if (this.IsForwarder(evt, agent) && this.Next!=null)
                    this.Next.handleOrLetTheNextDoIt(evt, agent);
            }
            else
            {
                if (this.Next != null)
                    this.Next.handleOrLetTheNextDoIt(evt, agent);
            }
        }

        #endregion --> Méthodes d'instance
    }
}
