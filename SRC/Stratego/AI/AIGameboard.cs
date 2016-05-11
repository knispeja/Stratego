using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.AI
{
    public class AIGameboard : Gameboard
    {
        public AIGameboard(Gameboard g) : base(g.getWidth(), g.getHeight())
        {
        }

        public bool isWin()
        {
            return base.getWinner() == 2;
        }
    }
}
