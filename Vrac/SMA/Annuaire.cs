using System.Collections.Generic;
using System.Linq;
using Vrac.SMA.Agents;
using Vrac.Tools;

namespace Vrac.SMA
{
    public class Annuaire
    {
        #region --> Attributs

        // La liste de tous les Agent instanciés.
        public List<Agent> clAgents { get; set; }
        public List<ISecteur> clSecteur { get; set; }
        public Dictionary<Agent, List<ISecteur>> annuaire;

        #endregion --> Attributs

        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Annuaire()
        {
            this.clAgents = new List<Agent>();
            this.clSecteur = new List<ISecteur>();
            this.annuaire = new Dictionary<Agent, List<ISecteur>>();
        }
        
        #endregion --> Constructeurs

        #region --> Méthodes d'instance

        public IEnumerable<Agent> Agents(Coordonnees position, double distance)
        {
            //if (distance == -1)
                return clAgents;

            //List<Agent> agentsDoubles = new List<Agent>();

            //Secteurs(position, distance)
            //    .Select(s => s.Agents)
            //    .ToList()
            //    .ForEach(agentsDoubles.AddRange);

            //return agentsDoubles.Distinct();
        }

        public IEnumerable<ISecteur> Secteurs(Coordonnees position, double distance)
        {
            return clSecteur.Where(s => s.Centre.getDistance(position) < s.Taille + distance);
        }

        public void majAgent(Agent a)
        {
            del(a);
            add(a);
        }

        public void add(Agent agent)
        {
            List<ISecteur> secteurs = this.Secteurs(agent.Coord, 0).ToList();   
            clAgents.Add(agent);
            annuaire.Add(agent, secteurs);
            secteurs.ForEach(s=>s.Agents.Add(agent));
        }

        public void del(Agent agent) 
        {
            annuaire.Remove(agent);
            clAgents.Remove(agent);
            List<ISecteur> secteurs = this.Secteurs(agent.Coord, 0).ToList();
            secteurs.ForEach(s => s.Agents.Remove(agent));
        }

        public void CreerSecteurs(int taille_x, int taille_y, int taille_secteur)
        {
            int x_SecteurAcreer = 0;
            int y_SecteurAcreer = 0;

            while (y_SecteurAcreer < taille_y)
            {
                while (x_SecteurAcreer < taille_x)
                {
                    CreerSecteur(x_SecteurAcreer, y_SecteurAcreer, taille_secteur);
                    x_SecteurAcreer += taille_secteur;
                    if (x_SecteurAcreer > taille_x)
                        x_SecteurAcreer = taille_x;
                }
                x_SecteurAcreer = 0;
                y_SecteurAcreer += taille_secteur;
                if (y_SecteurAcreer > taille_y)
                    y_SecteurAcreer = taille_y;
            }
        }

        private void CreerSecteur(int x_SecteurAcreer, int y_SecteurAcreer, int taille_secteur)
        {
            clSecteur.Add(
                new Secteur()
                {
                    Centre = new Coordonnees(x_SecteurAcreer, y_SecteurAcreer),
                    Taille = taille_secteur,
                });
        }

        #endregion --> Méthodes d'instance
    }
}
