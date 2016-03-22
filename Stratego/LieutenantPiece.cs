using System;

namespace Stratego
{
    class LieutenantPiece : GamePiece
    {
        public static readonly String LIEUTENANT_NAME = "Lieutenant";
        public static readonly int LIEUTENANT_RANK = 5;

        public LieutenantPiece(int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueLieutenant);
            this.imageDict.Add(1, Properties.Resources.RedLieutenant);
            this.pieceRank = LIEUTENANT_RANK;
            this.pieceName = LIEUTENANT_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }
    }
}
