using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class ScrollBackground : GameObject
    {
        private Bitmap backGround;
        private float scrollSpeed;
        private PointF pos;
        private float bgHeight;


        public ScrollBackground(Bitmap bg, float speed, PointF initPos, float height)
        {
            this.backGround = bg;
            this.scrollSpeed = speed;
            this.pos = initPos;
            this.bgHeight = height;
        }


        public void draw(Graphics g)
        {
            // Create rectangle for displaying image.
            RectangleF destRect = new RectangleF(0, this.pos.Y, Game.viewPort.Width, this.bgHeight);

            // Create rectangle for source image.
            RectangleF srcRect = new RectangleF(this.pos.X, 0, Game.viewPort.Width, backGround.Height);

            GraphicsUnit units = GraphicsUnit.Pixel;

            // Draw image to screen.
            g.DrawImage(this.backGround, destRect, srcRect, units);
        }

        public void update()
        {
            this.pos.X += this.scrollSpeed;
            if (this.pos.X + Game.viewPort.Width > this.backGround.Width)
                this.pos.X = 0;
        }

    }
}
