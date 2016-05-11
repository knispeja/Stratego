using System;

namespace Stratego
{
    [Serializable]
    public class SimplyDie : BattleBehavior
    {
        public SimplyDie() : base()
        {
        }

        public override bool decideFate(GamePiece defendPiece, GamePiece attackPiece)
        {
            return true;
        }
    }
}
