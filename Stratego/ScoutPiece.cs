using System;

namespace Stratego
{
    public class ScoutPiece : GamePiece
    {
        public static readonly String SCOUT_NAME = "Scout";
        public static readonly int SCOUT_RANK = 2;

        public ScoutPiece(int teamCode) : base(teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueScout);
            this.imageDict.Add(1, Properties.Resources.RedScout);
            this.pieceRank = SCOUT_RANK;
            this.pieceName = SCOUT_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }
    }
}
