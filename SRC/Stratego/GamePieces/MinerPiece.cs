using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class MinerPiece : GamePiece
    {
        public static readonly String MINER_NAME = "Miner";
        public static readonly int MINER_RANK = 3;

        public MinerPiece(int teamCode) : base(teamCode)
        {
            this.pieceRank = MINER_RANK;
            this.pieceName = MINER_NAME;
            this.attackBehavior = new ImperviousToBombs();
            this.defendBehavior = new ImperviousToBombs();
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueMiner : Properties.Resources.RedMiner;
        }
    }
}
