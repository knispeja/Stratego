namespace Stratego
{
    public class DiesToAllSaveFlag : BattleBehavior
    {
        public DiesToAllSaveFlag() : base()
        {

        }

        public override bool decideFate(GamePiece affectedPiece, GamePiece affectingPiece)
        {
            int otherRank = affectingPiece.getPieceRank();
            if (otherRank != FlagPiece.FLAG_RANK)
            {
                return true;
            }
            return false;
        }
    }
}
