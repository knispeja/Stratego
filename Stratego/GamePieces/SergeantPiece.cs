using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class SergeantPiece : GamePiece
    {
        public static readonly String SERGEANT_NAME = "Sergeant";
        public static readonly int SERGEANT_RANK = 4;

        public SergeantPiece (int teamCode) : base (teamCode)
        {
            this.pieceRank = SERGEANT_RANK;
            this.pieceName = SERGEANT_NAME;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueSergeant : Properties.Resources.RedSergeant;
        }
    }
}
