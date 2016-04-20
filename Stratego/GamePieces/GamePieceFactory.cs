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
        private Dictionary<String, int> addedPieces = new Dictionary<String, int>();

        private readonly Dictionary<String, Type> stringDict = new Dictionary<String, Type>();
        private readonly Dictionary<int, Type> intDict = new Dictionary<int, Type>();
        private readonly Dictionary<Type, Type> attackDict = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> defendDict = new Dictionary<Type, Type>();

        public GamePieceFactory()
        {
            this.stringDict = new Dictionary<String, Type>() { { MarshallPiece.MARSHALL_NAME, typeof(MarshallPiece) },
                {GeneralPiece.GENERAL_NAME, typeof(GeneralPiece) }, {ColonelPiece.COLONEL_NAME, typeof(ColonelPiece) },
                { MajorPiece.MAJOR_NAME, typeof(MajorPiece)}, {CaptainPiece.CAPTAIN_NAME, typeof(CaptainPiece) },
                {LieutenantPiece.LIEUTENANT_NAME, typeof(LieutenantPiece) }, {SergeantPiece.SERGEANT_NAME, typeof(SergeantPiece) },
                {MinerPiece.MINER_NAME, typeof(MinerPiece) }, {ScoutPiece.SCOUT_NAME, typeof(ScoutPiece) }, {SpyPiece.SPY_NAME, typeof(SpyPiece) },
                {"S", typeof(SpyPiece) }, {BombPiece.BOMB_NAME, typeof(BombPiece)}, {"B", typeof(BombPiece) }, {FlagPiece.FLAG_NAME, typeof(FlagPiece) },
                {"F", typeof(FlagPiece) }, {ObstaclePiece.OBSTACLE_NAME, typeof(ObstaclePiece)} };

            this.intDict = new Dictionary<int, Type>() { { 1, typeof(MarshallPiece) }, { 2, typeof(GeneralPiece) }, { 3, typeof(ColonelPiece) },
                {4, typeof(MajorPiece) }, {5, typeof(CaptainPiece) }, {6, typeof(LieutenantPiece) }, {7, typeof(SergeantPiece) }, {8, typeof(MinerPiece) },
                {9, typeof(ScoutPiece) }, {10, typeof(SpyPiece) }, {11, typeof(BombPiece) }, {12, typeof(FlagPiece) }, {42, typeof(ObstaclePiece) },
                {0, null }
            };

            this.attackDict = new Dictionary<Type, Type>() { { typeof(BombPiece), typeof(DiestoMinerandBomb) },
                {typeof(CaptainPiece), typeof(DefaultComparativeFate) }, {typeof(ColonelPiece), typeof(DefaultComparativeFate) },
                {typeof(FlagPiece), typeof(DiesToAllSaveFlag) }, {typeof(GeneralPiece), typeof(DefaultComparativeFate) },
                {typeof(LieutenantPiece), typeof(DefaultComparativeFate) }, {typeof(MajorPiece), typeof(DefaultComparativeFate) },
                {typeof(MarshallPiece), typeof(DefaultComparativeFate) }, {typeof(MinerPiece), typeof(ImperviousToBombs) },
                {typeof(ObstaclePiece), typeof(Impassible) }, {typeof(ScoutPiece), typeof(DefaultComparativeFate) },
                {typeof(SergeantPiece), typeof(DefaultComparativeFate) }, {typeof(SpyPiece), typeof(ImperviousToMarshall) } };

            this.defendDict = new Dictionary<Type, Type>() { { typeof(BombPiece), typeof(DiestoMinerandBomb) }, 
                {typeof(CaptainPiece), typeof(DefaultComparativeFate) }, {typeof(ColonelPiece), typeof(DefaultComparativeFate) },
                {typeof(FlagPiece), typeof(SimplyDie) }, {typeof(GeneralPiece), typeof(DefaultComparativeFate) },
                {typeof(LieutenantPiece), typeof(DefaultComparativeFate) }, {typeof(MajorPiece), typeof(DefaultComparativeFate) },
                {typeof(MarshallPiece), typeof(DiesToSpy) }, {typeof(MinerPiece), typeof(ImperviousToBombs) },
                {typeof(ObstaclePiece), typeof(Impassible) }, {typeof(ScoutPiece), typeof(DefaultComparativeFate) },
                {typeof(SergeantPiece), typeof(DefaultComparativeFate) }, {typeof(SpyPiece), typeof(SimplyDie) } };
            
            this.resetPlacements();
        }

        public void addNameForPiece(String name, Type pieceType)
        {
            if (!this.stringDict.ContainsKey(name))
            {
                this.stringDict.Add(name, pieceType);
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
                var ctors = pieceType.GetConstructors();
                GamePiece piece = (GamePiece)ctors[0].Invoke(new object[] { StrategoGame.RED_TEAM_CODE });
                this.attackDict.Add(pieceType, piece.getAttackBehavior().GetType());
                this.defendDict.Add(pieceType, piece.getDefendBehavior().GetType());
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
