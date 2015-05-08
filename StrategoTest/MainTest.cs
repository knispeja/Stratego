using System;
using Stratego;
using NUnit.Framework;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace StrategoTest
{
    [TestFixture()]
    class MainTest
    {
        [TestCase(1, 100, 100, Result = false)] //Here we test that the piece value does not 
        [TestCase(2, 100, 100, Result = false)] // affect whether it will be placed.
        [TestCase(3, 100, 100, Result = false)] //Note that these cases are all at the top-left corner of squares
        [TestCase(4, 100, 100, Result = false)]
        [TestCase(5, 100, 100, Result = false)]
        [TestCase(6, 100, 100, Result = false)]
        [TestCase(7, 100, 100, Result = false)]
        [TestCase(8, 100, 100, Result = false)]
        [TestCase(9, 100, 100, Result = false)]
        [TestCase(10, 100, 100, Result = false)]
        [TestCase(11, 100, 100, Result = false)]
        [TestCase(12, 100, 100, Result = false)]
        [TestCase(-1, 100, 100, Result = false)]
        [TestCase(-2, 100, 100, Result = false)]
        [TestCase(-3, 100, 100, Result = false)]
        [TestCase(-4, 100, 100, Result = false)]
        [TestCase(-5, 100, 100, Result = false)]
        [TestCase(-6, 100, 100, Result = false)]
        [TestCase(-7, 100, 100, Result = false)]
        [TestCase(-8, 100, 100, Result = false)]
        [TestCase(-9, 100, 100, Result = false)]
        [TestCase(-10, 100, 100, Result = false)]
        [TestCase(-11, 100, 100, Result = false)]

        [TestCase(-12, 100, 100, Result = false)] //Here we test that the function acts appropriately 
        [TestCase(-12, 200, 200, Result = false)] // for placing in any diagonal space.
        [TestCase(-12, 300, 300, Result = false)]
        [TestCase(-12, 400, 400, Result = false)]

        [TestCase(-12, 101, 101, Result = false)] // Here we test that the function recognizes the  
        [TestCase(-12, 201, 201, Result = false)] // correct square for coordinates not on the top-left corner.
        [TestCase(-12, 301, 301, Result = false)]
        [TestCase(-12, 401, 401, Result = false)]
        // This tests that pieces will not be placed on an obstacle space on a 1000 x 1000 pixel board.
        public bool? TestThatNothingCanBePlacedOnObstacle(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x/100, y/100] = 42;

            StrategoWin game = new StrategoWin(1000, 1000, map);
            game.nextTurn();
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 100, 100)]//Here we test that the piece value does not 
        [TestCase(2, 100, 100)] // affect whether it will be placed.
        [TestCase(3, 100, 100)] //Note that these cases are all at the top-left corner of squares
        [TestCase(4, 100, 100)]
        [TestCase(5, 100, 100)]
        [TestCase(6, 100, 100)]
        [TestCase(7, 100, 100)]
        [TestCase(8, 100, 100)]
        [TestCase(9, 100, 100)]
        [TestCase(10, 100, 100)]
        [TestCase(11, 100, 100)]
        [TestCase(12, 100, 100)]
        [TestCase(-1, 100, 100)]
        [TestCase(-2, 100, 100)]
        [TestCase(-3, 100, 100)]
        [TestCase(-4, 100, 100)]
        [TestCase(-5, 100, 100)]
        [TestCase(-6, 100, 100)]
        [TestCase(-7, 100, 100)]
        [TestCase(-8, 100, 100)]
        [TestCase(-9, 100, 100)]
        [TestCase(-10, 100, 100)]
        [TestCase(-11, 100, 100)]

        [TestCase(-12, 100, 100)]//Here we test that the function acts appropriately 
        [TestCase(-12, 200, 200)]// for placing in any diagonal space.
        [TestCase(-12, 300, 300)]
        [TestCase(-12, 400, 400)]

        [TestCase(-12, 101, 101)]// Here we test that the function recognizes the  
        [TestCase(-12, 201, 201)] // correct square for coordinates not on the top-left corner.
        [TestCase(-12, 301, 301)]
        [TestCase(-12, 401, 401)]
        // This tests that pieces will be placed into an empty space on a 1000 x 1000 pixel board.
        public void TestThatPieceIsPlacedIntoEmptySpace(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.nextTurn();
            if (piece < 0)
                game.nextTurn();
            bool? result = game.placePiece(piece, x, y);
            Assert.AreEqual(game.getPiece(x/100, y/100),piece);
            Assert.IsTrue(result.Value); 
        }

        [TestCase(1, 100, 100, Result = false)]//Here we test that the piece value does not 
        [TestCase(2, 100, 100, Result = false)] // affect whether it will be placed.
        [TestCase(3, 100, 100, Result = false)] //Note that these cases are all at the top-left corner of squares
        [TestCase(4, 100, 100, Result = false)]
        [TestCase(5, 100, 100, Result = false)]
        [TestCase(6, 100, 100, Result = false)]
        [TestCase(7, 100, 100, Result = false)]
        [TestCase(8, 100, 100, Result = false)]
        [TestCase(9, 100, 100, Result = false)]
        [TestCase(10, 100, 100, Result = false)]
        [TestCase(11, 100, 100, Result = false)]
        [TestCase(12, 100, 100, Result = false)]
        [TestCase(-1, 100, 100, Result = false)]
        [TestCase(-2, 100, 100, Result = false)]
        [TestCase(-3, 100, 100, Result = false)]
        [TestCase(-4, 100, 100, Result = false)]
        [TestCase(-5, 100, 100, Result = false)]
        [TestCase(-6, 100, 100, Result = false)]
        [TestCase(-7, 100, 100, Result = false)]
        [TestCase(-8, 100, 100, Result = false)]
        [TestCase(-9, 100, 100, Result = false)]
        [TestCase(-10, 100, 100, Result = false)]
        [TestCase(-11, 100, 100, Result = false)]

        [TestCase(-12, 100, 100, Result = false)] //Here we test that the function acts appropriately 
        [TestCase(-12, 200, 200, Result = false)] // for placing in any diagonal space.
        [TestCase(-12, 300, 300, Result = false)]
        [TestCase(-12, 400, 400, Result = false)]

        [TestCase(-12, 101, 101, Result = false)] // Here we test that the function recognizes the  
        [TestCase(-12, 201, 201, Result = false)] // correct square for coordinates not on the top-left corner.
        [TestCase(-12, 301, 301, Result = false)]
        [TestCase(-12, 401, 401, Result = false)]
        // This tests that pieces will not be placed on an occupied space on a 1000 x 1000 pixel board.
        public bool? TestThatNothingCanBePlacedInFilledSpace(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 100, y / 100] = 1;

            StrategoWin game = new StrategoWin(1000, 1000, map);
            game.nextTurn();
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 200, 200, Result = false)] //Here we test that the function acts appropriately
        [TestCase(1, 400, 400, Result = false)] // for placing in any diagonal space.
        [TestCase(1, 600, 600, Result = false)]
        [TestCase(1, 800, 800, Result = false)]

        [TestCase(1, 201, 201, Result = false)] // Here we test that the function recognizes the
        [TestCase(1, 401, 401, Result = false)] // correct square for coordinates not on the top-left corner
        [TestCase(1, 601, 601, Result = false)]
        [TestCase(1, 801, 801, Result = false)]

        [TestCase(1, 200, 201, Result = false)] //Coordinates on top edge of square
        [TestCase(1, 400, 401, Result = false)]
        [TestCase(1, 600, 601, Result = false)]
        [TestCase(1, 800, 801, Result = false)]

        [TestCase(1, 201, 200, Result = false)] //Coordinates on left edge of square
        [TestCase(1, 401, 400, Result = false)]
        [TestCase(1, 601, 600, Result = false)]
        [TestCase(1, 801, 800, Result = false)]

        [TestCase(1, 0, 201, Result = false)] //Left edge of grid
        [TestCase(1, 0, 401, Result = false)]
        [TestCase(1, 0, 601, Result = false)]
        [TestCase(1, 0, 801, Result = false)]
        // This tests that pieces will not be placed on an obstacle space on a 2000 x 2000 pixel board.
        public bool? TestThatNothingCanBePlacedOnObstacleV2(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 200, y / 200] = 42;

            StrategoWin game = new StrategoWin(2000, 2000, map);
            game.nextTurn();
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 200, 200, Result = false)] //Here we test that the function acts appropriately
        [TestCase(1, 400, 400, Result = false)] // for placing in any diagonal space.
        [TestCase(1, 600, 600, Result = false)]
        [TestCase(1, 800, 800, Result = false)]

        [TestCase(1, 201, 201, Result = false)]// Here we test that the function recognizes the
        [TestCase(1, 401, 401, Result = false)] // correct square for coordinates not on the top-left corner
        [TestCase(1, 601, 601, Result = false)]
        [TestCase(1, 801, 801, Result = false)]

        [TestCase(1, 200, 201, Result = false)]//Coordinates on top edge of square
        [TestCase(1, 400, 401, Result = false)]
        [TestCase(1, 600, 601, Result = false)]
        [TestCase(1, 800, 801, Result = false)]

        [TestCase(1, 201, 200, Result = false)]//Coordinates on left edge of square
        [TestCase(1, 401, 400, Result = false)]
        [TestCase(1, 601, 600, Result = false)]
        [TestCase(1, 801, 800, Result = false)]

        [TestCase(1, 0, 201, Result = false)] //Left edge of grid
        [TestCase(1, 0, 401, Result = false)]
        [TestCase(1, 0, 601, Result = false)]
        [TestCase(1, 0, 801, Result = false)]
        // This tests that pieces will not be placed on an occupied space on a 2000 x 2000 pixel board.
        public bool? TestThatNothingCanBePlacedInFilledSpaceV2(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 200, y / 200] = 1;

            StrategoWin game = new StrategoWin(2000, 2000, map);
            game.nextTurn();
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 200, 200)]//Here we test that the function acts appropriately
        [TestCase(1, 400, 400)] // for placing in any diagonal space.
        [TestCase(1, 600, 600)]
        [TestCase(1, 800, 800)]

        [TestCase(1, 201, 201)]// Here we test that the function recognizes the
        [TestCase(1, 401, 401)] // correct square for coordinates not on the top-left corner
        [TestCase(1, 601, 601)]
        [TestCase(1, 801, 801)]

        [TestCase(1, 200, 201)]//Coordinates on top edge of square
        [TestCase(1, 400, 401)]
        [TestCase(1, 600, 601)]
        [TestCase(1, 800, 801)]

        [TestCase(1, 201, 200)]//Coordinates on left edge of square
        [TestCase(1, 401, 400)]
        [TestCase(1, 601, 600)]
        [TestCase(1, 801, 800)]

        [TestCase(1, 0, 200)] //Left edge of grid
        [TestCase(1, 0, 400)]
        [TestCase(1, 0, 600)]
        [TestCase(1, 0, 800)]
        // This tests that pieces can be placed on an empty space on a 2000 x 2000 pixel board.
        public void TestThatPieceIsPlacedIntoEmptySpaceV2(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(2000, 2000, new int[10, 10]);
            game.nextTurn();
            bool? result = game.placePiece(piece, x, y);
            Assert.AreEqual(game.getPiece(x / 200, y / 200), piece);
            Assert.IsTrue(result.Value);
        }

        [TestCase(1, 200, 100)] //These tests work with a non-square board grid
        [TestCase(1, 200, 200)]
        [TestCase(1, 400, 200)]
        [TestCase(1, 600, 300)]
        [TestCase(1, 0, 200)]
        // This tests that pieces can be placed in an empty space on a 2000 x 1000 pixel board.
        public void TestThatPieceIsPlacedIntoEmptySpaceV3(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(2000, 1000, new int[10, 10]);
            game.nextTurn();
            bool? result = game.placePiece(piece, x, y);
            Assert.AreEqual(game.getPiece(x / 200, y / 100), piece);
            Assert.IsTrue(result.Value);
        }

        [TestCase(1, 200, 100, Result = false)] //These tests work with a non-square board grid
        [TestCase(1, 200, 200, Result = false)]
        [TestCase(1, 400, 200, Result = false)]
        [TestCase(1, 600, 300, Result = false)]
        [TestCase(1, 0, 200, Result = false)]
        // This tests that pieces will not be placed on an occupied space on a 2000 x 1000 pixel board.
        public bool? TestThatNothingCanBePlacedInFilledSpaceV3(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 200, y / 100] = 1;

            StrategoWin game = new StrategoWin(2000, 1000, map);
            game.nextTurn();
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 200, 100, Result = false)] //These tests work with a non-square board grid
        [TestCase(1, 200, 200, Result = false)]
        [TestCase(1, 400, 200, Result = false)]
        [TestCase(1, 600, 300, Result = false)]
        [TestCase(1, 0, 200, Result = false)]
        // This tests that pieces will not be placed on an obstacle space on a 2000 x 1000 pixel board.
        public bool? TestThatNothingCanBePlacedOnObstacleV3(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 200, y / 100] = 42;

            StrategoWin game = new StrategoWin(2000, 1000, map);
            game.nextTurn();
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 123, 254, Result = false)] //These tests work with a non-square board grid 
        [TestCase(1, 246, 508, Result = false)]
        [TestCase(1, 369, 762, Result = false)]
        // This tests that pieces will not be placed on an obstacle space on a 1230 x 2540 pixel board.
        public bool? TestThatNothingCanBePlacedOnObstacleV4(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 123, y / 254] = 42;

            StrategoWin game = new StrategoWin(1230, 2540, map);
            game.nextTurn();
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 123, 254, Result = false)] //These tests work with a non-square board grid
        [TestCase(1, 246, 508, Result = false)]
        [TestCase(1, 369, 762, Result = false)]
        // This tests that pieces will not be placed on an occupied space on a 1230 x 2540 pixel board.
        public bool? TestThatNothingCanBePlacedInFilledSpaceV4(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 123, y / 254] = 42;

            StrategoWin game = new StrategoWin(1230, 2540, map);
            game.nextTurn();
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 123, 254)] //These tests work with a non-square board grid
        [TestCase(1, 246, 508)]
        [TestCase(1, 369, 762)]
        // This tests that pieces can be placed in an empty space on a 1230 x 2540 pixel board.
        public void TestThatPieceIsPlacedIntoEmptySpaceV4(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1240, 2540, new int[10, 10]);
            game.nextTurn();
            bool? result = game.placePiece(piece, x, y);
            Assert.AreEqual(game.getPiece(x / 124, y / 254), piece);
            Assert.IsTrue(result.Value);
        }

        [TestCase(1, 123, 254)]
        [TestCase(2, 246, 508)]
        [TestCase(3, 369, 762)]
        // This tests that pieces are actually removed from the proper array after single placements
        public void TestThatPieceReducesNumberofPiecesLeft(int piece, int x, int y)
        {
            int[] defaults = new int[13] { 0, 1, 1, 2, 3, 4, 4, 4, 5, 8, 1, 6, 1 };
            StrategoWin game = new StrategoWin(1230, 2540, new int[10, 10]);
            game.nextTurn();
            bool? result = game.placePiece(piece, x, y);
            Assert.AreEqual(defaults[Math.Abs(piece)]-1, game.getPiecesLeft(Math.Abs(piece)));
            Assert.IsTrue(result.Value);
        }

        [TestCase(1, 50, 50)]
        [TestCase(2, 150, 50)]
        [TestCase(3, 250, 50)]
        public void TestThatPiecesCantBePlacedIfArrayIsEmpty(int piece, int x, int y)
        {
            //int[] defaults = new int[13] { 0, 1, 1, 2, 3, 4, 4, 4, 5, 8, 1, 6, 1 };
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.nextTurn();
            int[] defaults = game.defaults;
            bool? result = true;
            for (int i = 0; i <= defaults[Math.Abs(piece)]; i++)
            {
                result = game.placePiece(piece, x, y+100*i);
            }
            Assert.IsFalse(result.Value);
            Assert.AreEqual(0, game.getPiecesLeft(Math.Abs(piece)));
        }

        [TestCase(0, 123, 254)]
        [TestCase(0, 246, 508)]
        [TestCase(0, 369, 762)]
        [TestCase(0, 220, 900)]
        [TestCase(0, 500, 750)]
        // This tests that you can remove pieces with placePiece(0, ...) as intended
        public void TestThatRemoveActuallyRemovesPiecesAndUpdatesPiecesLeft(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(2000, 2200, new int[10, 10]);
            game.nextTurn();
            if (piece < 0)
                game.nextTurn();
            int[] defaults = game.defaults;
            
            for(int p=1; p<defaults.Length; p++)
            {
                for(int num=0; num<defaults[p]; num++)
                {
                    int piecesLeft = game.getPiecesLeft(p);
                    game.placePiece(p, x, y);
                    bool? result = game.placePiece(piece, x, y);

                    // Make sure remove actually removed the piece
                    Assert.AreEqual(piece, game.getPiece(x / 200, y / 220));

                    // Make sure remove is returning true when you try to remove a valid piece
                    Assert.IsTrue(result.Value);

                    // Make sure remove is resetting the number of pieces left
                    Assert.AreEqual(piecesLeft, game.getPiecesLeft(p));
                }
            }
        }
        [TestCase(0, 123, 254)]
        [TestCase(0, 246, 508)]
        [TestCase(0, 369, 762)]
        [TestCase(0, 220, 900)]
        [TestCase(0, 500, 750)]
        //This tests that you cannot remove (place 0) on a space that is empty
        public void TestThatPieceCannotBeRemovedFromEmpty(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(2000, 2200, new int[10, 10]);
            game.nextTurn();
            bool? result = game.placePiece(piece, x, y);
            Assert.IsFalse(result.Value);
        }

        [TestCase(0, 123, 254)]
        [TestCase(0, 246, 508)]
        [TestCase(0, 369, 762)]
        [TestCase(0, 220, 900)]
        [TestCase(0, 500, 750)]
        //This tests that you cannot remove (place 0) on a space that is an obstacle
        public void TestThatObstacleCannotBeRemoved(int piece, int x, int y)
        {
            int[,] map = new int[10, 10];
            map[x / 200, y / 220] = 42;
            StrategoWin game = new StrategoWin(2000, 2200, map);
            game.nextTurn();
            bool? result = game.placePiece(piece, x, y);
            Assert.IsFalse(result.Value);
        }

        [TestCase(13, 200, 200)]
        [TestCase(-13, 200, 200)]
        [TestCase(42, 200, 200)]
        [TestCase(-42, 200, 200)]
        [TestCase(43, 200, 200)]
        [TestCase(-43, 200, 200)]
        [TestCase(8, -1, 200)]
        [TestCase(-8, 200, -1)]
        [TestCase(8, 2001, 200)]
        [TestCase(-8, 200, 2201)]
        //Tests that "piece" must be a valid piece and that "x" and "y" cannot be outside the bounds of the window
        public void TestThatPlacePieceThrowsException(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(2000, 2200, new int[10, 10]);
            game.nextTurn();
            if (piece < 0)
                game.nextTurn();
            Assert.Throws<ArgumentException>(() => game.placePiece(piece, x, y));
        }

        [TestCase(1, 650, 950)]
        [TestCase(1, 550, 950)]
        [TestCase(-1, 650, 950)]
        [TestCase(-1, 550, 950)]
        //Tests that the SelectPiece function properly selects a piece
        public void TestThatSelectWorks(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.nextTurn();
            if (piece < 0)
                game.nextTurn();
            game.placePiece(piece, x, y);
            Assert.True(game.SelectPiece(x, y).Value);
            Assert.AreEqual(new Point(x/100, y/100), game.pieceSelectedCoords);
            Assert.AreEqual(piece, game.boardState[game.pieceSelectedCoords.X, game.pieceSelectedCoords.Y]);
            Assert.AreEqual(false, game.SelectPiece(x, y).Value);
        }

        [TestCase(11, 650, 950)]
        [TestCase(12, 650, 950)]
        [TestCase(-11, 650, 950)]
        [TestCase(-12, 650, 950)]
        public void TestThatSelectWorksForBombsAndFlags(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.nextTurn();
            if (piece < 0)
                game.nextTurn();
            game.placePiece(piece, x, y);
            Assert.False(game.SelectPiece(x, y).Value);
        }

        [TestCase(1, 650, 950)]
        [TestCase(2, 650, 950)]
        [TestCase(9, 650, 950)]
        [TestCase(-10, 650, 950)]
        [TestCase(-9, 650, 950)]
        //Tests that the MovePiece function properly moves pieces up down left right, no special cases.
        //Except for the case where you try to move and nothing is selected.
        public void TestThatMovePieceWorksInsideMap(int piece, int x, int y)
        {
            int turn = 0; //Note these lines of code don't really do anything right now
            if (piece > 0) //Someone else is going to make some other test thing that will test for it
                turn = 1;
            else if (piece < 0)
                turn = -1;
            else Assert.Fail("Invalid piece argument for this test");
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.turn = turn;
            game.preGameActive = true;
            game.placePiece(piece, x, y);
            game.preGameActive = false;
            Assert.True(game.SelectPiece(x, y).Value);
            y -= 100;
            Assert.True(game.MovePiece(x,y));
            Assert.AreEqual(piece, game.boardState[x / 100, y / 100]);
            Assert.False(game.pieceIsSelected);
            Assert.AreEqual(0, game.boardState[x / 100, (y + 100) / 100]);
            game.nextTurn();

            game.SelectPiece(x, y);
            x -= 100;
            Assert.True(game.MovePiece(x, y));
            Assert.AreEqual(piece, game.boardState[x / 100, y / 100]);
            Assert.False(game.pieceIsSelected);
            Assert.AreEqual(0, game.boardState[(x + 100) / 100, y / 100]);
            game.nextTurn();

            Assert.True(game.SelectPiece(x, y).Value);
            y += 100;
            Assert.True(game.MovePiece(x, y));
            Assert.AreEqual(piece, game.boardState[x / 100, y / 100]);
            Assert.False(game.pieceIsSelected);
            Assert.AreEqual(0, game.boardState[x / 100, (y - 100) / 100]);
            game.nextTurn();

            x += 100;
            Assert.False(game.MovePiece(x, y));
            Assert.AreEqual(piece, game.boardState[(x - 100) / 100, y / 100]);
            Assert.False(game.pieceIsSelected);
            x -= 100;
            game.nextTurn();

            game = new StrategoWin(2000, 2000, new int[10, 10]);
            game.turn = turn;
            game.preGameActive = true;
            game.placePiece(piece, x, y);
            game.preGameActive = false;

            Assert.True(game.SelectPiece(x, y).Value);
            y -= 200;
            Assert.True(game.MovePiece(x, y));
            Assert.AreEqual(piece, game.boardState[x / 200, y / 200]);
            Assert.False(game.pieceIsSelected);
            Assert.AreEqual(0, game.boardState[x / 200, (y + 200) / 200]);
        }

        [TestCase(1, 650, 950)]
        [TestCase(9, 650, 950)]
        [TestCase(-2, 650, 950)]
        [TestCase(-9, 650, 950)]
        public void TestThatMovePieceCantMoveOnAlliedPiece(int piece, int x, int y)
        {
            int turn = 0; //Note these lines of code don't really do anything right now
            if (piece > 0) //Someone else is going to make some other test thing that will test for it
                turn = 1;
            else if (piece < 0)
                turn = -1;
            else Assert.Fail("Invalid piece argument for this test");

            int secondaryPiece = turn * 5;
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.preGameActive = true;
            game.turn = turn;
            game.placePiece(piece, x, y);
            game.placePiece(secondaryPiece, x + 100, y);
            game.preGameActive = false;
            Assert.True(game.SelectPiece(x, y).Value);
            x += 100;
            Assert.False(game.MovePiece(x, y));
            Assert.AreEqual(secondaryPiece, game.boardState[x / 100, y / 100]);
            Assert.False(game.pieceIsSelected);
            Assert.AreEqual(piece, game.boardState[(x - 100) / 100, y / 100]);
        }

        [TestCase(1, 650, 950)]
        [TestCase(2, 650, 950)]
        [TestCase(9, 650, 950)]
        [TestCase(-3, 650, 950)]
        [TestCase(-9, 650, 950)]
        //Test to make sure that a piece can only move one space, unless that piece is a Scout (9)
        public void TestThatMovePieceCanOnlyMoveOne(int piece, int x, int y)
        {
            int turn = 0;
            if (piece > 0)
                turn = 1;
            else if (piece < 0)
                turn = -1;
            else Assert.Fail("Invalid piece argument for this test");

            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.turn = turn;
            game.preGameActive = true;
            game.placePiece(piece, x, y);
            game.preGameActive = false;
            Assert.True(game.SelectPiece(x, y).Value);
            x += 200;
            if (Math.Abs(piece) != 9)
            {
                Assert.False(game.MovePiece(x, y));
                Assert.False(game.pieceIsSelected);
                Assert.AreEqual(piece, game.boardState[(x - 200) / 100, y / 100]);
                Assert.AreEqual(0, game.boardState[x / 100, y / 100]);
            }
            else
            {
                Assert.True(game.MovePiece(x, y));
                Assert.False(game.pieceIsSelected);
                Assert.AreEqual(0, game.boardState[(x - 200) / 100, y / 100]);
                Assert.AreEqual(9*turn, game.boardState[x / 100, y / 100]);
            }
        }

        [TestCase(1, 650, 950)]
        [TestCase(2, 650, 950)]
        [TestCase(9, 650, 950)]
        [TestCase(-1, 650, 950)]
        [TestCase(-2, 650, 950)]
        [TestCase(-9, 650, 950)]
        //Test to make sure that a piece can only move up/down/left/right, not diagonal
        public void TestThatMovePieceCantMoveDiagonal(int piece, int x, int y)
        {
            int turn = 0; //Note these lines of code don't really do anything right now
            if (piece > 0) //Someone else is going to make some other test thing that will test for it
                turn = 1;
            else if (piece < 0)
                turn = -1;
            else Assert.Fail("Invalid piece argument for this test");

            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.turn = turn;
            game.preGameActive = true;
            game.placePiece(piece, x, y);
            game.preGameActive = false;
            Assert.True(game.SelectPiece(x, y).Value);
            x += 100;
            y -= 100;

            Assert.False(game.MovePiece(x, y));
            Assert.AreEqual(piece, game.boardState[(x-100) / 100, (y+100) / 100]);
            Assert.False(game.pieceIsSelected);
            Assert.AreEqual(0, game.boardState[x / 100, y / 100]);
        }

        [TestCase(1, 650, 950)]
        [TestCase(-5, 650, 950)]
        [TestCase(8, 650, 950)]
        [TestCase(-3, 650, 950)]
        [TestCase(-4, 650, 950)]
        //Test to make sure that a piece gets resolved
        public void TestThatMovePieceResolvesCombat(int piece, int x, int y)
        {
            int turn = 0; //Note these lines of code don't really do anything right now
            if (piece > 0) //Someone else is going to make some other test thing that will test for it
                turn = 1;
            else if (piece < 0)
                turn = -1;
            else Assert.Fail("Invalid piece argument for this test");

            int secondaryPiece = turn * -5;
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.turn = turn;
            game.preGameActive = true;
            game.placePiece(piece, x, y);
            game.turn = -turn;
            game.placePiece(secondaryPiece, x + 100, y);
            game.turn = turn;
            game.preGameActive = false;
            Assert.True(game.SelectPiece(x, y).Value);
            x += 100;
            Assert.True(game.MovePiece(x, y));
            Assert.AreEqual(Piece.attack(piece, secondaryPiece), game.boardState[x / 100, y / 100]);
            Assert.False(game.pieceIsSelected);
        }

        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        public void TestThatNextTurnChangesTurn(int initialTurn, int expectedNewTurn)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.turn = initialTurn;
            game.nextTurn();
            Assert.AreEqual(expectedNewTurn, game.turn);     
        }
       
        [TestCase(0, false, true)]
        [TestCase(-1, true, false)]
        [TestCase(-1, false, false)]
        [TestCase(1, true, true)]
        public void TestThatNextTurnChangesPreGame(int initialTurn, bool initialbool, bool expectedFinal)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.turn = initialTurn;
            game.preGameActive = initialbool;
            game.placePiece(3, 4, 4);
            game.nextTurn();
            if (initialTurn == 1 && initialbool == true) Assert.AreEqual(game.getPiecesLeft(3), game.defaults[3]);
            Assert.AreEqual(expectedFinal, game.preGameActive);
        }

        
        [TestCase(-1, -2, true)]
        [TestCase(-1, -3, true)]
        [TestCase(-1, -4, true)]
        [TestCase(-1, -5, true)]
        [TestCase(-1, -6, true)]
        [TestCase(1, -4, false)]
        [TestCase(1, -5, false)]
        [TestCase(1, -6, false)]
        [TestCase(-1, 2, false)]
        [TestCase(-1, 3, false)]
        [TestCase(-1, 6, false)]
        public void TestSelectPieceTakesTurnIntoAccount(int initialTurn, int piece, bool expectedResult)
        {
            int[,] gameState = new int[10, 10];
            gameState[0, 0] = piece;
            StrategoWin game = new StrategoWin(1000, 1000, gameState);
            game.turn = initialTurn;
            game.preGameActive = false;
            Assert.AreEqual(expectedResult, game.SelectPiece(0, 0));
        }

        
        [TestCase(1, -5, 200, 150, false)]
        [TestCase(1, -3, 200, 250, false)]
        [TestCase(1, -4, 300, 150, false)]
        [TestCase(1, -10, 800, 25, false)]
        [TestCase(-1, -5, 200, 150, true)]
        [TestCase(-1, -3, 200, 250, true)]
        [TestCase(-1, -4, 300, 150, true)]
        [TestCase(-1, -10, 800, 25, true)]
        [TestCase(-1, 5, 200, 150, false)]
        [TestCase(-1, 3, 200, 250, false)]
        [TestCase(-1, 4, 300, 150, false)]
        [TestCase(-1, 10, 800, 25, false)]
        public void TestPlacePieceTakesTurnIntoAccount(int initialTurn, int piece, int x, int y, bool expectedResult)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.turn = initialTurn;
            game.preGameActive = true;
            Assert.AreEqual(expectedResult, game.placePiece(piece, x, y));
        }

        [TestCase(-1, 0, 0)]
        [TestCase(1, 0, 0)]
        public void TestPlacePieceCantRemoveEnemyPieces(int initialTurn, int x, int y)
        {
            int[,] gameBoard = new int[10, 10];
            gameBoard[x, y] = -initialTurn * 4;
            StrategoWin game = new StrategoWin(1000, 1000, gameBoard);
            game.turn = initialTurn;
            game.preGameActive = true;
            Assert.IsFalse(game.placePiece(0, x * 100, y * 100) == true);
            Assert.IsFalse(game.getPiece(0, 0) == 0);
        }

        [TestCase(-5, 1)]
        [TestCase(42, 6)]
        [TestCase(0, 3)]
        public void TestFillRow(int value, int row)
        {
            int width;
            if (value > 0) width = 7;
            else width = 10;
            int[,] gameBoard = new int[width, 10];
            StrategoWin game = new StrategoWin(1000, 1000, gameBoard);
            game.fillRow(value, row);
            for (int x = 0; x < width; x++) Assert.AreEqual(value, game.boardState[x, row]);
        }

        [Test()]
        public void TestSaveGame()
        {
            StringWriter writer = new StringWriter();
            string result = "1 0\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n";
            int[,] gameBoard = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameBoard[j, i] = j;
                }
            }

            StrategoWin game = new StrategoWin(1000, 1000, gameBoard);
            game.turn = 1;

            Assert.IsTrue(game.saveGame(writer));
            Assert.AreEqual(result, writer.ToString());

            result = "1 1\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n";
            game.isSinglePlayer = true;
            writer = new StringWriter();
            Assert.IsTrue(game.saveGame(writer));
            Assert.AreEqual(result, writer.ToString());

            game.nextTurn();

            result = "-1 0\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n";
            game.isSinglePlayer = false;
            writer = new StringWriter();
            Assert.IsTrue(game.saveGame(writer));
            Assert.AreEqual(result, writer.ToString());

            result = "-1 1\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n";
            game.isSinglePlayer = true;
            writer = new StringWriter();
            Assert.IsTrue(game.saveGame(writer));
            Assert.AreEqual(result, writer.ToString());
            writer.Close();
        }

        [Test()]
        public void TestSaveGameV2()
        {
            StringWriter writer = new StringWriter();
            string result = "1 0\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n";
            int[,] gameBoard = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                gameBoard[0, i] = 42;
                gameBoard[1, i] = 0;
                gameBoard[2, i] = 0;
                gameBoard[3, i] = 1;
                gameBoard[4, i] = 1;
                gameBoard[5, i] = 9;
                gameBoard[6, i] = 8;
                gameBoard[7, i] = 3;
                gameBoard[8, i] = 4;
                gameBoard[9, i] = 2;
            }

            StrategoWin game = new StrategoWin(1000, 1000, gameBoard);

            game.turn = 1;

            Assert.IsTrue(game.saveGame(writer));
            Assert.AreEqual(result, writer.ToString());

            result = "1 1\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n";
            game.isSinglePlayer = true;
            writer = new StringWriter();
            Assert.IsTrue(game.saveGame(writer));
            Assert.AreEqual(result, writer.ToString());

            game.nextTurn();

            result = "-1 0\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n";
            game.isSinglePlayer = false;
            writer = new StringWriter();
            Assert.IsTrue(game.saveGame(writer));
            Assert.AreEqual(result, writer.ToString());

            result = "-1 1\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n";
            game.isSinglePlayer = true;
            writer = new StringWriter();
            Assert.IsTrue(game.saveGame(writer));
            Assert.AreEqual(result, writer.ToString());
            writer.Close();
        }
        [Test()]
        public void TestThatGameCannotBeSavedBeforePiecesArePlaced()
        {
            StringWriter writer = new StringWriter();
            int[,] gameBoard = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                gameBoard[i, 0] = 42;
                gameBoard[i, 1] = 0;
                gameBoard[i, 2] = 0;
                gameBoard[i, 3] = 1;
                gameBoard[i, 4] = 1;
                gameBoard[i, 5] = 9;
                gameBoard[i, 6] = 8;
                gameBoard[i, 7] = 3;
                gameBoard[i, 8] = 4;
                gameBoard[i, 9] = 2;
            }
            StrategoWin game = new StrategoWin(1000, 1000, gameBoard);

            Assert.IsFalse(game.saveGame(writer));
            Assert.AreEqual("", writer.ToString());

            game.nextTurn();

            Assert.IsFalse(game.saveGame(writer));
            Assert.AreEqual("", writer.ToString());

        }
        [Test()]
        public void TestLoadGame()
        {
            string input = "1 0\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n";
            int[,] gameBoard = new int[,] { {42,42,42,42,42,42,42,42,42,42}, { 0,0,0,0,0,0,0,0,0,0},
                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, { 1,1,1,1,1,1,1,1,1,1 },
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, { 9,9,9,9,9,9,9,9,9,9 },
                                            { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8}, { 3,3,3,3,3,3,3,3,3,3 },
                                            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4}, { 2,2,2,2,2,2,2,2,2,2}};

            StringReader reader = new StringReader(input);
            StrategoWin game = new StrategoWin(1000, 1000, new int[10,10]);

            Assert.IsTrue(game.loadGame(reader));

            Assert.AreEqual(game.turn, 1);
            Assert.IsFalse(game.isSinglePlayer);
            Assert.AreEqual(gameBoard, game.boardState);

            reader.Close();
        }

        [Test()]
        public void TestLoadGameV2()
        {
            string input = "-1 1\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n0 1 2 3 4 5 6 7 8 9\r\n";
            int[,] gameBoard = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameBoard[j, i] = j;
                }
            }

            StringReader reader = new StringReader(input);
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);

            Assert.IsTrue(game.loadGame(reader));

            Assert.AreEqual(game.turn, -1);
            Assert.IsTrue(game.isSinglePlayer);
            Assert.AreEqual(gameBoard, game.boardState);
            reader.Close();
        }

        [Test()]
        //Tests that game initializes with the lastFought at the point -1, -1
        public void TestThatLastFoughtInitializes()
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            Assert.AreEqual(-1, game.lastFought.X);
            Assert.AreEqual(-1, game.lastFought.Y);
        }

        [Test()]
        //Tests that moving a piece to attack another changes lastFought
        public void TestThatMovePieceChangesLastFought()
        {
            int[,] gameboard = new int[10, 10];
            gameboard[5, 5] = 3;
            gameboard[6, 5] = -7;
            gameboard[7, 7] = -3;
            gameboard[7 , 6] = 7;
            StrategoWin game = new StrategoWin(1000, 1000, gameboard);
            game.turn = -1;
            game.SelectPiece(600, 500);
            game.MovePiece(500, 500);
            Assert.AreEqual(new Point(5, 5), game.lastFought);

            game.SelectPiece(700, 600);
            game.MovePiece(700, 700);
            Assert.AreEqual(new Point(7,7), game.lastFought);
           
        }
        [Test()]
        //Tests that invalid moves don't change LastFought
        public void TestThatInvalidMoveDoesNotChangeLastFought()
        {
            int[,] gameboard = new int[10, 10];
            gameboard[5, 5] = -3;
            gameboard[6, 5] = -7;
            gameboard[7, 7] = 3;
            gameboard[7, 6] = 7;
            StrategoWin game = new StrategoWin(1000, 1000, gameboard);
            game.turn = -1;
            game.SelectPiece(600, 500);
            game.MovePiece(500, 500);
            Assert.AreEqual(new Point(-1, -1), game.lastFought);

            game.turn = 1;
            game.SelectPiece(700, 600);
            game.MovePiece(700, 700);
            Assert.AreEqual(new Point(-1, -1), game.lastFought);

        }
        [Test()]
        //Tests that moving into empty spaces resets lastFought
        public void TestThatMovementIntoEmptySpaceResetsLastFought()
        {
            int[,] gameboard = new int[10, 10];
            gameboard[5, 5] = 3;
            gameboard[6, 5] = -7;
            gameboard[7, 6] = 7;
            StrategoWin game = new StrategoWin(1000, 1000, gameboard);
            game.turn = -1;
            game.SelectPiece(600, 500);
            game.MovePiece(500, 500);

            game.turn = 1;
            game.SelectPiece(700, 600);
            game.MovePiece(700, 700);
            Assert.AreEqual(new Point(-1, -1), game.lastFought);

        }

        [Test()]
        //Tests that attacking a piece of the same number resets lastFought (because both were killed)
        public void TestThatTiesResetLastFought()
        {
            int[,] gameboard = new int[10, 10];
            gameboard[5, 5] = -3;
            gameboard[6, 5] = -7;
            gameboard[7, 6] = 7;
            gameboard[7, 7] = -7;
            StrategoWin game = new StrategoWin(1000, 1000, gameboard);
            game.turn = -1;
            game.SelectPiece(600, 500);
            game.MovePiece(500, 500);

            game.turn = 1;
            game.SelectPiece(700, 600);
            game.MovePiece(700, 700);
            Assert.AreEqual(new Point(-1, -1), game.lastFought);

        }

        //static int[,] board = new int[,] { 
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
        //static int[,] moveBoard = new int[,] { 
        //    {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 1, 0, 1, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
        //static int[,] board = new int[,] { 
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //  {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
        //  {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
        [Test()]
        public void TestGetPieceMovesFourDirectionsBaseCase()
        {
            int piece = 4;
            int x = 4;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            Assert.AreEqual(moveArray, game.GetPieceMoves(y, x));

        }

        [Test()]
        public void TestGetPieceMovesFourDirectionsBaseCaseTwo()
        {
            int piece = 5;
            int x = 5;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 1, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 5, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            Assert.AreEqual(moveArray, game.GetPieceMoves(y, x));

        }

        [Test()]
        public void TestGetPieceMovesFourDirectionsNine()
        {
            int piece = 9;
            int x = 5;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 9, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            Assert.AreEqual(moveArray, game.GetPieceMoves(y, x));

        }

        [Test()]
        public void TestGetPieceMovesFourDirectionsNineWater()
        {
            int piece = 9;
            int x = 6;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {1, 1, 1, 1, 1, 1, 0, 1, 1, 1},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 9, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);

        }

        [Test()]
        public void TestGetPieceMovesFourDirectionsNineNextToEnemy()
        {
            int piece = 9;
            int x = 6;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, -8, 9, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);

        }

        [Test()]
        public void TestGetPieceMovesFourDirectionsNineNextToAlly()
        {
            int piece = 9;
            int x = 6;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 7, 9, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);

        }

        [Test()]
        public void TestGetPieceMovesFourDirectionsNineFarFromEnemy()
        {
            int piece = 9;
            int x = 6;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 1, 1, 0, 1, 1, 1},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, -8, 0, 0, 9, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);

        }

        [Test()]
        public void TestGetPieceMovesFourDirectionsnegativeNineFarFromEnemy()
        {
            int piece = -9;
            int x = 6;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 1, 1, 0, 1, 1, 1},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 8, 0, 0, -9, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);
        }

        [Test()]
        public void TestGetPieceMovesBombCantMove()
        {
            int piece = 11;
            int x = 6;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 11, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);
        }

        [Test()]
        public void TestGetPieceMovesFlagCantMove()
        {
            int piece = 12;
            int x = 6;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 12, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);
        }

        [Test()]
        public void TestGetPieceMoves42CantMove()
        {
            int piece = 42;
            int x = 6;
            int y = 4;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);
        }

        [Test()]
        public void TestGetPieceMoves0CantMoveCorner()
        {
            int piece = 0;
            int x = 9;
            int y = 9;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);
        }

        [Test()]
        public void TestGetPieceMovesSurroundedCantMove()
        {
            int piece = 0;
            int x = 4;
            int y = 1;
            int[,] moveArray = new int[,] { 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            int[,] boardstate = new int[,] { 
            {0, 0, 0, 0, 5, 0, 0, 0, 0, 0},
            {0, 0, 0, 5, 5, 5, 0, 0, 0, 0},
            {0, 0, 0, 0, 8, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
          {0, 0, 42, 42, 0, 0, 42, 42, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };
            StrategoWin game = new StrategoWin(1000, 1000, boardstate);
            int[,] expected = game.GetPieceMoves(y, x);
            Assert.AreEqual(moveArray, expected);
        }
        [Test()]
        public void TestSaveSetUp()
        {
            int[,] gameBoard = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                gameBoard[0, i] = 42;
                gameBoard[1, i] = 0;
                gameBoard[2, i] = 0;
                gameBoard[3, i] = 1;
                gameBoard[4, i] = 1;
                gameBoard[5, i] = 9;
                gameBoard[6, i] = 8;
                gameBoard[7, i] = 3;
                gameBoard[8, i] = 4;
                gameBoard[9, i] = 2;
            }
            StrategoWin game = new StrategoWin(1000, 1000, gameBoard);
            StringWriter writer = new StringWriter();
            String expected = "42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n42 0 0 1 1 9 8 3 4 2\r\n";

            game.nextTurn();
            Assert.IsTrue(game.saveSetUp(writer));
            Assert.AreEqual(expected, writer.ToString());

        }
    }


}
