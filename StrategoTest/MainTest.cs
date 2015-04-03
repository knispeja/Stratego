using System;
using Stratego;
using NUnit.Framework;

namespace StrategoTest
{
    [TestFixture()]
    class MainTest
    {

        [TestCase(1, 100, 100)]
        public bool TestThatNothingCanBePlacedInNullSpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(100, new int[10, 10]);
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 100, 100)]
        public bool TestThatPieceIsPlacedIntoEmptySpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(100, new int[10, 10] {{0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0},
                                                                     {0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0},
                                                                     {0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0},
                                                                     {0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0},
                                                                     {0,0,0,0,0,0,0,0,0,0}, {0,0,0,0,0,0,0,0,0,0}});
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 100, 100)]
        public bool TestThatNothingCanBePlacedInFilledSpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(100, new int[10, 10] {{1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1},
                                                                     {1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1},
                                                                     {1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1},
                                                                     {1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1},
                                                                     {1,1,1,1,1,1,1,1,1,1}, {1,1,1,1,1,1,1,1,1,1}});
            return game.placePiece(piece, x, y);
        }

    }
}
