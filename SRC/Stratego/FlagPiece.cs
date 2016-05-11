using System;

namespace Stratego
{
    public class FlagPiece : GamePiece
    {
        public static readonly String FLAG_NAME = "Flag";
        public static readonly int FLAG_RANK = -1;

        public FlagPiece (int teamCode) : base(teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueFlag);
            this.imageDict.Add(1, Properties.Resources.RedFlag);
            this.pieceRank = FLAG_RANK;
            this.pieceName = FLAG_NAME;
            this.pieceImage = this.imageDict[teamCode];
            this.attackBehavior = new DiesToAllSaveFlag();
            this.defendBehavior = new SimplyDie();
        }
    }
}
