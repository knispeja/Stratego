using System;
using Stratego;
using NUnit.Framework;
using System.Windows.Forms;
using System.Drawing;

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
            return game.placePiece(piece, x, y);
        }

        [TestCase(1, 123, 254)] //These tests work with a non-square board grid
        [TestCase(1, 246, 508)]
        [TestCase(1, 369, 762)]
        // This tests that pieces can be placed in an empty space on a 1230 x 2540 pixel board.
        public void TestThatPieceIsPlacedIntoEmptySpaceV4(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1240, 2540, new int[10, 10]);
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
            Assert.Throws<ArgumentException>(() => game.placePiece(piece, x, y));
        }

        [TestCase(1, 550, 950)]
        //Tests that the SelectPiece function properly selects a piece
        public void TestThaOrSelectnWorks(int piece, int x, int y)
        {
            StrategoWin game = new StrategoWin(1000, 1000, new int[10, 10]);
            game.placePiece(piece, x, y);
            game.turn = 1;
            Assert.True(game.SelectPiece(x,y).Value);
            Assert.AreEqual(new Point(x/100, y/100), game.pieceSelectedCoords);

            //int[] defaults = game.defaults;

        //    for (int p = 1; p < defaults.Length; p++)
        //    {
        //        for (int num = 0; num < defaults[p]; num++)
        //        {
        //            int piecesLeft = game.getPiecesLeft(p);
        //            game.placePiece(p, x, y);
        //            bool? result = game.placePiece(piece, x, y);

        //            // Make sure remove actually removed the piece
        //            Assert.AreEqual(piece, game.getPiece(x / 200, y / 220));

        //            // Make sure remove is returning true when you try to remove a valid piece
        //            Assert.IsTrue(result.Value);

        //            // Make sure remove is resetting the number of pieces left
        //            Assert.AreEqual(piecesLeft, game.getPiecesLeft(p));
        //        }
        //    }
        }
    }
}
