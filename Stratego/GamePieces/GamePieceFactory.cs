using Stratego.BattleBehaviors;
using System;
using System.Collections.Generic;

namespace Stratego.GamePieces
{
    public class GamePieceFactory
    {
        /// <summary>
        /// The default amount of pieces for each piece. (EX: 0 0s; 1 1; 1 2; 2 3s; 4 4s; etc..)
        /// </summary>
        public static readonly Dictionary<String, int> defaults = new Dictionary<String, int>()
        { { FlagPiece.FLAG_NAME, 1 }, { BombPiece.BOMB_NAME, 6 }, { SpyPiece.SPY_NAME, 1 },
            { ScoutPiece.SCOUT_NAME, 8 }, { MinerPiece.MINER_NAME, 5 }, { SergeantPiece.SERGEANT_NAME, 4 },
            {LieutenantPiece.LIEUTENANT_NAME, 4 }, {CaptainPiece.CAPTAIN_NAME, 4 }, { MajorPiece.MAJOR_NAME, 3}, { ColonelPiece.COLONEL_NAME, 2},
            { GeneralPiece.GENERAL_NAME, 1}, { MarshallPiece.MARSHALL_NAME, 1 }
        };

        /// <summary>
        /// The array which holds information on how many pieces of each type can still be placed
        /// </summary>
        public Dictionary<String, int> placements;
        public int minPieces = 0;
        private Dictionary<String, int> addedPieces;

        private readonly Dictionary<String, Type> stringDict = new Dictionary<String, Type>();
        private readonly Dictionary<int, Type> intDict = new Dictionary<int, Type>();
        private readonly Dictionary<Type, Type> attackDict = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> defendDict = new Dictionary<Type, Type>();
        
        public GamePieceFactory()
        {
            this.stringDict.Add(MarshallPiece.MARSHALL_NAME, typeof(MarshallPiece));
            this.intDict.Add(1, typeof(MarshallPiece));

            this.stringDict.Add(GeneralPiece.GENERAL_NAME, typeof(GeneralPiece));
            this.intDict.Add(2, typeof(GeneralPiece));

            this.stringDict.Add(ColonelPiece.COLONEL_NAME, typeof(ColonelPiece));
            this.intDict.Add(3, typeof(ColonelPiece));

            this.stringDict.Add(MajorPiece.MAJOR_NAME, typeof(MajorPiece));
            this.intDict.Add(4, typeof(MajorPiece));

            this.stringDict.Add(CaptainPiece.CAPTAIN_NAME, typeof(CaptainPiece));
            this.intDict.Add(5, typeof(CaptainPiece));

            this.stringDict.Add(LieutenantPiece.LIEUTENANT_NAME, typeof(LieutenantPiece));
            this.intDict.Add(6, typeof(LieutenantPiece));

            this.stringDict.Add(SergeantPiece.SERGEANT_NAME, typeof(SergeantPiece));
            this.intDict.Add(7, typeof(SergeantPiece));

            this.stringDict.Add(MinerPiece.MINER_NAME, typeof(MinerPiece));
            this.intDict.Add(8, typeof(MinerPiece));

            this.stringDict.Add(ScoutPiece.SCOUT_NAME, typeof(ScoutPiece));
            this.intDict.Add(9, typeof(ScoutPiece));
           
            this.stringDict.Add(SpyPiece.SPY_NAME, typeof(SpyPiece));
            this.stringDict.Add("S", typeof(SpyPiece));
            this.stringDict.Add("s", typeof(SpyPiece));
            this.intDict.Add(10, typeof(SpyPiece));

            this.stringDict.Add(BombPiece.BOMB_NAME, typeof(BombPiece));
            this.stringDict.Add("B", typeof(BombPiece));
            this.stringDict.Add("b", typeof(BombPiece));
            this.intDict.Add(11, typeof(BombPiece));

            this.stringDict.Add(FlagPiece.FLAG_NAME, typeof(FlagPiece));
            this.stringDict.Add("F", typeof(FlagPiece));
            this.stringDict.Add("f", typeof(FlagPiece));
            this.intDict.Add(12, typeof(FlagPiece));

            this.stringDict.Add(ObstaclePiece.OBSTACLE_NAME, typeof(ObstaclePiece));
            this.intDict.Add(42, typeof(ObstaclePiece));

            this.intDict.Add(0, null);

            this.addedPieces = new Dictionary<String, int>();
            this.resetPlacements();

            this.attackDict.Add(typeof(BombPiece), typeof(DiestoMinerandBomb));
            this.defendDict.Add(typeof(BombPiece), typeof(DiestoMinerandBomb));

            this.attackDict.Add(typeof(BondTierSpyPiece), typeof(BondLevelLiving));
            this.defendDict.Add(typeof(BondTierSpyPiece), typeof(DiesToBondAndMarshall));

            this.attackDict.Add(typeof(CaptainPiece), typeof(DefaultComparativeFate));
            this.defendDict.Add(typeof(CaptainPiece), typeof(DefaultComparativeFate));

            this.attackDict.Add(typeof(ColonelPiece), typeof(DefaultComparativeFate));
            this.defendDict.Add(typeof(ColonelPiece), typeof(DefaultComparativeFate));

            this.attackDict.Add(typeof(FlagPiece), typeof(DiesToAllSaveFlag));
            this.defendDict.Add(typeof(FlagPiece), typeof(SimplyDie));

            this.attackDict.Add(typeof(GeneralPiece), typeof(DefaultComparativeFate));
            this.defendDict.Add(typeof(GeneralPiece), typeof(DefaultComparativeFate));

            this.attackDict.Add(typeof(LieutenantPiece), typeof(DefaultComparativeFate));
            this.defendDict.Add(typeof(LieutenantPiece), typeof(DefaultComparativeFate));

            this.attackDict.Add(typeof(MajorPiece), typeof(DefaultComparativeFate));
            this.defendDict.Add(typeof(MajorPiece), typeof(DefaultComparativeFate));

            this.attackDict.Add(typeof(MarshallPiece), typeof(DefaultComparativeFate));
            this.defendDict.Add(typeof(MarshallPiece), typeof(DiesToSpy));

            this.attackDict.Add(typeof(MinerPiece), typeof(ImperviousToBombs));
            this.defendDict.Add(typeof(MinerPiece), typeof(ImperviousToBombs));

            this.attackDict.Add(typeof(ObstaclePiece), typeof(Impassible));
            this.defendDict.Add(typeof(ObstaclePiece), typeof(Impassible));

            this.attackDict.Add(typeof(ScoutPiece), typeof(DefaultComparativeFate));
            this.defendDict.Add(typeof(ScoutPiece), typeof(DefaultComparativeFate));

            this.attackDict.Add(typeof(SergeantPiece), typeof(DefaultComparativeFate));
            this.defendDict.Add(typeof(SergeantPiece), typeof(DefaultComparativeFate));

            this.attackDict.Add(typeof(SpyPiece), typeof(ImperviousToMarshall));
            this.defendDict.Add(typeof(SpyPiece), typeof(SimplyDie));
        }

        public void addNamesForPiece(List<String> names, Type pieceType)
        {
            foreach (String iden in names)
            {
                if (!this.stringDict.ContainsKey(iden))
                {
                    this.stringDict.Add(iden, pieceType);
                }
            }
        }
        public void addNumForPiece(int numberRef, Type pieceType)
        {
            if (!this.intDict.ContainsKey(numberRef))
            {
                this.intDict.Add(numberRef, pieceType);
            }
        }
        public void addPieceToPlacements(String name, Type pieceType, int numAllowed)
        {
            if(!this.stringDict.ContainsKey(name))
            {
                this.stringDict.Add(name, pieceType);
            }
            if (this.placements.ContainsKey(name))
            {
                this.placements.Remove(name);
            }
            this.placements.Add(name, numAllowed);
            if (!this.addedPieces.ContainsKey(name))
            {
                this.addedPieces.Add(name, numAllowed);
            }
            this.setMinPieces(this.minPieces + numAllowed);
        }

        public void resetPlacements()
        {
            this.placements = new Dictionary<string, int>();
            foreach (string key in GamePieceFactory.defaults.Keys)
            {
                this.placements.Add(key, GamePieceFactory.defaults[key]);
            }
            this.setMinPieces(0);
            foreach (string key in this.addedPieces.Keys)
            {
                this.addPieceToPlacements(key, null, this.addedPieces[key]);
            }
        }
        /// <summary>
        /// Retrieves the number of pieces still available for
        /// placement of a given type
        /// </summary>
        /// <param name="piece">Type of the piece you want to check</param>
        /// <returns>Number of pieces available for placement</returns>
        public int getPiecesLeft(GamePiece piece)
        {
            return this.placements[piece.getPieceName()];
        }

        public GamePiece getPiece(String identifier, int teamCode)
        {
            if (identifier.Equals(GamePiece.NULL_PIECE_NAME) || !this.stringDict.ContainsKey(identifier))
            {
                return null;
            }
            Type type = this.stringDict[identifier];

            return materializePiece(type, teamCode);
        }

        public GamePiece getPiece(int identifier, int teamCode)
        {
            if (!this.intDict.ContainsKey(identifier))
            {
                return null;
            }
            Type type = this.intDict[identifier];

            return materializePiece(type, teamCode);  
        }

        private GamePiece materializePiece(Type type, int teamCode)
        {
            var ctors = type.GetConstructors();

            GamePiece toReturn = (GamePiece)ctors[0].Invoke(new object[] { teamCode });

            Type attack = this.attackDict[type];

            Type defend = this.defendDict[type];

            var attackConst = attack.GetConstructors();

            var defendConst = defend.GetConstructors();

            BattleBehavior attackBehav = (BattleBehavior)attackConst[0].Invoke(new object[] { });

            BattleBehavior defendBehav = (BattleBehavior)defendConst[0].Invoke(new object[] { });

            toReturn.setAttackBehavior(attackBehav);

            toReturn.setDefendBehavior(defendBehav);

            return toReturn;
        }

        public void incrementPiecesLeft(String piece)
        {
            this.placements[piece]++;
        }

        public void decrementPiecesLeft(String piece)
        {
            this.placements[piece]--;
        }
        
        public void setMinPieces(int min)
        {
            this.minPieces = min;
        }

        public bool donePlacing()
        {
            if (this.placements[FlagPiece.FLAG_NAME] != 0)
            {
                return false;
            }
            int sum = 0;
            foreach(String key in this.placements.Keys)
            {
                sum += this.placements[key];
            }
            return (sum==this.minPieces);
        }

        public void setPlacements(Dictionary<string, int> dictionary)
        {
            this.placements = dictionary;
        }

        public Dictionary<string, int> getPlacements()
        {
            return this.placements;
        }

        public void changeAttackBehav(Type pieceType, Type behavType)
        {
            if (this.attackDict.ContainsKey(pieceType))
            {
                this.attackDict.Remove(pieceType);
            }
            this.attackDict.Add(pieceType, behavType);
        }

        public void changeDefendBehav(Type pieceType, Type behavType)
        {
            System.Diagnostics.Debug.WriteLine("Changing Defense Behavior");
            if (this.defendDict.ContainsKey(pieceType))
            {
                this.defendDict.Remove(pieceType);
            }
            this.defendDict.Add(pieceType, behavType);
        }
    }
}
