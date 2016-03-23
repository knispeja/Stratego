using System;
using Stratego;
using NUnit.Framework;
using NUnit.Core;
using System.Collections.Generic;

namespace StrategoTest
{

    /*
    S v S *
    S v D *
    S v M
    S v F
    S v B

    M v M
    M v B
    M v F
    M v D

    B v B
    B v F
    B v D

    F v F
    F v D

    D v D
    */

    [TestFixture()]
    public class PieceTest
    {
        public static readonly int BOTH_DEAD = 99;
        public static readonly int FAILURE = 100;

        //Marshal = 1, General = 2, Colonel = 3, Major = 4, Captain = 5, Lieutenant = 6, Sergeant = 7, Miner = 8, Cout = 9, Spy = 10, Bomb = 11, Flag = 12;
        public static IEnumerable<TestCaseData> SpyVSSpyData
        {
            get
            {
                GamePiece newSpy0 = new SpyPiece(0);
                GamePiece newSpy1 = new SpyPiece(1);
                yield return new TestCaseData(newSpy0, newSpy1, BOTH_DEAD);
            }
        }
        
        [TestCaseSource("SpyVSSpyData")]
        public void TestSpyVSpyBattle(GamePiece a, GamePiece b, int c)
        {
            Assert.AreEqual(c, returnExpectedOnAttack(a, b));
            GamePiece attack2Spy = new SpyPiece(0);
            GamePiece defend2Spy = new SpyPiece(1);
            Assert.AreEqual(c, returnExpectedOnAttack(attack2Spy, defend2Spy));
        }

        [TestCase(1, 2)]
        public void TestSpyVDefaultBattle(int r1, int r2)
        {
            GamePiece a1Spy = new SpyPiece(0);
            GamePiece d1Scout = new ScoutPiece(1);
            Assert.AreEqual(r1, returnExpectedOnAttack(a1Spy, d1Scout));
            GamePiece a2Scout = new ScoutPiece(0);
            GamePiece d2Spy = new SpyPiece(1);
            Assert.AreEqual(r2, returnExpectedOnAttack(a2Scout, d2Spy));
        }

        public int returnExpectedOnAttack(GamePiece a, GamePiece b)
        {
            a.attack(b);
            b.defend(a);
            if (a.isAlive())
            {
                return a.getPieceRank();
            }
            else if (b.isAlive())
            {
                return b.getPieceRank();
            }
            else if (a.isAlive() && b.isAlive())
            {
                return FAILURE;
            }
            else
            {
                return BOTH_DEAD;
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
