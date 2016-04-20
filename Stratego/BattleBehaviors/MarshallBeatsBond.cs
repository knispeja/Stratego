using Stratego.GamePieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.BattleBehaviors
{
    [Serializable]
    public class MarshallBeatsBond : BattleBehavior
    {
        public MarshallBeatsBond() : base()
        {
        }

        public override bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece)
        {
            if (affectedPiece.GetType().Equals(typeof(BondTierSpyPiece)))
            {
                return false;
            }
            return (new DefaultComparativeFate()).decideFate(affectedPiece, affectingPiece);
        }
    }
}
