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
        public void TestThatPieceIsPlacedIntoEmptySpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            bool? result = game.placePiece(piece, x, y);
            Assert.IsTrue(game.getPiece(x/100, y/100)==piece);
            Assert.IsTrue(result.Value); 
        }

        [TestCase(1, 100, 100, Result = false)]
        [TestCase(2, 100, 100, Result = false)]
        [TestCase(3, 100, 100, Result = false)]
        [TestCase(4, 100, 100, Result = false)]
       
        public bool? TestThatNothingCanBePlacedInFilledSpace(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 100, y / 100] = 1;

            StrategoWin game = new StrategoWin(1000, 1000, map);
            return game.placePiece(piece, x, y);
        }

    }
}
