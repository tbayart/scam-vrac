using System.Collections;
using System.Collections.Generic;
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
    public class Agent : IAgent, IEcouteur
    {
        #region -> Attributs statics privés

        private static Comportement ComportementDefaut = new Comportement()
        {
            IsHandler = (evt, agt) => { return true; },

            IsForwarder = (evt, agt) => { return false; },

            Handle = (evt, agt) =>
            {
            },

            Next = null
        };

        #endregion -> Attributs statics privés

        #region -> Attributs

        // Objet verrou de l'instance.
        protected readonly object _locker = new object();

        // La liste des hubs dont on reçoit les signaux.
        protected List<AgentHub> _hubsProches;

        #endregion -> Attributs

        #region -> Membres de IAgent

        // L'id de l'agent.
        public string Id { get; protected set; }

        // Les coordonnées de l'agent.
        public Coordonnees Coord { get; protected set; }

        // Les caractéristiques de l'agent.
        public Dictionary<NomCaracteristique, Caracteristique> Caracteristiques { get; protected set; }

        // La chaîne de responsabilité du comportement de l'agent.
        public Comportement ChaineComportement { get; protected set; }

        // Le dictionnaire des capacités de l'agent.
        public Dictionary<NomAction, Capacite> Capacites { get; protected set; }

        // L'état actuel de l'agent.
        public EtatAgent Etat { get; protected set; }

        // Les objectifs de l'agent.
        public Dictionary<TypeObjectif, Objectif> Objectifs { get; protected set; }

        // Les connaissances acquises pour la réalisation des objectifs.
        public Dictionary<TypeObjectif, Hashtable> ConnaissancesObjectifs { get; protected set; }

        // Le monde auquel appartient l'agent.
        public Monde World { get; protected set; }

        #endregion -> Membres de IAgent

        #region -> Constructeurs

        /// <summary>
        /// Constructeur.
        /// </summary>
        protected Agent()
        {
            this._hubsProches = new List<AgentHub>();

            this.Caracteristiques = new Dictionary<NomCaracteristique, Caracteristique>();
            this.ChaineComportement = ComportementDefaut;
            this.Capacites = new Dictionary<NomAction, Capacite>();
            this.Etat = EtatAgent.Disponible;

            this.Objectifs = new Dictionary<TypeObjectif, Objectif>();
            this.ConnaissancesObjectifs = new Dictionary<TypeObjectif, Hashtable>();
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="id">L'id de l'agent.</param>
        /// <param name="coord">Les coordonnées de l'agent.</param>
        /// <param name="monde">Le monde auquel appartient l'agent.</param>
        public Agent(string id, Coordonnees coord, Monde monde)
            : this()
        {
            this.Id = id;
            this.Coord = coord;
            this.World = monde;
        }

        #endregion -> Constructeurs

        #region -> Membres de IAgent

        public Resultat FaireAction(NomAction nomAction, IAgent cible, object p)
        {
            return this.Capacites[nomAction].doIt(this, cible, p);
        }

        #endregion -> Membres de IAgent

        #region -> Membres de IEcouteur

        public void poster(Message msg)
        {
            if (msg.Performatif == PerformatifMessage.SignalHub && msg.Emetteur is AgentHub)
            {
                this.capterHub(msg.Emetteur as AgentHub);
            }
            else if (msg.Performatif == PerformatifMessage.DefinirObjectif)
            {
                Objectif o = (Objectif)msg.Contenu[1];

                this.Objectifs.Add(o.TypeObj, o);
                this.ConnaissancesObjectifs.Add(o.TypeObj, new Hashtable());

                this.World.poster(CatalogueMessages.GetMessageRealiserObjectif(o.TypeObj, this));
            }
            else if (msg.Performatif == PerformatifMessage.RealiserObjectif)
            {
                this.Objectifs[(TypeObjectif)msg.Contenu[0]].RealisationObjectif
                    .handleOrLetTheNextDoIt(null, this);
            }
            else
            {
                lock (this._locker)
                {
                    if (this.ChaineComportement != null)
                        this.ChaineComportement.handleOrLetTheNextDoIt(msg, this);
                }
            }
        }

        #endregion -> Membres de IEcouteur

        public void capterHub(AgentHub hub)
        {
            lock (this._locker)
            {
                if (!this._hubsProches.Contains(hub))
                {
                    this._hubsProches.Add(hub);

                    if (this._hubsProches.Count > 2)
                        this._hubsProches.RemoveAt(0);
                }
            }
        }

        public void setPosition(Coordonnees coord)
        {
            lock (this._locker)
            {
                this.Coord = coord;
            }
        }


        public List<AgentHub> Test_Hubs()
        {
            return new List<AgentHub>(/*this._hubsProches.ToArray()*/);
        }
    }
}
