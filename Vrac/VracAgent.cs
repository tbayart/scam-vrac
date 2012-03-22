using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Vrac.Tools;

namespace VracAgent
{
    public class Annuaire{
	
	public List<Agent> agents = new List<Agent>();
        public void add(Agent a ){agents.Add(a);}
        public void del(Agent a ){agents.Remove(a);}
	}

    public class EvtDispatcher
    {


        public static object SyncRoot = new object();
        public static Queue<Evenement> BAL = new Queue<Evenement>();

        public static void Start()
        {
            while (!stop)
            {
                Do();
                Thread.Sleep(0);
            }
        }

        private static bool stop = false;
        public static void Stop()
        {
            stop = true;
        }

        public static void Post(Evenement e)
        {
            lock (SyncRoot) { BAL.Enqueue(e); }
        }

        private static void Do()
        {
            Evenement e = null;
            lock (SyncRoot)
            {
                if (BAL.Count > 0)
                    e = BAL.Dequeue();
                else
                    Thread.Sleep(10);
            }

            if (e != null)
                Do(e);
        }

        private static void Do(Evenement e)
        {
            Kernel.Receive(e);

            // Récupérer les agents qui sont à portée
            var q = Kernel.pagesBlanches.agents
                .Where(
                    a => e.Portee == -1 || e.emetteur.Coord.getDistance(a.Coord) < e.Portee && e.emetteur!=a
                );
            
            q.ToList().ForEach(
                    agtConcerne => agtConcerne.Do("Hear", e.emetteur, e)
                );


        }
    }

    public class Kernel
    {
        public static Annuaire pagesBlanches = new Annuaire();

        public static Vrac.GenerateurCarte.Carte carte;

        public static T create<T>() where T : Agent, new()
        {
            T agt = new T();
            pagesBlanches.add(agt);
            return agt;
        }

        public static void Receive(Evenement e)
        {
            if(e is Dead)
            {
                pagesBlanches.del(e.emetteur);
            }
        }

        public static void NewTurn()
        {
            EvtDispatcher.Post(Evenement.NewTurn);
        }


        private static int nbIter = 2000;
        private static bool stop = false;
        public static void Stop()
        {
            stop = true;
        }

        public static void Start()
        {
            while (!stop && --nbIter > 0)
            {
                NewTurn();
                Thread.Sleep(0);

                Bitmap bmp = carte.getBitmap();

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    pagesBlanches.agents.ForEach(a =>
                        g.DrawEllipse(Pens.White, a.Coord.X-1, a.Coord.Y-1, 2,2)
                        );
                }

                bmp.Save(@"./Temp/AgentEtape" + String.Format("{0:000}", (20 - nbIter)) + ".bmp");
            }
        }

        public static void Init()
        {
            int taille = 1024;
            carte = Vrac.GenerateurCarte.Carte.GetCarteTest(taille, taille);
            for (int i = 0; i < taille/32; i++)
            {
                Dryad d = create<Dryad>();
                d.Coord = new Coordonnees(Randomizer.Next((int)(taille * 0.05), (int)(taille * 0.95)), Randomizer.Next((int)(taille * 0.05), (int)(taille * 0.95)));
            }
            Start();
        }
    }

    public class Dryad : Agent
    {
        public Dryad()
        {
            chaine = CatalogueBehavior.DryadTurn;
            capacites = new Dictionary<string, Capacity>();
            capacites["Teleport"] = new  ICanTeleport_L1();
            capacites["Plant"] = new  ICanPlant();
            capacites["Retreat"] = new  ICanRetreat();
            capacites["Hear"] = new  ICanHear();
            capacites["Send"] = new  ICanSpeak();

            Caracteristique dep = CatalogueCaracteristique.DistanceDeDeplacement();
            dep.valeur = 6;
            caracteristiques[dep.nom] = dep;

        }
    }


    public class Agent
    {
        public Coordonnees Coord;

        public Behavior chaine;
        public Dictionary<string, Capacity> capacites;
        public Dictionary<string, Caracteristique> caracteristiques; // Key = carac name

        //public SortedList obj;
        
        //public ArrayList knPlc; // Known place
        //public List<Item> invtry;

        //public bool evalAbilitReq = false;

        //public void EvalAbilitOnCar()
        //{
        //    if (!evalAbilitReq)
        //        return;

        //    // parcours abilit et refait la distrib en fonction des carac
        //}

        public void Receive(Evenement e)
        {
            chaine.handleOrLetTheNextDoIt(e, this);
        }

        public void Send(Evenement e)
        {
            EvtDispatcher.Post(e);
        }

        public Resultat Do(string ActionName, Agent cible, object p)
        {
            return capacites[ActionName].doIt(this, cible, p);
        }
    }

    public class CatalogueBehavior
    {

        public static Behavior DryadTurn;
        private static Behavior NearTo;
                               

        static CatalogueBehavior()
        {
            DryadTurn = new Behavior()
                            {
                                isForwarder = (e, agent) => false,
                                isHandler = (e, agent) => e == Evenement.NewTurn,
                                handle = (e, agent) =>
                                             {
                                                 Coordonnees newcoord = new Coordonnees(agent.Coord.X + Randomizer.Next(-3, 3), agent.Coord.Y + Randomizer.Next(-3, 3));
                                                 newcoord.X = Math.Max(0, newcoord.X);
                                                 newcoord.X = Math.Min(Kernel.carte._carte.Length-1, newcoord.X);
                                                 newcoord.Y = Math.Max(0, newcoord.Y);
                                                 newcoord.Y = Math.Min(Kernel.carte._carte[0].Length-1, newcoord.Y);
                                                 Resultat r = agent.Do("Teleport", agent, newcoord);
                                                 if (r == Resultat.Succes)
                                                 {
                                                     agent.Do("Send", agent, new Moved(agent, agent.Coord.X, agent.Coord.Y));
                                                 }
                                             }
                            };

            NearTo = new Behavior()
                         {
                             isForwarder = (e, agent) => false,
                             isHandler = (e, agent) => e is Moved,
                             handle = (e, agent) =>
                                          {
                                              Resultat r = agent.Do("Plant", agent, new Coordonnees(agent.Coord.X, agent.Coord.Y));

                                              if (r == Resultat.Succes)
                                              {
                                                  Resultat rDie = agent.Do("Retreat", agent, null);
                                                  if (rDie == Resultat.Succes)
                                                      agent.Do("Send", agent, new Dead(agent));
                                              }
                                          }
                         };

            DryadTurn.next = NearTo;
        }
    }


    public class Moved : Evenement
    {
        public Moved(Agent emet, int x, int y) : base(emet, new ArrayList(){x,y})
        {
		Portee = 4;
        }
    }
    public class Dead : Evenement
    {
        public Dead(Agent emet)
            : base(emet, new ArrayList())
        {
        }
    }


    public class Behavior
    {
        public Behavior next;
        public void handleOrLetTheNextDoIt(Evenement e, Agent agent)
        {
            if (isHandler(e, agent))
            {
                handle(e, agent);

                if (isForwarder(e, agent))
                    next.handleOrLetTheNextDoIt(e, agent);
            }
            else
            {
                next.handleOrLetTheNextDoIt(e, agent);
            }

        }

        public Action<Evenement, Agent> handle;
        public Func<Evenement, Agent, bool> isHandler;
        public Func<Evenement, Agent, bool> isForwarder;

    }

    public class Capacity
    {
        public Dictionary<Resultat, Action> act;
        public Distribution<Resultat> proba;

        public Resultat doIt(Agent acteur, Agent cible, object param)
        {
            Resultat r = proba.get();
            act[r].doIt(acteur, cible, param);
            return r;
        }
    }

    public class ICanTeleport_L1 : Capacity
    {
        public ICanTeleport_L1()
        {
            proba = new Distribution<Resultat>() { DicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Echec, 0.6d }, { Resultat.Succes, 0.4d }}};
            act = new Dictionary<Resultat, Action>() { { Resultat.Echec, CatalogueAction.None }, { Resultat.Succes, CatalogueAction.Teleport } };
        }
    }
    public class ICanTeleport_L2 : Capacity
    {
        public ICanTeleport_L2()
        {
            proba = new Distribution<Resultat>() { DicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Echec, 0.1d }, { Resultat.Succes, 0.9d } } };
            act = new Dictionary<Resultat, Action>() { { Resultat.Echec, CatalogueAction.None }, { Resultat.Succes, CatalogueAction.Teleport } };
        }
    }
    public class ICanPlant : Capacity
    {
        public ICanPlant()
        {

            proba = new Distribution<Resultat>() { DicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Critique, 0.0001d }, { Resultat.Echec, 0.01d }, { Resultat.Succes, 1.0d - 0.01d - 0.0001d } } };
            act = new Dictionary<Resultat, Action>() { { Resultat.Critique, CatalogueAction.Die }, { Resultat.Echec, CatalogueAction.None }, { Resultat.Succes, CatalogueAction.Plant } };
        }
    }
    public class ICanRetreat : Capacity
    {
        public ICanRetreat()
        {
            proba = new Distribution<Resultat>() { DicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Echec, 0.2d }, { Resultat.Succes, 0.8d } } };
            act = new Dictionary<Resultat, Action>() { { Resultat.Echec, CatalogueAction.None }, { Resultat.Succes, CatalogueAction.Die } };
        }
    }
    public class ICanHear : Capacity
    {
        public ICanHear()
        {
            proba = new Distribution<Resultat>() { DicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Echec, 0.01d }, { Resultat.Succes, 1.0d-0.01d } } };
            act = new Dictionary<Resultat, Action>() { { Resultat.Echec, CatalogueAction.None }, { Resultat.Succes, CatalogueAction.Hear } };
        }
    }
    public class ICanSpeak : Capacity
    {
        public ICanSpeak()
        {
            proba = new Distribution<Resultat>() { DicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Echec, 0.01d }, { Resultat.Succes, 1.0d-0.01d } } };
            act = new Dictionary<Resultat, Action>() { { Resultat.Echec, CatalogueAction.None }, { Resultat.Succes, CatalogueAction.Speak } };
        }
    }

    public class CatalogueAction
    {
        public static Action Teleport = new Action()
                                            {
                                                doIt = (acteur, cible, coord) =>
                                                           {
                                                               Coordonnees C = ((Coordonnees)coord);
                                                               cible.Coord.X = C.X;
                                                               cible.Coord.Y = C.Y;
                                                           }
                                            };

        public static Action Plant = new Action()
                                         {
                                             doIt = (acteur, cible, coord) =>
                                                        {
                                                            Coordonnees C = ((Coordonnees)coord);
                                                            Kernel.carte._carte[C.X][C.Y] = Vrac.GenerateurCarte.TypeElementBiome.Arbre;
                                                        }
                                         };

        public static Action Die = new Action()
        {
            doIt = (acteur, cible, coord) => { }
        };
        public static Action None = new Action()
        {
            doIt = (acteur, cible, coord) => { }
        };

        public static Action Hear = new Action()
                                       {
                                           doIt = (acteur, cible, e) => acteur.Receive((Evenement)e)
                                       };

        public static Action Speak = new Action()
                                       {
                                           doIt = (acteur, cible, e) => acteur.Send((Evenement)e)
                                       };
	
        //public static Action Manger = new Action()
        //{
        //    doIt = (agt, v) =>
        //    {
        //        agt.carac["Faim"].valeur -= (int)v;
        //    }
        //};

        static CatalogueAction(){}
    }

    public class Action
    {
        public delegate void DoIt(Agent acteur, Agent cible, object param);

        public DoIt doIt;
    }

    public class Evenement
    {
	public double Portee;

        public Agent emetteur;
        public ArrayList parametres;

        public Evenement(Agent emet, ArrayList param)
        {
            this.emetteur = emet;
            this.parametres = param;
        }

        public static Evenement NewTurn = new Evenement(null, null) { Portee =-1};
    }

    public class Resultat
    {
        private int key;

        public static Resultat Critique = new Resultat(){key=1};
        public static Resultat Echec = new Resultat() { key = 2 };
        public static Resultat Succes = new Resultat() { key = 3 };
        public static Resultat Epic = new Resultat() { key = 4 };

        public override bool Equals(object obj)
        {
            return key == (obj as Resultat).key;
        }
    }

    public class Caracteristique // Generic mal adapté
    {
        public string nom;
        public int valeur;
    }

    public class CatalogueCaracteristique
    {
        public static Caracteristique Faim()
        {
            return new Caracteristique { nom = "Faim" };
        }

        public static Caracteristique Sommeil()
        {
            return new Caracteristique { nom = "Sommeil" };
        }

        public static Caracteristique DistanceDeDeplacement()
        {
            return new Caracteristique { nom = "DistanceDeDeplacement" };
        }
    }
    /*unused
    
    public class Item
    {
        public static Item Pomme = new Item(); // Pomme
    }

    public class Objectif
    {
        public delegate int Eval(Agent c);

        public Eval eval;

        public static Objectif BonneSante = new Objectif() { eval = (Agent c) => 100 - c.carac["Faim"].valeur - c.carac["Sommeil"].valeur };
    }*/
}
