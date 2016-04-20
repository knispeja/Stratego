using NUnit.Framework;
using Stratego;
using Stratego.GamePieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategoTest
{
    [TestFixture()]
    class FactoryTest
    {
        [TestCase()]
        public void TestGetPiecesLeftRed()
        {
            Dictionary<GamePiece, int> placements = new Dictionary<GamePiece, int>(){
                { new FlagPiece(StrategoGame.RED_TEAM_CODE), 1 }, { new BombPiece(StrategoGame.RED_TEAM_CODE), 6 },
                { new SpyPiece(StrategoGame.RED_TEAM_CODE), 1 }, { new ScoutPiece(StrategoGame.RED_TEAM_CODE), 8 },
                { new MinerPiece(StrategoGame.RED_TEAM_CODE), 5 }, { new SergeantPiece(StrategoGame.RED_TEAM_CODE), 4 },
                { new LieutenantPiece(StrategoGame.RED_TEAM_CODE), 4 }, {new CaptainPiece(StrategoGame.RED_TEAM_CODE), 4 },
                { new MajorPiece(StrategoGame.RED_TEAM_CODE), 3}, { new ColonelPiece(StrategoGame.RED_TEAM_CODE), 2},
                { new GeneralPiece(StrategoGame.RED_TEAM_CODE), 1}, { new MarshallPiece(StrategoGame.RED_TEAM_CODE), 1 }
            };

            GamePieceFactory factory = new GamePieceFactory();
            foreach(GamePiece key in placements.Keys){
                Assert.AreEqual(placements[key], factory.getPiecesLeft(key.getPieceName()));
            }
        }

        [TestCase()]
        public void TestGetPiecesLeftBlue()
        {
            Dictionary<GamePiece, int> placements = new Dictionary<GamePiece, int>(){
                { new FlagPiece(StrategoGame.BLUE_TEAM_CODE), 1 }, { new BombPiece(StrategoGame.BLUE_TEAM_CODE), 6 },
                { new SpyPiece(StrategoGame.BLUE_TEAM_CODE), 1 }, { new ScoutPiece(StrategoGame.BLUE_TEAM_CODE), 8 },
                { new MinerPiece(StrategoGame.BLUE_TEAM_CODE), 5 }, { new SergeantPiece(StrategoGame.BLUE_TEAM_CODE), 4 },
                { new LieutenantPiece(StrategoGame.BLUE_TEAM_CODE), 4 }, {new CaptainPiece(StrategoGame.BLUE_TEAM_CODE), 4 },
                { new MajorPiece(StrategoGame.BLUE_TEAM_CODE), 3}, { new ColonelPiece(StrategoGame.BLUE_TEAM_CODE), 2},
                { new GeneralPiece(StrategoGame.BLUE_TEAM_CODE), 1}, { new MarshallPiece(StrategoGame.BLUE_TEAM_CODE), 1 }
            };

            GamePieceFactory factory = new GamePieceFactory();
            foreach (GamePiece key in placements.Keys)
            {
                Assert.AreEqual(placements[key], factory.getPiecesLeft(key.getPieceName()));
            }
        }

        [TestCase()]
        public void TestAddNameForPiece()
        {
            GamePieceFactory factory = new GamePieceFactory();

            String name = BondTierSpyPiece.BOND_NAME;
            Type pieceType = typeof(BondTierSpyPiece);

            Assert.AreEqual(null, factory.getPieceFromName(name));
            factory.addNameForPiece(name, pieceType);
            Assert.AreEqual(pieceType, factory.getPieceFromName(name));
        }

        [TestCase()]
        public void TestAddNumForPiece()
        {
            GamePieceFactory factory = new GamePieceFactory();

            int num = 888;
            Type pieceType = typeof(BondTierSpyPiece);

            Assert.AreEqual(null, factory.getPieceFromInt(num));
            factory.addNumForPiece(num, pieceType);
            Assert.AreEqual(pieceType, factory.getPieceFromInt(num));
        }

        [TestCase()]
        public void TestAddNewPieceToPlacements()
        {
            GamePieceFactory factory = new GamePieceFactory();

            String name = BondTierSpyPiece.BOND_NAME;
            Type pieceType = typeof(BondTierSpyPiece);
            int numAllowed = 3;

            Assert.AreEqual(null, factory.getPieceFromName(name));

            factory.addPieceToPlacements(name, pieceType, numAllowed);
            Assert.AreEqual(pieceType, factory.getPieceFromName(name));
            Assert.AreEqual(numAllowed, factory.getPiecesLeft(name));
            Assert.AreEqual(numAllowed, factory.getAddedPieces()[name]);
            Assert.AreEqual(3, factory.getMinPieces());
        }

        [TestCase()]
        public void TestAddExistingPieceToPlacements()
        {
            GamePieceFactory factory = new GamePieceFactory();
            String name = MarshallPiece.MARSHALL_NAME;
            Type pieceType = typeof(MarshallPiece);
            int numAllowed = 9;

            Assert.AreEqual(1, factory.getPiecesLeft(name));

            factory.addPieceToPlacements(name, pieceType, numAllowed);
            Assert.AreEqual(numAllowed, factory.getPiecesLeft(name));
            Assert.AreEqual(8, factory.getMinPieces());

        }

        [TestCase()]
        public void TestIncrementPiecesLeft()
        {
            GamePieceFactory factory = new GamePieceFactory();
            String name = ScoutPiece.SCOUT_NAME;
            factory.placements[name] = 0;
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(i, factory.getPiecesLeft(name));
                factory.incrementPiecesLeft(name);
            }
        }

        [TestCase()]
        public void TestDecrementPiecesLeft()
        {
            GamePieceFactory factory = new GamePieceFactory();
            String name = ScoutPiece.SCOUT_NAME;
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(8 - i, factory.getPiecesLeft(name));
                factory.decrementPiecesLeft(name);
            }
        }
    }
}
