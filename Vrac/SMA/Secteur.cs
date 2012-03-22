using System.Collections.Generic;
using System.Linq;
using Vrac.SMA.Agents;
using Vrac.Tools;

namespace Vrac.SMA
{
    public interface ISecteur
    {
        List<Agent> Agents { get; set; }
        Coordonnees Centre { get; set; }
        double Taille { get; set; }

    }
    public class Secteur : ISecteur
    {
        public List<Agent> Agents { get; set; }
        public Coordonnees Centre { get; set; }
        public double Taille { get; set; }

        public Secteur()
        {
            Agents = new List<Agent>();
        }
    }


}
