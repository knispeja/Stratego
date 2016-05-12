using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class CaptainPiece : GamePiece
    {
        public static readonly String CAPTAIN_NAME = "Captain";
        public static readonly int CAPTAIN_RANK = 6;

        public CaptainPiece(int teamCode) : base (teamCode)
        {
            this.pieceRank = CAPTAIN_RANK;
            this.pieceName = CAPTAIN_NAME;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueCaptain : Properties.Resources.RedCaptain;
        }
    }
}
