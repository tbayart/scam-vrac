using System;
using System.Collections.Generic;
using System.Threading;
using SMA.Messages;

namespace SMA.Agents
{
    public class Relai : IMessageManager
    {
        #region -> Attributs

        // Objet verrou de l'instance.
        private readonly object _locker = new object();

        // Boite aux lettres des messages reçus.
        private Queue<Message> _boiteAuxLettres;

        // Booléen gérant la boucle des abonnements.
        private bool _continuerGestionAbonnements;

        #endregion -> Attributs

        #region -> Propriétés

        // L'id du relai.
        public string Id { get; protected set; }

        // Le monde auquel appartient le relai.
        public Monde World { get; protected set; }

        #endregion -> Propriétés

        #region -> Constructeurs

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="id">L'id du relai.</param>
        /// <param name="monde">Le monde auquel appartient le relai.</param>
        public Relai(string id, Monde monde)
        {
            this._boiteAuxLettres = new Queue<Message>();
            this._continuerGestionAbonnements = true;

            this.Id = id;
            this.World = monde;
        }

        #endregion -> Constructeurs

        #region -> Méthodes privées

        /// <summary>
        /// Démarre la boucle d'écoute.
        /// </summary>
        private void demarrerThreadEcoute()
        {
            Console.WriteLine("Relai {0} démarré", this.Id);

            this.consommer();
        }

        /// <summary>
        /// Démarre la boucle des abonnements.
        /// </summary>
        private void demarrerThreadAbonnement()
        {
            while (this._continuerGestionAbonnements)
            {
                //lock (this._locker)
                //{
                //}

                Thread.Sleep(100);
            }
        }

        #endregion -> Méthodes privées

        #region -> Membres de IMessageManager

        /// <summary>
        /// Démarre l'agent hub dans un nouveau thread.
        /// </summary>
        public void start()
        {
            // Démarrage du thread d'écoute.
            Thread tEcoute = new Thread(this.demarrerThreadEcoute);
            tEcoute.Start();

            // Démarrage du thread des abonnements.
            Thread tAbonnement = new Thread(this.demarrerThreadAbonnement);
            tAbonnement.Start();
        }

        /// <summary>
        /// Arrête le thread de l'agent hub.
        /// </summary>
        public void stop()
        {
            // Arrêt du thread des abonnements.
            this._continuerGestionAbonnements = false;

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
