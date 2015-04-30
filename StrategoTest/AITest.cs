using System;
using Stratego;
using NUnit.Framework;

namespace StrategoTest
{
    [TestFixture()]
    public class AITest
    {
        [TestCase(1)]
        [TestCase(-1)]
        // Tests AI's constructor, which sets the AI's team
        public void TestConstructor(int team)
        {
            int[,] gameBoard = new int[10, 10];
            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);
            AI ai = new AI(win, team);
            Assert.AreEqual(team, ai.team);
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
                    gameBoard[row, column] = 42;
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
            for (int i = 0; i < win.defaults.Length; i++)
                Assert.AreEqual(0, win.getPiecesLeft(i));

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
    }
}
