using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class GeneralPiece : GamePiece
    {
        public static readonly String GENERAL_NAME = "General";
        public static readonly int GENERAL_RANK = 9;

        public GeneralPiece (int teamCode) : base (teamCode)
        {
            this.pieceRank = GENERAL_RANK;
            this.pieceName = GENERAL_NAME;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueGeneral : Properties.Resources.RedGeneral;
        }
    }
}
