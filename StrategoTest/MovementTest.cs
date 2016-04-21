using Stratego;
using NUnit.Framework;
using System.Collections.Generic;
using Stratego.GamePieces;
using System;

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

        public class FakeStrategoGame : StrategoGame
        {
            public FakeStrategoGame(Gameboard gb, GUICallback guiCall) : base (gb, guiCall)
            {
                this.boardState = gb;
                this.callback = guiCall;
            }

            public void fakePlacePiece(int x, int y, GamePiece p)
            {
                BoardPosition newPos = new BoardPosition(x, y);
                this.boardState.setPiece(x, y, p);
                this.selectedPosition = newPos;
            }
        }

        public class FakeGUI : GUICallback
        {
            public FakeGUI() { }

            public void adjustTurnButtonState(string buttonText) { }

            public void gameOver(int teamCode) { }

            public void invalidateBackpanel() { }

            public void setSidePanelVisibility(bool visible) { }

            public void onNextTurnButtonClick() { }
        }

        [TestCase()]
        public void TestBondDiag()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(3, 3);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, null);
            GamePiece pieceToPlace = newGame.factory.getPiece(BondTierSpyPiece.BOND_NAME, 0);
            newGame.fakePlacePiece(2, 2, pieceToPlace);
            newGame.boardState.setPiece(2, 2, pieceToPlace);
            bool actual = newGame.MovePiece(1, 1);
            Assert.IsFalse(actual);
        }

        [TestCase()]
        public void TestBondUp()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(3, 3);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new BondTierSpyPiece(0);
            newGame.fakePlacePiece(2, 2, pieceToPlace);
            bool actual = newGame.MovePiece(2, 1);
            Assert.IsTrue(actual);
            Assert.AreEqual(newGame.boardState.getPiece(2, 1).GetType(), typeof(BondTierSpyPiece));
            Assert.AreEqual(newGame.boardState.getPiece(2, 2), null);
        }

        [TestCase()]
        public void TestBondLeft()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(3, 3);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new BondTierSpyPiece(0);
            newGame.fakePlacePiece(2, 2, pieceToPlace);
            bool actual = newGame.MovePiece(0, 2);
            Assert.IsTrue(actual);
            Assert.AreEqual(newGame.boardState.getPiece(0, 2).GetType(), typeof(BondTierSpyPiece));
            Assert.AreEqual(newGame.boardState.getPiece(2, 2), null);
        }

        [TestCase()]
        public void TestBombMove()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(3, 3);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new BombPiece(0);
            newGame.fakePlacePiece(2, 2, pieceToPlace);
            bool actual = newGame.MovePiece(0, 2);
            Assert.IsFalse(actual);
            Assert.AreEqual(newGame.boardState.getPiece(2, 2).GetType(), typeof(BombPiece));
            Assert.AreEqual(newGame.boardState.getPiece(0, 0), null);
        }

        [TestCase()]
        public void TestSpyPieceFalse()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(3, 3);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new SpyPiece(0);
            newGame.fakePlacePiece(2, 2, pieceToPlace);
            bool actual = newGame.MovePiece(0, 2);
            Assert.IsTrue(!actual);
            Assert.AreEqual(newGame.boardState.getPiece(2, 2).GetType(), typeof(SpyPiece));
            Assert.AreEqual(newGame.boardState.getPiece(0, 2), null);
        }

        [TestCase()]
        public void TestSpyPieceLeft()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(3, 3);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new SpyPiece(0);
            newGame.fakePlacePiece(2, 2, pieceToPlace);
            bool actual = newGame.MovePiece(1, 2);
            Assert.IsTrue(actual);
            Assert.AreEqual(newGame.boardState.getPiece(1, 2).GetType(), typeof(SpyPiece));
            Assert.AreEqual(newGame.boardState.getPiece(2, 2), null);
        }

        [TestCase()]
        public void TestSpyPieceUp()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(3, 3);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new SpyPiece(0);
            newGame.fakePlacePiece(2, 2, pieceToPlace);
            bool actual = newGame.MovePiece(2, 1);
            Assert.IsTrue(actual);
            Assert.AreEqual(newGame.boardState.getPiece(2, 1).GetType(), typeof(SpyPiece));
            Assert.AreEqual(newGame.boardState.getPiece(2, 2), null);
        }

        [TestCase()]
        public void TestScoutPieceCollision()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(3, 3);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new ScoutPiece(0);
            GamePiece obstaclePiece = new ObstaclePiece(0);
            newGame.fakePlacePiece(2, 2, pieceToPlace);
            newGame.boardState.setPiece(1, 2, obstaclePiece);
            bool actual = newGame.MovePiece(0, 2);
            Assert.IsTrue(!actual);
            Assert.AreEqual(newGame.boardState.getPiece(2, 2).GetType(), typeof(ScoutPiece));
            Assert.AreEqual(newGame.boardState.getPiece(0, 2), null);
        }

        [TestCase()]
        public void TestScoutPieceUp()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(10, 10);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new ScoutPiece(0);
            newGame.fakePlacePiece(8, 8, pieceToPlace);
            bool actual = newGame.MovePiece(8, 1);
            Assert.IsTrue(actual);
            Assert.AreEqual(newGame.boardState.getPiece(8, 1).GetType(), typeof(ScoutPiece));
            Assert.AreEqual(newGame.boardState.getPiece(8, 8), null);
        }

        [TestCase()]
        public void TestBlockSameTeam()
        {
            System.Diagnostics.Debug.AutoFlush = true;
            Gameboard newBoard = new Gameboard(10, 10);
            FakeStrategoGame newGame = new FakeStrategoGame(newBoard, new FakeGUI());
            GamePiece pieceToPlace = new ScoutPiece(0);
            GamePiece pieceToBlock = new GeneralPiece(0);
            newGame.fakePlacePiece(8, 8, pieceToPlace);
            newGame.fakePlacePiece(8, 3, pieceToBlock);
            bool actual = newGame.MovePiece(8, 1);
            Assert.IsTrue(!actual);
            Assert.AreEqual(newGame.boardState.getPiece(8, 8).GetType(), typeof(ScoutPiece));
            Assert.AreEqual(newGame.boardState.getPiece(8, 1), null);
            Assert.AreEqual(newGame.boardState.getPiece(8, 3).GetType(), typeof(GeneralPiece));
        }
    }
}
