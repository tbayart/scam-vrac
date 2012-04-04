using System;
using SMA.Agents;
using SMA.Objectifs;

namespace SMA.Messages
{
    public class ContenuMessage
    {
        // Contenu d'un message 'Requete' :
        //    - Type de la requête
        //    - Paramètres

        public static object[] GetContenu_Requete_GenererAgents(int nbAgentsAGenerer)
        {
            return new object[] { TypeRequete.GenererAgents, nbAgentsAGenerer, typeof(Agent) };
        }

        public static object[] GetContenu_Requete_GenererAgents(int nbAgentsAGenerer, Type typeAgent)
        {
            if (typeAgent.GetInterface("IAgent") == null)
                throw new Exception("Le type n'implémente pas l'interface IAgent.");

            return new object[] { TypeRequete.GenererAgents, nbAgentsAGenerer, typeAgent };
        }

        // Contenu d'un message 'DefinirObjectif' :
        //    - Nb d'agents à utiliser pour atteindre l'objectif (-1 pour tous les agents existants)
        //    - Objet représentant l'objectif

        public static object[] GetContenu_DefinirObjectif_Test()
        {
            return new object[] { -1, CatalogueObjectifs.Objectif_Test };
        }

        public static object[] GetContenu_DefinirObjectif_Dryad()
        {
            return new object[] { -1, CatalogueObjectifs.Objectif_Dryad };
        }


        public static object[] GetContenu_RealiserObjectif(TypeObjectif typeObjectif)
        {
            return new object[] { typeObjectif };
        }
    }
}
