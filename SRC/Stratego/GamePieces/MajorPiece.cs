using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class MajorPiece : GamePiece
    {
        public static readonly String MAJOR_NAME = "Major";
        public static readonly int MAJOR_RANK = 7;

        public MajorPiece (int teamCode) : base (teamCode)
        {
            this.pieceRank = MAJOR_RANK;
            this.pieceName = MAJOR_NAME;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueMajor : Properties.Resources.RedMajor;
        }
    }
}
