using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int attack(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            if(otherRank > this.pieceRank)
            {
                return 0;
            }
            else if (otherRank < this.pieceRank)
            {
                return 1;
            }
            else if (otherRank == this.pieceRank)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        public int defend(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            if (otherRank > this.pieceRank)
            {
                return 0;
            }
            else if (otherRank < this.pieceRank)
            {
                return 1;
            }
            else if (otherRank == this.pieceRank)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

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
