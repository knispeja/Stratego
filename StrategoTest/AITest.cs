using System;
using Stratego;
using NUnit.Framework;

namespace StrategoTest
{
    [TestFixture()]
    public class AITest
    {
        [TestCase(1)]
        [TestCase(-1)]
        // Tests AI's constructor, which sets the AI's team
        public void TestConstructor(int team)
        {
            int[,] gameBoard = new int[10, 10];
            StrategoWin win = new StrategoWin(1000, 1000, gameBoard);
            AI ai = new AI(win, team);
            Assert.AreEqual(team, ai.team);
        }
    }
}
