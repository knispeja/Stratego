using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    [Serializable]
    public class Gameboard
    {
        private int width;
        private int height;
        private BoardPosition lastFought;
        private int winner = 0;
        private GamePiece[,] board;

        public Gameboard(int width, int height)
        {
            resetBoard(width, height);
        }

        public void resetBoard()
        {
            // Board should be filled with null automatically (empty spaces)
            this.board = new GamePiece[this.width, this.height];
        }

        public void resetBoard(int width, int height)
        {
            this.width = width;
            this.height = height;
            resetBoard();
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

        /// <summary>
        /// Flips the board upside down
        /// (the bottom-right piece becomes the top-left)
        /// </summary>
        public void flipBoard()
        {
            GamePiece[,] oldBoard = this.board;
            this.board = new GamePiece[this.width, this.height];
            oldBoard = oldBoard;

            for (int i = 0; i < this.width; ++i)
            {
                for (int j = 0; j < this.height; ++j)
                {
                    this.board[j, this.height - i - 1] = oldBoard[this.width - j - 1, i];
                }
            }
        }

        /// <summary>
        /// Overrides pieces of team [teamCode] with pieces from [other] if the piece in [other] is not null
        /// </summary>
        public void overridePiecesOfTeam(Gameboard other, int teamCode)
        {
            for(int col = 0; col < this.width; col++)
            {
                for(int row = 0; row < this.height; row++)
                {
                    GamePiece newPiece = other.getPiece(col, row);

                    GamePiece oldPiece = getPiece(col, row);
                    if (oldPiece == null || oldPiece.getTeamCode() == teamCode)
                    {
                        if (newPiece != null)
                        {
                            newPiece.setTeamCode(teamCode);
                            newPiece.setXVal(col);
                            newPiece.setYVal(row);
                        }
                        setPiece(col, row, newPiece);   
                    }
                }
            }
        }

        public void removeAllInstances(Type pieceType)
        {
            for (int col = 0; col < this.width; col++)
            {
                for (int row = 0; row < this.height; row++)
                {
                    GamePiece piece = getPiece(col, row);
                    if(piece != null)
                    {
                        if (piece.GetType() == pieceType)
                            setPiece(col, row, null);
                    }
                }
            }
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

        public void setPiece(BoardPosition pos, GamePiece piece)
        {
            this.board[pos.getX(), pos.getY()] = piece;
        }

        public GamePiece getPiece(BoardPosition pos)
        {
            return this.board[pos.getX(), pos.getY()];
        }

        public void setPiece(int x, int y, GamePiece piece)
        {
            this.board[x, y] = piece;
        }

        public GamePiece getPiece(int x, int y)
        {
            return this.board[x, y];
        }

        /// <summary>
        /// Fills the given row in the board state with the given value
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        public void fillRow(GamePiece piece, int row)
        {
            for (int x = 0; x < this.width; x++)
                setPiece(x, row, piece);
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
