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
        private int minPieces = 0;

        private readonly Dictionary<String, Type> stringDict = new Dictionary<String, Type>();
        private readonly Dictionary<int, Type> intDict = new Dictionary<int, Type>();
        
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

            this.stringDict.Add(BombPiece.BOMB_NAME, typeof(BombPiece));
            this.stringDict.Add("B", typeof(BombPiece));
            this.stringDict.Add("b", typeof(SpyPiece));

            this.stringDict.Add(FlagPiece.FLAG_NAME, typeof(FlagPiece));
            this.stringDict.Add("F", typeof(FlagPiece));
            this.stringDict.Add("f", typeof(SpyPiece));

            this.stringDict.Add(ObstaclePiece.OBSTACLE_NAME, typeof(ObstaclePiece));
            this.intDict.Add(42, typeof(ObstaclePiece));

            this.intDict.Add(0, null);
        }

        public void addPieceToFactory(String identifier, int numberRef, Type pieceType)
        {
            this.stringDict.Add(identifier, pieceType);
            this.intDict.Add(numberRef, pieceType);
        }

        public void resetPlacements()
        {
            this.placements = new Dictionary<string, int>();
            foreach (string key in GamePieceFactory.defaults.Keys)
            {
                this.placements.Add(key, GamePieceFactory.defaults[key]);
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

            var ctors = type.GetConstructors();

            return (GamePiece)ctors[0].Invoke(new object[] { teamCode });
        }

        public GamePiece getPiece(int identifier, int teamCode)
        {
            if (!this.intDict.ContainsKey(identifier))
            {
                return null;
            }
            Type type = this.intDict[identifier];

            if (type == null) return null;

            var ctors = type.GetConstructors();

            return (GamePiece) ctors[0].Invoke(new object[] { teamCode });   
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
    }
}
