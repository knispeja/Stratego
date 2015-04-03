using System;
using Stratego;
using NUnit.Framework;

namespace StrategoTest
{
    [TestFixture()]
    public class PieceTest
    {
        //static int Marshal = 1, General = 2, Colonel = 3, Major = 4, Captain = 5, Lieutenant = 6, Sergeant = 7, Miner = 8, Cout = 9, Spy = 10, Bomb = 11, Flag = 12;

        [TestCase(1, -2, Result = 1)]
        [TestCase(-4, 7, Result = -4)]
        [TestCase(8, -11, Result = 8)]
        [TestCase(-3, 11, Result = 11)]
        [TestCase(9, -5, Result = -5)]
        [TestCase(1, -10, Result = -10)]
        [TestCase(8, -12, Result = 8)]
        [TestCase(6, -6, Result = 0)]
        [TestCase(10, -1, Result = 10)]
        public int SuperTest2(int a, int b)
        {
            return Piece.attack(a, b).Value;
        }
    }
}
