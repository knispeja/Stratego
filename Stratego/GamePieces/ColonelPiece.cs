using System;

namespace Stratego
{
    [Serializable]
    public class ColonelPiece : GamePiece
    {
        public static readonly String COLONEL_NAME = "Colonel";
        public static readonly int COLONEL_RANK = 8;

        public ColonelPiece (int teamCode) : base (teamCode)
        {
            this.pieceImage = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? Properties.Resources.BlueColonel : Properties.Resources.RedColonel;

            this.pieceRank = COLONEL_RANK;
            this.pieceName = COLONEL_NAME;
        }
    }
}
