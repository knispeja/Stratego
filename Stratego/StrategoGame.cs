using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    class StrategoGame
    {

        /// <summary>
        /// The default amount of pieces for each piece. (EX: 0 0s; 1 1; 1 2; 2 3s; 4 4s; etc..)
        /// </summary>
        public static readonly Dictionary<String, int> defaults = new Dictionary<String, int>();

        // public readonly int[] defaults = new int[13] { 0, 1, 1, 2, 3, 4, 4, 4, 5, 8, 1, 6, 1 };
        //public readonly int[] defaults = new int[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        //public readonly int[] defaults = new int[13] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 
        public Dictionary<int, Type> checkboxFactorySim = new Dictionary<int, Type>();

        public Dictionary<int, Type> pieceTypes = new Dictionary<int, Type>();

        /// <summary>
        /// The piece currently being placed by the user
        /// </summary>
        GamePiece piecePlacing = null;

        private GamePiece selectedGamePiece;

        /// <summary>
        /// The 2DArray full of all pieces on the board
        /// </summary>
        public GamePiece[,] boardState { get; set; }

        /// <summary>
        /// The array which holds information on how many pieces of each type can still be placed
        /// </summary>
        public Dictionary<String, int> placements;

        /// <summary>
        /// Whether or not the pre game has begun
        /// </summary>
        public bool preGameActive { get; set; }

        /// <summary>
        /// Current level of the game. Equals -1 if not in campaign mode
        /// </summary>
        public int level { get; set; }

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
        public AI_Old ai;

        /// <summary>
        /// If levels can be skipped using keypresses
        /// </summary>
        private Boolean skippableLevels { get; set; }

        public static readonly int NO_TEAM_CODE = 0;
        public static readonly int RED_TEAM_CODE = -1;
        public static readonly int BLUE_TEAM_CODE = 1;

        public StrategoGame()
        {
            this.turn = NO_TEAM_CODE;
            this.preGameActive = false;
            this.skippableLevels = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);
            this.movableBombs = false;
            this.movableFlags = false;
            this.level = -1;

            this.selectedGamePiece = null;

            boardState = new GamePiece[10, 10];
            for (int row = 0; row < 6; row++) fillRow(null, row);

            //    this.ai = new AI(this, -1);

        }
        public StrategoGame(GamePiece[,] boardState)
        {
            this.boardState = boardState;
          //  this.placements = (int[])this.defaults.Clone();
            this.preGameActive = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);
            this.movableBombs = false;
            this.movableFlags = false;
            //      this.ai = new AI(this, -1);
            this.placements = StrategoWin.defaults;
            this.placements.Add(FlagPiece.FLAG_NAME, 1);
            this.placements.Add(BombPiece.BOMB_NAME, 6);
            this.placements.Add(SpyPiece.SPY_NAME, 1);
            this.placements.Add(ScoutPiece.SCOUT_NAME, 8);
            this.placements.Add(MinerPiece.MINER_NAME, 5);
            this.placements.Add(SergeantPiece.SERGEANT_NAME, 4);
            this.placements.Add(LieutenantPiece.LIEUTENANT_NAME, 4);
            this.placements.Add(CaptainPiece.CAPTAIN_NAME, 4);
            this.placements.Add(MajorPiece.MAJOR_NAME, 3);
            this.placements.Add(ColonelPiece.COLONEL_NAME, 2);
            this.placements.Add(GeneralPiece.GENERAL_NAME, 1);
            this.placements.Add(MarshallPiece.MARSHALL_NAME, 1);

            this.checkboxFactorySim.Add(0, typeof(FlagPiece));
            this.checkboxFactorySim.Add(1, typeof(BombPiece));
            this.checkboxFactorySim.Add(2, typeof(SpyPiece));
            this.checkboxFactorySim.Add(3, typeof(ScoutPiece));
            this.checkboxFactorySim.Add(4, typeof(MinerPiece));
            this.checkboxFactorySim.Add(5, typeof(SergeantPiece));
            this.checkboxFactorySim.Add(6, typeof(LieutenantPiece));
            this.checkboxFactorySim.Add(7, typeof(CaptainPiece));
            this.checkboxFactorySim.Add(8, typeof(MajorPiece));
            this.checkboxFactorySim.Add(9, typeof(ColonelPiece));
            this.checkboxFactorySim.Add(10, typeof(GeneralPiece));
            this.checkboxFactorySim.Add(11, typeof(MarshallPiece));
        }

        /// <summary>
        /// Gets the piece at a given board cell
        /// </summary>
        /// <param name="x">x-coordinate of the cell we want</param>
        /// <param name="y">y-coordinate of the cell we want</param>
        /// <returns>The number of the piece located at (x,y)</returns>
        public GamePiece getPiece(int x, int y)
        {
            return this.boardState[x, y];
        }

        /// <summary>
        /// Fills the given row in the board state with the given value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        public void fillRow(GamePiece value, int row)
        {
            for (int x = 0; x < this.boardState.GetLength(0); x++) this.boardState[x, row] = value;
        }

        /// <summary>
        /// Retrieves the number of pieces still available for
        /// placement of a given type
        /// </summary>
        /// <param name="piece">Type of the piece you want to check</param>
        /// <returns>Number of pieces available for placement</returns>
        public int getPiecesLeft(int piece)
        {
            return this.placements[piece];
        }

        /// <summary>
        /// Places a piece at the given coordinates
        /// </summary>
        /// <param name="piece">Number of the piece you want to place</param>
        /// <param name="x">x-coordinate you want to place it at</param>
        /// <param name="y">y-coordinate you want to place it at</param>
        /// <returns>Whether or not the placement was successful</returns>
        public bool? placePiece(int piece, int x, int y)
        {
            if (turn == 0 || Math.Abs(turn) == 2) return false;
            if (!(pieceTypes.ContainsKey(piece)) || x < 0 || y < 0 || x > this.panelWidth || y > this.panelHeight) throw new ArgumentException();
            if ((Math.Sign(piece) != Math.Sign(this.turn)) && (piece != 0)) return false;
            Boolean retVal = true;
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY = this.panelHeight / this.boardState.GetLength(1);
            GamePiece pieceAtPos = this.boardState[x / scaleX, y / scaleY];
            if ((piece == 0 && pieceAtPos == null))
            {
                retVal = false;
                this.placements[Math.Abs(piece)]++;
            }
            else if (piece == 0 && !pieceAtPos.GetType().Equals(null))
            {
                // We are trying to remove
                if (Math.Sign(pieceAtPos.getTeamCode()) != Math.Sign(this.turn)) return false;
                this.placements[Math.Abs(piece)]++;
            }
            else if (pieceAtPos == null && this.placements[Math.Abs(piece)] > 0)
            {
                // We are trying to add
                this.placements[Math.Abs(piece)] -= 1;
            }
            else retVal = false;

            if (retVal)
            {
                ConstructorInfo ctor = this.pieceTypes[piece].GetConstructor(new[] { this.pieceTypes[piece] });
                this.boardState[x / scaleX, y / scaleY] = (GamePiece) ctor.Invoke(new object[] { 0 });
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
            // We just came here from the main menu
            if (this.turn == 0)
            {
                preGameActive = true;
                this.turn = 1;
            }
            // It's blue player's turn
            else if (this.turn == 1)
            {
                if (this.preGameActive)
                {
                    this.turn = -1;
                    this.placements = this.defaults;
                }
                else
                {
                    this.turn = 2;
                }
            }
            // It's red player's turn
            else if (this.turn == -1)
            {
                if (this.preGameActive)
                {
                    for (int i = 4; i < 6; i++)
                    {
                        for (int x = 0; x < 2; x++)
                            this.boardState[x, i] = null;
                        for (int x = 4; x < 6; x++)
                            this.boardState[x, i] = null;
                        for (int x = 8; x < 10; x++)
                            this.boardState[x, i] = null;
                    }
                    this.preGameActive = false;
                }
                if (!this.isSinglePlayer || (this.lastFought != new Point(-1, -1))) this.turn = -2;
                else this.turn = 1;
            }
            else if (this.turn == -2)
            {
                turn = 1;
            }
            else
            {
                turn = -1;
            }

            if (this.isSinglePlayer && this.turn == this.ai.team)
            {
                if (this.preGameActive)
                    this.ai.placePieces();
                else
                    this.ai.takeTurn();
            }
        }
        /// <summary>
        /// Selects a piece if no piece is selected.
        /// </summary>
        /// <param name="x">x coords of the click in pixels</param>
        /// <param name="y">y coord of the click in pixels</param>
        /// <returns></returns>
        public bool? SelectPiece(int x, int y)
        {
            if ((Math.Abs(turn) == 2) || (turn == -1 && isSinglePlayer)) return false;
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY = this.panelHeight / this.boardState.GetLength(1);
            if ((this.pieceSelectedCoords == new Point(x / scaleX, y / scaleY)) && this.pieceIsSelected)
            {
                this.pieceIsSelected = false;
                return false;
            }
            if ((this.boardState[x / scaleX, y / scaleY].getPieceName().Equals("Bomb") && !this.movableBombs) 
                || (this.boardState[x / scaleX, y / scaleY].getPieceName().Equals("Flag") && !this.movableFlags) ||
                  this.boardState[x / scaleX, y / scaleY].getTeamCode()!= Math.Sign(this.turn))
            {
                return false;
            }
            this.pieceSelectedCoords = new Point(x / scaleX, y / scaleY);
            this.pieceIsSelected = true;
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
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY = this.panelHeight / this.boardState.GetLength(1);
            if (!this.pieceIsSelected)
                return false;
            this.pieceIsSelected = false;
            //if (Piece.attack(this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y],
            //    this.boardState[x / scaleX, y / scaleY]) == null)
            //    return false;
            if (this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y].getPieceName().Equals("Scout"))
            {
                if (Math.Abs((x / scaleX) - this.pieceSelectedCoords.X) > 1 || Math.Abs((y / scaleY) - this.pieceSelectedCoords.Y) > 1)
                    return false;
            }
            else
            {
                //Check for the scout's special cases
                if (Math.Abs((x / scaleX) - this.pieceSelectedCoords.X) > 1)
                {
                    if (((x / scaleX) - this.pieceSelectedCoords.X) > 1)
                    {
                        for (int i = 1; i < (x / scaleX) - this.pieceSelectedCoords.X; i++)
                        {
                            if (this.boardState[this.pieceSelectedCoords.X + i, this.pieceSelectedCoords.Y] !=null)
                                return false;
                        }
                    }
                    else if (((x / scaleX) - this.pieceSelectedCoords.X) < -1)
                    {
                        for (int i = -1; i > (x / scaleX) - this.pieceSelectedCoords.X; i--)
                        {
                            if (this.boardState[this.pieceSelectedCoords.X + i, this.pieceSelectedCoords.Y] != null)
                                return false;
                        }
                    }
                }
                else if (Math.Abs((y / scaleY) - this.pieceSelectedCoords.Y) > 1)
                {
                    if (((y / scaleY) - this.pieceSelectedCoords.Y) > 1)
                    {
                        for (int i = 1; i < (y / scaleY) - this.pieceSelectedCoords.Y; i++)
                        {
                            if (this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y + i] != null)
                                return false;
                        }
                    }
                    else if (((y / scaleY) - this.pieceSelectedCoords.Y) < -1)
                    {
                        for (int i = -1; i > (y / scaleY) - this.pieceSelectedCoords.Y; i--)
                        {
                            if (this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y + i] != null)
                                return false;
                        }
                    }
                }
            }
            if (Math.Abs((x / scaleX) - this.pieceSelectedCoords.X) >= 1 && Math.Abs((y / scaleY) - this.pieceSelectedCoords.Y) >= 1)
                return false;
            if (Math.Abs((x / scaleX) - this.pieceSelectedCoords.X) == 0 && Math.Abs((y / scaleY) - this.pieceSelectedCoords.Y) == 0)
                return false;
           GamePiece defender = this.boardState[x / scaleX, y / scaleY];
    //        this.boardState[x / scaleX, y / scaleY] = Piece.attack(this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y], this.boardState[x / scaleX, y / scaleY]).Value;
            if ((defender == null) || this.boardState[x / scaleX, y / scaleY] == null)
                this.lastFought = new Point(-1, -1);
            else
                this.lastFought = new Point(x / scaleX, y / scaleY);
            this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y] = null;
            if (defender.getPieceName().Equals("Flag"))
            {
                this.nextTurn();
            }
            return true;
        }

        /// <summary>
        /// Finds all of the possible moves for a piece with the given X and Y coordinates using the board state passed in.
        /// </summary>
        /// <param name="X">>X position in the board state (not in pixels)</param>
        /// <param name="Y">Y position in the board state (not in pixels)</param>
        /// <param name="boardState">A 2D array representing the state of the board.</param>
        /// <returns>A 2D array containing 1 in every space where the deisgnated piece can move and 0 otherwise</returns>
        public int[,] GetPieceMoves(int X, int Y, GamePiece[,] boardState)
        {
            int xDirLength = boardState.GetLength(0);
            int yDirLength = boardState.GetLength(1);
            int[,] moveArray = new int[xDirLength, yDirLength];
            for (int i = 0; i < xDirLength; i++)
            {
                for(int j = 0; j < yDirLength; j++)
                {
                    moveArray[i, j] = 0;
                }
            }
            GamePiece selectedPiece = boardState[X, Y];
            return moveArray;
            /*
            if ((Math.Abs(boardState[X, Y]) == 0) || (Math.Abs(boardState[X, Y]) == 11 && !this.movableBombs) || (Math.Abs(boardState[X, Y]) == 12 && !this.movableFlags) || (Math.Abs(boardState[X, Y]) == 42))
                return moveArray;
            if (Math.Abs(boardState[X, Y]) == 9)
            {
                //for (int yD = Y + 1; yD < boardState.GetLength(1) && boardState[X, yD] == 0; yD++)
                //    moveArray[X, yD] = 1;
                //for (int yU = Y - 1; yU >= 0 && boardState[X, yU] == 0; yU--)
                //    moveArray[X, yU] = 1;
                //for (int xR = X + 1; xR < boardState.GetLength(0) && boardState[xR, Y] == 0; xR++)
                //    moveArray[xR, Y] = 1;
                //for (int xL = X - 1; xL >= 0 && boardState[xL, Y] == 0; xL--)
                //    moveArray[xL, Y] = 1;
                for (int yD = Y + 1; yD < boardState.GetLength(1) && ((Math.Sign(boardState[X, yD]) != Math.Sign(boardState[X, Y])) && boardState[X, yD] != 42); yD++)
                {
                    moveArray[X, yD] = 1;
                    if ((Math.Sign(boardState[X, yD]) != Math.Sign(boardState[X, Y])) && (Math.Sign(boardState[X, yD]) != 0))
                        break;
                }
                for (int yU = Y - 1; yU >= 0 && ((Math.Sign(boardState[X, yU]) != Math.Sign(boardState[X, Y])) && boardState[X, yU] != 42); yU--)
                {
                    moveArray[X, yU] = 1;
                    if ((Math.Sign(boardState[X, yU]) != Math.Sign(boardState[X, Y])) && (Math.Sign(boardState[X, yU]) != 0))
                        break;
                }
                for (int xR = X + 1; xR < boardState.GetLength(0) && ((Math.Sign(boardState[xR, Y]) != Math.Sign(boardState[X, Y])) && boardState[xR, Y] != 42); xR++)
                {
                    moveArray[xR, Y] = 1;
                    if ((Math.Sign(boardState[xR, Y]) != Math.Sign(boardState[X, Y])) && (Math.Sign(boardState[xR, Y]) != 0))
                        break;
                }
                for (int xL = X - 1; xL >= 0 && ((Math.Sign(boardState[xL, Y]) != Math.Sign(boardState[X, Y])) && boardState[xL, Y] != 42); xL--)
                {
                    moveArray[xL, Y] = 1;
                    if ((Math.Sign(boardState[xL, Y]) != Math.Sign(boardState[X, Y])) && (Math.Sign(boardState[xL, Y]) != 0))
                        break;
                }
            }
            if (Y > 0)
                if ((Math.Sign(boardState[X, Y - 1]) != Math.Sign(boardState[X, Y])) && boardState[X, Y - 1] != 42)
                    moveArray[X, Y - 1] = 1;
            if (Y < boardState.GetLength(1) - 1)
                if ((Math.Sign(boardState[X, Y + 1]) != Math.Sign(boardState[X, Y])) && boardState[X, Y + 1] != 42)
                    moveArray[X, Y + 1] = 1;
            if (X < boardState.GetLength(0) - 1)
                if ((Math.Sign(boardState[X + 1, Y]) != Math.Sign(boardState[X, Y])) && boardState[X + 1, Y] != 42)
                    moveArray[X + 1, Y] = 1;
            if (X > 0)
                if ((Math.Sign(boardState[X - 1, Y]) != Math.Sign(boardState[X, Y])) && boardState[X - 1, Y] != 42)
                    moveArray[X - 1, Y] = 1;
            */
        }
        public Boolean checkMoves()
        {
            /*
            for (int x1 = 0; x1 < this.boardState.GetLength(0); x1++)
            {
                for (int y1 = 0; y1 < this.boardState.GetLength(1); y1++)
                {
                    GamePiece piece = boardState[x1, y1];
                    if ((piece.getTeamCode() < 0 && (this.turn == -1 || this.turn == 2)) || (piece.getTeamCode() > 0 && (this.turn == 1 || turn == -2)))
                    {
               //         int[,] validPlaces = GetPieceMoves(x1, y1, this.boardState);
                        for (int x2 = 0; x2 < this.boardState.GetLength(0); x2++)
                        {
                            for (int y2 = 0; y2 < this.boardState.GetLength(1); y2++)
                            {
                                //if (validPlaces[x2, y2] == 1)
                                //    return true;
                            }
                        }
                    }
                }
            }
            */

            return false;
        }
    }
}
