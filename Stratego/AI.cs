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

        /// <summary>
        /// Initializes this AI player
        /// </summary>
        /// <param name="win">The window upon which to perform most game functions directly</param>
        /// <param name="team">This AI player's team, either -1 or 1 (typically that translates to red or blue)</param>
        public AI(StrategoWin win, int team)
        {
            if (Math.Abs(team) != 1) throw new ArgumentException();
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
            //int origXPos;
            //int origYPos;
            //int newXPos;
            //int newYPos;
        }
    }
}
