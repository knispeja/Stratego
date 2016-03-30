using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    [Serializable]
    public class DiesToSpy : BattleBehavior
    {
        public DiesToSpy() : base()
        {
        }

        public override bool decideFate(GamePiece defendPiece, GamePiece attackPiece)
        {
            int otherRank = defendPiece.getPieceRank();
            if (otherRank == SpyPiece.SPY_RANK)
            {
                return true;
            }
            else
            {
                return (new DefaultComparativeFate()).decideFate(defendPiece, attackPiece);
            }
        }
    }
}
