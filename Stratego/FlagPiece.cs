using System;

namespace Stratego
{
    class FlagPiece : GamePiece
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
        }

        public override void attack(GamePiece otherPiece)
        {
            this.killPiece();
        }

        public override void defend(GamePiece otherPiece)
        {
            this.killPiece();
        }
    }
}
