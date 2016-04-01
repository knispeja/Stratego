using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class MarshallPiece : GamePiece
    {
        public static readonly String MARSHALL_NAME = "Marshall";
        public static readonly int MARSHALL_RANK = 10;

        public MarshallPiece (int teamCode) : base (teamCode)
        {
            this.pieceRank = MARSHALL_RANK;
            this.pieceName = MARSHALL_NAME;
            this.defendBehavior = new DiesToSpy();
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueMarshall : Properties.Resources.RedMarshall;
        }
    }
}
