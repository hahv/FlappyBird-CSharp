using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    public interface IGameComponent
    {
        void draw(Graphics g);
        void update();

    }
}
