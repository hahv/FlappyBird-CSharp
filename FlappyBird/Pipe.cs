using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Pipe: GameObject
    {
        public static readonly float pipeWidth = 50.0f;
        private bool scored;

        private Bitmap top;
        private RectangleF topRect;

        private Bitmap bottom;
        private RectangleF bottomRect;

        public RectangleF TopRect
        {
            get
            {
                return topRect;
            }

            set
            {
                topRect = value;
            }
        }

        public RectangleF BottomRect
        {
            get
            {
                return bottomRect;
            }

            set
            {
                bottomRect = value;
            }
        }

        public Pipe(PointF pos, Bitmap topPipe, Bitmap bottomPipe)
        {
            this.scored = false;

            this.top = topPipe;
            this.bottom = bottomPipe;

            float centerOfGapBetweenTwoPipes = pos.Y;
            topRect = new RectangleF(pos.X, 0, pipeWidth, centerOfGapBetweenTwoPipes - Game.pipeSpread/2.0f);

            float bottomRectY = centerOfGapBetweenTwoPipes + Game.pipeSpread / 2.0f;
            bottomRect = new RectangleF(pos.X, bottomRectY , pipeWidth, Game.viewPort.Height - bottomRectY);
        }

        public void scroll(float x)
        {
            topRect.X -= x;
            bottomRect.X = TopRect.X;
        }
        public void draw(Graphics g)
        {
            g.DrawImage(this.top, this.TopRect);
            g.DrawImage(this.bottom, this.BottomRect);
        }
        public bool collides(Bird bird)
        {
            RectangleF birdBoundingBox = bird.getBoundingBox();

            if (this.checkCollisionTwoRect(birdBoundingBox, this.TopRect) ||
               this.checkCollisionTwoRect(birdBoundingBox, this.BottomRect))
                return true;

            return false;
        }

        public bool didScore(Bird bird)
        {
            if (scored) return false;

            bool inside = bird.getBoundingBox().X >= TopRect.X;
            if (inside)
                scored = true;

            return inside;
        }
        public bool offScreen()
        {
            return (this.TopRect.Right <= 0);
        }

    }
}
