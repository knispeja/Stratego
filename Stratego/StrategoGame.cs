using Stratego.GamePieces;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Stratego
{
    public class StrategoGame
    {
        /// <summary>
        /// The 2DArray full of all pieces on the board
        /// </summary>
        public Gameboard boardState { get; set; }

        /// <summary>
        /// Whether or not the pre game has begun
        /// </summary>
        public bool preGameActive { get; set; }

        /// <summary>
        /// -1 for player2 and 1 for player 1. 0 when game isn't started. 
        /// 2 for transition from player1 to player2; -2 for transition from player2 to player1.
        /// </summary>
        public int turn { get; set; }

        /// <summary>
        /// Whether player 2 is an AI or not
        /// </summary>
        public Boolean isSinglePlayer { get; set; }

        /// <summary>
        /// If bombs can be moved
        /// </summary>
        public Boolean movableBombs { get; set; }

        /// <summary>
        /// If flags can be moved
        /// </summary>
        public Boolean movableFlags { get; set; }

        /// <summary>
        /// Coordinates of the last piece to win a battle
        /// </summary>
        public Point lastFought { get; set; }

        /// <summary>
        /// The AI that the player will play against, if they choose single player.
        /// </summary>
  //      public AI_Old ai;

        /// <summary>
        /// If levels can be skipped using keypresses
        /// </summary>
        public Boolean skippableLevels { get; set; }

        public BoardPosition selectedPosition = new BoardPosition(-1, -1);

        public GamePieceFactory factory = new GamePieceFactory();

        public GUICallback callback;

        public static readonly int NO_TEAM_CODE = 0;
        public static readonly int RED_TEAM_CODE = -1;
        public static readonly int BLUE_TEAM_CODE = 1;
        public static readonly int KILL_FEED_SIZE = 5;
        private string[] killFeed = new string[KILL_FEED_SIZE];
        private GamePiece selectedPiece;

        public StrategoGame(GUICallback callback)
        {
            this.turn = NO_TEAM_CODE;
            this.preGameActive = false;
            this.skippableLevels = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);
            this.movableBombs = false;
            this.movableFlags = false;
            this.callback = callback;

            for(int i = 0; i < KILL_FEED_SIZE; i++)
            {
                killFeed[i] = "";
            }

            boardState = new Gameboard(10, 10);
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 10; col++)
                    this.boardState.setPiece(col, row, new ObstaclePiece(0));
            }

            //      this.ai = new AI_Old(this, -1);

        }
        public StrategoGame(Gameboard boardState, GUICallback callback)
        {
            this.boardState = boardState;
            this.turn = NO_TEAM_CODE;
            this.preGameActive = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);
            this.movableBombs = false;
            this.movableFlags = false;
            this.callback = callback;
            //      this.ai = new AI(this, -1);
            this.factory.resetPlacements();
        }

        /// <summary>
        /// Places a piece at the given coordinates
        /// </summary>
        /// <param name="piece">Number of the piece you want to place</param>
        /// <param name="x">x-coordinate you want to place it at</param>
        /// <param name="y">y-coordinate you want to place it at</param>
        /// <returns>Whether or not the placement was successful</returns>
        public bool placePiece(int x, int y)
        {
            GamePiece piece = this.selectedPiece;
            if (turn == 0 || Math.Abs(turn) == 2) return false;
            if (piece != null && piece.getTeamCode() != turn) return false;

            GamePiece pieceAtPos = this.boardState.getPiece(x, y);

            if (piece == null)
            {
                // We are trying to remove
                if (pieceAtPos == null || pieceAtPos.getTeamCode() == NO_TEAM_CODE) return false;
                if (pieceAtPos.getTeamCode() != this.turn) return false;
                this.factory.incrementPiecesLeft(pieceAtPos.getPieceName());
            }
            else if (pieceAtPos == null && piece!=null && this.factory.placements[piece.getPieceName()] > 0)
            {
                // We are trying to add
                this.factory.decrementPiecesLeft(piece.getPieceName());
            }
            else return false;

            this.boardState.setPiece(x, y, piece);
            return true;
        }

        public void behavioralChange(Type gamePieceType, Type attackBehav, Type defendBehav)
        {
            var attackConst = attackBehav.GetConstructors();

            var defendConst = defendBehav.GetConstructors();

            BattleBehavior newAttackObj = (BattleBehavior)attackConst[0].Invoke(new object[] { });

            BattleBehavior newDefendObj = (BattleBehavior)defendConst[0].Invoke(new object[] { });

            this.boardState.changePieceTypeBehavior(typeof(MarshallPiece), newAttackObj, newDefendObj);
            this.factory.changeAttackBehav(gamePieceType, attackBehav);
            this.factory.changeDefendBehav(gamePieceType, defendBehav);
        }

        /// <summary>
        /// Looks at the current turn, and changes it to whatever the next turn should be.
        /// Handles global game variables like the stage of the game and so on.
        /// Also sends a call to the AI to notify it that it's time to take its turn when necessary.
        /// </summary>
        public void nextTurn()
        {
            this.callback.invalidateBackpanel();
            // We just came here from the main menu
            if (this.turn == NO_TEAM_CODE)
            {
                preGameActive = true;
                this.turn = BLUE_TEAM_CODE;
            }
            // It's blue player's turn
            else if (this.turn == BLUE_TEAM_CODE)
            {
                if (this.preGameActive)
                {
                    this.turn = -1;
                    this.factory.resetPlacements();
                }
                else
                {
                    this.turn = 2;
                    if (!this.checkMoves())
                        this.callback.gameOver(StrategoGame.BLUE_TEAM_CODE);
                    else
                    {
                        if (!this.isSinglePlayer)
                            this.callback.adjustTurnButtonState("Player 2's Turn");
                        else
                            this.callback.adjustTurnButtonState("AI's Turn");
                    }
                }
            }
            // It's red player's turn
            else if (this.turn == RED_TEAM_CODE)
            {
                if (this.preGameActive)
                {
                    this.callback.setSidePanelVisibility(false);
                    for (int i = 4; i < 6; i++)
                    {
                        for (int x = 0; x < 2; x++)
                            this.boardState.setPiece(x, i, null);
                        for (int x = 4; x < 6; x++)
                            this.boardState.setPiece(x, i, null);
                        for (int x = 8; x < 10; x++)
                            this.boardState.setPiece(x, i, null);
                    }
                    this.preGameActive = false;
                }
                if (!this.isSinglePlayer || !this.boardState.getLastFought().Equals(BoardPosition.NULL_BOARD_POSITION)) this.turn = -2;
                else this.turn = BLUE_TEAM_CODE;
                if (!this.checkMoves())
                    this.callback.gameOver(RED_TEAM_CODE);
                else
                    this.callback.adjustTurnButtonState("Player 1's Turn");
            }
            else if (this.turn == -2)
            {
                turn = BLUE_TEAM_CODE;
            }
            else
            {
                turn = RED_TEAM_CODE;
            }

            //if (this.isSinglePlayer && this.turn == this.ai.team)
            //{
            //    if (this.preGameActive)
            //        this.ai.placePieces();
            //    else
            //        this.ai.takeTurn();
            //}
        }

        /// <summary>
        /// Selects a piece if no piece is selected.
        /// </summary>
        /// <param name="x">x coords of the click in pixels</param>
        /// <param name="y">y coord of the click in pixels</param>
        /// <returns></returns>
        public bool? SelectPiece(int x, int y)
        {
            /*
                The if-block immediate below is NOT what we want, but haven't changed it yet to remind us
                to change the general strategy other places too. We'll need to come back and spot-check this stuff.
            */
            if ((Math.Abs(turn) == 2) || (turn == -1 && isSinglePlayer))
            {
                return false;
            }
            if (this.selectedPosition != null && !this.selectedPosition.Equals(BoardPosition.NULL_BOARD_POSITION))
            {
                return true;
            }
            GamePiece potentialSel = this.boardState.getPiece(x, y);
            if (potentialSel == null)
            {
                this.selectedPosition = BoardPosition.NULL_BOARD_POSITION;
                return false;
            }
            if ((!potentialSel.isMovable() || potentialSel.getTeamCode() != this.turn))
            {
                this.selectedPosition = BoardPosition.NULL_BOARD_POSITION;
                return false;
            }
            this.selectedPosition = new BoardPosition(x, y);
            return true;
        }

        /// <summary>
        /// Moves the selected piece(if there is one) to the tile tile which corresponds to the x,y coords (if valid)
        /// </summary>
        /// <param name="x">x coordinate of the mouse click of where to move (pixels)</param>
        /// <param name="y">y coordinate of the mouse click of where to move (pixels)</param>
        /// <returns>true if a piece was moved, false otherwise</returns>
        public bool MovePiece(int x, int y)
        {
            GamePiece defender = this.boardState.getPiece(x, y);
            GamePiece attacker = this.boardState.getPiece(this.selectedPosition);
            if (attacker == null)
            {
                return false;
            }
            else if (this.selectedPosition.getX() == x && this.selectedPosition.getY() == y)
            {
                // Initialize "Selection Phase"
                this.selectedPosition = BoardPosition.NULL_BOARD_POSITION;
                return false;
            }
            int[,] moves = this.GetPieceMoves(this.selectedPosition.getX(), this.selectedPosition.getY());
            if (moves[x, y] != 1)
            {
                return false;
            }
            Move move = new Stratego.Move(this.selectedPosition.getX(), this.selectedPosition.getY(), x, y);
            bool res = this.boardState.move(move);
            if (defender != null&&!defender.isAlive())
            {
                updateKillFeed(attacker, defender);
            }
            if (defender != null&&!attacker.isAlive())
            {
                updateKillFeed(defender, attacker);
            }
            this.selectedPosition = BoardPosition.NULL_BOARD_POSITION;
            if (!boardState.isGameOver() && res)
            {
                this.nextTurn();
            }
            else if (boardState.isGameOver())
            {
                this.callback.gameOver(boardState.getWinner());
            }
            return res;
        }

        private void updateKillFeed(GamePiece killer, GamePiece killed)
        {
            for(int i = KILL_FEED_SIZE - 1; i > 0; i--)
            {
                killFeed[i] = killFeed[i - 1];
            }
            killFeed[0] = killer.getPieceName() + "->" + killed.getPieceName();
        }

        /// <summary>
        /// Finds all of the possible moves for a piece with the given X and Y coordinates using the games board state.
        /// </summary>
        /// <param name="pieceX">X position in the board state (not in pixels)</param>
        /// <param name="pieceY">Y position in the board state (not in pixels)</param>
        /// <returns>A 2D array containing 1 in every space where the deisgnated piece can move and 0 otherwise</returns>
        public int[,] GetPieceMoves(int pieceX, int pieceY)
        {
            return this.boardState.getPieceMoves(pieceX, pieceY);
        }

        internal string[] getKillFeed()
        {
            return this.killFeed;
        }


        public void resetPiecePlacing()
        {
            if(this.selectedPiece!=null)
                this.selectedPiece = this.factory.getPiece(this.selectedPiece.getPieceName(), this.turn);
        }

        public Boolean checkMoves()
        {
            for (int x1 = 0; x1 < this.boardState.getWidth(); x1++)
            {
                for (int y1 = 0; y1 < this.boardState.getHeight(); y1++)
                {
                    GamePiece piece = this.boardState.getPiece(x1, y1);
                    if (piece!=null && Math.Sign(piece.getTeamCode()) == Math.Sign(this.turn))
                    {
                        int[,] validPlaces = GetPieceMoves(x1, y1);
                        for (int x2 = 0; x2 < this.boardState.getWidth(); x2++)
                        {
                            for (int y2 = 0; y2 < this.boardState.getHeight(); y2++)
                            {
                                if (validPlaces[x2, y2] == 1)
                                    return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void resetPlacements()
        {
            this.factory.resetPlacements();
        }

        public bool isDonePlacing()
        {
            return this.factory.donePlacing();
        }

        public void setPlacements(Dictionary<string, int> dictionary)
        {
            this.factory.setPlacements(dictionary);
        }

        public Dictionary<string, int> getPlacements()
        {
            return this.factory.getPlacements();
        }

        public void setPiecePlacing(int num)
        {
            this.selectedPiece = this.factory.getPiece(num, this.turn);
        }

        public void setPiecePlacing(string name)
        {
            this.selectedPiece = this.factory.getPiece(name, this.turn);
        }
    }
}
