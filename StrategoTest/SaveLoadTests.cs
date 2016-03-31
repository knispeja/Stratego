using System;
using Stratego;
using NUnit.Framework;
using System.Drawing;
using System.IO;

namespace StrategoTest
{
    [TestFixture()]
    class SaveLoadTests
    {
        [TestCase(2, 3, 2, 1, true)]
        [TestCase(6, 2, 4, -1, false)]
        [TestCase(2, 205, 5, 0, true)]
        [TestCase(100, 30, 2, 1, true)]
        [TestCase(6, 2, 3, 4, false)]
        [TestCase(2, 25, 51, 0, true)]
        public void TestSaveLoad(int gbW, int gbH, int difficulty, int turn, bool isSinglePlayer)
        {
            SaveData saveData = new SaveData(new Gameboard(gbW, gbH), difficulty, turn, true);

            SaveLoadOperations.storeData("test." + SaveLoadOperations.SAVE_FILE_EXTENSION, saveData);
            saveData = SaveLoadOperations.loadSaveData("test." + SaveLoadOperations.SAVE_FILE_EXTENSION);

            Assert.AreEqual(gbW, saveData.boardState.getWidth());
            Assert.AreEqual(gbH, saveData.boardState.getHeight());
            Assert.AreEqual(difficulty, saveData.difficulty);
            Assert.AreEqual(turn, saveData.turn);
            Assert.AreEqual(isSinglePlayer, saveData.isSinglePlayer);
        }
    }
}
