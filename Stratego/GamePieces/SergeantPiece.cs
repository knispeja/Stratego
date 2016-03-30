using System;

namespace Stratego
{
    [Serializable]
    public class SergeantPiece : GamePiece
    {
        public static readonly String SERGEANT_NAME = "Sergeant";
        public static readonly int SERGEANT_RANK = 4;

        public SergeantPiece (int teamCode) : base (teamCode)
        {
            this.pieceImage = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? Properties.Resources.BlueSergeant : Properties.Resources.RedSergeant;

            this.pieceRank = SERGEANT_RANK;
            this.pieceName = SERGEANT_NAME;
        }
    }
}
