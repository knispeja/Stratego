using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class BoardPosition
    {
        private int x;
        private int y;
        public static readonly BoardPosition NULL_BOARD_POSITION = new BoardPosition(-1, -1);
        public BoardPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }
    }
    public class Gameboard
    {
        private int width;
        private int height;
        private BoardPosition lastFought;
        private int winner = 0;

        public Gameboard(int v1, int v2)
        {
            this.width = v1;
            this.height = v2;
        }

        public bool move(Move move)
        {
            BoardPosition start = move.getStart();
            BoardPosition end = move.getEnd();
            GamePiece defender = getPiece(end);
            GamePiece attacker = getPiece(start);
            if (defender == null)
            {
                this.lastFought = BoardPosition.NULL_BOARD_POSITION;
                this.setPiece(end, attacker);
                this.setPiece(start, null);
                attacker.setXVal(end.getX());
                attacker.setYVal(end.getY());
                attacker = null;
                return true;
            }
            else
            {
                if (attacker.getTeamCode() == defender.getTeamCode())
                {
                    return false;
                }
                int numOfSpacesPoss = attacker.getLimitToMovement();
                int deltaX = Math.Abs(end.getX() - start.getX());
                int deltaY = Math.Abs(end.getY() - start.getY());
                if (numOfSpacesPoss < deltaX || numOfSpacesPoss < deltaY)
                {
                    return false;
                }
            }
            battlePieces(attacker, defender);
            this.lastFought = end;
            if (!defender.isAlive() && defender.isEssential())
            {
                gameOver(attacker.getTeamCode());
            }
            if (!attacker.isAlive() && attacker.isEssential())
            {
                gameOver(defender.getTeamCode());
            }
            return true;
        }

        private void setPiece(BoardPosition end, GamePiece attacker)
        {
            throw new NotImplementedException();
        }

        private GamePiece getPiece(BoardPosition start)
        {
            throw new NotImplementedException();
        }

        private void gameOver(int v)
        {
            this.winner = v;
        }



        private void battlePieces(GamePiece attacker, GamePiece defender)
        {
            attacker.attack(defender);
            defender.defend(attacker);
            if (!defender.isAlive())
            {
                this.setPiece(defender.getXVal(), defender.getYVal(), null);
            }
            if (!attacker.isAlive())
            {
                this.setPiece(attacker.getXVal(), attacker.getYVal(), null);
            }
            else
            {
                this.setPiece(defender.getXVal(), defender.getYVal(), attacker);
            }
        }

        public int getHeight()
        {
            return this.height;
        }

        public int getWidth()
        {
            return this.width;
        }

        internal GamePiece getPiece(int x, int y)
        {
            throw new NotImplementedException();
        }

        internal void setPiece(int x, int row, GamePiece value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given row in the board state with the given value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        public void fillRow(GamePiece value, int row)
        {
            for (int x = 0; x < this.width; x++) setPiece(x, row, value);
        }

        internal object getLastFought()
        {
            return this.lastFought;
        }

        internal bool isGameOver()
        {
            return this.winner != 0;
        }

        internal int getWinner()
        {
            return this.winner;
        }
    }
}
