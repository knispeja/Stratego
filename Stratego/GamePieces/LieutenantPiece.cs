using System;

namespace Stratego
{
    [Serializable]
    public class LieutenantPiece : GamePiece
    {
        public static readonly String LIEUTENANT_NAME = "Lieutenant";
        public static readonly int LIEUTENANT_RANK = 5;

        public LieutenantPiece(int teamCode) : base (teamCode)
        {
            this.pieceImage = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? Properties.Resources.BlueLieutenant : Properties.Resources.RedLieutenant;

            this.pieceRank = LIEUTENANT_RANK;
            this.pieceName = LIEUTENANT_NAME;
        }
    }
}
