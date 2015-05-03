using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Holds methods and classes relating to the AI player.
    /// </summary>
    public class AI
    {
        private StrategoWin win;
        public int team { get; set; }
        public int difficulty { get; set; }
        private Random rnd;
        private int boardX;
        private int boardY;
        private int targetPiece;

        /// <summary>
        /// Initializes this AI player
        /// </summary>
        /// <param name="win">The window upon which to perform most game functions directly</param>
        /// <param name="team">This AI player's team, either -1 or 1 (typically that translates to red or blue)</param>
        /// <param name="difficulty">This is the AI player's difficulty. A higher number indicates the AI is more difficult, 5 being insanity</param>
        public AI(StrategoWin win, int team, int difficulty = 5)
        {
            if (Math.Abs(team) != 1) throw new ArgumentException();
            this.team = team;
            this.difficulty = difficulty;
            this.win = win;

            this.rnd = new Random();
            this.boardX = this.win.boardState.GetLength(0);
            this.boardY = this.win.boardState.GetLength(1);
        }

        /// <summary>
        /// Puts together most of the AI's functions and takes the AI's turn
        /// </summary>
        public void takeTurn()
        {
            if (win.preGameActive)
                placePieces();
            else
            {
                List<Move> moves = generateValidMoves();
                executeHighestPriorityMove(moves);
            }
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
                    if(x >= this.boardX)
                    {
                        x = 0;
                        y++;

                        if (y >= this.boardY)
                            // We've run out of places to place our pieces!
                            throw new InvalidOperationException();
                    }
                }
            }

            win.nextTurn();
        }

        /// <summary>
        /// Converts x and y tiles to x and y coordinates and places the piece
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool? placePieceByTile(int piece, int xTile, int yTile)
        {
            // Convert tiles to coordinates
            int scaleX = win.panelWidth / this.boardX;
            int scaleY = win.panelHeight / this.boardY;
            int x = xTile*scaleX;
            int y = yTile*scaleY;

            // Actually place the piece
            return win.placePiece(piece, x, y);
        }

        /// <summary>
        /// Converts x and y tiles to x and y coordinates and executes the move
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void movePiece(Move move)
        {
            // Convert tiles to coordinates
            int scaleX = win.panelWidth / this.boardX;
            int scaleY = win.panelHeight / this.boardY;
            int ox = move.origX * scaleX;
            int oy = move.origY * scaleY;
            int nx = move.newX * scaleX;
            int ny = move.newY * scaleY;

            // Select and move the piece
            win.SelectPiece(ox, oy);
            win.MovePiece(nx, ny);
        }

        /// <summary>
        /// Generate a list of all valid moves (evaluateMove() may throw these out!!)
        /// </summary>
        /// <returns>A list of moves</returns>
        public List<Move> generateValidMoves()
        {
            List<Move> moves = new List<Move>();
            for (int x = 0; x < this.boardX; x++)
            {
                for (int y = 0; y < this.boardY; y++)
                {
                    int piece = this.win.getPiece(x, y);
                    int? attackVal = Piece.attack(this.win.getPiece(x,y), -1*this.team);
                    if (piece != 0 && Math.Abs(piece) != 42 && Math.Abs(piece) != 11 && Math.Abs(piece) != 12 && attackVal != null)
                    {
                        // Generate 4 moves per friendly piece, one for each direction
                        // (as such, scouts currently won't be moved further than 1 space)
                        if (x != 0)
                            moves.Add(new Move(x, y, x - 1, y));
                        if (x != this.boardX-1)
                            moves.Add(new Move(x, y, x + 1, y));
                        if (y != 0)
                            moves.Add(new Move(x, y, x, y - 1));
                        if (y != this.boardY-1)
                            moves.Add(new Move(x, y, x, y + 1));
                    }
                }
            }

            List<Move> finalMoves = new List<Move>();
            foreach (Move move in moves) if (evaluateMove(move)) finalMoves.Add(move);

            return finalMoves;
        }

        /// <summary>
        /// Evaluates whether or not a move is valid. If it is, it updates that move's priority.
        /// </summary>
        /// <param name="move">The move to be evaluated</param>
        /// <returns>Whether or not the move is valid</returns>
        public bool evaluateMove(Move move)
        {
            int defender = win.getPiece(move.newX, move.newY);
            int attacker = win.getPiece(move.origX, move.origY);
            int? attackVal = Piece.attack(attacker, defender);
            if (attackVal == null) return false;

            // ---------- Update this move's priority -----------

            if (!(move.newY < move.origY))
                // Prioritize downward movement, side-to-side is fine
                move.priority++;

            if (difficulty == 0)
                // Just choose moves randomly, don't change priority
                return true;

            if (difficulty >= 2)
            {
                // Try not to move right next to unknown enemy pieces
                int nPiece = 0;
                int ePiece = 0;
                int sPiece = 0;
                int wPiece = 0;
                if(move.newY != 0)
                    nPiece = this.win.getPiece(move.newX, move.newY - 1);
                if(move.newX != this.boardX - 1)
                    ePiece = this.win.getPiece(move.newX + 1, move.newY);
                if(move.newY != this.boardY - 1)
                    sPiece = this.win.getPiece(move.newX, move.newY + 1);
                if(move.newX != 0)
                    wPiece = this.win.getPiece(move.newX - 1, move.newY);

                // If pieces nearby are friendly, up the priority
                // When a piece is currently in danger, move it away

                // Reduce priority for each of these that is an unknown (unknown part is unimplemented) enemy
                if (this.difficulty != 5)
                {
                    // Check for unknowns
                }
                else
                {
                    // Difficulty 5, time to look at the opponent's pieces (>")>

                    if (defender == 12)
                    {
                        // If the defender is the enemy flag, raise priority an insane amount
                        move.priority += 1000;
                        return true;
                    }

                    if (attackVal == attacker && isEnemyPiece(defender))
                    {
                        // If the AI is going to come out on top, raise priority
                        // by the perceived value of the piece to be executed
                        move.priority += getPieceValue(defender);
                    }
                    else
                    {
                        // The AI is going to die, and this move is a terrible decision!
                        move.priority -= 50;
                        return true;
                    }

                    // Now we can see whether or not the opponent will win if they
                    // try to attack us after we move, so we take that into account...
                    int? nResult = Piece.attack(nPiece, attacker);
                    int? eResult = Piece.attack(ePiece, attacker);
                    int? sResult = Piece.attack(sPiece, attacker);
                    int? wResult = Piece.attack(wPiece, attacker);

                    if (nResult != nPiece && eResult != ePiece && sResult != sPiece && wResult != wPiece)
                    {
                        // If this piece will be safe, raise the priority a little bit to encourage safe movement
                        move.priority += 1;
                        if (isEnemyPiece(nPiece))
                            move.priority += 3;
                        if (isEnemyPiece(ePiece))
                            move.priority += 3;
                        if (isEnemyPiece(sPiece))
                            move.priority += 3;
                        if (isEnemyPiece(wPiece))
                            move.priority += 3;
                    }
                    else
                        // This piece will be in danger, so lower the priority
                        // by the value of the potentially lost piece
                        move.priority -= getPieceValue(attacker);
                }
            }

            return true;
        }

        /// <summary>
        /// Does a check to see if the given piece is an enemy
        /// </summary>
        /// <param name="piece">A piece to check</param>
        /// <returns>True if the piece is an enemy, false if it is not</returns>
        public bool isEnemyPiece(int piece)
        {
            if (piece == 42 || piece == 0) return false;
            return Math.Sign(piece) != this.team;
        }

        /// <summary>
        /// Returns an arbitrary value representing the perceived value of the given piece
        /// </summary>
        /// <param name="piece">Piece to get the value of</param>
        /// <returns>Value of the piece</returns>
        public int getPieceValue(int piece)
        {
            switch(piece)
            {
                case(1):
                    return 16;
                case(2):
                    return 14;
                case(3):
                    return 12;
                case(4):
                    return 10;
                case(5):
                    return 9;
                case(6):
                    return 8;
                case(7):
                    return 7;
                case(8):
                    return 11;
                case(9):
                    return 5;
                case(10):
                    return 13;
                case(11):
                    return 2;
                case(12):
                    return 1000;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Executes the move with the highest priority out of a given list of moves.
        /// If several moves have the same priority, it chooses randomly.
        /// </summary>
        /// <param name="moves">The list of moves to choose from.</param>
        public void executeHighestPriorityMove(List<Move> moves)
        {
            List<Move> intermediateMoves = new List<Move>();
            List<Move> finalMoves = new List<Move>();

            // Initialize max
            int max = moves[0].priority;

            // Find the true max while removing some obviously insignificant moves
            foreach (Move move in moves)
            {
                if (move.priority >= max)
                {
                    intermediateMoves.Add(move);
                    if(move.priority > max)
                        max = move.priority;
                }
            }

            // The max may have changed, so remove any straggling small-priority moves
            foreach (Move move in intermediateMoves)
            {
                if (move.priority >= max)
                    finalMoves.Add(move);
            }

            int r = rnd.Next(finalMoves.Count);
            movePiece(finalMoves[r]);
        }

        /// <summary>
        /// Represents a move to be made by the AI.
        /// Useful for evaluating which move should be taken from a list.
        /// </summary>
        public class Move
        {
            public int origX;
            public int origY;
            public int newX;
            public int newY;

            public int priority;

            public Move(int origX, int origY, int newX, int newY)
            {
                this.origX = origX;
                this.origY = origY;
                this.newX = newX;
                this.newY = newY;

                this.priority = 0;
            }
        }
    }
}
