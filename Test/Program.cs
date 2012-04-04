using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using ScamCarte;
using ScamCarte.Cartes;
using SMA.Agents;
using Tools;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Carte map = GenerateurCarte.GenererNouvelleCarte(1500000);
            
            //ElementCarte[][] ec = map.Elements;
            //ElementCarte[,] ec2 = new ElementCarte[map.Largeur, map.Hauteur];

            //for (int i = 0; i < map.Largeur; i++)
            //    for (int j = 0; j < map.Hauteur; j++)
            //    {
            //        ec2[i, j] = ec[i][j];
            //    }

            string cmd = "";

            while (cmd != "stop")
            {
                cmd = Console.ReadLine();

                if (cmd != String.Empty && cmd != "stop")
                {
                    string[] c = cmd.Split(';');

                    if (c.Length == 2)
                    {
                        Coordonnees coord = map.getPostionElement(TypeElementBiome.Terre, new Coordonnees(int.Parse(c[0].Trim()), int.Parse(c[1].Trim())));
                        if (coord != null)
                            Console.WriteLine("Ok");
                        else
                            Console.WriteLine("Rien trouvé !!");
                    }
                }
            }

             
        }

        /*
        static void Main(string[] args)
        {
            //BenchGenerateur.BenchGenerateurCarte();
            //Console.ReadLine();
            //return;
            
            Kernel.Start();
            //Kernel.Start(@".\CarteTest.scam");

            string cmd = "";

            while (cmd != "stop")
            {
                cmd = Console.ReadLine();

                if (cmd == "test")
                {
                    Kernel.Test_GenererAgents(20);
                }
            }

            Kernel.Stop();

            //Kernel.SauvegarderCarteMonde(@".\CarteTest.scam");

            Carte c = Kernel.GetCarteMonde();
            List<Coordonnees> coords = Kernel.GetCoordonneesHubs();
            List<Agent> agents = Kernel.GetAgents();

            //Console.WriteLine("Génération de l'image de la carte...");
            //DessinerCarte(c, coords, agents);
            Console.WriteLine("Génération des images debug...");
            DessinerCartesDebug(c, coords, agents);
        }
        */

        private static void DessinerCarte(Carte carte, List<Coordonnees> coordsHubs, List<Agent> agents)
        {
            int largeurCarte = carte.Elements.Length;
            int hauteurCarte = carte.Elements[0].Length;

            Bitmap image = new Bitmap(largeurCarte, hauteurCarte);

            // Dessin de la carte.
            for (int i = 0; i < largeurCarte; i++)
            {
                for (int j = 0; j < hauteurCarte; j++)
                {
                    image.SetPixel(i, j, GetColor(carte.Elements[i][j].ElementBiome));
                }
            }
            
            using (Graphics g = Graphics.FromImage(image))
            {
                // Dessin des hubs.
                int rayon = (int)Math.Round(Math.Sqrt(2) * 256, 0);
                int diametre = 2 * rayon;

                foreach (Coordonnees c in coordsHubs)
                {
                    g.DrawEllipse(Pens.LightCyan, c.X - rayon, c.Y - rayon, diametre, diametre);
                }
            
                // Dessin des agents.
                foreach (Agent agt in agents)
                {
                    g.DrawEllipse(Pens.Yellow, agt.Coord.X - 1, agt.Coord.Y - 1, 3, 3);
                }
            }

            image.Save(@"CarteSMA.bmp");
        }

        private static void DessinerCartesDebug(Carte carte, List<Coordonnees> coordsHubs, List<Agent> agents)
        {
            if (Directory.Exists(@".\CartesDebug\"))
            {
                Directory.Delete(@".\CartesDebug\", true);
            }
            Directory.CreateDirectory(@".\CartesDebug\");

            int largeurCarte = carte.Elements.Length;
            int hauteurCarte = carte.Elements[0].Length;
            
            //int rayon = (int)Math.Round(Math.Sqrt(2) * 256, 0);
            //int diametre = 2 * rayon;
            int rayon = AgentHub.RAYON_ACTION;
            int diametre = 2 * rayon;

            Pen penHubProche = new Pen(Color.Blue, 4);
            penHubProche.DashStyle = DashStyle.Dot;

            Brush brushHubProche = new SolidBrush(Color.FromArgb(127, Color.Blue));

            // Création image de référence.
            Bitmap imageRef = new Bitmap(largeurCarte, hauteurCarte);

            for (int i = 0; i < largeurCarte; i++)
            {
                // Dessin de la carte.
                for (int j = 0; j < hauteurCarte; j++)
                {
                    imageRef.SetPixel(i, j, GetColor(carte.Elements[i][j].ElementBiome));
                }

                using (Graphics g = Graphics.FromImage(imageRef))
                {
                    // Dessin des hubs.
                    foreach (Coordonnees c in coordsHubs)
                    {
                        g.DrawEllipse(Pens.Yellow, c.X - rayon, c.Y - rayon, diametre, diametre);
                    }
                }
            }

            // Génération des images des agents.
            foreach (Agent agt in agents)
            {
                Bitmap image = new Bitmap(imageRef);

                using (Graphics g = Graphics.FromImage(image))
                {
                    // Dessin des hubs proches.
                    foreach (AgentHub hub in agt.Test_Hubs())
                    {
                        Rectangle r = new Rectangle(hub.Coord.X - rayon, hub.Coord.Y - rayon, diametre, diametre);

                        g.FillEllipse(brushHubProche, r);
                        g.DrawEllipse(penHubProche, r);

                    }

                    // Dessin de l'agent.
                    g.DrawEllipse(Pens.Yellow, agt.Coord.X - 2, agt.Coord.Y - 2, 5, 5);
                }

                image.Save(String.Format(@".\CartesDebug\{0}.bmp", agt.Id));
            }
        }

        private static Color GetColor(TypeElementBiome typeElementBiome)
        {
            switch (typeElementBiome)
            {
                case TypeElementBiome.Arbre:
                    return Color.DarkGreen;
                case TypeElementBiome.Eau:
                    return Color.MediumAquamarine;
                case TypeElementBiome.Pierre:
                    return Color.Gray;
                case TypeElementBiome.Sable:
                    return Color.SandyBrown;
                case TypeElementBiome.Terre:
                    return Color.Brown;
                default:
                    return Color.Black;
            }
        }
    }
}
