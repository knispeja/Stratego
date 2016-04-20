using System;
using Stratego;
using NUnit.Framework;
using System.Collections.Generic;
using Stratego.GamePieces;
using Stratego.BattleBehaviors;

namespace StrategoTest
{

    /*
    S v S *
    S v D *
    S v M *
    S v F *
    S v B *

    M v M *
    M v B *
    M v F *
    M v D *

    B v B *
    B v F *
    B v D *

    F v F *
    F v D *

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
                yield return new TestCaseData(newSpy0, newSpy1);
            }
        }
        
        [TestCaseSource("SpyVSSpyData")]
        public void TestSpyVSpyBattle(GamePiece a, GamePiece b)
        {
            Assert.AreEqual(BOTH_DEAD, returnExpectedOnAttack(a, b));
            GamePiece attack2Spy = new SpyPiece(0);
            GamePiece defend2Spy = new SpyPiece(1);
            Assert.AreEqual(BOTH_DEAD, returnExpectedOnAttack(attack2Spy, defend2Spy));
        }

        [TestCase()]
        public void TestSpyVDefaultBattle()
        {
            GamePiece a1Spy = new SpyPiece(0);
            GamePiece d1Scout = new ScoutPiece(1);
            Assert.AreEqual(ScoutPiece.SCOUT_RANK, returnExpectedOnAttack(a1Spy, d1Scout));
            GamePiece a2Scout = new ScoutPiece(0);
            GamePiece d2Spy = new SpyPiece(1);
            Assert.AreEqual(ScoutPiece.SCOUT_RANK, returnExpectedOnAttack(a2Scout, d2Spy));
        }

        [TestCase()]
        public void TestSpyVMinerBattle()
        {
            GamePiece a1Spy = new SpyPiece(0);
            GamePiece d1Miner = new MinerPiece(1);
            Assert.AreEqual(MinerPiece.MINER_RANK, returnExpectedOnAttack(a1Spy, d1Miner));
            GamePiece a2Miner= new MinerPiece(0);
            GamePiece d2Spy = new SpyPiece(1);
            Assert.AreEqual(MinerPiece.MINER_RANK, returnExpectedOnAttack(a2Miner, d2Spy));
        }

        [TestCase()]
        public void TestSpyVFlagBattle()
        {
            GamePiece a1 = new SpyPiece(0);
            GamePiece d1 = new FlagPiece(1);
            Assert.AreEqual(SpyPiece.SPY_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new FlagPiece(0);
            GamePiece d2 = new SpyPiece(1);
            Assert.AreEqual(BOTH_DEAD, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestSpyVMarshallBattle()
        {
            GamePiece a1 = new MarshallPiece(0);
            GamePiece d1 = new SpyPiece(1);
            Assert.AreEqual(MarshallPiece.MARSHALL_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new SpyPiece(0);
            GamePiece d2 = new MarshallPiece(1);
            Assert.AreEqual(SpyPiece.SPY_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestSpyVBombBattle()
        {
            GamePiece a1 = new BombPiece(0);
            GamePiece d1 = new SpyPiece(1);
            Assert.AreEqual(BombPiece.BOMB_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new SpyPiece(0);
            GamePiece d2 = new BombPiece(1);
            Assert.AreEqual(BombPiece.BOMB_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestMinerVMinerBatte()
        {
            GamePiece a1 = new MinerPiece(0);
            GamePiece d1 = new MinerPiece(1);
            Assert.AreEqual(BOTH_DEAD, returnExpectedOnAttack(a1, d1));
        }

        [TestCase()]
        public void TestMinerVBombBattle()
        {
            GamePiece a1 = new BombPiece(0);
            GamePiece d1 = new MinerPiece(1);
            Assert.AreEqual(MinerPiece.MINER_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new MinerPiece(0);
            GamePiece d2 = new BombPiece(1);
            Assert.AreEqual(MinerPiece.MINER_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestMinerVFlagBattle()
        {
            GamePiece a1 = new MinerPiece(0);
            GamePiece d1 = new FlagPiece(1);
            Assert.AreEqual(MinerPiece.MINER_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new FlagPiece(0);
            GamePiece d2 = new MinerPiece(1);
            Assert.AreEqual(MinerPiece.MINER_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestMinerVDefaultBattle()
        {
            GamePiece a1 = new MinerPiece(0);
            GamePiece d1 = new CaptainPiece(1);
            Assert.AreEqual(CaptainPiece.CAPTAIN_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new CaptainPiece(0);
            GamePiece d2 = new MinerPiece(1);
            Assert.AreEqual(CaptainPiece.CAPTAIN_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestBombVBombBattle()
        {
            GamePiece a1 = new BombPiece(0);
            GamePiece d1 = new BombPiece(1);
            Assert.AreEqual(BOTH_DEAD, returnExpectedOnAttack(a1, d1));
        }

        [TestCase()]
        public void TestBombVFlagBattle()
        {
            GamePiece a1 = new BombPiece(0);
            GamePiece d1 = new FlagPiece(1);
            Assert.AreEqual(BombPiece.BOMB_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new FlagPiece(0);
            GamePiece d2 = new BombPiece(1);
            Assert.AreEqual(BombPiece.BOMB_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestBombVDefaultBattle()
        {
            GamePiece a1 = new BombPiece(0);
            GamePiece d1 = new CaptainPiece(1);
            Assert.AreEqual(BombPiece.BOMB_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new CaptainPiece(0);
            GamePiece d2 = new BombPiece(1);
            Assert.AreEqual(BombPiece.BOMB_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestFlagVFlagBattle()
        {
            GamePiece a1 = new FlagPiece(0);
            GamePiece d1 = new FlagPiece(1);
            Assert.AreEqual(FlagPiece.FLAG_RANK, returnExpectedOnAttack(a1, d1));
        }

        [TestCase()]
        public void TestFlagVDefaultBattle()
        {
            GamePiece a1 = new FlagPiece(0);
            GamePiece d1 = new MajorPiece(1);
            Assert.AreEqual(MajorPiece.MAJOR_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new MajorPiece(0);
            GamePiece d2 = new FlagPiece(1);
            Assert.AreEqual(MajorPiece.MAJOR_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestLowerVHigherBattle()
        {
            GamePiece a1 = new SergeantPiece(0);
            GamePiece d1 = new CaptainPiece(1);
            Assert.AreEqual(CaptainPiece.CAPTAIN_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new SergeantPiece(0);
            GamePiece d2 = new MinerPiece(1);
            Assert.AreEqual(SergeantPiece.SERGEANT_RANK, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestBondVersusBond()
        {
            GamePiece a1 = new BondTierSpyPiece(0);
            GamePiece d1 = new BondTierSpyPiece(1);
            Assert.AreEqual(BOTH_DEAD, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new BondTierSpyPiece(0);
            GamePiece d2 = new BondTierSpyPiece(1);
            Assert.AreEqual(BOTH_DEAD, returnExpectedOnAttack(a2, d2));
        }

        [TestCase()]
        public void TestBondVersusMarshall()
        {
            GamePiece a1 = new BondTierSpyPiece(0);
            GamePiece d1 = new MarshallPiece(1);
            d1.setDefendBehavior(new DiesToSpyNotBond());
            Assert.AreEqual(MarshallPiece.MARSHALL_RANK, returnExpectedOnAttack(a1, d1));
            GamePiece a2 = new MarshallPiece(0);
            GamePiece d2 = new BondTierSpyPiece(1);
            a2.setAttackBehavior(new MarshallBeatsBond());
            Assert.AreEqual(MarshallPiece.MARSHALL_RANK, returnExpectedOnAttack(a2, d2));
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
