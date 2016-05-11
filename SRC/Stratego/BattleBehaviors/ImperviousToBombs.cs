using System;

namespace Stratego
{
    [Serializable]
    class ImperviousToBombs : BattleBehavior
    {
        public ImperviousToBombs() : base()
        {
        }

        public override bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece)
        {
            int otherRank = affectingPiece.getPieceRank();
            if (otherRank != BombPiece.BOMB_RANK)
            {
                return (new DefaultComparativeFate()).decideFate(affectedPiece, affectingPiece);
            }
            return false;
        }
    }
}
