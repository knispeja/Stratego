using Stratego;
using NUnit.Framework;
using System.Collections.Generic;

namespace StrategoTest
{
    [TestFixture()]
    public class MovementTest
    {

        //Marshal = 1, General = 2, Colonel = 3, Major = 4, Captain = 5, Lieutenant = 6, Sergeant = 7, Miner = 8, Cout = 9, Spy = 10, Bomb = 11, Flag = 12;
        public static IEnumerable<TestCaseData> EmptyData
        {
            get
            {
                yield return new TestCaseData();
            }
        }

        [TestCaseSource("EmptyData")]
        public void TestNothingExample()
        {
            Assert.True(true);
        }

        [TestCase()]
        public void TestNoValidMoves()
        {
            int xSize = 4;
            int ySize = 4;
            int[,] expectedMoves = new int[xSize, ySize];
        }
    }
}
