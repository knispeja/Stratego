using System;

namespace Stratego
{
    abstract class GamePiece
    {
        public static readonly String NULL_NAME = "NULL_NAME";
        public static readonly int NULL_NUM = 42;
        protected string pieceName;
        protected int pieceRank;
        protected int teamCode;
        protected Boolean lifeStatus;

        public GamePiece(int teamCode)
        {
            this.pieceRank = NULL_NUM;
            this.pieceName = NULL_NAME;
            this.teamCode = teamCode;
            this.lifeStatus = true;
        }

        public virtual void attack(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            if (otherRank > this.pieceRank)
            {
                this.killPiece();
            }
            else if (otherRank == this.pieceRank)
            {
                this.killPiece();
            }
        }

        public virtual void defend(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            if (otherRank > this.pieceRank)
            {
                this.killPiece();
            }
            else if (otherRank == this.pieceRank)
            {
                this.killPiece();
            }
        }

        public Boolean isAlive()
        {
            return lifeStatus;
        }

        protected void killPiece()
        {
            this.lifeStatus = false;
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
