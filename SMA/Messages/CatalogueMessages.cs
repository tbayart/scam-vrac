using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMA.Objectifs;
using SMA.Agents;

namespace SMA.Messages
{
    public class CatalogueMessages
    {
        public static Message GetMessageRealiserObjectif(TypeObjectif typeObjectif, IAgent agent)
        {
            Message msgRappel = new Message();

            msgRappel.Performatif = PerformatifMessage.RealiserObjectif;
            msgRappel.Emetteur = agent;
            msgRappel.Destinataires = new List<IAgent>();
            msgRappel.Destinataires.Add(agent);
            msgRappel.Contenu = ContenuMessage.GetContenu_RealiserObjectif(typeObjectif);

            return msgRappel;
        }
    }
}
