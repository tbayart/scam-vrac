using System.Collections.Generic;
using System.Drawing;
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
        public Secteur SecteurPrincipal { get; set; }
        public Dictionary<Agent, List<ISecteur>> annuaire;

        #endregion --> Attributs

        #region --> Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Annuaire()
        {
            this.clAgents = new List<Agent>();
            this.annuaire = new Dictionary<Agent, List<ISecteur>>();
        }
        
        #endregion --> Constructeurs

        #region --> Méthodes d'instance

        public IEnumerable<Agent> Agents(Coordonnees position, double distance)
        {
            if (distance == -1)
                return clAgents;

            List<Agent> agentsDoubles = new List<Agent>();

            Secteurs(position, distance)
                .Select(s => s.Agents)
                .ToList()
                .ForEach(agentsDoubles.AddRange);

            return agentsDoubles.Distinct();
        }

        public IEnumerable<ISecteur> Secteurs(Coordonnees position, double distance)
        {
            return SecteurPrincipal.Secteurs( position, distance).OfType<FloorSecteur>();
        }

        private void newAgent(Agent a)
        {
            this.annuaire.Add(a, this.Secteurs(a.Coord, 0.1).ToList());
            this.annuaire[a].ForEach(s => s.Agents.Add(a));
        }

        public void majAgent(Agent a)
        {
            if (!this.annuaire.ContainsKey(a)) return;

            this.annuaire[a].ForEach(s => s.Agents.Remove(a));
            this.annuaire[a] = this.Secteurs(a.Coord, 0.1).ToList();
            this.annuaire[a].ForEach(s => s.Agents.Add(a));
        }

        public void add(Agent agent)
        {
            List<ISecteur> secteurs = this.Secteurs(agent.Coord, 0.1).ToList();
            clAgents.Add(agent);
            annuaire.Add(agent, secteurs);
            secteurs.ForEach(s => s.Agents.Add(agent));
        }

        public void del(Agent agent)
        {
            List<ISecteur> lst;
            annuaire.TryGetValue(agent, out lst);

            if(lst!=null)
                lst.ForEach(s => s.Agents.Remove(agent));
            annuaire.Remove(agent);
            clAgents.Remove(agent);
        }

        public void CreerSecteurPrincipal(int taille_Carte, int x_SecteurAcreer, int y_SecteurAcreer, int taille_secteur)
        {
            SecteurPrincipal = 
                new Secteur()
                {
                    Centre = new Coordonnees(x_SecteurAcreer, y_SecteurAcreer),
                    Taille = taille_secteur,
                };

            SecteurPrincipal.CreerSecteurs(SecteurPrincipal, taille_secteur/4, true);
        }

        public Bitmap DrawSecteurs()
        {
            Bitmap bmp2 = Kernel.CarteManipulee.getBitmap();

            using (Graphics g = Graphics.FromImage(bmp2))
            {
                Secteurs(null,-1).ToList().ForEach(s => g.DrawEllipse(Pens.White, (float)(s.Centre.X - s.Taille), (float)(s.Centre.Y - s.Taille), (float)(2 * s.Taille), (float)(2 * s.Taille)));

            }

            return bmp2;
        }
        public Bitmap DrawSecteurs(Agent a)
        {
            Bitmap bmp2 = Kernel.CarteManipulee.getBitmap();

            using (Graphics g = Graphics.FromImage(bmp2))
            {
                annuaire[a].ForEach(s => g.DrawEllipse(Pens.White, (float)(s.Centre.X - s.Taille), (float)(s.Centre.Y - s.Taille), (float)(2 * s.Taille), (float)(2 * s.Taille)));

                g.DrawEllipse(Pens.White, a.Coord.X - 1, a.Coord.Y - 1, 2, 2);
            }

            return bmp2;
        }
        #endregion --> Méthodes d'instance
    }
}
