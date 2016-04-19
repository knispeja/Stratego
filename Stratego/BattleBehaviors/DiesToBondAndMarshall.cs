using Stratego.GamePieces;
using System;

namespace Stratego.BattleBehaviors
{
    [Serializable]
    public class DiesToBondAndMarshall : BattleBehavior
    {
        public DiesToBondAndMarshall() : base()
        {
        }

        public override bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece)
        {
            if (affectedPiece.getPieceRank() == BondTierSpyPiece.BOND_RANK || affectedPiece.getPieceRank() == MarshallPiece.MARSHALL_RANK)
            {
                return true;
            }
            return false;
        }
    }
}
