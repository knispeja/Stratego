using System;

namespace Stratego
{
    class BombPiece : GamePiece
    {
        public static readonly String BOMB_NAME = "Bomb";
        public static readonly int BOMB_RANK = 0; // TODO: talk to Jacob about this because he had a question

        public BombPiece (int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueBomb);
            this.imageDict.Add(1, Properties.Resources.RedBomb);
            this.pieceRank = BOMB_RANK;
            this.pieceName = BOMB_NAME;
            this.pieceImage = this.imageDict[teamCode];
        }

        public override void attack(GamePiece otherPiece)
        {
            defend(otherPiece);
        }

        public override void defend(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            if (otherRank == MinerPiece.MINER_RANK || otherRank == BOMB_RANK)
            {
                this.killPiece();
            }
        }
    }
}
