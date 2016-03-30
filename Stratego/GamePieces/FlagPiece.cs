using System;

namespace Stratego
{
    [Serializable]
    public class FlagPiece : GamePiece
    {
        public static readonly String FLAG_NAME = "Flag";
        public static readonly int FLAG_RANK = -1;

        public FlagPiece (int teamCode) : base(teamCode)
        {
            this.pieceImage = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? Properties.Resources.BlueFlag : Properties.Resources.RedFlag;

            this.pieceRank = FLAG_RANK;
            this.pieceName = FLAG_NAME;
            this.attackBehavior = new DiesToAllSaveFlag();
            this.defendBehavior = new SimplyDie();
            this.movable = false;
            this.essential = true;
        }
    }
}
