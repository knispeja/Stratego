using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class AI
    {
        private StrategoWin win;
        public int team { get; set; }

        public AI(StrategoWin win, int team)
        {
            this.team = team;
        }

        public void placePieces()
        {
            
        }

        private List<Move> generateValidMoves()
        {
            return null;
        }


        private class Move
        {

        }
    }
}
