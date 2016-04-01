using System;
using Stratego;
using NUnit.Framework;
using System.Drawing;
using System.IO;

namespace StrategoTest
{
    [TestFixture()]
    class GameBoardTests
    {
        [TestCase(2, 3)]
        [TestCase(6, 2)]
        [TestCase(2, 205)]
        public void TestGameBoard(int gbW, int gbH)
        {
            Gameboard g = new Gameboard(gbW, gbH);

            Assert.AreEqual(gbW, g.getWidth());
            Assert.AreEqual(gbH, g.getHeight());

            Assert.AreEqual(null, g.getPiece(0, 0));
            g.setPiece(0, 0, new SpyPiece(1));
            Assert.AreEqual(SpyPiece.SPY_RANK, g.getPiece(0, 0).getPieceRank());

            g.fillRow(null, 0);

            Assert.AreEqual(null, g.getPiece(0, 0));
        }
    }
}
