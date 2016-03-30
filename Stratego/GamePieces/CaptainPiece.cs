using System;

namespace Stratego
{
    [Serializable]
    public class CaptainPiece : GamePiece
    {
        public static readonly String CAPTAIN_NAME = "Captain";
        public static readonly int CAPTAIN_RANK = 6;

        public CaptainPiece(int teamCode) : base (teamCode)
        {
            this.pieceImage = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? Properties.Resources.BlueCaptain : Properties.Resources.RedCaptain;

            this.pieceRank = CAPTAIN_RANK;
            this.pieceName = CAPTAIN_NAME;
        }
    }
}
