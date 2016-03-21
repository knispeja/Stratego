using System;

namespace Stratego
{
    class SpyPiece : GamePiece
    {
        public static readonly int SPY_NUM = 1;
        public static readonly String SPY_NAME = "Spy";

        public SpyPiece (int teamCode) : base(teamCode)
        {
            this.pieceRank = SPY_NUM;
            this.pieceName = SPY_NAME;
        }

        public override void attack(GamePiece otherPiece)
        {
            if (otherPiece.getPieceRank() == this.pieceRank)
            {
                this.killPiece();
            }
        }

        public override void defend(GamePiece otherPiece)
        {
            if(otherPiece.getPieceRank() != FlagPiece.FLAG_RANK)
            {
                this.killPiece();
            }
        }
    }
}
