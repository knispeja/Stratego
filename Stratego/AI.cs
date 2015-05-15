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

        public int[,] knownBoardState;

        private bool boardValDisabled = false;
        private int boardValue = 0;
        private int recursionLevel = 0;
        private static int MAX_RECURSION_DEPTH = 1;
        private static int MAX_TURNS_FORWARD = 20;

        //private int targetPiece;

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
            this.knownBoardState = win.boardState;

            this.rnd = new Random();
            this.boardX = this.win.boardState.GetLength(0);
            this.boardY = this.win.boardState.GetLength(1);
        }

        public void takeTurn()
        {
            takeTurn(win.boardState);
        }

        /// <summary>
        /// Puts together most of the AI's functions and takes the AI's turn
        /// </summary>
        public void takeTurn(int[,] boardState)
        {
            if (win.preGameActive)
                placePieces();
            else
            {
                List<Move> moves = generateValidMoves(boardState);
                foreach (Move move in moves) evaluateMove(move, boardState);
                executeHighestPriorityMove(moves, boardState);
            }
        }

        /// <summary>
        /// Places this AI player's pieces. Currently places them very stupidly, can be updated later
        /// </summary>
        public void placePieces()
        {
            int x = 0;
            int y = 0;

            // Put the flag in the back, protected by some bombs
            int flagX = rnd.Next(this.boardX-1);
            int flagY;
            if (this.difficulty == 3) flagY = rnd.Next(1);
            else if (this.difficulty >= 4) flagY = 0;
            else flagY = rnd.Next(2);
            placePieceByTile(12 * this.team, flagX, flagY);
            if (this.difficulty >= 2)
            {
                // We only surround the flag in bombs if the difficulty is sufficient
                try { placePieceByTile(11 * this.team, flagX + 1, flagY); }
                catch (ArgumentException) { }
                try { placePieceByTile(11 * this.team, flagX - 1, flagY); }
                catch (ArgumentException) { }
                try { placePieceByTile(11 * this.team, flagX, flagY + 1); }
                catch (ArgumentException) { }
                try { placePieceByTile(11 * this.team, flagX, flagY - 1); }
                catch (ArgumentException) { }
            }

            // Place fake bomb clusters around 7s (to kill the 8
            // that will inevitably defuse the bombs), advanced technique
            if (this.difficulty >= 4)
            {
                flagX = rnd.Next(this.boardX - 1);
                flagY = rnd.Next(2);
                placePieceByTile(7 * this.team, flagX, flagY);
                try { placePieceByTile(11 * this.team, flagX + 1, flagY); }
                catch (ArgumentException) { }
                try { placePieceByTile(11 * this.team, flagX - 1, flagY); }
                catch (ArgumentException) { }
                try { placePieceByTile(11 * this.team, flagX, flagY + 1); }
                catch (ArgumentException) { }
                try { placePieceByTile(11 * this.team, flagX, flagY - 1); }
                catch (ArgumentException) { }
            }

            // Place some scout clusters near the front
            for (int i = 0; i < win.defaults[9]; i++ )
            {
                x = rnd.Next(this.boardX-1);
                y = rnd.Next(1)+2;
                placePieceByTile(9 * this.team, x, y);
            }

            // Scatter miners, but don't put them at the front
            for (int i = 0; i < win.defaults[8]; i++)
            {
                x = rnd.Next(this.boardX-1);
                y = rnd.Next(2);
                placePieceByTile(8 * this.team, x, y);
            }

            // Place any remaining pieces
            for (int piece = win.defaults.Length-1; piece > 0; piece--)
            {
                // Try to scatter them at first
                for (int i = 0; i < win.getPiecesLeft(piece); i++ )
                {
                    placePieceByTile(piece, rnd.Next(this.boardX - 1), rnd.Next(this.boardY - 1));
                }

                // Place them in order when that inevitably fails
                x = 0;
                y = 0;
                while (win.getPiecesLeft(piece) != 0)
                {
                    placePieceByTile(piece * this.team, x, y);
                    x++;
                    if (x >= this.boardX)
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

        public void executeMove(Move move)
        {
            executeMove(move, win.boardState);
        }

        /// <summary>
        /// Converts x and y tiles to x and y coordinates and executes the move
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void executeMove(Move move, int[,] boardState)
        {
            int piece = boardState[move.origX, move.origY];
            boardState[move.origX, move.origY] = 0;
            int? attackVal = Piece.attack(piece, boardState[move.newX, move.newY]);
            if(attackVal == null)
                throw new Exception();
            boardState[move.newX, move.newY] = (int) attackVal;
            
            if(this.recursionLevel == 0)
                win.nextTurn();
        }

        public List<Move> generateValidMoves()
        {
            return generateValidMoves(win.boardState);
        }

        /// <summary>
        /// Generate a list of all valid moves
        /// </summary>
        /// <returns>A list of moves</returns>
        public List<Move> generateValidMoves(int[,] boardState)
        {
            List<Move> moves = new List<Move>();

            for(int x1 = 0; x1 < this.boardX; x1++)
            {
                for(int y1 = 0; y1 < this.boardY; y1++)
                {
                    int piece = boardState[x1, y1];
                    if (isFriendlyPiece(piece))
                    {
                        int[,] validPlaces = this.win.GetPieceMoves(x1, y1, boardState);
                        for (int x2 = 0; x2 < this.boardX; x2++)
                        {
                            for (int y2 = 0; y2 < this.boardY; y2++)
                            {
                                if (validPlaces[x2, y2] == 1)
                                    moves.Add(new Move(x1, y1, x2, y2));
                            }
                        }
                    }
                }
            }

            return moves;
        }

        /// <summary>
        /// Evaluates the priority of the move on long-term predictions
        /// </summary>
        /// <param name="move"></param>
        /// <param name="depth"></param>
        public void evaluateMoveRecursive(Move move)
        {
            if (this.recursionLevel >= MAX_RECURSION_DEPTH)
                return;

            this.recursionLevel++;

            // Create a fake board state to use for simulating turns
            int[,] boardState = (int[,]) win.boardState.Clone();
            int backupTeam = this.team;
            int backupTeam2;
            int backupValue = this.boardValue;

            // Evaluate our current board "value"
            int enemyPieces=0;
            int friendlyPieces=0;
            for(int x=0; x<this.boardX; x++)
            {
                for(int y=0; y<this.boardY; y++)
                {
                    int piece = boardState[x,y];
                    if(isFriendlyPiece(piece))
                        friendlyPieces++;
                    else if(isEnemyPiece(piece))
                        enemyPieces++;
                }
            }

            // Execute the move
            executeMove(move, boardState);

            // Simulate several turns into the future
            int i = 0;
            while(i < MAX_TURNS_FORWARD)
            {
                this.team *= -1;
                if (this.team != backupTeam) boardValDisabled = true;
                else boardValDisabled = false;
                backupTeam2 = this.team;
                takeTurn(boardState);
                i++;

                this.team = backupTeam;

                // Evaluate whether or not this move resulted in good things
                int enemyPiecesNew = 0;
                int friendlyPiecesNew = 0;
                for (int x = 0; x < this.boardX; x++)
                {
                    for (int y = 0; y < this.boardY; y++)
                    {
                        int piece = boardState[x, y];
                        if (isFriendlyPiece(piece))
                            friendlyPiecesNew++;
                        else if (isEnemyPiece(piece))
                            enemyPiecesNew++;
                    }
                }
                if (enemyPiecesNew < enemyPieces)
                    move.priority += (enemyPieces - enemyPiecesNew)/(i+1);
                if (friendlyPiecesNew < friendlyPieces)
                    move.priority -= (friendlyPieces - friendlyPiecesNew)/(i+1);

                this.team = backupTeam2;
            }

            move.priority += (this.boardValue - backupValue) / (MAX_TURNS_FORWARD - 1);
            
            // Reset the game to its previous state
            this.team = backupTeam;
            this.boardValue = backupValue;
            this.recursionLevel = 0;
        }

        public void evaluateMove(Move move)
        {
            evaluateMove(move, win.boardState);
        }

        /// <summary>
        /// Updates the priority of the move based on short-term goals
        /// </summary>
        /// <param name="move">The move to be evaluated</param>
        /// <returns>Whether or not the move is valid</returns>
        public void evaluateMove(Move move, int[,] boardState)
        {
            int defender = boardState[move.newX, move.newY];
            int attacker = boardState[move.origX, move.origY];
            int? attackVal = Piece.attack(attacker, defender);

            // ---------- Update this move's priority -----------

            if ((!(move.newY < move.origY) && this.team == -1) || (!(move.newY > move.origY) && this.team == 1))
            {
                // Discourages movement away from the enemy
                move.priority+=2;
            }

            if (difficulty == 0)
            {
                // Just choose moves randomly, don't change priority
                return;
            }

            if (difficulty >= 2)
            {
                // When a piece is currently in danger, moving it away is a good plan
                // Basically, pieces next to enemy pieces take priority frequently
                int nPiece = 0;
                int ePiece = 0;
                int sPiece = 0;
                int wPiece = 0;
                if (move.origY != 0)
                    nPiece = boardState[move.origX, move.origY - 1];
                if (move.origX != this.boardX - 1)
                    ePiece = boardState[move.origX + 1, move.origY];
                if (move.origY != this.boardY - 1)
                    sPiece = boardState[move.origX, move.origY + 1];
                if (move.origX != 0)
                    wPiece = boardState[move.origX - 1, move.origY];
                if (isEnemyPiece(nPiece)) move.priority += (getPieceValue(attacker) / 3);
                if (isEnemyPiece(nPiece)) move.priority += (getPieceValue(attacker) / 3);
                if (isEnemyPiece(nPiece)) move.priority += (getPieceValue(attacker) / 3);
                if (isEnemyPiece(nPiece)) move.priority += (getPieceValue(attacker) / 3);

                // Try not to move right next to unknown enemy pieces
                nPiece = 0;
                ePiece = 0;
                sPiece = 0;
                wPiece = 0;
                if(move.newY != 0)
                    nPiece = boardState[move.newX, move.newY - 1];
                if(move.newX != this.boardX - 1)
                    ePiece = boardState[move.newX + 1, move.newY];
                if(move.newY != this.boardY - 1)
                    sPiece = boardState[move.newX, move.newY + 1];
                if(move.newX != 0)
                    wPiece = boardState[move.newX - 1, move.newY];

                // If pieces nearby are friendly, up the priority
                if (isFriendlyPiece(nPiece))
                    move.priority++;
                if (isFriendlyPiece(ePiece))
                    move.priority++;
                if (isFriendlyPiece(sPiece))
                    move.priority++;
                if (isFriendlyPiece(wPiece))
                    move.priority++;

                // Reduce priority for each of these that is an unknown (unknown part is unimplemented) enemy
                if (this.difficulty != 5)
                {
                    // Check for unknowns

                }
                else
                {
                    // Difficulty 5, time to look at the opponent's pieces (>")>

                    if (Math.Abs(defender) == 12)
                    {
                        // If the defender is the enemy flag, raise priority an insane amount
                        move.priority += 1000;
                        return;
                    }

                    if (attackVal == attacker)
                    {
                        if (isEnemyPiece(defender))
                        {
                            // If the AI is going to come out on top, raise priority
                            // by the perceived value of the piece to be executed
                            move.priority += getPieceValue(defender)*4;
                        }
                        else
                        {
                            // This is just a normal, empty space we're trying to move into. 
                            // Probably don't do anything here?
                        }

                    }
                    else
                    {
                        // The AI is going to die, and this move is a terrible decision 
                        // unless this is an even trade, in which case everything is mostly okay
                        if (attackVal != 0) move.priority -= 500;
                        else move.priority+=getPieceValue(attacker)/3;

                        return;
                    }

                    // Now we can see whether or not the opponent will win if they
                    // try to attack us after we move, so we take that into account...

                    /*
                    int? nResult = Piece.attack(nPiece, attacker);
                    int? eResult = Piece.attack(ePiece, attacker);
                    int? sResult = Piece.attack(sPiece, attacker);
                    int? wResult = Piece.attack(wPiece, attacker);
                    if (Math.Abs(nPiece) == 11) nResult = attacker;
                    if (Math.Abs(ePiece) == 11) eResult = attacker;
                    if (Math.Abs(sPiece) == 11) sResult = attacker;
                    if (Math.Abs(wPiece) == 11) wResult = attacker;

                    if (nResult != nPiece && eResult != ePiece && sResult != sPiece && wResult != wPiece)
                    {
                        // If this piece will be safe despite nearby enemy pieces, up the priority more
                        if (isEnemyPiece(nPiece) && (Math.Abs(nPiece) != 11 || Math.Abs(attacker) == 8))
                            move.priority += getPieceValue(nPiece)/3;
                        if (isEnemyPiece(ePiece) && (Math.Abs(ePiece) != 11 || Math.Abs(attacker) == 8))
                            move.priority += getPieceValue(ePiece)/3;
                        if (isEnemyPiece(sPiece) && (Math.Abs(sPiece) != 11 || Math.Abs(attacker) == 8))
                            move.priority += getPieceValue(sPiece)/3;
                        if (isEnemyPiece(wPiece) && (Math.Abs(wPiece) != 11 || Math.Abs(attacker) == 8))
                            move.priority += getPieceValue(wPiece)/3;
                    }
                    else
                    {
                        bool protectorPresent = false;
                        int dangerousPiece = 0;

                        // Check if this piece is protected by a friendly one
                        if (nResult == nPiece && Math.Abs(nPiece) != 11 && getPieceValue(nPiece) > getPieceValue(dangerousPiece)) dangerousPiece = nPiece;
                        if (eResult == ePiece && Math.Abs(ePiece) != 11 && getPieceValue(ePiece) > getPieceValue(dangerousPiece)) dangerousPiece = ePiece;
                        if (sResult == sPiece && Math.Abs(sPiece) != 11 && getPieceValue(sPiece) > getPieceValue(dangerousPiece)) dangerousPiece = sPiece;
                        if (wResult == wPiece && Math.Abs(wPiece) != 11 && getPieceValue(wPiece) > getPieceValue(dangerousPiece)) dangerousPiece = wPiece;

                        if (isFriendlyPiece(nPiece))
                            if (Piece.attack(nPiece, dangerousPiece) == nPiece && Math.Abs(nPiece) != 11)
                            {
                                protectorPresent = true;
                                move.priority += getPieceValue(dangerousPiece) / 4;
                            }

                        if (isFriendlyPiece(ePiece))
                            if (Piece.attack(ePiece, dangerousPiece) == ePiece && Math.Abs(ePiece) != 11)
                            {
                                protectorPresent = true;
                                move.priority += getPieceValue(dangerousPiece) / 4;
                            }

                        if (isFriendlyPiece(sPiece))
                            if (Piece.attack(sPiece, dangerousPiece) != sPiece && Math.Abs(sPiece) != 11)
                            {
                                protectorPresent = true;
                                move.priority += getPieceValue(dangerousPiece) / 4;
                            }

                        if (isFriendlyPiece(wPiece))
                            if (Piece.attack(wPiece, dangerousPiece) != wPiece && Math.Abs(wPiece) != 11)
                            {
                                protectorPresent = true;
                                move.priority += getPieceValue(dangerousPiece) / 4;
                            }

                        // This piece will be in danger, so lower the priority
                        // by the value of the potentially lost piece
                        if(!protectorPresent)
                            move.priority -= getPieceValue(attacker)*4;
                    } */
                }
            }

            if (difficulty >= 3 && move.priority >= 1)
            {
                evaluateMoveRecursive(move);
            }
            else if (move.priority < 1)
            {
                // Try to prioritize moves that were positive in the first place
                move.priority -= 100;
            }

            return;
        }

        /// <summary>
        /// Returns the number of adjacent enemies the piece at (x, y) is safe from via protection or rank.
        /// Return value of 0 means no enemies are present, return value of -1 means this piece is in danger!
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="piece"></param>
        /// <param name="boardState"></param>
        /// <returns></returns>
        public int safetyCheck(int x, int y, int piece, int[,] boardState)
        {
            int enemies = 0;
            for(int i=0; i<4; i++)
            {
                int newx = x, newy = y;
                int? result = null;
                if (i == 0 && x != 0)
                    newx = x - 1;
                else if (i == 1 && y != this.boardY - 1)
                    newy = y + 1;
                else if (i == 2 && x != this.boardX - 1)
                    newx = x + 1;
                else if (i == 3 && y != 0)
                    newy = y - 1;

                int dPiece = 0;
                if (newx != x || newy != y)
                {
                    dPiece = boardState[newx, newy];
                    result = Piece.attack(dPiece, piece);
                }

                if (result != null && result != piece)
                    return -1;
                else if (isEnemyPiece(dPiece)) enemies++;
            }
            return enemies;
        }

        /// <summary>
        /// Does a check to see if the given piece is an enemy
        /// </summary>
        /// <param name="piece">A piece to check</param>
        /// <returns>True if the piece is an enemy, false otherwise</returns>
        public bool isEnemyPiece(int piece)
        {
            if (piece == 42 || piece == 0) return false;

            return Math.Sign(piece) != this.team;
        }

        /// <summary>
        /// Does a check to see if the given piece is friendly
        /// </summary>
        /// <param name="piece">A piece to check</param>
        /// <returns>True if the piece is friendly, false otherwise</returns>
        public bool isFriendlyPiece(int piece)
        {
            if (piece == 42 || piece == 0) return false;

            return Math.Sign(piece) == this.team;
        }

        /// <summary>
        /// Returns an arbitrary value representing the perceived value of the given piece
        /// </summary>
        /// <param name="piece">Piece to get the value of</param>
        /// <returns>Value of the piece</returns>
        public int getPieceValue(int piece)
        {
            switch(Math.Abs(piece))
            {
                case(1):
                    return 17;
                case(2):
                    return 15;
                case(3):
                    return 13;
                case(4):
                    return 11;
                case(5):
                    return 10;
                case(6):
                    return 9;
                case(7):
                    return 8;
                case(8):
                    return 13;
                case(9):
                    return 6;
                case(10):
                    return 14;
                case(11):
                    return 12;
                case(12):
                    return 1000;
                default:
                    return 0;
            }
        }

        public void executeHighestPriorityMove(List<Move> moves)
        {
            executeHighestPriorityMove(moves, win.boardState);
        }

        /// <summary>
        /// Executes the move with the highest priority out of a given list of moves.
        /// If several moves have the same priority, it chooses randomly.
        /// </summary>
        /// <param name="moves">The list of moves to choose from.</param>
        public void executeHighestPriorityMove(List<Move> moves, int[,] boardState)
        {
            if (moves.Count == 0)
                return;

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
            if(!this.boardValDisabled) this.boardValue += finalMoves[r].priority;
            executeMove(finalMoves[r], boardState);
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
