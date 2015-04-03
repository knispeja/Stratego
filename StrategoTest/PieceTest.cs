﻿using System;
using Stratego;
using NUnit.Framework;

namespace StrategoTest
{
    [TestFixture()]
    public class PieceTest
    {
        //Marshal = 1, General = 2, Colonel = 3, Major = 4, Captain = 5, Lieutenant = 6, Sergeant = 7, Miner = 8, Cout = 9, Spy = 10, Bomb = 11, Flag = 12;
        [TestCase(1, -2, Result = 1)]
        [TestCase(-4, 7, Result = -4)]
        [TestCase(8, -11, Result = 8)]
        [TestCase(-3, 11, Result = 11)]
        [TestCase(9, -5, Result = -5)]
        [TestCase(1, -10, Result = -10)]
        [TestCase(8, -12, Result = 8)]
        [TestCase(6, -6, Result = 0)]
        [TestCase(10, -1, Result = 10)]
        [TestCase(1, -1, Result = 0)]
        [TestCase(1, -2, Result = 1)]
        [TestCase(1, -3, Result = 1)]
        [TestCase(1, -4, Result = 1)]
        [TestCase(1, -5, Result = 1)]
        [TestCase(1, -6, Result = 1)]
        [TestCase(1, -7, Result = 1)]
        [TestCase(1, -8, Result = 1)]
        [TestCase(1, -9, Result = 1)]
        [TestCase(1, -10, Result = -10)]
        [TestCase(1, -11, Result = -11)]
        [TestCase(1, -12, Result = 1)]
        [TestCase(2, -1, Result = -1)]
        [TestCase(2, -2, Result = 0)]
        [TestCase(2, -3, Result = 2)]
        [TestCase(2, -4, Result = 2)]
        [TestCase(2, -5, Result = 2)]
        [TestCase(2, -6, Result = 2)]
        [TestCase(2, -7, Result = 2)]
        [TestCase(2, -8, Result = 2)]
        [TestCase(2, -9, Result = 2)]
        [TestCase(2, -10, Result = 2)]
        [TestCase(2, -11, Result = -11)]
        [TestCase(2, -12, Result = 2)]
        [TestCase(3, -1, Result = -1)]
        [TestCase(3, -2, Result = -2)]
        [TestCase(3, -3, Result = 0)]
        [TestCase(3, -4, Result = 3)]
        [TestCase(3, -5, Result = 3)]
        [TestCase(3, -6, Result = 3)]
        [TestCase(3, -7, Result = 3)]
        [TestCase(3, -8, Result = 3)]
        [TestCase(3, -9, Result = 3)]
        [TestCase(3, -10, Result = 3)]
        [TestCase(3, -11, Result = -11)]
        [TestCase(3, -12, Result = 3)]
        [TestCase(8, -1, Result = -1)]
        [TestCase(8, -2, Result = -2)]
        [TestCase(8, -3, Result = -3)]
        [TestCase(8, -4, Result = -4)]
        [TestCase(8, -5, Result = -5)]
        [TestCase(8, -6, Result = -6)]
        [TestCase(8, -7, Result = -7)]
        [TestCase(8, -8, Result = 0)]
        [TestCase(8, -9, Result = 8)]
        [TestCase(8, -10, Result = 8)]
        [TestCase(8, -11, Result = 8)]
        [TestCase(8, -12, Result = 8)]
        [TestCase(-1, 1, Result = 0)]
        [TestCase(-1, 2, Result = -1)]
        [TestCase(-1, 3, Result = -1)]
        [TestCase(-1, 4, Result = -1)]
        [TestCase(-1, 5, Result = -1)]
        [TestCase(-1, 6, Result = -1)]
        [TestCase(-1, 7, Result = -1)]
        [TestCase(-1, 8, Result = -1)]
        [TestCase(-1, 9, Result = -1)]
        [TestCase(-1, 10, Result = 10)]
        [TestCase(-1, 11, Result = 11)]
        [TestCase(-1, 12, Result = -1)]
        [TestCase(-2, 1, Result = 1)]
        [TestCase(-2, 2, Result = 0)]
        [TestCase(-2, 3, Result = -2)]
        [TestCase(-2, 4, Result = -2)]
        [TestCase(-2, 5, Result = -2)]
        [TestCase(-2, 6, Result = -2)]
        [TestCase(-2, 7, Result = -2)]
        [TestCase(-2, 8, Result = -2)]
        [TestCase(-2, 9, Result = -2)]
        [TestCase(-2, 10, Result = -2)]
        [TestCase(-2, 11, Result = 11)]
        [TestCase(-2, 12, Result = -2)]
        [TestCase(-3, 1, Result = 1)]
        [TestCase(-3, 2, Result = 2)]
        [TestCase(-3, 3, Result = 0)]
        [TestCase(-3, 4, Result = -3)]
        [TestCase(-3, 5, Result = -3)]
        [TestCase(-3, 6, Result = -3)]
        [TestCase(-3, 7, Result = -3)]
        [TestCase(-3, 8, Result = -3)]
        [TestCase(-3, 9, Result = -3)]
        [TestCase(-3, 10, Result = -3)]
        [TestCase(-3, 11, Result = 11)]
        [TestCase(-3, 12, Result = -3)]
        [TestCase(-8, 1, Result = 1)]
        [TestCase(-8, 2, Result = 2)]
        [TestCase(-8, 3, Result = 3)]
        [TestCase(-8, 4, Result = 4)]
        [TestCase(-8, 5, Result = 5)]
        [TestCase(-8, 6, Result = 6)]
        [TestCase(-8, 7, Result = 7)]
        [TestCase(-8, 8, Result = 0)]
        [TestCase(-8, 9, Result = -8)]
        [TestCase(-8, 10, Result = -8)]
        [TestCase(-8, 11, Result = -8)]
        [TestCase(-8, 12, Result = -8)]
        [TestCase(-10, 10, Result = 0)]
        public int SuperTest2(int a, int b)
        {
            return Piece.attack(a, b).Value;
        }
    }
}
