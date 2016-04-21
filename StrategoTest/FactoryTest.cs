using NUnit.Framework;
using Stratego;
using Stratego.GamePieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        [TestCase()]
        public void TestResetPlacementsDefault()
        {
            GamePieceFactory factory = new GamePieceFactory();
            Dictionary<String, int> defaults = new Dictionary<String, int>(){
                { FlagPiece.FLAG_NAME, 1 }, { BombPiece.BOMB_NAME, 6 }, { SpyPiece.SPY_NAME, 1 },
                { ScoutPiece.SCOUT_NAME, 8 }, { MinerPiece.MINER_NAME, 5 }, { SergeantPiece.SERGEANT_NAME, 4 },
                { LieutenantPiece.LIEUTENANT_NAME, 4 }, { CaptainPiece.CAPTAIN_NAME, 4 }, { MajorPiece.MAJOR_NAME, 3},
                { ColonelPiece.COLONEL_NAME, 2}, { GeneralPiece.GENERAL_NAME, 1}, { MarshallPiece.MARSHALL_NAME, 1 }
            };
            Dictionary<String, int> zeroed = new Dictionary<String, int>(){
                { FlagPiece.FLAG_NAME, 0 }, { BombPiece.BOMB_NAME, 0 }, { SpyPiece.SPY_NAME, 0 },
                { ScoutPiece.SCOUT_NAME, 0}, { MinerPiece.MINER_NAME, 0 }, { SergeantPiece.SERGEANT_NAME, 0 },
                { LieutenantPiece.LIEUTENANT_NAME, 0 }, { CaptainPiece.CAPTAIN_NAME, 0 }, { MajorPiece.MAJOR_NAME, 0},
                { ColonelPiece.COLONEL_NAME, 0}, { GeneralPiece.GENERAL_NAME, 0}, { MarshallPiece.MARSHALL_NAME, 0 }
            };
            factory.setPlacements(zeroed);

            Assert.AreEqual(zeroed, factory.placements);
            Assert.AreEqual(0, factory.getMinPieces());

            factory.resetPlacements();

            Assert.AreEqual(defaults, factory.placements);
            Assert.AreEqual(0, factory.getMinPieces());
        }

        [TestCase()]
        public void TestResetPlacementsWithAdded()
        {
            GamePieceFactory factory = new GamePieceFactory();
            Dictionary<String, int> defaults = new Dictionary<String, int>(){
                { FlagPiece.FLAG_NAME, 1 }, { BombPiece.BOMB_NAME, 6 }, { SpyPiece.SPY_NAME, 1 },
                { ScoutPiece.SCOUT_NAME, 8 }, { MinerPiece.MINER_NAME, 5 }, { SergeantPiece.SERGEANT_NAME, 4 },
                { LieutenantPiece.LIEUTENANT_NAME, 4 }, { CaptainPiece.CAPTAIN_NAME, 4 }, { MajorPiece.MAJOR_NAME, 3},
                { ColonelPiece.COLONEL_NAME, 2}, { GeneralPiece.GENERAL_NAME, 1}, { MarshallPiece.MARSHALL_NAME, 1 },
                { BondTierSpyPiece.BOND_NAME, 5 }
            };
            factory.addPieceToPlacements(BondTierSpyPiece.BOND_NAME, typeof(BondTierSpyPiece), 5);
            Dictionary<String, int> zeroed = new Dictionary<String, int>(){
                { FlagPiece.FLAG_NAME, 0 }, { BombPiece.BOMB_NAME, 0 }, { SpyPiece.SPY_NAME, 0 },
                { ScoutPiece.SCOUT_NAME, 0}, { MinerPiece.MINER_NAME, 0 }, { SergeantPiece.SERGEANT_NAME, 0 },
                { LieutenantPiece.LIEUTENANT_NAME, 0 }, { CaptainPiece.CAPTAIN_NAME, 0 }, { MajorPiece.MAJOR_NAME, 0},
                { ColonelPiece.COLONEL_NAME, 0}, { GeneralPiece.GENERAL_NAME, 0}, { MarshallPiece.MARSHALL_NAME, 0 }
            };
            factory.setPlacements(zeroed);

            zeroed.Add(BondTierSpyPiece.BOND_NAME, 5);
            Assert.AreEqual(zeroed, factory.placements);
            Assert.AreEqual(5, factory.getMinPieces());

            factory.resetPlacements();

            Assert.AreEqual(defaults, factory.placements);
            Assert.AreEqual(5, factory.getMinPieces());
        }

        public GamePiece reflectMaterialize(GamePieceFactory factory, Type type, int teamCode)
        {
            Type factoryType = typeof(GamePieceFactory);
            
            MethodInfo factoryMethod =factoryType.GetMethod("materializePiece", BindingFlags.NonPublic |BindingFlags.Instance);

            return (GamePiece)factoryMethod.Invoke(factory, new object[] { type, teamCode });
        }

        [TestCase(typeof(BombPiece), -1)]
        [TestCase(typeof(BombPiece), 1)]
        [TestCase(typeof(CaptainPiece), -1)]
        [TestCase(typeof(CaptainPiece), 1)]
        [TestCase(typeof(ColonelPiece), -1)]
        [TestCase(typeof(ColonelPiece), 1)]
        [TestCase(typeof(FlagPiece), -1)]
        [TestCase(typeof(FlagPiece), 1)]
        [TestCase(typeof(GeneralPiece), -1)]
        [TestCase(typeof(GeneralPiece), 1)]
        [TestCase(typeof(LieutenantPiece), -1)]
        [TestCase(typeof(LieutenantPiece), 1)]
        [TestCase(typeof(MajorPiece), -1)]
        [TestCase(typeof(MajorPiece), 1)]
        [TestCase(typeof(MinerPiece), -1)]
        [TestCase(typeof(MinerPiece), 1)]
        [TestCase(typeof(ObstaclePiece), 0)]
        [TestCase(typeof(ScoutPiece), -1)]
        [TestCase(typeof(ScoutPiece), 1)]
        [TestCase(typeof(SergeantPiece), -1)]
        [TestCase(typeof(SergeantPiece), 1)]
        [TestCase(typeof(SpyPiece), -1)]
        [TestCase(typeof(SpyPiece), 1)]
        public void TestMaterializePiece(Type type, int teamCode)
        {
            GamePieceFactory factory = new GamePieceFactory();
            GamePiece piece = this.reflectMaterialize(factory, type, teamCode);

            Assert.AreEqual(type, piece.GetType());
            Assert.AreEqual(teamCode, piece.getTeamCode());
        }

        [TestCase()]
        public void TestDonePlacing()
        {
            GamePieceFactory factory = new GamePieceFactory();

            Assert.IsFalse(factory.donePlacing());

            Dictionary<String, int> zeroed = new Dictionary<String, int>(){
                { FlagPiece.FLAG_NAME, 0 }, { BombPiece.BOMB_NAME, 0 }, { SpyPiece.SPY_NAME, 0 },
                { ScoutPiece.SCOUT_NAME, 0}, { MinerPiece.MINER_NAME, 0 }, { SergeantPiece.SERGEANT_NAME, 0 },
                { LieutenantPiece.LIEUTENANT_NAME, 0 }, { CaptainPiece.CAPTAIN_NAME, 0 }, { MajorPiece.MAJOR_NAME, 0},
                { ColonelPiece.COLONEL_NAME, 0}, { GeneralPiece.GENERAL_NAME, 0}, { MarshallPiece.MARSHALL_NAME, 0 }
            };

            factory.setPlacements(zeroed);

            Assert.IsTrue(factory.donePlacing());

            zeroed[FlagPiece.FLAG_NAME] = 1;
            factory.setMinPieces(1);
            factory.setPlacements(zeroed);
            Assert.IsFalse(factory.donePlacing());

            zeroed[FlagPiece.FLAG_NAME] = 0;
            zeroed[MinerPiece.MINER_NAME] = 1;
            factory.setPlacements(zeroed);
            Assert.IsTrue(factory.donePlacing());
        }

    }
}
