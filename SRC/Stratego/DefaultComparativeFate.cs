namespace Stratego
{
    public class DefaultComparativeFate : BattleBehavior
    {
        public DefaultComparativeFate() : base()
        {
        }

        public override bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece)
        {
            int comparisonValue = affectedPiece.compareRanks(affectingPiece);
            if (comparisonValue <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
