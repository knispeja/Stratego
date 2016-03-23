using System;

namespace Stratego
{
    public class CaptainPiece : GamePiece
    {
        public static readonly String CAPTAIN_NAME = "Captain";
        public static readonly int CAPTAIN_RANK = 6;

        public CaptainPiece(int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueCaptain);
            this.imageDict.Add(1, Properties.Resources.RedCaptain);
            this.pieceRank = CAPTAIN_RANK;
            this.pieceName = CAPTAIN_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }
    }
}
