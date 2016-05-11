using NUnit.Framework;
using Stratego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategoTest
{

    [TestFixture()]
    class AITest
    {
        [TestCase()]
        public void testAIConstructs()
        {
            AIv2 test = new AIv2(new Gameboard(10, 10), 0);
            Assert.IsNotNull(test);
        }

        [TestCase()]
        public void testMoveStartsNull()
        {
            AIv2 test = new AIv2(new Gameboard(10, 10), 0);
            Assert.IsNull(test.getMoveToMake());
        }

        [TestCase()]
        public void testMoveIsSetByTakeTurn()
        {
            Gameboard g = new Gameboard(10, 10);
            g.setPiece(new BoardPosition(5, 5), new SpyPiece(-1));
            AIv2 test = new AIv2(g, 0);
            test.takeTurn();
            Assert.IsNotNull(test.getMoveToMake());
        }

        [TestCase()]
        public void testMoveChoiceIsValid()
        {
            Gameboard g = new Gameboard(10, 10);
            g.setPiece(new BoardPosition(5, 5), new SpyPiece(-1));
            AIv2 test = new AIv2(g, 0);
            test.takeTurn();
            Move m = test.getMoveToMake();
            BoardPosition start = new BoardPosition(5, 5);
            Assert.IsTrue(m.Equals(new Move(start, new BoardPosition(4, 5)))
                       || m.Equals(new Move(start, new BoardPosition(6, 5)))
                       || m.Equals(new Move(start, new BoardPosition(5, 4)))
                       || m.Equals(new Move(start, new BoardPosition(5, 6))));
        }

        [TestCase()]
        public void testMoveChoiceIsValidWhenBlocked()
        {
            Gameboard g = new Gameboard(10, 10);
            g.setPiece(new BoardPosition(5, 5), new SpyPiece(-1));
            g.setPiece(new BoardPosition(5, 4), new SpyPiece(-1));
            AIv2 test = new AIv2(g, 0);
            test.takeTurn();
            Move m = test.getMoveToMake();
            BoardPosition start = m.getStart();
            if(start.Equals(new BoardPosition(5, 5)))
            {
                Assert.IsTrue(m.Equals(new Move(start, new BoardPosition(4, 5)))
                           || m.Equals(new Move(start, new BoardPosition(6, 5)))
                           || m.Equals(new Move(start, new BoardPosition(5, 6))));
            }
            else
            {
                Assert.IsTrue(m.Equals(new Move(start, new BoardPosition(5, 3)))
                           || m.Equals(new Move(start, new BoardPosition(6, 4)))
                           || m.Equals(new Move(start, new BoardPosition(4, 4))));
            }

        }
    }
}
