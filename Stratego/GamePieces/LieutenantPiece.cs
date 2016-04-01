using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class LieutenantPiece : GamePiece
    {
        public static readonly String LIEUTENANT_NAME = "Lieutenant";
        public static readonly int LIEUTENANT_RANK = 5;

        public LieutenantPiece(int teamCode) : base (teamCode)
        {
            this.pieceRank = LIEUTENANT_RANK;
            this.pieceName = LIEUTENANT_NAME;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueLieutenant : Properties.Resources.RedLieutenant;
        }
    }
}
