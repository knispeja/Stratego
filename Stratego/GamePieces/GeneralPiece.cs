using System;

namespace Stratego
{
    [Serializable]
    public class GeneralPiece : GamePiece
    {
        public static readonly String GENERAL_NAME = "General";
        public static readonly int GENERAL_RANK = 9;

        public GeneralPiece (int teamCode) : base (teamCode)
        {
            this.pieceImage = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? Properties.Resources.BlueGeneral : Properties.Resources.RedGeneral;

            this.pieceRank = GENERAL_RANK;
            this.pieceName = GENERAL_NAME;
        }
    }
}
