using System;

namespace Stratego
{
    [Serializable]
    public class SergeantPiece : GamePiece
    {
        public static readonly String SERGEANT_NAME = "Sergeant";
        public static readonly int SERGEANT_RANK = 4;

        public SergeantPiece (int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueSergeant);
            this.imageDict.Add(1, Properties.Resources.RedSergeant);
            this.pieceRank = SERGEANT_RANK;
            this.pieceName = SERGEANT_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }
    }
}
