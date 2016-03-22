using System;

namespace Stratego
{
    class GeneralPiece : GamePiece
    {
        public static readonly String GENERAL_NAME = "General";
        public static readonly int GENERAL_RANK = 9;

        public GeneralPiece (int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueGeneral);
            this.imageDict.Add(1, Properties.Resources.RedGeneral);
            this.pieceRank = GENERAL_RANK;
            this.pieceName = GENERAL_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }
    }
}
