using Stratego.GamePieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.BattleBehaviors
{
    [Serializable]
    public class BondLevelLiving : BattleBehavior
    {
        public BondLevelLiving() : base()
        {
        }

        public override bool decideFate(GamePiece attackingPiece, GamePiece defendingPiece)
        {
            if (defendingPiece.GetType().Equals(typeof(BondTierSpyPiece)) || defendingPiece.GetType().Equals(typeof(MarshallPiece)))
            {
                return true;
            }
            return false;
        }
    }
}
