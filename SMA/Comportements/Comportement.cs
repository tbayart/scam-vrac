using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMA.Agents;
using SMA.Messages;

namespace SMA.Comportements
{
    public class Comportement
    {
        #region -> Attributs

        public Comportement Next { get; set; }

        public Action<Message, IAgent> Handle { get; set; }
        public Func<Message, IAgent, bool> IsHandler { get; set; }
        public Func<Message, IAgent, bool> IsForwarder { get; set; }

        #endregion -> Attributs

        #region -> Méthodes d'instance

        public void handleOrLetTheNextDoIt(Message msg, IAgent agent)
        {
            if (this.IsHandler(msg, agent))
            {
                this.Handle(msg, agent);

                if (this.IsForwarder(msg, agent) && this.Next != null)
                    this.Next.handleOrLetTheNextDoIt(msg, agent);
            }
            else
            {
                if (this.Next != null)
                    this.Next.handleOrLetTheNextDoIt(msg, agent);
            }
        }

        #endregion -> Méthodes d'instance
    }
}
