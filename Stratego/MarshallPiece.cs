using System;

namespace Stratego
{
    class MarshallPiece : GamePiece
    {
        public static readonly String MARSHALL_NAME = "Marshall";
        public static readonly int MARSHALL_RANK = 10;

        public MarshallPiece (int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueMarshal);
            this.imageDict.Add(1, Properties.Resources.RedMarshal);
            this.pieceRank = MARSHALL_RANK;
            this.pieceName = MARSHALL_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }
    }
}
