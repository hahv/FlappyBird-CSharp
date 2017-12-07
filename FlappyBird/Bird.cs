using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Bird: GameObject
    {
        private static float maxSpeed = 10.0f;
        private static float flapSpeed = -14.0f;
        private static float gravity = 7.0f;


        private bool isDead;
        private Bitmap birdImage;
        private PointF pos;
        private float speedY;

        private float birdWidth;
        private float birdHeight;

        private int currentFrame;
        private int totalFrame;

        public bool IsDead
        {
            get
            {
                return isDead;
            }

            set
            {
                isDead = value;
            }
        }

        public Bird(Bitmap birdImg, PointF birdPos)
        {
            this.birdImage = birdImg;
            this.pos = birdPos;
            this.speedY = 0.0f;
            this.currentFrame = 0;
            this.totalFrame = 3;

            this.IsDead = false;
            float frameBirdWidth = this.birdImage.Width / (float)totalFrame;

            this.birdWidth = 35;
            this.birdHeight = this.birdWidth * (this.birdImage.Height / frameBirdWidth);


        }

        public RectangleF getBoundingBox()
        {
            return new RectangleF(this.pos.X - this.birdWidth/2.0f, 
                                  this.pos.Y - this.birdHeight/2.0f, 
                                  this.birdWidth, this.birdHeight);
        }

        public void die()
        {
            this.IsDead = true;
        }

        public void update()
        {
            if (this.IsDead) return;

            this.pos.Y += this.speedY;
            this.speedY = this.speedY + gravity;

            if (this.pos.Y + this.birdHeight/2 >= (Game.viewPort.Height - Game.groundHeight))
            {
                this.pos.Y = Game.viewPort.Height - Game.groundHeight - this.birdHeight / 2.0f;
                this.die();
            }

            this.updateAnimation();

        }

        private void updateAnimation()
        {
            this.currentFrame += 1;
            if (this.currentFrame >= this.totalFrame)
            {
                this.currentFrame = 0;
            }
        }

        public void flap()
        {
            if (IsDead) return;

            if (pos.X < 0.0f) return;
            speedY = flapSpeed;
        }

        public void draw(Graphics g)
        {
            // Create rectangle for displaying image.
            RectangleF destRect = new RectangleF(this.pos.X - this.birdWidth/2, this.pos.Y - this.birdHeight / 2, this.birdWidth, this.birdHeight);

            float frameWidth = this.birdImage.Width / this.totalFrame;
            // Create rectangle for source image.
            RectangleF srcRect = new RectangleF(this.currentFrame* frameWidth, 0, frameWidth, this.birdImage.Height);

            GraphicsUnit units = GraphicsUnit.Pixel;

            // Draw image to screen.
            g.DrawImage(this.birdImage, destRect, srcRect, units);
        }
    }
}
