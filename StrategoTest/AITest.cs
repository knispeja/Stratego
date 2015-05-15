using System;
using Stratego;
using NUnit.Framework;

namespace StrategoTest
{
    [TestFixture()]
    public class AITest
    {
        [TestCase(1, 2)]
        [TestCase(-1, 3)]
        [TestCase(1, 0)]
        [TestCase(-1, 5)]
        // Tests AI's constructor, which sets the AI's team & difficulty
        public void TestConstructor(int team, int difficulty)
        {
            int[,] gameBoard = new int[10, 10];
            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);
            AI ai = new AI(win, team, difficulty);
            Assert.AreEqual(team, ai.team);
            Assert.AreEqual(difficulty, ai.difficulty);
            Assert.AreEqual(gameBoard, ai.knownBoardState);
        }

        [TestCase(0)]
        [TestCase(3)]
        [TestCase(-4)]
        [TestCase(99)]
        [TestCase(-11)]
        [TestCase(32)]
        [TestCase(4)]
        [TestCase(-99)]
        // Tests AI's constructor for exceptions by setting the AI's team incorrectly
        public void TestConstructorThrowsException(int invalidTeam)
        {
            int[,] gameBoard = new int[10, 10];
            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);
            Assert.Throws<ArgumentException>(() => new AI(win, invalidTeam));
        }

        [TestCase(1)]
        [TestCase(-1)]
        // Tests that invalid calls to AI.placePiece() throw exceptions
        public void TestPlacePiecesThrowsInvalidOperationException(int team)
        {
            int[,] gameBoard = new int[10, 10];
            for (int row = 0; row < 10; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    // Make every cell of the gameBoard invalid for placement
                    gameBoard[column, row] = 42;
                }
            }
            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);
            win.nextTurn();
            if(team < 0) win.nextTurn();
            AI ai = new AI(win, team);

            Assert.Throws<InvalidOperationException>(() => ai.placePieces());
        }

        [TestCase(1)]
        [TestCase(-1)]
        // Tests that AI.placePiece() places as many pieces as possible
        // Makes sure AI.placePiece() places its own pieces...
        // Verifies that AI.placePiece() cycles the turn once it's finished
        public void TestPlacePieces(int team)
        {
            int[,] gameBoard = new int[10, 10];
            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);
            win.nextTurn();
            if (team < 0) win.nextTurn();

            int initialTurn = win.turn;

            AI ai = new AI(win, team);

            ai.placePieces();

            // If the team is 1, the placements array gets reset after nextTurn() is called.
            // Therefore, we should only test this if the team is -1 to begin with
            if (team != 1)
            {
                for (int i = 0; i < win.defaults.Length; i++)
                    Assert.AreEqual(0, win.getPiecesLeft(i));
            }

            for (int x = 0; x < win.boardState.GetLength(0); x++)
            {
                for (int y = 0; y < win.boardState.GetLength(1); y++)
                {
                    if (team < 0) Assert.True(win.getPiece(x, y) <= 0);
                    else Assert.True(win.getPiece(x, y) >= 0);
                }
            }

            Assert.AreNotEqual(initialTurn, win.turn);
        }

        /*
        [TestCase(true, 1, 3, 0)]
        [TestCase(true, -1, -3, 0)]
        [TestCase(false, 1, 2, 5)]
        [TestCase(false, -1, -5, -3)]
        [TestCase(false, 1, 5, 42)]
        [TestCase(false, -1, -5, 42)]
        // Tests that evaluate move properly finds valid and invalid moves to some extent
        public void TestEvaluateMoveFindsInvalidMoves(bool expected, int team, int attacker, int defender)
        {
            int[,] gameBoard = new int[10, 10];
            for (int row = 0; row < 10; row++)
            {
                int column;
                for (column = 6; column < 10; column++)
                {
                    // Stick some 42s to the right of the attackers
                    gameBoard[column, row] = 42;
                }

                // Place the attackers in column 5
                column = 5;
                gameBoard[column, row] = attacker;

                for (column = 0; column < 5; column++)
                {
                    // Put a row of defenders to the left of the attackers
                    gameBoard[column, row] = defender;
                }
            }
            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);

            AI ai = new AI(win, team);

            for (int row = 0; row < 10; row++)
                Assert.AreEqual(expected, ai.evaluateMove(new AI.Move(5, row, 4, row)));
        }*/
        

        [TestCase(1)]
        [TestCase(-1)]
        // Verifies that generateValidMoves does not return a null or empty list
        // Verifies that generateValidMoves actually generates valid moves
            // NOTE: Any other functionality (WHICH moves are gen'd) 
            // is arguably subjective and won't be tested
        public void TestGenerateValidMoves(int team)
        {
            int[,] gameBoard = new int[10, 10];
            for (int row = 0; row < 10; row++)
            {
                int column;
                for (column = 6; column < 10; column++)
                {
                    // Stick some 42s to the right of the attackers
                    gameBoard[column, row] = 42;
                }

                // Place the attackers in column 5
                column = 5;
                gameBoard[column, row] = 5*team;

                for (column = 0; column < 5; column++)
                {
                    // Put a row of defenders to the left of the attackers
                    gameBoard[column, row] = 0;
                }
            }
            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);

            AI ai = new AI(win, team);
            System.Collections.Generic.List<AI.Move> moves = ai.generateValidMoves();
            Assert.AreNotEqual(null, moves);
            Assert.IsNotEmpty(moves);
            
            foreach (AI.Move move in moves)
            {
                int first = win.getPiece(move.origX, move.origY);
                int second = win.getPiece(move.newX, move.newY);
                Assert.AreNotEqual(0, first);
                Assert.AreNotEqual(11, first);
                Assert.AreNotEqual(12, first);
                Assert.AreNotEqual(42, first);
                int? returned = Piece.attack(first, second);
                Assert.AreEqual(5*team, returned);
            }
        }

        [TestCase(1)]
        [TestCase(-1)]
        // Tests that AI.executeHighestPriorityMove() works as expected
        public void TestExecuteHighestPriorityMoveWorks(int team)
        {
            int[,] gameBoard = new int[10, 10];
            gameBoard[0, 0] = 5*team;

            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);

            win.nextTurn();
            win.nextTurn();
            win.nextTurn();
            if (team < 0) win.nextTurn();
            int initialTurn = win.turn;

            AI ai = new AI(win, team);
            System.Collections.Generic.List<AI.Move> moves = ai.generateValidMoves();
            moves[0] = new AI.Move(0, 0, 1, 0);
            moves[0].priority = 100000;
            ai.executeHighestPriorityMove(moves);
            Assert.AreEqual(0, win.boardState[0, 0]);
            Assert.AreEqual(5 * team, win.boardState[1, 0]);
            Assert.AreNotEqual(initialTurn, win.turn);
        }

        [TestCase(1, -1, true)]
        [TestCase(-1, 1, true)]
        [TestCase(-1, 5, true)]
        [TestCase(1, -5, true)]
        [TestCase(-1, -1, false)]
        [TestCase(1, 1, false)]
        [TestCase(1, 5, false)]
        [TestCase(-1, -5, false)]
        [TestCase(-1, 42, false)]
        [TestCase(1, 42, false)]
        [TestCase(-1, 0, false)]
        [TestCase(1, 0, false)]
        // Tests that AI.isEnemyPiece() works as expected
        public void TestIsEnemyPiece(int team, int piece, bool expected)
        {
            int[,] gameBoard = new int[10, 10];

            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);

            AI ai = new AI(win, team);
            Assert.AreEqual(expected, ai.isEnemyPiece(piece));
        }

        [TestCase(1, 1, true)]
        [TestCase(-1, -1, true)]
        [TestCase(-1, -5, true)]
        [TestCase(1, 5, true)]
        [TestCase(-1, 1, false)]
        [TestCase(1, -1, false)]
        [TestCase(1, -5, false)]
        [TestCase(-1, 5, false)]
        [TestCase(-1, 42, false)]
        [TestCase(1, 42, false)]
        [TestCase(-1, 0, false)]
        [TestCase(1, 0, false)]
        // Tests that AI.isFriendlyPiece() works as expected
        public void TestIsFriendlyPiece(int team, int piece, bool expected)
        {
            int[,] gameBoard = new int[10, 10];

            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);

            AI ai = new AI(win, team);
            Assert.AreEqual(expected, ai.isFriendlyPiece(piece));
        }

        // Tests that safetyCheck() notices when the piece is in danger
        [TestCase(-1, 5, 5, 0, 0, 0, 1, -9, -1)]
        [TestCase(-1, 5, 5, 0, 0, 0, 5, -7, -1)]
        [TestCase(1, 8, 4, 0, 0, -4, 9, 5, -1)]
        [TestCase(-1, 1, 4, 0, 0, 2, 8, -5, -1)]
        [TestCase(1, 8, 3, 0, 0, -10, -5, 1, -1)]
        [TestCase(1, 2, 5, 0, -4, 6, 0, 6, -1)]
        [TestCase(-1, 2, 5, 0, 1, 6, 0, -4, -1)]

        [TestCase(-1, 5, 3, 3, -10, 10, -7, -5, -1)]
        [TestCase(1, 2, 4, -4, -9, -8, 0, 6, -1)]
        [TestCase(-1, 7, 6, 6, 0, 0, 0, -8, -1)]
        // Tests that safetyCheck() returns 0 when there are no enemy pieces around
        [TestCase(-1, 4, 7, 0, 0, 0, 0, -4, 0)]
        [TestCase(1, 2, 1, 0, 0, 0, 1, 9, 0)]
        // Tests that safetyCheck() returns n when there are n harmless enemies around

        // Tests that safetyCheck() recognizes an enemy is harmless when a friendly is acting as a protector
        // In other words, makes sure -1 is not returned if after the enemy kills this piece, a different friendly piece can take the enemy

        // Tests that safetyCheck() doesn't break on the edges of the map
        [TestCase(1, 0, 8, 0, 0, 0, -10, 1, 0)]
        [TestCase(-1, 0, 9, 0, 0, 0, 0, -4, 0)]
        [TestCase(-1, 9, 5, 0, 0, 5, 1, 4, 0)]
        [TestCase(1, 9, 0, 1, 4, 5, 1, 4, 0)]
        [TestCase(-1, 0, 0, -5, 0, -8, 9, 1, 0)]
        public void TestSafetyCheck(int team, int x, int y, int nPiece, int ePiece, int sPiece, int wPiece, int piece, int expected)
        {
            int[,] gameBoard = new int[10, 10];

            gameBoard[x, y] = piece;
            if (y != 0) gameBoard[x, y - 1] = nPiece;
            if (x != 9) gameBoard[x + 1, y] = ePiece;
            if (y != 9) gameBoard[x, y + 1] = sPiece;
            if (x != 0) gameBoard[x - 1, y] = wPiece;

            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);

            AI ai = new AI(win, team);
            Assert.AreEqual(expected, ai.safetyCheck(x, y, piece, gameBoard));
        }
    }

    }


