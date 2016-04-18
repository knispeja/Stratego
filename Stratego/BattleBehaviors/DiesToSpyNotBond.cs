using Stratego.GamePieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.BattleBehaviors
{
    [Serializable]
    public class DiesToSpyNotBond : BattleBehavior
    {
        public DiesToSpyNotBond() : base()
        {
        }

        public override bool decideFate(GamePiece defendPiece, GamePiece attackPiece)
        {
            int otherRank = defendPiece.getPieceRank();
            if (otherRank == SpyPiece.SPY_RANK)
            {
                return true;
            }
            else if (attackPiece.getPieceName() == BondTierSpyPiece.BOND_NAME)
            {
                return false;
            }
            else
            {
                return (new DefaultComparativeFate()).decideFate(defendPiece, attackPiece);
            }
        }
    }
}
