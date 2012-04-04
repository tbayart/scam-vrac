using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMA.Agents;

namespace SMA.Messages
{
    public class Message
    {
        #region --> Attributs

        /// <summary>
        /// Type de l’acte communicatif.
        /// </summary>
        public PerformatifMessage Performatif;

        /// <summary>
        /// Emetteur du message.
        /// </summary>
        public IAgent Emetteur;

        /// <summary>
        /// Destinataires du message.
        /// </summary>
        public List<IAgent> Destinataires;

        /// <summary>
        /// Le type des destinataires du message.
        /// </summary>
        public Type TypeDestinataires;

        /// <summary>
        /// Destinataires de la réponse au message.
        /// </summary>
        //public List<IAgent> DestinatairesReponse;

        /// <summary>
        /// Paramètres passés avec le message.
        /// </summary>
        public object[] Contenu;

        /// <summary>
        /// Identifiant de la conversation.
        /// </summary>
        //public Guid IdConversation;

        /// <summary>
        /// Type de réponse souhaitée.
        /// </summary>
        //public PerformatifMessage? PerformatifReponseSouhaitee;

        #endregion --> Attributs
    }
}
