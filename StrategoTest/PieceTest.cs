using System;
using Stratego;
using NUnit.Framework;
using NUnit.Core;
using System.Collections.Generic;

namespace StrategoTest
{
    [TestFixture()]
    public class PieceTest
    {
        //Marshal = 1, General = 2, Colonel = 3, Major = 4, Captain = 5, Lieutenant = 6, Sergeant = 7, Miner = 8, Cout = 9, Spy = 10, Bomb = 11, Flag = 12;
        public static IEnumerable<TestCaseData> SpyVSSpyData
        {
            get
            {
                GamePiece newSpy0 = new SpyPiece(0);
                GamePiece newSpy1 = new SpyPiece(1);
                yield return new TestCaseData(newSpy0, newSpy1, 99);
            }
        }
        /*

        [TestCase(newSpy, newSpy, 1)] //Tests for regular cases where lowest absolute
        [TestCase(-4, 7, -4)] //value wins
        [TestCase(9, -5, -5)]
        [TestCase(1, -10, 1)]
        [TestCase(8, -12, 8)]
        [TestCase(1, -2, 1)]
        [TestCase(1, -3, 1)]
        [TestCase(1, -4, 1)]
        [TestCase(1, -5, 1)]
        [TestCase(1, -6, 1)]
        [TestCase(1, -7, 1)]
        [TestCase(1, -8, 1)]
        [TestCase(1, -9, 1)]
        [TestCase(1, -10, 1)]
        [TestCase(1, -12, 1)]
        [TestCase(2, -1, -1)]
        [TestCase(2, -2, 0)]
        [TestCase(2, -3, 2)]
        [TestCase(2, -4, 2)]
        [TestCase(2, -5, 2)]
        [TestCase(2, -6, 2)]
        [TestCase(2, -7, 2)]
        [TestCase(2, -8, 2)]
        [TestCase(2, -9, 2)]
        [TestCase(2, -10, 2)]
        [TestCase(2, -12, 2)]
        [TestCase(3, -1, -1)]
        [TestCase(3, -2, -2)]
        [TestCase(3, -3, 0)]
        [TestCase(3, -4, 3)]
        [TestCase(3, -5, 3)]
        [TestCase(3, -6, 3)]
        [TestCase(3, -7, 3)]
        [TestCase(3, -8, 3)]
        [TestCase(3, -9, 3)]
        [TestCase(3, -10, 3)]
        [TestCase(3, -12, 3)]
        [TestCase(8, -1, -1)]
        [TestCase(8, -2, -2)]
        [TestCase(8, -3, -3)]
        [TestCase(8, -4, -4)]
        [TestCase(8, -5, -5)]
        [TestCase(8, -6, -6)]
        [TestCase(8, -7, -7)]
        [TestCase(8, -8, 0)]
        [TestCase(8, -9, 8)]
        [TestCase(8, -10, 8)]
        [TestCase(8, -12, 8)]
        [TestCase(-1, 2, -1)]
        [TestCase(-1, 3, -1)]
        [TestCase(-1, 4, -1)]
        [TestCase(-1, 5, -1)]
        [TestCase(-1, 6, -1)]
        [TestCase(-1, 7, -1)]
        [TestCase(-1, 8, -1)]
        [TestCase(-1, 9, -1)]
        [TestCase(-1, 10, -1)]
        [TestCase(-1, 12, -1)]
        [TestCase(-2, 1, 1)]
        [TestCase(-2, 3, -2)]
        [TestCase(-2, 4, -2)]
        [TestCase(-2, 5, -2)]
        [TestCase(-2, 6, -2)]
        [TestCase(-2, 7, -2)]
        [TestCase(-2, 8, -2)]
        [TestCase(-2, 9, -2)]
        [TestCase(-2, 10, -2)]
        [TestCase(-2, 12, -2)]
        [TestCase(-3, 1, 1)]
        [TestCase(-3, 2, 2)]
        [TestCase(-3, 4, -3)]
        [TestCase(-3, 5, -3)]
        [TestCase(-3, 6, -3)]
        [TestCase(-3, 7, -3)]
        [TestCase(-3, 8, -3)]
        [TestCase(-3, 9, -3)]
        [TestCase(-3, 10, -3)]
        [TestCase(-3, 12, -3)]
        [TestCase(-8, 1, 1)]
        [TestCase(-8, 2, 2)]
        [TestCase(-8, 3, 3)]
        [TestCase(-8, 4, 4)]
        [TestCase(-8, 5, 5)]
        [TestCase(-8, 6, 6)]
        [TestCase(-8, 7, 7)]
        [TestCase(-8, 9, -8)]
        [TestCase(-8, 10, -8)]
        [TestCase(-8, 12, -8)]

        [TestCase(-3, 3, 0)] // Test tie cases
        [TestCase(-10, 10, 0)]
        [TestCase(6, -6, 0)]
        [TestCase(-8, 8, 0)]
        [TestCase(-2, 2, 0)]
        [TestCase(-1, 1, 0)]
        [TestCase(1, -1, 0)]

        [TestCase(-3, 11, 11)] // Test that bomb beats all attackers except the miner
        [TestCase(3, -11, -11)]
        [TestCase(-2, 11, 11)]
        [TestCase(2, -11, -11)]
        [TestCase(-1, 11, 11)]
        [TestCase(1, -11, -11)]

        [TestCase(8, -11, 8)] // Test that bomb is beaten by miners
        [TestCase(-8, 11, -8)]

        [TestCase(10, -1, 10)] // Test that Spy beats marshall if it attacks
        [TestCase(-10, 1, -10)]

        [TestCase(11, -11, 0)] // Test that movable bombs kill one another
        [TestCase(-11, 11, 0)]
        */
        // Tests the various cases for the attack function for pieces.
        [TestCaseSource("SpyVSSpyData")]
        public void TestAttack(GamePiece a, GamePiece b, int c)
        {
            int BOTH_DEAD = 99;
            a.attack(b);
            b.defend(a);
            if (a.isAlive())
            {
                Assert.AreEqual(c, a.getPieceRank());
            }
            else if (b.isAlive())
            {
                Assert.AreEqual(c, b.getPieceRank());
            }
            else if (a.isAlive() && b.isAlive()){
                Assert.IsTrue(false);
            }
            else
            {
                Assert.AreEqual(c, BOTH_DEAD);
            }
        }

        /*
        [TestCase(-1,-1)]
        [TestCase(1, 1)]
        [TestCase(-4, -4)]
        [TestCase(4, 4)]
        [TestCase(8, 11)]
        [TestCase(-8, -11)]
        [TestCase(7, 3)]
        [TestCase(-7, -3)]
        [TestCase(-7, 42)]
        [TestCase(7, 42)]
        // Tests that attack returns null two pieces of the same team
        // Or if the piece being attacked is the obsticle piece (42)
        public void TestThatAttackReturnsNullOnInvalidAttack(int a, int b)
        {
            Assert.Null(Piece.attack(a, b));
        }
        */

        /*
        [TestCase(1, "Marshall")]
        [TestCase(2, "General")]
        [TestCase(3, "Colonel")]
        [TestCase(4, "Major")]
        [TestCase(5, "Captain")]
        [TestCase(6, "Lieutenant")]
        [TestCase(7, "Sergeant")]
        [TestCase(8, "Miner")]
        [TestCase(9, "Scout")]
        [TestCase(10, "Spy")]
        [TestCase(11, "Bomb")]
        [TestCase(12, "Flag")]
        [TestCase(-4, "Major")]
        [TestCase(-5, "Captain")]
        [TestCase(-8, "Miner")]
        // Tests that toString() method in Piece will convert integers to the appropriate 
        // name for that piece.
        public string TestPieceToString(int pieceNumber)
        {
            return Piece.toString(pieceNumber);
        }
        */

        /*
        [TestCase(0)]
        [TestCase(42)]
        [TestCase(-100)]
        // Tests Piece's toString() method for invalid inputs
        public void TestToStringThrowsException(int invalidPiece)
        {
            Assert.Throws<ArgumentException>(() => Piece.toString(invalidPiece));
        }
        */
    }
}
