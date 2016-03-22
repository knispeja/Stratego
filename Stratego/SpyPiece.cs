using System;

namespace Stratego
{
    class SpyPiece : GamePiece
    {
        public static readonly String SPY_NAME = "Spy";
        public static readonly int SPY_RANK = 1;

        public SpyPiece (int teamCode) : base(teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueSpy);
            this.imageDict.Add(1, Properties.Resources.RedSpy);
            this.pieceRank = SPY_RANK;
            this.pieceName = SPY_NAME;
            this.pieceImage = this.imageDict[teamCode];
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
