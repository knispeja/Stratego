using System;
using Stratego;
using NUnit.Framework;
using System.Windows.Forms;

namespace StrategoTest
{
    [TestFixture()]
    class MainTest
    {
        [TestCase(1, 100, 100, Result = false)]
        [TestCase(2, 100, 100, Result = false)]
        [TestCase(3, 100, 100, Result = false)]
        [TestCase(4, 100, 100, Result = false)]
        [TestCase(5, 100, 100, Result = false)]
        [TestCase(6, 100, 100, Result = false)]
        [TestCase(7, 100, 100, Result = false)]
        [TestCase(8, 100, 100, Result = false)]
        [TestCase(9, 100, 100, Result = false)]
        [TestCase(10, 100, 100, Result = false)]
        [TestCase(11, 100, 100, Result = false)]
        [TestCase(12, 100, 100, Result = false)]
        [TestCase(-1, 100, 100, Result = false)]
        [TestCase(-2, 100, 100, Result = false)]
        [TestCase(-3, 100, 100, Result = false)]
        [TestCase(-4, 100, 100, Result = false)]
        [TestCase(-5, 100, 100, Result = false)]
        [TestCase(-6, 100, 100, Result = false)]
        [TestCase(-7, 100, 100, Result = false)]
        [TestCase(-8, 100, 100, Result = false)]
        [TestCase(-9, 100, 100, Result = false)]
        [TestCase(-10, 100, 100, Result = false)]
        [TestCase(-11, 100, 100, Result = false)]
        [TestCase(-12, 100, 100, Result = false)]
        [TestCase(-12, 200, 200, Result = false)]
        [TestCase(-12, 300, 300, Result = false)]
        [TestCase(-12, 400, 400, Result = false)]
        [TestCase(-12, 101, 101, Result = false)]
        [TestCase(-12, 201, 201, Result = false)]
        [TestCase(-12, 301, 301, Result = false)]
        [TestCase(-12, 401, 401, Result = false)]
        public bool? TestThatNothingCanBePlacedOnObstacle(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x/100, y/100] = 42;

            StrategoWin game = new StrategoWin(1000, 1000, map);
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 100, 100)]
        [TestCase(2, 100, 100)]
        [TestCase(3, 100, 100)]
        [TestCase(4, 100, 100)]
        [TestCase(5, 100, 100)]
        [TestCase(6, 100, 100)]
        [TestCase(7, 100, 100)]
        [TestCase(8, 100, 100)]
        [TestCase(9, 100, 100)]
        [TestCase(10, 100, 100)]
        [TestCase(11, 100, 100)]
        [TestCase(12, 100, 100)]
        [TestCase(-1, 100, 100)]
        [TestCase(-2, 100, 100)]
        [TestCase(-3, 100, 100)]
        [TestCase(-4, 100, 100)]
        [TestCase(-5, 100, 100)]
        [TestCase(-6, 100, 100)]
        [TestCase(-7, 100, 100)]
        [TestCase(-8, 100, 100)]
        [TestCase(-9, 100, 100)]
        [TestCase(-10, 100, 100)]
        [TestCase(-11, 100, 100)]
        [TestCase(-12, 100, 100)]
        [TestCase(-12, 200, 200)]
        [TestCase(-12, 300, 300)]
        [TestCase(-12, 400, 400)]
        [TestCase(-12, 101, 101)]
        [TestCase(-12, 201, 201)]
        [TestCase(-12, 301, 301)]
        [TestCase(-12, 401, 401)]
        public void TestThatPieceIsPlacedIntoEmptySpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            bool? result = game.placePiece(piece, x, y);
            Assert.AreEqual(game.getPiece(x/100, y/100),piece);
            Assert.IsTrue(result.Value); 
        }

        [TestCase(1, 100, 100, Result = false)]
        [TestCase(2, 100, 100, Result = false)]
        [TestCase(3, 100, 100, Result = false)]
        [TestCase(4, 100, 100, Result = false)]
        [TestCase(5, 100, 100, Result = false)]
        [TestCase(6, 100, 100, Result = false)]
        [TestCase(7, 100, 100, Result = false)]
        [TestCase(8, 100, 100, Result = false)]
        [TestCase(9, 100, 100, Result = false)]
        [TestCase(10, 100, 100, Result = false)]
        [TestCase(11, 100, 100, Result = false)]
        [TestCase(12, 100, 100, Result = false)]
        [TestCase(-1, 100, 100, Result = false)]
        [TestCase(-2, 100, 100, Result = false)]
        [TestCase(-3, 100, 100, Result = false)]
        [TestCase(-4, 100, 100, Result = false)]
        [TestCase(-5, 100, 100, Result = false)]
        [TestCase(-6, 100, 100, Result = false)]
        [TestCase(-7, 100, 100, Result = false)]
        [TestCase(-8, 100, 100, Result = false)]
        [TestCase(-9, 100, 100, Result = false)]
        [TestCase(-10, 100, 100, Result = false)]
        [TestCase(-11, 100, 100, Result = false)]
        [TestCase(-12, 100, 100, Result = false)]
        [TestCase(-12, 200, 200, Result = false)]
        [TestCase(-12, 300, 300, Result = false)]
        [TestCase(-12, 400, 400, Result = false)]
        [TestCase(-12, 101, 101, Result = false)]
        [TestCase(-12, 201, 201, Result = false)]
        [TestCase(-12, 301, 301, Result = false)]
        [TestCase(-12, 401, 401, Result = false)]
        public bool? TestThatNothingCanBePlacedInFilledSpace(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 100, y / 100] = 1;

            StrategoWin game = new StrategoWin(1000, 1000, map);
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 200, 200, Result = false)]
        [TestCase(1, 400, 400, Result = false)]
        [TestCase(1, 600, 600, Result = false)]
        [TestCase(1, 800, 800, Result = false)]
        public bool? TestThatNothingCanBePlacedOnObstacleV2(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 200, y / 200] = 42;

            StrategoWin game = new StrategoWin(2000, 2000, map);
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 200, 200, Result = false)]
        [TestCase(1, 400, 400, Result = false)]
        [TestCase(1, 600, 600, Result = false)]
        [TestCase(1, 800, 800, Result = false)]
        [TestCase(1, 201, 201, Result = false)]
        [TestCase(1, 401, 401, Result = false)]
        [TestCase(1, 601, 601, Result = false)]
        [TestCase(1, 801, 801, Result = false)]
        public bool? TestThatNothingCanBePlacedInFilledSpaceV2(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 200, y / 200] = 1;

            StrategoWin game = new StrategoWin(2000, 2000, map);
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 200, 200)]
        [TestCase(1, 400, 400)]
        [TestCase(1, 600, 600)]
        [TestCase(1, 800, 800)]
        [TestCase(1, 201, 201)]
        [TestCase(1, 401, 401)]
        [TestCase(1, 601, 601)]
        [TestCase(1, 801, 801)]
        public void TestThatPieceIsPlacedIntoEmptySpaceV2(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(2000, 2000, new int[10, 10]);
            bool? result = game.placePiece(piece, x, y);
            Assert.AreEqual(game.getPiece(x / 200, y / 200), piece);
            Assert.IsTrue(result.Value);
        }

    }
}
