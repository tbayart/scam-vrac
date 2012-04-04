using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using ScamCarte.Cartes;
using SMA;
using SMA.Agents;
using Tools;

namespace UITest
{
    public partial class Form1 : Form
    {
        private Bitmap _imageRef;
        private Pen _penHubProche;
        private Brush _brushHubProche;
        private int _rayon;
        private int _diametre;
        private int _agentTraite;

        public Form1()
        {
            InitializeComponent();

            this._penHubProche = new Pen(Color.Green, 4);
            this._penHubProche.DashStyle = DashStyle.Dot;

            this._brushHubProche = new SolidBrush(Color.FromArgb(63, Color.Green));

            this._rayon = AgentHub.RAYON_ACTION;
            this._diametre = 2 * this._rayon;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //Kernel.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Kernel.Stop();
        }

        private void btnGenererCarteRef_Click(object sender, EventArgs e)
        {
            this.btnGenererCarteRef.Enabled = false;

            Carte c = Kernel.GetCarteMonde();
            List<Coordonnees> coords = Kernel.GetCoordonneesHubs();

            this.creerImageRef(c, coords);

            this.pictureBox1.Image = this._imageRef;

            this.btnPlacerAgentsTest.Enabled = true;
            this.btnDryad.Enabled = true;
        }

        private void btnPlacerAgentsTest_Click(object sender, EventArgs e)
        {
            this.btnPlacerAgentsTest.Enabled = false;

            Kernel.Test_GenererAgents(50);
            Thread.Sleep(1000);

            this._agentTraite = -1;
            this.afficherImage();

            //this.btnStartAffichage.Enabled = true;
            this.btnObjectifTerre.Enabled = true;
        }

        private void btnStartAffichage_Click(object sender, EventArgs e)
        {
            this.btnStartAffichage.Enabled = false;
            
            this._agentTraite = 0;
            
            this.timer1.Start();

            this.btnPauseAffichage.Enabled = true;
            this.btnStopAffichage.Enabled = true;
        }

        private void btnPauseAffichage_Click(object sender, EventArgs e)
        {
            if (this.btnPauseAffichage.Text == "Pause")
            {
                this.timer1.Stop();
                this.btnPauseAffichage.Text = "Reprise";
            }
            else
            {
                this.timer1.Start();
                this.btnPauseAffichage.Text = "Pause";
            }
        }

        private void btnStopAffichage_Click(object sender, EventArgs e)
        {
            this.btnPauseAffichage.Enabled = false;
            this.btnStopAffichage.Enabled = false;

            this.timer1.Stop();

            this._agentTraite = -1;
            this.afficherImage();

            this.btnStartAffichage.Enabled = true;
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image.Save(@".\Image.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }
        
        private void btnObjectifTerre_Click(object sender, EventArgs e)
        {
            Kernel.Test_DefinirObjectifAgents();

            this.btnStartAffichage_Click(null, null);
        }

        private void btnDryad_Click(object sender, EventArgs e)
        {
            Kernel.Test_Dryads();

            //this.btnStartAffichage_Click(null, null);
        }

        private void btnRefreshImage_Click(object sender, EventArgs e)
        {
            this.btnGenererCarteRef_Click(null, null);
            this.afficherImage();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.afficherImage();
        }

        private void creerImageRef(Carte c, List<Coordonnees> coords)
        {
            int largeurCarte = c.Largeur;
            int hauteurCarte = c.Hauteur;

            int rayon = AgentHub.RAYON_ACTION;
            int diametre = 2 * rayon;

            // Création image de référence.
            this._imageRef = new Bitmap(largeurCarte, hauteurCarte);

            for (int i = 0; i < largeurCarte; i++)
            {
                // Dessin de la carte.
                for (int j = 0; j < hauteurCarte; j++)
                {
                    this._imageRef.SetPixel(i, j, this.getColor(c.Elements[i][j].ElementBiome));
                }

                using (Graphics g = Graphics.FromImage(this._imageRef))
                {
                    // Dessin des hubs.
                    foreach (Coordonnees coord in coords)
                    {
                        g.DrawEllipse(Pens.Yellow, coord.X - rayon, coord.Y - rayon, diametre, diametre);
                    }
                }
            }
        }

        private void afficherImage()
        {
            List<Agent> agents = Kernel.GetAgents();

            Bitmap image = new Bitmap(this._imageRef);

            using (Graphics g = Graphics.FromImage(image))
            {
                // Génération des images des agents.
                for (int i = 0; i < agents.Count; i++)
                {
                    Agent agt = agents[i];
                
                    //if (i == this._agentTraite)
                    //{
                    //    // Dessin des hubs proches.
                    //    foreach (AgentHub hub in agt.Test_Hubs())
                    //    {
                    //        Rectangle r = new Rectangle(hub.Coord.X - this._rayon, hub.Coord.Y - this._rayon, this._diametre, this._diametre);

                    //        g.FillEllipse(this._brushHubProche, r);
                    //        g.DrawEllipse(this._penHubProche, r);

                    //    }

                    //    // Dessin de l'agent.
                    //    g.FillEllipse(Brushes.Orange, agt.Coord.X - 2, agt.Coord.Y - 2, 5, 5);
                    //}
                    //else
                        // Dessin de l'agent.
                        g.DrawEllipse(Pens.White, agt.Coord.X - 2, agt.Coord.Y - 2, 5, 5);
                }
            }

            //if (++this._agentTraite > agents.Count)
            //    this._agentTraite = 0;

            this.pictureBox1.Image = null;
            this.pictureBox1.Image = image;
        }

        private Color getColor(TypeElementBiome typeElementBiome)
        {
            switch (typeElementBiome)
            {
                case TypeElementBiome.Arbre:
                    return Color.DarkGreen;
                case TypeElementBiome.Eau:
                    //return Color.MediumAquamarine;
                    return Color.FromArgb(28, 132, 220);
                case TypeElementBiome.Pierre:
                    return Color.Gray;
                case TypeElementBiome.Sable:
                    return Color.SandyBrown;
                case TypeElementBiome.Terre:
                    //return Color.Brown;
                    return Color.FromArgb(114, 71, 21);
                default:
                    return Color.Black;
            }
        }


        private Carte _map;

        private void btnTest_Click(object sender, EventArgs e)
        {
            /*
            if (_map == null)
                _map = Kernel.GetCarteMonde();

            int x = Randomizer.Next(16, 513);
            int y = Randomizer.Next(16, 513);


            DateTime debut = DateTime.Now;
            Coordonnees pos = _map.getPostionElement(TypeElementBiome.Terre, new Coordonnees(x, y));
            double tempsExecution = DateTime.Now.Subtract(debut).TotalMilliseconds;

            string s = String.Format("Point de départ : ({0} ; {1})\nElément 'Terre' trouvé à la position ({2} ; {3}) en {4:0.000} ms",
                                x, y, pos.X, pos.Y, tempsExecution);

            MessageBox.Show(s);
            */

            Coordonnees centre = new Coordonnees(511, 511);
            //int rayon = 512;
            int rayonCarre; // = rayon * rayon;
            int yMax, yPrec = 0;
            int x1, x2, y1, y2;

            Bitmap bmp = new Bitmap(1024, 1024);

            //for (int x = 0; x < rayon; x++)
            //{
            //    x1 = centre.X - x;
            //    x2 = centre.X + x;
                
            //    yMax = (int)Math.Sqrt(rayonCarre - (x * x));
                
            //    for (int y = 0; y < yMax; y++)
            //    {
            //        y1 = centre.Y - y;
            //        y2 = centre.Y + y;

            //        bmp.SetPixel(x1, y1, Color.Black);
            //        bmp.SetPixel(x1, y2, Color.Black);

            //        bmp.SetPixel(x2, y1, Color.Black);
            //        bmp.SetPixel(x2, y2, Color.Black);
            //    }
            //}

            for (int rayon = 0; rayon < 512; rayon++)
            {
                rayonCarre = rayon * rayon;

                for (int x = 0; x <= rayon; x++)
                {
                    int y = (int)Math.Sqrt(rayonCarre - (x * x));

                    //for (int y = yPrec; y <= yMax; y++)
                    //{
                        x1 = centre.X - x;
                        x2 = centre.X + x;
                        y1 = centre.Y - y;
                        y2 = centre.Y + y;

                        bmp.SetPixel(x1, y1, Color.Black);
                        bmp.SetPixel(x1, y2, Color.Black);
                        bmp.SetPixel(x2, y1, Color.Black);
                        bmp.SetPixel(x2, y2, Color.Black);
                    //}

                    //yPrec = yMax;
                }

                yPrec = 0;
            }

            bmp.Save(@"Test_Recherche_Concentrique.bmp");

            MessageBox.Show("Terminé !");
        }
    }
}
