using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    [Serializable]
    class DiestoMinerandBomb : BattleBehavior
    {
        public DiestoMinerandBomb() : base()
        {
        }

        public override bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece)
        {
            int otherRank = affectingPiece.getPieceRank();
            if (otherRank == MinerPiece.MINER_RANK || otherRank == BombPiece.BOMB_RANK)
            {
                return true;
            }
            return false;
        }
    }
}
