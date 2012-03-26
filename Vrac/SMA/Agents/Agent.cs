using System;
using System.Collections.Generic;
using Vrac.SMA.Actions;
using Vrac.SMA.Capacites;
using Vrac.SMA.Caracteristiques;
using Vrac.SMA.Comportements;
using Vrac.SMA.Evenements;
using Vrac.SMA.Resultats;
using Vrac.Tools;

namespace Vrac.SMA.Agents
{
    public class Agent
    {
        #region --> Attributs

        public Guid id;

        public Agent()
        {
            id = Guid.NewGuid();
        }

        // Les coordonnées de l'Agent.
        public Coordonnees Coord;

        // La chaîne de responsabilité des Comportement de l'Agent.
        public Comportement Chaine;

        // Le dictionnaire des Capacite de l'Agent.
        public Dictionary<NomAction, Capacite> Capacites;

        //public SortedList obj;
        public Dictionary<LesCaracteristiques, Caracteristique> caracteristiques; // Key = carac name
        //public ArrayList knPlc; // Known place
        //public List<Item> invtry;
        //public bool evalAbilitReq = false;

        #endregion --> Attributs

        #region --> Méthodes d'instance

        public void Recevoir(Evenement evt)
        {
            this.Chaine.handleOrLetTheNextDoIt(evt, this);
        }

        public void Envoyer(Evenement evt)
        {
            Kernel.managerEvenements.Poster(evt);
        }

        public Resultat Do(NomAction nomAction, Agent cible, object p)
        {
            return this.Capacites[nomAction].doIt(this, cible, p);
        }

        //public void EvalAbilitOnCar()
        //{
        //    if (!evalAbilitReq)
        //        return;

        //    // parcours abilit et refait la distrib en fonction des carac
        //}

        #endregion --> Méthodes d'instance
    }
}
