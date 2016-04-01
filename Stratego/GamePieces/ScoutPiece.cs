using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class ScoutPiece : GamePiece
    {
        public static readonly String SCOUT_NAME = "Scout";
        public static readonly int SCOUT_RANK = 2;

        public ScoutPiece(int teamCode) : base(teamCode)
        {
            this.pieceRank = SCOUT_RANK;
            this.pieceName = SCOUT_NAME;
            this.limitToMovement = int.MaxValue;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueScout : Properties.Resources.RedScout;
        }
    }
}
