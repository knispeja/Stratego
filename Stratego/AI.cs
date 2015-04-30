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
            this.win = win;
        }

        /// <summary>
        /// Places this AI player's pieces. Currently places them very stupidly, can be updated later
        /// </summary>
        public void placePieces()
        {
            int x = 0;
            int y = 0;
            for (int piece = 1; piece < win.defaults.Length; piece++)
            {
                while (win.getPiecesLeft(piece) != 0)
                {
                    placePieceByTile(piece*this.team, x, y);
                    x++;
                    if(x >= win.boardState.GetLength(0))
                    {
                        x = 0;
                        y++;

                        // We've run out of tiles to place upon, looks like we're done here
                        if (y >= win.boardState.GetLength(1)) throw new InvalidOperationException();
                    }
                }
            }
        }

        /// <summary>
        /// Converts x and y tiles to x and y coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool? placePieceByTile(int piece, int xTile, int yTile)
        {
            int scaleX = win.panelWidth / win.boardState.GetLength(0);
            int scaleY = win.panelHeight / win.boardState.GetLength(1);
            int x = xTile*scaleX;
            int y = yTile*scaleY;

            return win.placePiece(piece, x, y);
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
