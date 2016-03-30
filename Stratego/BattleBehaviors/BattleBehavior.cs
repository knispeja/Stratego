using System;

namespace Stratego
{
    [Serializable]
    public abstract class BattleBehavior
    {
        public BattleBehavior()
        {
        }

        public abstract bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece);
    }
}
