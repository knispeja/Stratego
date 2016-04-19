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
            if (affectingPiece.getPieceName().Equals(BombPiece.BOMB_NAME) || affectingPiece.getPieceName().Equals(MarshallPiece.MARSHALL_NAME) || affectingPiece.getPieceName().Equals(BondTierSpyPiece.BOND_NAME))
            {
                return true;
            }
            return false;
        }
    }
}
