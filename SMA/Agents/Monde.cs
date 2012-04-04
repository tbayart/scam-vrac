using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using ScamCarte.Cartes;
using SMA.Messages;
using Tools;

namespace SMA.Agents
{
    public class Monde : IMessageManager
    {
        #region -> Attributs

        // Objet verrou de l'instance.
        private readonly object _locker = new object();
        
        // Boite aux lettres des événements reçus.
        Queue<Message> _boiteAuxLettres = new Queue<Message>();

        #endregion -> Attributs

        #region -> Propriétés

        /// <summary>
        /// L'id du monde.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// La carte utilisée.
        /// </summary>
        public Carte Map { get; private set; }

        /// <summary>
        /// L'annuaire des agents hubs.
        /// </summary>
        //public Annuaire<AgentHub> Hubs { get; set; }

        public List<Relai> Relais { get; private set; }

        /// <summary>
        /// L'annuaire des agents.
        /// </summary>
        public Annuaire<Agent> Agents { get; set; }

        #endregion -> Propriétés

        #region -> Constructeurs

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="map">La carte manipulée par l'instance du Monde.</param>
        public Monde(Carte map)
        {
            this.Id = "MND";
            this.Map = map;
            //this.Hubs = new Annuaire<AgentHub>();
            this.Relais = new List<Relai>();
            this.Agents = new Annuaire<Agent>();
        }

        #endregion -> Constructeurs

        #region -> Méthodes privées

        /// <summary>
        /// Démarre les threads des hubs puis la boucle d'écoute.
        /// </summary>
        private void demarrerThreadEcoute()
        {
            //this.demarrerHubs();

            Console.WriteLine("Monde démarré");

            this.consommer();
        }

        private void demarrerThreadControleCharge()
        {
            while (true)
            {

                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Génère et démarre les hubs nécessaires à la carte.
        /// </summary>
        private void demarrerHubs()
        {
            // On va créer les hubs suivant 2 grilles :
            //   - 1 grille X x Y
            //       avec X = (largeur carte / diamètre hub) + 1
            //            Y = (hauteur carte / diamètre hub) + 1
            //            Centre 1er cercle = (rayon hub ; rayon hub)
            //
            //   - 1 grille (X+1) x (Y+1)
            //       avec Centre 1er cercle = (0 ; 0)

            /*
            int rayonHub = AgentHub.RAYON_ACTION;
            int diametreHub = rayonHub * 2;

            int x = (this.Map.Largeur / diametreHub) + 1;
            int y = (this.Map.Hauteur / diametreHub) + 1;

            // 1ère grille.
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    AgentHub hub = new AgentHub(String.Format("HUB-1-{0:00}-{1:00}", i, j),
                        new Coordonnees(rayonHub + (i * diametreHub), rayonHub + (j * diametreHub)), this);

                    hub.start();

                    this.Hubs.Ajouter(hub);
                }

            // 2ème grille.
            x++;
            y++;

            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    AgentHub hub = new AgentHub(String.Format("HUB-2-{0:00}-{1:00}", i, j),
                        new Coordonnees(i * diametreHub, j * diametreHub), this);

                    hub.start();

                    this.Hubs.Ajouter(hub);
                }
            */
        }

        #endregion -> Méthodes privées

        #region -> Membres de IMessageManager

        /// <summary>
        /// Démarre le monde dans un nouveau thread.
        /// </summary>
        public void start()
        {
            Thread tEcoute = new Thread(this.demarrerThreadEcoute);
            tEcoute.Start();

            //Thread tControleCharge = new Thread(this.demarrerThreadControleCharge);
            //tControleCharge.Start();
        }

        /// <summary>
        /// Arrête le thread du monde et tous les threads des agents hubs.
        /// </summary>
        public void stop()
        {
            /*
            foreach (AgentHub hub in this.Hubs.PagesBlanches)
            {
                hub.stop();
            }
            */

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
            if (msg.Performatif == PerformatifMessage.Requete)
            {
                this.traiterMessageRequete(msg);
            }
            else if (msg.Performatif == PerformatifMessage.DefinirObjectif)
            {
                this.traiterMessageDefinirObjectif(msg);
            }
            else if (msg.Performatif == PerformatifMessage.RealiserObjectif)
            {
                this.traiterMessageRealiserObjectif(msg);
            }
        }

        #endregion -> Membres de IMessageManager


        private void traiterMessageRequete(Message msg)
        {
            // Si Destinataires = null, le message s'adresse au monde.
            if (msg.Destinataires == null)
            {
                TypeRequete typeRequete = (TypeRequete)msg.Contenu[0];

                if (typeRequete == TypeRequete.GenererAgents)
                {
                    this.genererAgents((int)msg.Contenu[1], (Type)msg.Contenu[2]);
                }
                //this.test((int)msg.Contenu[0]);
            }
        }

        private void traiterMessageDefinirObjectif(Message msg)
        {
            int xMax = this.Map.Largeur - 9;
            int yMax = this.Map.Hauteur - 9;

            if (msg.TypeDestinataires.GetInterface("IAgent") != null)
            {
                int nbAgents = (int)msg.Contenu[0];

                if (nbAgents == -1)
                    foreach (Agent agt in this.Agents.PagesBlanches(msg.TypeDestinataires))
                    {
                        agt.poster(msg);
                    }
            }
        }

        private void traiterMessageRealiserObjectif(Message msg)
        {
            Agent dest = (Agent)msg.Destinataires[0];

            dest.poster(msg);
        }

        
        private void genererAgents(int nbAgents, Type typeAgent)
        {
            if (typeAgent.GetInterface("IAgent") == null)
                throw new Exception("Le type n'implémente pas l'interface IAgent.");

            int xMax = this.Map.Largeur - 9;
            int yMax = this.Map.Hauteur - 9;

            //AgentTest fixe = new AgentTest(String.Format("AGT-TEST-FIXE"),
            //        new Coordonnees(511, 255), this);
            //this.Agents.Ajouter(fixe);

            for (int i = 1; i <= nbAgents; i++)
            {
                ConstructorInfo ci = typeAgent.GetConstructor(new[] { typeof(string), typeof(Coordonnees), typeof(Monde) });
                Agent a = (Agent)ci.Invoke(new object[] { String.Format("AGT-TEST-{0:00}", i),
                                         new Coordonnees(Randomizer.Next(10, xMax), Randomizer.Next(10, yMax)),
                                         this });

                //Agent a = new Agent(String.Format("AGT-TEST-{0:00}", i),
                //    new Coordonnees(Randomizer.Next(10, xMax), Randomizer.Next(10, yMax)), this);

                this.Agents.Ajouter(a);
            }
        }
    }
}
