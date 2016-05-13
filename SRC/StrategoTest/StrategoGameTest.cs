using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategoTest
{
    [TestFixture()]
    class StrategoGameTest
    {
        [TestCase()]
        public void TestInitialBoardState()
        {
            Stratego.StrategoGame game = new Stratego.StrategoGame(new DummyStrategoWin());

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 10; col++)
                    Assert.True(game.boardState.getPiece(col, row).GetType() == (new Stratego.ObstaclePiece(0)).GetType());
            }
            for (int row = 6; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                    Assert.True(game.boardState.getPiece(col, row) == null);
            }
        }
    }
}
