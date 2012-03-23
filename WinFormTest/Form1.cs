using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vrac.SMA;

namespace WinFormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //while (true)
            //{
            //    Thread.Sleep(3000);
            //}
                this.pictureBox1.Image = Kernel.Draw();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Kernel.Draw();
        }
    }
}
