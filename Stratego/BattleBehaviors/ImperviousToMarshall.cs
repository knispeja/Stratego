namespace Stratego
{
    public class ImperviousToMarshall : BattleBehavior
    {
        public ImperviousToMarshall() : base()
        {
        }

        public override bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece)
        {
            int otherRank = affectingPiece.getPieceRank();
            if (otherRank != MarshallPiece.MARSHALL_RANK)
            {
                return (new DefaultComparativeFate()).decideFate(affectedPiece, affectingPiece);
            }
            return false;
        }
    }
}
