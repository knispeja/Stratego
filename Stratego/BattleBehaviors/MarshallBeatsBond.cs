using Stratego.GamePieces;
using System;

namespace Stratego.BattleBehaviors
{
    [Serializable]
    public class MarshallBeatsBond : BattleBehavior
    {
        public MarshallBeatsBond() : base()
        {
        }

        public override bool decideFate(GamePiece attackingPiece, GamePiece defendingPiece)
        {
            if (defendingPiece.GetType().Equals(typeof(BondTierSpyPiece)))
            {
                return false;
            }
            return (new DefaultComparativeFate()).decideFate(attackingPiece, defendingPiece);
        }
    }
}
