using System;

namespace Stratego
{
    class MinerPiece : GamePiece
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
        }

        public override void attack(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            int comparisonValue = this.compareRanks(otherRank);
            if (otherRank != BombPiece.BOMB_RANK)
            {
                base.attack(otherPiece);
            }
        }

        public override void defend(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            if (otherRank != BombPiece.BOMB_RANK)
            {
                base.defend(otherPiece);
            }
        }
    }
}
