﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Vrac.SMA.Agents;
using Tools;

namespace Vrac.SMA
{
    public interface ISecteur
    {

        List<Agent> Agents { get; set; }
        Coordonnees Centre { get; set; }
        double Taille { get; set; }

        IEnumerable<ISecteur> Secteurs(Coordonnees position, double distance);
    }

    public class FloorSecteur : ISecteur
    {
        public Coordonnees Centre { get; set; }
        public double Taille { get; set; }
        public List<Agent> Agents { get; set; }

        public IEnumerable<ISecteur> Secteurs(Coordonnees position, double distance)
        {
            return new List<ISecteur>();
        }
        public FloorSecteur()
        {
            Agents = new List<Agent>();
        }
    }

    public class Secteur : ISecteur
    {
        public List<ISecteur> clSecteurs { get; set; }

        public List<Agent> Agents { get; set; }
        public Coordonnees Centre { get; set; }
        public double Taille { get; set; }

        public Secteur()
        {
            Agents = new List<Agent>();
            clSecteurs = new List<ISecteur>();
        }
        
        public IEnumerable<ISecteur> Secteurs(Coordonnees position, double distance)
        {
            List<ISecteur> ret = new List<ISecteur>();
            if (distance == -1)
                ret.AddRange(clSecteurs);
            else
            {

                ret.AddRange(clSecteurs.Where(s =>
                                                  {
                                                        double distanceCarree = s.Taille + distance;
                                                        distanceCarree *= distanceCarree;
                                                        return Coordonnees.GetDistanceCarree(s.Centre,position) < distanceCarree;
                                                  }));
            }

            List<ISecteur> enfants = new List<ISecteur>();
            ret.ForEach(s => enfants.AddRange(s.Secteurs(position, distance)));

            ret.AddRange(enfants);
            return ret; 
        }

        public static double RacineDeDeux=  Math.Sqrt(2);
        public static double RacineDeDeuxSurDeux = Math.Sqrt(2) / 2;

        public void CreerSecteurs(ISecteur parent, int taille_secteur, bool encore)
        {
            double RacineDeDeuxSurDeuxR = RacineDeDeuxSurDeux*parent.Taille;
            
            CreerSecteurs(
                            parent, 
                            (int) (parent.Centre.X - RacineDeDeuxSurDeuxR/2), 
                            (int) (parent.Centre.Y - RacineDeDeuxSurDeuxR/2), 
                            (int)RacineDeDeuxSurDeuxR, 
                            taille_secteur, 
                            encore);
        }

        public void CreerSecteurs(ISecteur parent, int x_Carre, int y_Carre, int taille_Carre, int taille_secteur, bool encore)
        {
            int x_SecteurAcreer = x_Carre;
            int y_SecteurAcreer = y_Carre;

            bool lastY = false;
            bool stopY = false;
            do
            {
                bool lastX = false;
                bool stopX = false;
                do
                {
                    ISecteur enfant = CreerSecteur(parent, x_SecteurAcreer, y_SecteurAcreer, taille_secteur, encore);
                    if (encore)
                        CreerSecteurs(enfant, taille_secteur/4, false);

                    x_SecteurAcreer += (taille_secteur);
                    if (lastX)
                        stopX = true;
                    if (!lastX && x_SecteurAcreer > x_Carre + taille_Carre)
                    {
                        //x_SecteurAcreer = x_Carre + taille_Carre;
                        lastX = true;
                    }
                } while (!stopX); //x_SecteurAcreer <= x_Carre + taille_Carre);

                x_SecteurAcreer = 0;
                y_SecteurAcreer += (taille_secteur);

                if (lastY)
                    stopY = true;
                if (!lastY && y_SecteurAcreer > y_Carre + taille_Carre)
                {
                    //y_SecteurAcreer = y_Carre + taille_Carre;
                    lastY = true;
                }
            } while (!stopY);//y_SecteurAcreer <= y_Carre + taille_Carre);

        }
        
        
        private ISecteur CreerSecteur(ISecteur parent, int xSecteurAcreer, int ySecteurAcreer, int tailleSecteur, bool secteurIntermediaire)
        {
            ISecteur s = secteurIntermediaire ?

                             (ISecteur) new Secteur()
                                                {
                                                    Centre = new Coordonnees(xSecteurAcreer, ySecteurAcreer),
                                                    Taille = tailleSecteur*0.71
                                                }

                             : (ISecteur) new FloorSecteur()
                                              {
                                                  Centre = new Coordonnees(xSecteurAcreer, ySecteurAcreer),
                                                  Taille = tailleSecteur * 0.71
                                              };

            ((Secteur)parent).clSecteurs.Add(s);
            return s;
        }

    }


}
