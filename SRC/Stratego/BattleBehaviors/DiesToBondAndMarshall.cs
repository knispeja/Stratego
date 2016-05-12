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

        public override bool decideFate(GamePiece defendingPiece, GamePiece attackingPiece)
        {
            if (attackingPiece.getPieceName().Equals(BombPiece.BOMB_NAME) || attackingPiece.getPieceName().Equals(MarshallPiece.MARSHALL_NAME) || attackingPiece.getPieceName().Equals(BondTierSpyPiece.BOND_NAME))
            {
                return true;
            }
            return false;
        }
    }
}
