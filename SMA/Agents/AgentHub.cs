using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using SMA.Actions;
using SMA.Agents.Caracteristiques;
using SMA.Capacites;
using SMA.Comportements;
using SMA.Messages;
using SMA.Objectifs;
using SMA.Resultats;
using Tools;

namespace SMA.Agents
{
    public class AgentHub : IAgent, IMessageManager
    {
        #region -> Constantes

        public const int RAYON_ACTION = 256;

        private const int SLEEP_SIGNAUX = 1500;

        #endregion -> Constantes

        #region -> Attributs

        // Objet verrou de l'instance.
        private readonly object _locker = new object();

        // Boite aux lettres des événements reçus.
        private Queue<Message> _boiteAuxLettres;

        // Booléen gérant la boucle d'émission.
        private bool _continuerEmission;

        #endregion -> Attributs

        #region -> Propriétés

        // L'id du AgentHub.
        public string Id { get; protected set; }

        // Les coordonnées du AgentHub.
        public Coordonnees Coord { get; protected set; }

        // Les caractéristiques du AgentHub.
        public Dictionary<NomCaracteristique, Caracteristique> Caracteristiques { get; protected set; }


        // La chaîne de responsabilité du comportement du AgentHub.
        public Comportement ChaineComportement { get; protected set; }

        // Le dictionnaire des capacités du AgentHub.
        public Dictionary<NomAction, Capacite> Capacites { get; protected set; }

        // L'état actuel du AgentHub.
        public EtatAgent Etat { get; protected set; }

        // Le monde auquel appartient le AgentHub.
        public Monde World { get; protected set; }

        // Les objectifs du AgentHub.
        public Dictionary<TypeObjectif, Objectif> Objectifs { get; protected set; }

        // Les connaissances acquises pour la réalisation des objectifs.
        public Dictionary<TypeObjectif, Hashtable> ConnaissancesObjectifs { get; protected set; }

        #endregion -> Propriétés

        #region -> Constructeurs

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="id">L'id de l'agent hub.</param>
        /// <param name="coord">Les coordonnées de l'agent hub.</param>
        /// <param name="monde">Le monde auquel appartient l'agent hub.</param>
        public AgentHub(string id, Coordonnees coord, Monde monde)
        {
            this._boiteAuxLettres = new Queue<Message>();
            this._continuerEmission = true;

            this.Id = id;
            this.Coord = coord;
            this.World = monde;
        }

        #endregion -> Constructeurs

        #region -> Méthodes privées

        /// <summary>
        /// Démarre la boucle d'écoute.
        /// </summary>
        private void demarrerThreadEcoute()
        {
            Console.WriteLine("Hub {0} démarré", this.Id);

            this.consommer();
        }

        /// <summary>
        /// Démarre la boucle d'emission de signaux.
        /// </summary>
        private void demarrerThreadEmission()
        {
            List<Agent> agents;

            Message msg = new Message();
            msg.Performatif = PerformatifMessage.SignalHub;
            msg.Emetteur = this;

            while (this._continuerEmission)
            {
                Thread.Sleep(SLEEP_SIGNAUX);

                lock (this._locker)
                {
                    agents = this.World.Agents.GetAgents(this.Coord, RAYON_ACTION);
                }

                for (int i = 0; i < agents.Count; i++)
                {
                    agents[i].poster(msg);
                }
            }
        }

        #endregion -> Méthodes privées

        #region -> Membres de IAgent

        public Resultat FaireAction(NomAction nomAction, IAgent cible, object p)
        {
            return this.Capacites[nomAction].doIt(this, cible, p);
        }

        #endregion -> Membres de IAgent

        #region -> Membres de IMessageManager

        /// <summary>
        /// Démarre l'agent hub dans un nouveau thread.
        /// </summary>
        public void start()
        {
            // Démarrage du thread d'écoute.
            Thread tEcoute = new Thread(this.demarrerThreadEcoute);
            tEcoute.Start();

            // Démarrage du thread d'émission.
            Thread tEmission = new Thread(this.demarrerThreadEmission);
            tEmission.Start();
        }

        /// <summary>
        /// Arrête le thread de l'agent hub.
        /// </summary>
        public void stop()
        {
            // Arrêt du thread d'émission.
            this._continuerEmission = false;

            // Arrêt du thread d'écoute.
            this.poster(null);
        }

        public void poster(Message msg)
        {
            lock (this._locker)
            {
                this._boiteAuxLettres.Enqueue(msg);

                // On appelle 'Pulse(..)' car on modifie une condition de blocage.
                Monitor.Pulse(this._locker);
            }
        }

        public void consommer()
        {
            while (true)
            {
                Message item;

                lock (this._locker)
                {
                    while (this._boiteAuxLettres.Count == 0)
                        Monitor.Wait(this._locker);

                    item = this._boiteAuxLettres.Dequeue();
                }

                // Signal de sortie.
                if (item == null)
                    return;

                // Action de l'événement.
                this.faire(item);
            }
        }

        public void faire(Message msg)
        {
        }

        #endregion -> Membres de IMessageManager
    }
}
