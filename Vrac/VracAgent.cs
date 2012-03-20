using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Vrac.GenerateurCarte;
using Vrac.Tools;

namespace VracAgent
{

    public class Dryad : Agent
    {
        public Dryad()
        {
            chaine =  CatalogueBehavior.DryadTurn;
        }
    }


    public class Agent
    {
        public int x, y;

        public Behavior chaine;
        public Dictionary<string, Capacity> abilit;

        //public SortedList obj;
        //public Dictionary<string, Caract<int>> carac; // Key = carac name

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
            chaine.handle(e, this);
        }

        public void Send(Evenement e)
        {
            // wrd.Receive(e) ?
        }

        public Resultat Do(string ActionName, Agent c, object p)
        {
            return abilit[ActionName].doIt(c, p);
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
                                handle = (e, agent) => agent.Do("Teleport",agent, new int[] {agent.x + Randomizer.Next(-3, 3), agent.y + Randomizer.Next(-3, 3)})
                                    // emit evt Moved
                                    // modif seuil fail if failed
                                    // modif abilit to L2 if success and L1
                            };
            
            NearTo = new Behavior()
                         {
                             isForwarder = (e, agent) => false,
                             isHandler = (e, agent) => e is Moved,
                             handle = (e, agent) =>
                                          {
                                              Resultat r = agent.Do("Plant", agent, new int[] { agent.x + Randomizer.Next(-3, 3), agent.y + Randomizer.Next(-3, 3) });
            
                                              if (r == Resultat.Succes)
                                                  agent.Do("Retreat", agent, null);
                                                 // emit evt Dead if succes
                                          }
                         };

            DryadTurn.next = NearTo;
        }
    }


    public class Moved : Evenement
    {
        public Moved(Agent emet, int x, int y) : base(emet, new ArrayList(){x,y})
        {
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

        public Resultat doIt(Agent agt, object param)
        {
            Resultat r = proba.get();
            act[r].doIt(agt, param);
            return r;
        }
    }

    public class ICanTeleport_L1 : Capacity
    {
        public ICanTeleport_L1()
        {
            proba = new Distribution<Resultat>() { dicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Echec, 0.6d }, { Resultat.Succes, 0.4d }}};
            act = new Dictionary<Resultat, Action>() { { Resultat.Echec, null }, { Resultat.Succes, Action.Teleport} };
        }
    }
    public class ICanTeleport_L2 : Capacity
    {
        public ICanTeleport_L2()
        {
            proba = new Distribution<Resultat>() { dicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Echec, 0.2d }, { Resultat.Succes, 0.8d } } };
            act = new Dictionary<Resultat, Action>() {  { Resultat.Echec, null }, { Resultat.Succes, Action.Teleport } };
        }
    }
    public class ICanPlant : Capacity
    {
        public ICanPlant()
        {

            proba = new Distribution<Resultat>() { dicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Critique, 0.0001d }, { Resultat.Echec, 0.01d }, { Resultat.Succes, 1.0d - 0.01d - 0.0001d } } }; 
            act = new Dictionary<Resultat, Action>() { { Resultat.Critique, Action.Die }, { Resultat.Echec, null }, { Resultat.Succes, Action.Plant } };
        }
    }
    public class ICanRetreat : Capacity
    {
        public ICanRetreat()
        {
            proba = new Distribution<Resultat>() { dicoSeuils = new Dictionary<Resultat, double>() { { Resultat.Echec, 0.2d }, { Resultat.Succes, 0.8d } } };
            act = new Dictionary<Resultat, Action>() {  { Resultat.Echec, null }, { Resultat.Succes, Action.Die } };
        }
    }

    public class Action
    {
        public delegate void DoIt(Agent agt, object param);

        public DoIt doIt;

        //public static Action Manger = new Action()
        //{
        //    doIt = (agt, v) =>
        //    {
        //        agt.carac["Faim"].valeur -= (int)v;
        //    }
        //};

        public static Action Teleport = new Action()
                                            {
                                                doIt = (agt, coord)=>
                                                           {
                                                               agt.x = ((int[]) coord)[0];
                                                               agt.y = ((int[]) coord)[1];
                                                           }
                                            };

        public static Action Plant = new Action()
                                         {
                                             doIt = (agt, coord) =>
                                                        {
                                                            Carte.getCurrent()._carte[((int[]) coord)[0]][((int[]) coord)[1]] = TypeElementBiome.Arbre;
                                                        }
                                         };

        public static Action Die = new Action()
                                       {
                                             doIt = (agt, coord) =>{}
                                       };
    }

    public class Evenement
    {
        public Agent emetteur;
        public ArrayList parametres;

        public Evenement(Agent emet, ArrayList param)
        {
            this.emetteur = emet;
            this.parametres = param;
        }
        
        public static Evenement NewTurn = new Evenement(null, null);
    }

    public class Resultat
    {
        public static Resultat Critique = new Resultat();
        public static Resultat Echec = new Resultat();
        public static Resultat Succes = new Resultat();
        public static Resultat Epic = new Resultat();
    }

    /*unused
    public class Caract<T> // Generic mal adapté
    {
        public string nom;
        public T valeur;

        public static Caract<int> Faim()
        {
            return new Caract<int> { nom = "Faim" };
        }

        public static Caract<int> Sommeil()
        {
            return new Caract<int> { nom = "Sommeil" };
        }

    }
    
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
