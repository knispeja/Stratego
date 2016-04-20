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
                attacker = null;
                return true;
            }
            else
            {
                if (attacker.getTeamCode() == defender.getTeamCode() || defender.getTeamCode() == StrategoGame.NO_TEAM_CODE)
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
        /// TODO: doesn't work on non-square boards
        /// (the bottom-right piece becomes the top-left)
        /// </summary>
        public void flipBoard()
        {
            GamePiece[,] oldBoard = this.board;
            this.board = new GamePiece[this.width, this.height];

            for (int x = 0; x < this.width; ++x)
            {
                for (int y = 0; y < this.height; ++y)
                {
                    this.board[y, this.height - x - 1] = oldBoard[this.width - y - 1, x];
                }
            }
        }

        /// <summary>
        /// Overrides pieces of team [teamCode] with pieces from [other] if the piece in [other] is not null
        /// </summary>
        public void overridePiecesOfTeam(Gameboard other, int teamCode)
        {
            for (int col = 0; col < this.width; col++)
            {
                for (int row = 0; row < this.height; row++)
                {
                    GamePiece newPiece = other.getPiece(col, row);

                    GamePiece oldPiece = getPiece(col, row);
                    if (oldPiece == null || oldPiece.getTeamCode() == teamCode)
                    {
                        if (newPiece != null)
                            newPiece.setTeamCode(teamCode);

                        setPiece(col, row, newPiece);
                    }
                }
            }
        }

        private void gameOver(int v)
        {
            this.winner = v;
        }

        public void changePieceTypeBehavior(Type typeToChange, BattleBehavior attackBehav, BattleBehavior defendBehav)
        {
            System.Diagnostics.Debug.WriteLine("in changePieceBehavior");
            foreach (GamePiece piece in this.board)
            {
                if (piece != null && piece.GetType().Equals(typeToChange))
                {
                    System.Diagnostics.Debug.WriteLine(piece.GetType().ToString());
                    piece.setAttackBehavior(attackBehav);
                    piece.setDefendBehavior(defendBehav);
                }
            }
        }

        private void battlePieces(GamePiece attacker, GamePiece defender)
        {
            BoardPosition defenderPos = this.getPositionOfPiece(defender);
            BoardPosition attackerPos = this.getPositionOfPiece(attacker);
            attacker.attack(defender);
            defender.defend(attacker);
            if (!defender.isAlive())
            {
                this.setPiece(defenderPos, null);
            }
            if (!attacker.isAlive())
            {
                this.setPiece(attackerPos, null);
            }
            else
            {
                this.setPiece(attackerPos, null);
                this.setPiece(defenderPos, attacker);
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

        public BoardPosition getPositionOfPiece(GamePiece piece)
        {
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    if (board[i, j] == piece)
                    {
                        return new BoardPosition(i, j);
                    }
                }
            }
            return null;
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

        public int[,] getPieceMoves(int x, int y)
        {
            int[,] moveArray = new int[this.getHeight(), this.getWidth()];

            GamePiece pieceInQuestion = this.getPiece(x, y);
            if (pieceInQuestion == null || !pieceInQuestion.isMovable() || pieceInQuestion.getLimitToMovement() == 0)
            {
                return moveArray;
            }
            int startingX = x;
            int startingY = y;
            int spacesPossible = pieceInQuestion.getLimitToMovement();
            if (spacesPossible == int.MaxValue)
                spacesPossible = Math.Max(this.getHeight(), this.getWidth());
            MovementGrouping rightForward = new MovementGrouping(startingX + 1, this.getWidth(), startingY, true, startingX + spacesPossible);
            MovementGrouping rightBackward = new MovementGrouping(startingX - 1, 0, startingY, true, startingX - spacesPossible);
            MovementGrouping leftForward = new MovementGrouping(startingY + 1, this.getHeight(), startingX, false, startingY + spacesPossible);
            MovementGrouping leftBackward = new MovementGrouping(startingY - 1, 0, startingX, false, startingY - spacesPossible);
            moveArray = moveArrayAdjust(rightForward, 1, pieceInQuestion, moveArray);
            moveArray = moveArrayAdjust(rightBackward, -1, pieceInQuestion, moveArray);
            moveArray = moveArrayAdjust(leftForward, 1, pieceInQuestion, moveArray);
            moveArray = moveArrayAdjust(leftBackward, -1, pieceInQuestion, moveArray);
            return moveArray;
        }


        private int[,] moveArrayAdjust(MovementGrouping mvmtGroup, int sign, GamePiece pieceInQuestion, int[,] moveArray)
        {
            int posX;
            int posY;
            GamePiece potenPiece = null;
            for (int i = mvmtGroup.getStarting(); (i * sign) < (sign * mvmtGroup.getEnding()); i += sign)
            {
                if ((sign * i) > (sign * mvmtGroup.getStopNum()))
                {
                    return moveArray;
                }
                if (mvmtGroup.isRight())
                {
                    posX = i;
                    posY = mvmtGroup.getInvariable();
                }
                else
                {
                    posX = mvmtGroup.getInvariable();
                    posY = i;
                }
                potenPiece = this.getPiece(posX, posY);
                if (potenPiece == null)
                {
                    moveArray[posX, posY] = 1;
                }
                else if (pieceInQuestion.getTeamCode() != potenPiece.getTeamCode() && potenPiece.getTeamCode() != 0)
                {
                    moveArray[posX, posY] = 1;
                    return moveArray;
                }
                else
                {
                    return moveArray;
                }
            }
            return moveArray;
        }

        public class MovementGrouping
        {

            private int starting;
            private int ending;
            private int invariable;
            private Boolean right;
            private int stopNum;

            public MovementGrouping(int starting, int ending, int invariable, Boolean right, int stopNum)
            {
                this.starting = starting;
                this.ending = ending;
                this.invariable = invariable;
                this.right = right;
                this.stopNum = stopNum;
            }

            public int getStarting()
            {
                return this.starting;
            }

            public int getEnding()
            {
                return this.ending;
            }

            public int getInvariable()
            {
                return this.invariable;
            }

            public Boolean isRight()
            {
                return this.right;
            }

            public int getStopNum()
            {
                return this.stopNum;
            }
        }
    }
}
