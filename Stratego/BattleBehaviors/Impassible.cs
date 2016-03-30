using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.BattleBehaviors
{
    class Impassible : BattleBehavior
    {
        public override bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece)
        {
            return false;
        }
    }
}
