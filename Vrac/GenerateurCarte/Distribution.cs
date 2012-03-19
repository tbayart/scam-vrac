using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vrac.Tools;

namespace Vrac.GenerateurCarte
{
    public class Distribution<T>
    {
        public Dictionary<T, double> dicoSeuils;

        public Distribution()
        {
            this.dicoSeuils = new Dictionary<T, double>();
        }

        public T get()
        {
            double d = Randomizer.NextDouble();
            double sommeSeuils = 0;

            foreach (T cle in this.dicoSeuils.Keys)
            {
                sommeSeuils += this.dicoSeuils[cle];

                if (d < sommeSeuils)
                    return cle;
            }

            throw new Exception("Erreur !");
        }
    }
}
