using System;
using System.Collections.Generic;

namespace Stratego.GamePieces
{
    public class GamePieceFactory
    {
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

        //public Boolean canConstructFrom(Object obj)
        //{

            //if (obj.GetType().Equals(typeof(int)) && this.intDict.ContainsKey((int)obj))
            //{
            //    return true;
            //}
            //else if (obj.GetType().Equals(typeof(String)) && this.stringDict.ContainsKey((String)obj))
            //{
            //    return true;
            //}
            //return false;
        //}
    }
}
