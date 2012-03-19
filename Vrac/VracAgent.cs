using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VracAgent
{
    public class Arbre_Move : Behavior
    {
        public Arbre_Move()
        {

        }
    }
    public class Arbre : Agent
    {
        public Arbre()
        {

        }
    }

    public class Agent
    {
        public Behavior chaine;
        public SortedList obj;
        public Dictionary<Action, Capacity> abilit;
        public Dictionary<string, Caract<int>> carac; // Key = carac name

        public ArrayList knPlc; // Known place
        public List<Item> invtry;

        public bool evalAbilitReq = false;

        public void EvalAbilitOnCar()
        {
            if (!evalAbilitReq) 
                return;

            // parcours abilit et refait la distrib en fonction des carac
        }

        public void Receive(Evenement e)
        {
            chaine.handle(e);
        }

        public void Send(Evenement e)
        {
            // wrd.Receive(e) ?
        }

        public void Do(Action a, Agent c, object p)
        {
            abilit[a].doIt(c, p);
        }
    }

    public class Objectif
    {
        public delegate int Eval(Agent c);

        public Eval eval;

        public static Objectif BonneSante = new Objectif() { eval = (Agent c) => 100 - c.carac["Faim"].valeur - c.carac["Sommeil"].valeur };
    }

    public class Behavior
    {
        public delegate void handle(Evenement e);

        // Chain of Responsability
        public Behavior next;

        public void handleOrLetTheNextDoIt(Evenement e)
        {
            if (isHandle(e))
            {
                handle(e);
                if (isForwarder(e))
                    letTheNextDoIt(e);
            }
            else
                letTheNextDoIt(e);
        }
        public void letTheNextDoIt(Evenement e)
        {
            if (next == null) return;
            next.handleOrLetTheNextDoIt(e);
        }

        // A surcharger : 

        protected virtual void handle(Evenement e)
        {

        }

        protected virtual bool isHandle(Evenement e)
        {
            return false;
        }

        protected virtual bool isForwarder(Evenement e)
        {
            return false;
        }

        public static Behavior BehJour = new Behavior() { /*do smthg*/ };
        public static Behavior BehNuit = new Behavior() { /*do smthg*/ };
    }

    public class Capacity
    {
        public Dictionary<Resultat, Action> act;
        // Distribution<Res> proba;

        public void doIt(Agent agt, object param)
        {
            Resultat r = Resultat.Succes; // proba.get();
            act[r].doIt(agt, param);
        }
    }

    public class Action
    {
        public delegate void DoIt(Agent agt, object param);

        public DoIt doIt;

        public static Action Manger = new Action()
        {
            doIt = (agt, v) =>
            {
                agt.carac["Faim"].valeur -= (int)v;
            }
        };
    }

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

    public class Evenement
    {
        public Agent emetteur;
        public ArrayList parametres;

        public static Evenement Move = new Evenement(null, null);

        public Evenement(Agent emet, ArrayList param) 
        {
            this.emetteur = emet;
            this.parametres = param;
        }
    }

    public class Resultat
    {
        public static Resultat Critique = new Resultat();
        public static Resultat Echec = new Resultat();
        public static Resultat Succes = new Resultat();
        public static Resultat Epic = new Resultat();
    }

    public class Item
    {
        public static Item Pomme = new Item(); // Pomme
    }
}
