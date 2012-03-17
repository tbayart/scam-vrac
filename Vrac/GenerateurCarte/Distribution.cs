using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vrac.Tools;

namespace Vrac.GenerateurCarte
{
    public class Distribution<T>
    {
        public List<double> seuils;
        public List<T> results;

        //public Dictionary<double, T> 

        public T get()
        {
            double d = Randomizer.NextDouble();
            int i = 0;

            while (this.seuils[i] < d)
                i++;

            //for (int k = 0; k < seuils.Count; k++)
            //{
            //    if (k == i)
            //        seuils[k] *= 1.01;
            //    else
            //        seuils[k] *= 0.99;
            //}

            return this.results[i];
        }
    }
}
