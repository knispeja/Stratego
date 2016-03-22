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
            int otherRank = otherPiece.getPieceRank();
            if (otherRank == BombPiece.BOMB_RANK || otherRank == SPY_RANK)
            {
                this.killPiece();
            }
        }

        public override void defend(GamePiece otherPiece)
        {
            this.killPiece();
        }
    }
}
