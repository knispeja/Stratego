using System;
using Stratego;
using NUnit.Framework;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace StrategoTest
{
    [TestFixture()]
    class SaveLoadTests
    {
        public static readonly string TEST_FILE_NAME = "UNIT_TEST_FILE_SAFE_TO_DELETE =) DONT_WORRY";

        [TestCase(2, 3, 2, 1, true, 0)]
        [TestCase(6, 2, 4, -1, false, 0)]
        [TestCase(2, 205, 5, 0, true, 3)]
        [TestCase(100, 30, 2, 1, true, 2)]
        [TestCase(6, 2, 3, 4, false, -1)]
        [TestCase(2, 25, 51, 0, true, 11)]
        public void TestLoadSaveData(int gbW, int gbH, int difficulty, int turn, bool isSinglePlayer, int level)
        {
            Gameboard board = new Gameboard(gbW, gbH);
            board.setPiece(0, 0, new BombPiece(-1));
            board.setPiece(gbW - 1, gbH - 1, new FlagPiece(1));

            SaveData saveData = new SaveData(board, difficulty, turn, isSinglePlayer, level);

            SaveLoadOperations.storeData(TEST_FILE_NAME + SaveLoadOperations.SAVE_FILE_EXTENSION, saveData);
            saveData = SaveLoadOperations.loadSaveData(TEST_FILE_NAME + SaveLoadOperations.SAVE_FILE_EXTENSION);

            Assert.AreEqual(gbW, saveData.boardState.getWidth());
            Assert.AreEqual(gbH, saveData.boardState.getHeight());

            for (int row = 0; row < gbH; row++ )
            {
                for (int col = 0; col < gbW; col++)
                {
                    GamePiece expectedPiece = board.getPiece(col, row);
                    GamePiece actualPiece = saveData.boardState.getPiece(col, row);
                    if (expectedPiece == null)
                        Assert.IsNull(actualPiece);
                    else
                    {
                        Assert.AreEqual(expectedPiece.GetType(), actualPiece.GetType());
                        Assert.AreEqual(expectedPiece.getPieceRank(), actualPiece.getPieceRank());
                        Assert.AreEqual(expectedPiece.getPieceColor(), actualPiece.getPieceColor());
                        Assert.AreEqual(expectedPiece.getPieceImage().Size, actualPiece.getPieceImage().Size);
                        Assert.AreEqual(expectedPiece.getLimitToMovement(), actualPiece.getLimitToMovement());
                        Assert.AreEqual(expectedPiece.getTeamCode(), actualPiece.getTeamCode());
                        Assert.AreEqual(expectedPiece.isEssential(), actualPiece.isEssential());
                        Assert.AreEqual(expectedPiece.getPieceName(), actualPiece.getPieceName());
                    }
                }
            }

            Assert.AreEqual(difficulty, saveData.difficulty);
            Assert.AreEqual(turn, saveData.turn);
            Assert.AreEqual(isSinglePlayer, saveData.isSinglePlayer);
            Assert.AreEqual(level, saveData.level);
        }

        [TestCase(10, 10, 4, 2)]
        [TestCase(14, 9, 9, 7)]
        [TestCase(10, 10, 10, 1)]
        [TestCase(3, 100, 11, 4)]
        [TestCase(11, 20, 3, 2)]
        [TestCase(10, 43, 5, 7)]
        [TestCase(5, 10, 8, 10)]
        public void TestLoadSetupDataBlueTeam(int gbW, int gbH, int minPieces, int placeSeed)
        {
            Gameboard board = new Gameboard(gbW, gbH);

            Dictionary<string, int> expPlacements = new Dictionary<string, int>();
            expPlacements.Add("big!", placeSeed * 100);
            expPlacements.Add("stupid", placeSeed);
            expPlacements.Add("dumb", placeSeed + 2);
            expPlacements.Add("genius omg", placeSeed + 1);
            expPlacements.Add("why is this here", placeSeed + 1000);

            SetupData data = new SetupData(board, expPlacements, minPieces, 1);
            SaveLoadOperations.storeData(TEST_FILE_NAME + SaveLoadOperations.SETUP_FILE_EXTENSION, data);
            data = SaveLoadOperations.loadSetupData(TEST_FILE_NAME + SaveLoadOperations.SETUP_FILE_EXTENSION);

            Assert.AreEqual(gbW, data.boardState.getWidth());
            Assert.AreEqual(gbH, data.boardState.getHeight());

            for (int row = 0; row < gbH; row++)
            {
                for (int col = 0; col < gbW; col++)
                {
                    GamePiece expectedPiece = board.getPiece(col, row);
                    GamePiece actualPiece = data.boardState.getPiece(col, row);
                    if (expectedPiece == null)
                        Assert.IsNull(actualPiece);
                    else
                    {
                        Assert.AreEqual(expectedPiece.GetType(), actualPiece.GetType());
                        Assert.AreEqual(expectedPiece.getPieceRank(), actualPiece.getPieceRank());
                        Assert.AreEqual(expectedPiece.getPieceColor(), actualPiece.getPieceColor());
                        Assert.AreEqual(expectedPiece.getPieceImage().Size, actualPiece.getPieceImage().Size);
                        Assert.AreEqual(expectedPiece.getLimitToMovement(), actualPiece.getLimitToMovement());
                        Assert.AreEqual(expectedPiece.getTeamCode(), actualPiece.getTeamCode());
                        Assert.AreEqual(expectedPiece.isEssential(), actualPiece.isEssential());
                        Assert.AreEqual(expectedPiece.getPieceName(), actualPiece.getPieceName());
                    }
                }
            }

            Assert.AreEqual(minPieces, data.minPieces);
            Assert.AreEqual(expPlacements, data.getPlacementsDictionary());
            Assert.AreEqual(expPlacements, data.getPlacementsDictionary()); // This call is supposed to be here twice!
        }

        // Breaks on non-square tests :(
        // [TestCase(15, 6, 2)]
        // [TestCase(5, 22, 8)]
        [TestCase(10, 10, 2)]
        [TestCase(9, 9, 7)]
        [TestCase(11, 11, 1)]
        [TestCase(3, 3, 4)]
        [TestCase(20, 20, 2)]
        [TestCase(43, 43, 7)]
        [TestCase(5, 5, 10)]
        public void TestLoadSetupDataRedTeam(int gbW, int gbH, int placeSeed)
        {
            Gameboard board = new Gameboard(gbW, gbH);

            Dictionary<string, int> expPlacements = new Dictionary<string, int>();
            expPlacements.Add("big!", placeSeed * 100);
            expPlacements.Add("stupid", placeSeed);
            expPlacements.Add("dumb", placeSeed + 2);
            expPlacements.Add("genius omg", placeSeed + 1);
            expPlacements.Add("why is this here", placeSeed + 1000);

            SetupData data = new SetupData(board, expPlacements, 5, -1);
            SaveLoadOperations.storeData(TEST_FILE_NAME + SaveLoadOperations.SETUP_FILE_EXTENSION, data);
            data = SaveLoadOperations.loadSetupData(TEST_FILE_NAME + SaveLoadOperations.SETUP_FILE_EXTENSION);

            Assert.AreEqual(gbW, data.boardState.getWidth());
            Assert.AreEqual(gbH, data.boardState.getHeight());

            for (int row = 0; row < gbH; row++)
            {
                for (int col = 0; col < gbW; col++)
                {
                    GamePiece expectedPiece = board.getPiece(row, gbH - col - 1);
                    GamePiece actualPiece = data.boardState.getPiece(gbW - row - 1, col);
                    if (expectedPiece == null)
                        Assert.IsNull(actualPiece);
                    else
                    {
                        Assert.AreEqual(expectedPiece.GetType(), actualPiece.GetType());
                        Assert.AreEqual(expectedPiece.getPieceRank(), actualPiece.getPieceRank());
                        Assert.AreEqual(expectedPiece.getPieceColor(), actualPiece.getPieceColor());
                        Assert.AreEqual(expectedPiece.getPieceImage().Size, actualPiece.getPieceImage().Size);
                        Assert.AreEqual(expectedPiece.getLimitToMovement(), actualPiece.getLimitToMovement());
                        Assert.AreEqual(expectedPiece.getTeamCode(), actualPiece.getTeamCode());
                        Assert.AreEqual(expectedPiece.isEssential(), actualPiece.isEssential());
                        Assert.AreEqual(expectedPiece.getPieceName(), actualPiece.getPieceName());
                    }
                }
            }

            Assert.AreEqual(expPlacements, data.getPlacementsDictionary());
            Assert.AreEqual(expPlacements, data.getPlacementsDictionary()); // This is supposed to be here twice to make sure SetupData works properly
        }
    }
}
