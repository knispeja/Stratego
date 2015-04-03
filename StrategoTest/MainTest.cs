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
        public bool? TestThatNothingCanBePlacedInNullSpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 100, 100)]
        public void TestThatPieceIsPlacedIntoEmptySpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10] {{0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0},
                                                                     {0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0},
                                                                     {0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0},
                                                                     {0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0},
                                                                     {0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0}});
            bool? result = game.placePiece(piece, x, y);
            Assert.IsTrue(game.getPiece(x/100, y/100)==piece);
            Assert.IsTrue(result.Value); 
        }

        [TestCase(1, 100, 100, Result = false)]
        public bool? TestThatNothingCanBePlacedInFilledSpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000,1000, new int[10, 10] {{1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1},
                                                                     {1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1},
                                                                     {1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1},
                                                                     {1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1},
                                                                     {1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1}});
            return game.placePiece(piece, x, y);
        }

    }
}
