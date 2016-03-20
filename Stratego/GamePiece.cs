using System;

namespace Stratego
{
    abstract class GamePiece
    {
        public static readonly String NULL_NAME = "null";
        public static readonly int NULL_NUM = 0;
        private string pieceName;
        private int pieceRank;
        private int teamCode;
        private Boolean lifeStatus;

        public GamePiece(int teamNum)
        {
            this.pieceRank = NULL_NUM;
            this.pieceName = NULL_NAME;
            this.teamCode = teamNum;
            this.lifeStatus = true;
        }

        public abstract int attack(GamePiece otherPiece);

        public abstract int defend(GamePiece otherPiece);

        public Boolean isAlive()
        {
            return lifeStatus;
        }

        public int getPieceRank()
        {
            return pieceRank;
        }

        public String getPieceName()
        {
            return pieceName;
        }

        public int getTeamCode()
        {
            return teamCode;
        }
    }
}
