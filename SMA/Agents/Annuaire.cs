using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using SMA.Actions;

namespace SMA.Agents
{
    public class Annuaire<T> where T : IAgent
    {
        private List<T> _pagesBlanches;
        private Dictionary<NomAction, List<T>> _pagesJaunes;

        public Annuaire()
        {
            this._pagesBlanches = new List<T>();
            this._pagesJaunes = new Dictionary<NomAction, List<T>>();
        }

        public void Ajouter(T item)
        {
            this._pagesBlanches.Add(item);

            foreach (var cap in item.Capacites)
            {
                if (!this._pagesJaunes.ContainsKey(cap.Key))
                    this._pagesJaunes.Add(cap.Key, new List<T>());

                if (!this._pagesJaunes[cap.Key].Contains(item))
                    this._pagesJaunes[cap.Key].Add(item);
            }
        }

        public List<T> GetAgents(Coordonnees centre, int rayon)
        {
            int rayonCarre = rayon * rayon;

            return this._pagesBlanches
                            .Where(a => (centre.getDistanceCarree(a.Coord) <= rayonCarre))
                            .ToList<T>();
        }

        public List<T> PagesBlanches()
        {
            return this._pagesBlanches;
        }

        public List<T> PagesBlanches(Type typeAgent)
        {
            return this._pagesBlanches
                            .Where(a => (a.GetType() == typeAgent))
                            .ToList<T>();
        }

        public List<T> PagesBlanches(Type typeAgent, Coordonnees centre, int rayon)
        {
            int rayonCarre = rayon * rayon;

            return this._pagesBlanches
                            .Where(a => (a.GetType() == typeAgent && centre.getDistanceCarree(a.Coord) <= rayonCarre))
                            .ToList<T>();
        }
    }
}
