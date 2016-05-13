using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class ColonelPiece : GamePiece
    {
        public static readonly String COLONEL_NAME = "Colonel";
        public static readonly int COLONEL_RANK = 8;

        public ColonelPiece (int teamCode) : base (teamCode)
        {
            this.pieceRank = COLONEL_RANK;
            this.pieceName = COLONEL_NAME;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueColonel : Properties.Resources.RedColonel;
        }
    }
}
