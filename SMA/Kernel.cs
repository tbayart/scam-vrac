using System.Collections.Generic;
using System.IO;
using ScamCarte;
using ScamCarte.Cartes;
using SMA.Agents;
using SMA.Messages;
using Tools;

namespace SMA
{
    public static class Kernel
    {
        #region -> Attributs

        // Le monde manipulé.
        private static Monde S_Monde;

        #endregion -> Attributs

        #region -> Méthodes statiques publiques

        #region -> Start / Stop

        /// <summary>
        /// Génère un nouveau monde et démarre le système.
        /// </summary>
        public static void Start()
        {
            S_Monde = new Monde(GenerateurCarte.GenererNouvelleCarte(800000));
            S_Monde.start();
        }

        /// <summary>
        /// Charge un monde et démarre le système.
        /// </summary>
        /// <param name="cheminCarte">Le chemin du fichier de sauvegarde du monde.</param>
        public static void Start(string cheminCarte)
        {
            Carte map = ChargerCarteMonde(cheminCarte);

            if (map != null)
            {
                S_Monde = new Monde(map);
                S_Monde.start();
            }
        }

        /// <summary>
        /// Arrête le système.
        /// </summary>
        public static void Stop()
        {
            S_Monde.stop();
        }

        #endregion -> Start / Stop

        #region -> Sauvegarde / Chargement

        /// <summary>
        /// Sauvegarde la carte du monde manipulé.
        /// </summary>
        /// <param name="cheminDestination">Le chemin du fichier de sauvegarde de la carte.</param>
        /// <returns>Booléen indiquant si la sauvegarde a réussi.</returns>
        public static bool SauvegarderCarteMonde(string cheminDestination)
        {
            try
            {
                string[] save = S_Monde.Map.getDonneeSauvegarde();

                StreamWriter sw = new StreamWriter(cheminDestination);

                for (int i = 0; i < save.Length; i++)
                {
                    sw.WriteLine(save[i]);
                }

                sw.Flush();
                sw.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Charge et retourne une carte.
        /// </summary>
        /// <param name="cheminCarte">Le chemin du fichier de sauvegarde de la carte.</param>
        /// <returns>La carte chargée. (null si erreur)</returns>
        public static Carte ChargerCarteMonde(string cheminCarte)
        {
            try
            {
                StreamReader sr = new StreamReader(cheminCarte);

                List<string> lignes = new List<string>();

                while (!sr.EndOfStream)
                {
                    lignes.Add(sr.ReadLine());
                }

                return Carte.ChargerDonneesSauvegarde(lignes.ToArray());
            }
            catch
            {
                return null;
            }
        }

        #endregion -> Sauvegarde / Chargement


        public static void Test_GenererAgents(int nbAgents)
        {
            Message msgGenererAgents = new Message();

            msgGenererAgents.Performatif = PerformatifMessage.Requete;
            msgGenererAgents.Destinataires = null;
            msgGenererAgents.Contenu = ContenuMessage.GetContenu_Requete_GenererAgents(nbAgents);

            S_Monde.poster(msgGenererAgents);
        }

        public static void Test_DefinirObjectifAgents()
        {
            Message msgDefinirObjectifAgents = new Message();

            msgDefinirObjectifAgents.Performatif = PerformatifMessage.DefinirObjectif;
            msgDefinirObjectifAgents.TypeDestinataires = typeof(Agent);
            msgDefinirObjectifAgents.Contenu = ContenuMessage.GetContenu_DefinirObjectif_Test();

            S_Monde.poster(msgDefinirObjectifAgents);
        }

        public static void Test_Dryads()
        {
            Message msgGenererAgents = new Message();

            msgGenererAgents.Performatif = PerformatifMessage.Requete;
            msgGenererAgents.Destinataires = null;
            msgGenererAgents.Contenu = ContenuMessage.GetContenu_Requete_GenererAgents(20, typeof(Dryad));
            
            S_Monde.poster(msgGenererAgents);

            Message msgDefinirObjectifDryads = new Message();

            msgDefinirObjectifDryads.Performatif = PerformatifMessage.DefinirObjectif;
            msgDefinirObjectifDryads.TypeDestinataires = typeof(Dryad);
            msgDefinirObjectifDryads.Contenu = ContenuMessage.GetContenu_DefinirObjectif_Dryad();

            S_Monde.poster(msgDefinirObjectifDryads);
        }



        public static Carte GetCarteMonde()
        {
            return S_Monde.Map;
        }

        public static List<Coordonnees> GetCoordonneesHubs()
        {
            List<Coordonnees> coords = new List<Coordonnees>();

            /*
            foreach (AgentHub hub in S_Monde.Hubs.PagesBlanches)
            {
                coords.Add(hub.Coord);
            }
            */

            return coords;
        }

        public static List<Agent> GetAgents()
        {
            return S_Monde.Agents.PagesBlanches();
        }

        #endregion -> Méthodes statiques publiques
    }
}
