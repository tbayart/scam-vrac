using System.Collections;
using System.Collections.Generic;
using SMA.Actions;
using SMA.Agents.Caracteristiques;
using SMA.Capacites;
using SMA.Comportements;
using SMA.Objectifs;
using Tools;
using SMA.Resultats;

namespace SMA.Agents
{
    public interface IAgent
    {
        // L'id du IAgent.
        string Id { get; }

        // Les coordonnées du IAgent.
        Coordonnees Coord { get; }

        // Les caractéristiques du IAgent.
        Dictionary<NomCaracteristique, Caracteristique> Caracteristiques { get; }

        // La chaîne de responsabilité du comportement du IAgent.
        Comportement ChaineComportement { get; }

        // Le dictionnaire des capacités du IAgent.
        Dictionary<NomAction, Capacite> Capacites { get; }

        // L'état actuel du IAgent.
        EtatAgent Etat { get; }

        // Les objectifs du IAgent.
        Dictionary<TypeObjectif, Objectif> Objectifs { get; }

        // Les connaissances acquises pour la réalisation des objectifs.
        Dictionary<TypeObjectif, Hashtable> ConnaissancesObjectifs { get; }

        // Le monde auquel appartient le IAgent.
        Monde World { get; }


        // 
        Resultat FaireAction(NomAction nomAction, IAgent cible, object p);
    }
}
