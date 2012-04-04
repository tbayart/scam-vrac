using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMA.Messages
{
    public enum PerformatifMessage
    {
        DefinirObjectif,
        RealiserObjectif,
        Informer,
        Requete,
        SignalHub
    }

    //AccepterPropositon,
    //Annuler,
    //AnnulerConfirmation,
    //AppelerProposition,
    //Approuver,
    //Confirmer,
    //DemanderSi,
    ////DemanderRef,  ??
    //Echec,
    //InformerSi,
    ////InformerRef,  ??
    //NonCompris,
    //Propager,
    //Proposer,
    //Proxy,
    //Refuser,
    //RejeterProposition,
    //RequeteWhen,
    //RequeteWhenever,
    //Souscrire

    public enum TypeRequete
    {
        GenererAgents
    }
}
