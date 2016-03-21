using System;

namespace Stratego
{
    class FlagPiece : GamePiece
    {
        public static readonly int FLAG_RANK = -1;
        public static readonly String FLAG_NAME = "flag";

        public FlagPiece (int teamCode) : base(teamCode)
        {
            this.pieceRank = FLAG_RANK;
            this.pieceName = FLAG_NAME;
        }

        public override void attack(GamePiece otherPiece)
        {
            // This Piece Cannot Attack
        }

        public override void defend(GamePiece otherPiece)
        {
            this.killPiece();
        }
    }
}
