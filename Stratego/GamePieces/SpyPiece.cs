using System;

namespace Stratego
{
    [Serializable]
    public class SpyPiece : GamePiece
    {
        public static readonly String SPY_NAME = "Spy";
        public static readonly int SPY_RANK = 1;

        public SpyPiece (int teamCode) : base(teamCode)
        {
            this.pieceImage = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? Properties.Resources.BlueSpy : Properties.Resources.RedSpy;

            this.pieceRank = SPY_RANK;
            this.pieceName = SPY_NAME;
            this.attackBehavior = new ImperviousToMarshall();
            this.defendBehavior = new SimplyDie();
        }
    }
}
