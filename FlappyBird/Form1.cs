using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBird
{
    public partial class Form1 : Form
    {
        private Bitmap buffer;
        private Game game;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buffer = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);

            Game.viewPort = this.ClientRectangle;
            Game.resourceDir = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\Resources\\";

            game = new Game();

            this.timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.update();

            Graphics bufferGraphic = Graphics.FromImage(this.buffer);
            bufferGraphic.Clear(Color.White);
            game.draw(bufferGraphic);
            this.CreateGraphics().DrawImage(this.buffer, this.ClientRectangle);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            this.game.handleMouseDown();
        }
    }
}
