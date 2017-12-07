using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Game: IGameComponent
    {
	    public static readonly float scrollSpeed = 10.0f;
        public static readonly float pipeDistance = 100.0f;
        public static readonly float pipeSpread = 65;

        public static readonly float groundHeight = 80;

        public static RectangleF viewPort;
        public static string resourceDir;

        private Bird bird;
        private Bitmap pipeTop;
        private Bitmap pipeBottom;

        private ScrollBackground background;
        private ScrollBackground ground;

        private float distance;
        private int score;
        private bool isGameOver;
        private bool isGameStart;

        private float distanceToFirstPipe;
        private float lastPipeX;

        List<Pipe> pipes;
        private Random rand;
        
        public Game()
        {
            this.pipeTop = this.getBitmapWithFileName("pipetop.png");
            this.pipeBottom = this.getBitmapWithFileName("pipebottom.png");

            background = new ScrollBackground(this.getBitmapWithFileName("background.jpg"), 3.0f, new PointF(0.0f, 0.0f), Game.viewPort.Height - Game.groundHeight);
            ground = new ScrollBackground(this.getBitmapWithFileName("ground.jpg"), 10.0f, new PointF(0.0f, Game.viewPort.Height - Game.groundHeight), Game.groundHeight);


            this.reset();

        }


        public void reset()
        {
            this.bird = new Bird(this.getBitmapWithFileName("bird.png"), new PointF(viewPort.Width / 2.0f, viewPort.Height / 2.0f));
            this.pipes = new List<Pipe>();

            this.rand = new Random();

            this.distance = 0.0f;
            this.distanceToFirstPipe = viewPort.Width * 2;

            this.score = 0;
            this.isGameOver = false;
            this.isGameStart = true;

            addPipe(distanceToFirstPipe);
            this.lastPipeX = distanceToFirstPipe;


        }

        void drawStringCenterAtRect(Graphics g, string drawString, float drawRectHeight)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            // Create font and brush.
            Font drawFont = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            RectangleF drawRect = new RectangleF(0.0f, 0.0f, viewPort.Width, drawRectHeight);
            g.DrawString(drawString, drawFont, drawBrush, drawRect, sf);
        }


        private Bitmap getBitmapWithFileName(string fileName)
        {
            return (Bitmap)Image.FromFile(Game.resourceDir + fileName);
        }

        public void handleMouseDown()
        {
            if (this.isGameOver)
            {
                this.reset();
            }
            else if (this.isGameStart)
            {
                this.isGameStart = false;
            }
            else
            {
                bird.flap();
            }

        }

        public void update()
        {
            if (this.isGameStart)
                return;


            if (bird.IsDead)
            {
                this.isGameOver = true;
                return;
            }

            background.update();
            ground.update();
            bird.update();

            foreach(Pipe pipeItem in pipes.ToList())
            {
                pipeItem.scroll(Game.scrollSpeed);

                if (pipeItem.didScore(bird))
                    ++score;

                if (pipeItem.collides(bird))
                {
                    bird.die();
                }

                if (pipeItem.offScreen())
                {
                    pipes.Remove(pipeItem);
                }

            }

            distance += Game.scrollSpeed;

            float xMaxAfterScroll = distance + Game.scrollSpeed + viewPort.Width;

            while (xMaxAfterScroll >= lastPipeX)
            {
                this.addPipe((this.lastPipeX + Pipe.pipeWidth + pipeDistance) - distance);
                this.lastPipeX += (pipeDistance + Pipe.pipeWidth);

            }

        }

        public void draw(Graphics g)
        {
            background.draw(g);
            bird.draw(g);

            foreach (Pipe pipeItem in pipes.ToList())
            {
                pipeItem.draw(g);
            }

            ground.draw(g);

            this.drawStringCenterAtRect(g, this.score + "", 50);

            if (this.isGameOver)
                this.drawStringCenterAtRect(g, "Game Over!", viewPort.Height);
            if (this.isGameStart)
                this.drawStringCenterAtRect(g, "Click to start", viewPort.Height - 80.0f);
        }

        void addPipe(float posX)
        {
            float x, y;

            x = posX;

            float actualGameAreaIgnoringGround = viewPort.Height - Game.groundHeight;

            int topPipeHeightMin = (int)(actualGameAreaIgnoringGround * 0.3f);
            int bottomPipHeightMin = (int)topPipeHeightMin;
            int topPipeHeightMax = (int)(actualGameAreaIgnoringGround - Game.pipeSpread - bottomPipHeightMin);

            int topPipeHeight =  rand.Next(topPipeHeightMax + 1 - topPipeHeightMin) + topPipeHeightMin;

            y = topPipeHeight + Game.pipeSpread / 2.0f;

            Pipe pipe = new Pipe(new PointF(x, y), this.pipeTop, this.pipeBottom);
            pipes.Add(pipe);

        }
    }
}
