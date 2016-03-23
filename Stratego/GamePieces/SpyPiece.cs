using System;

namespace Stratego
{
    public class SpyPiece : GamePiece
    {
        public static readonly String SPY_NAME = "Spy";
        public static readonly int SPY_RANK = 1;

        public SpyPiece (int teamCode) : base(teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueSpy);
            this.imageDict.Add(1, Properties.Resources.RedSpy);
            this.pieceRank = SPY_RANK;
            this.pieceName = SPY_NAME;
            this.pieceImage = this.imageDict[teamCode];
            this.attackBehavior = new ImperviousToMarshall();
            this.defendBehavior = new SimplyDie();
        }
    }
}
