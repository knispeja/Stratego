using System;
using Stratego;
using NUnit.Framework;

namespace StrategoTest
{
    [TestFixture()]
    public class PieceTest
    {
        [Test()]
        public void TestThatMarshallBeatsGeneral()
        {
            Assert.AreEqual(1, Piece.attack(1, 2));
        }

        [Test()]
        public void TestThatMajorBeatsSergeant()
        {
            Assert.AreEqual(4, Piece.attack(4, 7));
        }

        [Test()]
        public void TestThatMinerBeatsBomb()
        {
            Assert.AreEqual(8, Piece.attack(8, 12));
        }

        [Test()]
        public void TestThatBombBeatsNonMinerAttacker()
        {
            Assert.AreEqual(12, Piece.attack(3, 12));
        }

        [Test()]
        public void TestThatScoutLosesToCaptain()
        {
            Assert.AreEqual(5, Piece.attack(9, 5));
        }
    }
}
