using System;

namespace Stratego
{
    [Serializable]
    public class ColonelPiece : GamePiece
    {
        public static readonly String COLONEL_NAME = "Colonel";
        public static readonly int COLONEL_RANK = 8;

        public ColonelPiece (int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueColonel);
            this.imageDict.Add(1, Properties.Resources.RedColonel);
            this.pieceRank = COLONEL_RANK;
            this.pieceName = COLONEL_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }
    }
}
