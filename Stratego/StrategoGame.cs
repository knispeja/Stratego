using System;
using System.Collections.Generic;
using System.Drawing;

namespace Stratego
{
    public class StrategoGame
    {

        /// <summary>
        /// The default amount of pieces for each piece. (EX: 0 0s; 1 1; 1 2; 2 3s; 4 4s; etc..)
        /// </summary>
        public static readonly Dictionary<String, int> defaults = new Dictionary<String, int>()
        { { FlagPiece.FLAG_NAME, 1 }, { BombPiece.BOMB_NAME, 6 }, { SpyPiece.SPY_NAME, 1 },
            { ScoutPiece.SCOUT_NAME, 8 }, { MinerPiece.MINER_NAME, 5 }, { SergeantPiece.SERGEANT_NAME, 4 },
            {LieutenantPiece.LIEUTENANT_NAME, 4 }, {CaptainPiece.CAPTAIN_NAME, 4 }, { MajorPiece.MAJOR_NAME, 3}, { ColonelPiece.COLONEL_NAME, 2},
            { GeneralPiece.GENERAL_NAME, 1}, { MarshallPiece.MARSHALL_NAME, 1 }
        };

        public Dictionary<int, Type> pieceTypes = new Dictionary<int, Type>();

        /// <summary>
        /// Current level of the game. Equals -1 if not in campaign mode
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// The 2DArray full of all pieces on the board
        /// </summary>
        public Gameboard boardState { get; set; }

        /// <summary>
        /// The array which holds information on how many pieces of each type can still be placed
        /// </summary>
        public Dictionary<String, int> placements;

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

        public GUICallback callback;

        public static readonly int NO_TEAM_CODE = 0;
        public static readonly int RED_TEAM_CODE = -1;
        public static readonly int BLUE_TEAM_CODE = 1;
        public static readonly int KILL_FEED_SIZE = 5;
        private string[] killFeed = new string[KILL_FEED_SIZE];

        public StrategoGame(GUICallback callback)
        {
            this.turn = NO_TEAM_CODE;
            this.preGameActive = false;
            this.skippableLevels = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);
            this.movableBombs = false;
            this.movableFlags = false;
            this.level = -1;
            this.callback = callback;

            for(int i = 0; i < KILL_FEED_SIZE; i++)
            {
                killFeed[i] = "debug killfeed entry " + i;
            }

            boardState = new Gameboard(10, 10);
            for (int row = 0; row < 6; row++) this.boardState.fillRow(new ObstaclePiece(0), row);

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
            this.resetPlacements();
        }
        

        public void resetPlacements()
        {
            this.placements = new Dictionary<string, int>();
            foreach (string key in StrategoGame.defaults.Keys)
            {
                this.placements.Add(key, StrategoGame.defaults[key]);
            }
        }
        /// <summary>
        /// Retrieves the number of pieces still available for
        /// placement of a given type
        /// </summary>
        /// <param name="piece">Type of the piece you want to check</param>
        /// <returns>Number of pieces available for placement</returns>
        public int getPiecesLeft(GamePiece piece)
        {
            return this.placements[piece.getPieceName()];
        }

        /// <summary>
        /// Places a piece at the given coordinates
        /// </summary>
        /// <param name="piece">Number of the piece you want to place</param>
        /// <param name="x">x-coordinate you want to place it at</param>
        /// <param name="y">y-coordinate you want to place it at</param>
        /// <returns>Whether or not the placement was successful</returns>
        public bool? placePiece(GamePiece piece, int x, int y)
        {
            if (turn == 0 || Math.Abs(turn) == 2) return false;
            if (piece != null && piece.getTeamCode() != turn) return false;
            Boolean retVal = true;

            GamePiece pieceAtPos = this.boardState.getPiece(x, y);

            if (piece == null)
            {
                // We are trying to remove
 
                if (pieceAtPos == null || pieceAtPos.getTeamCode() == NO_TEAM_CODE) return false;
                if (pieceAtPos.getTeamCode() != this.turn) return false;
                this.placements[pieceAtPos.getPieceName()]++;
            }
            else if (pieceAtPos == null && piece!=null && this.placements[piece.getPieceName()] > 0)
            {
                // We are trying to add
                this.placements[piece.getPieceName()] -= 1;
            }
            else retVal = false;

            if (retVal)
            {
                this.boardState.setPiece(x, y, piece);
            }
            return retVal;
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
                    this.resetPlacements();
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
            if (this.selectedPosition != null && !this.selectedPosition.Equals(new BoardPosition(-1, -1)))
            {
                return true;
            }
            GamePiece potentialSel = this.boardState.getPiece(x, y);
            if (potentialSel == null)
            {
                this.selectedPosition = new BoardPosition(-1, -1);
                return false;
            }
            if ((!potentialSel.isMovable() || potentialSel.getTeamCode() != this.turn))
            {
                this.selectedPosition = new BoardPosition(-1, -1);
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
                this.selectedPosition = new BoardPosition(-1, -1);
                return false;
            }
            Move move = new Stratego.Move(this.selectedPosition.getX(), this.selectedPosition.getY(), x, y);
            bool res = this.boardState.move(move);
            if (defender != null&&!defender.isAlive())
            {
                updateKillFeed(attacker, defender);
            }
            if (!attacker.isAlive())
            {
                updateKillFeed(defender, attacker);
            }

            if (!boardState.isGameOver())
            {
                this.nextTurn();
            }
            this.selectedPosition = new BoardPosition(-1, -1);
            return res;
        }

        private void updateKillFeed(GamePiece killer, GamePiece killed)
        {
            for(int i = 1; i < KILL_FEED_SIZE; i++)
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
            return GetPieceMoves(pieceX, pieceY, this.boardState);
        }

        internal string[] getKillFeed()
        {
            return this.killFeed;
        }

        /// <summary>
        /// Finds all of the possible moves for a piece with the given X and Y coordinates using the board state passed in.
        /// </summary>
        /// <param name="x">>X position in the board state (not in pixels)</param>
        /// <param name="y">Y position in the board state (not in pixels)</param>
        /// <param name="boardState">A 2D array representing the state of the board.</param>
        /// <returns>A 2D array containing 1 in every space where the deisgnated piece can move and 0 otherwise</returns>
        public int[,] GetPieceMoves(int x, int y, Gameboard boardState=null)
        {
            if (this.boardState == null)
            {
                boardState = this.boardState;
            }
            int[,] moveArray = new int[boardState.getHeight(), boardState.getWidth()];

            GamePiece pieceInQuestion = boardState.getPiece(x, y);
            if (!pieceInQuestion.isMovable() || pieceInQuestion.getLimitToMovement() == 0)
            {
                return moveArray;
            }
            int startingX = x;
            int startingY = y;
            int spacesPossible = pieceInQuestion.getLimitToMovement();
            GamePiece potenPiece = null;
            for (int k = startingX; k <= startingX + spacesPossible; k++)
            {
                if (k >= boardState.getWidth())
                {
                    break;
                }
                potenPiece = boardState.getPiece(k, startingY);
                if (potenPiece == null || pieceInQuestion.getTeamCode() != potenPiece.getTeamCode() && pieceInQuestion.getTeamCode()!=NO_TEAM_CODE)
                {
                    moveArray[k, startingY] = 1;
                }
                else
                {
                    break;
                }
            }
            for (int i = startingX; i >= startingX - spacesPossible; i--)
            {
                if (i < 0)
                {
                    break;
                }
                potenPiece = boardState.getPiece(i, startingY);
                if (potenPiece == null || pieceInQuestion.getTeamCode() != potenPiece.getTeamCode() && pieceInQuestion.getTeamCode() != NO_TEAM_CODE)
                {
                    moveArray[i, startingY] = 1;
                }
                else
                {
                    break;
                }
            }
            for (int j = startingY; j <= startingY + spacesPossible; j++)
            {
                if (j >= boardState.getHeight())
                {
                    break;
                }
                potenPiece = boardState.getPiece(startingX, j);
                if (potenPiece == null || pieceInQuestion.getTeamCode() != potenPiece.getTeamCode() && pieceInQuestion.getTeamCode() != NO_TEAM_CODE)
                {
                    moveArray[startingX, j] = 1;
                }
                else
                {
                    break;
                }
            }
            for (int d = startingX; d >= startingY - spacesPossible; d--)
            {
                if (d < 0)
                {
                    break;
                }
                potenPiece = boardState.getPiece(startingX, d);
                if (potenPiece == null || pieceInQuestion.getTeamCode() != potenPiece.getTeamCode() && pieceInQuestion.getTeamCode() != NO_TEAM_CODE)
                {
                    moveArray[startingX, d] = 1;
                }
                else
                {
                    break;
                }
            }
            return moveArray;
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
                        int[,] validPlaces = GetPieceMoves(x1, y1, this.boardState);
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
    }
}
