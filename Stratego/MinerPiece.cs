using System;

namespace Stratego
{
    public class MinerPiece : GamePiece
    {
        public static readonly String MINER_NAME = "Miner";
        public static readonly int MINER_RANK = 3;

        public MinerPiece(int teamCode) : base(teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueMiner);
            this.imageDict.Add(1, Properties.Resources.RedMiner);
            this.pieceRank = MINER_RANK;
            this.pieceName = MINER_NAME;
            this.pieceImage = this.imageDict[teamCode];
            this.attackBehavior = new ImperviousToBombs();
            this.defendBehavior = new ImperviousToBombs();
        }
    }
}
