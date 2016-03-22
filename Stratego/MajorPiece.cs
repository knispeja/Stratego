using System;

namespace Stratego
{
    class MajorPiece : GamePiece
    {
        public static readonly String MAJOR_NAME = "Major";
        public static readonly int MAJOR_RANK = 7;

        public MajorPiece (int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueMajor);
            this.imageDict.Add(1, Properties.Resources.RedMajor);
            this.pieceRank = MAJOR_RANK;
            this.pieceName = MAJOR_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }
    }
}
