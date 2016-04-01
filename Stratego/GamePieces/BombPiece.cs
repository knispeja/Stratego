using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class BombPiece : GamePiece
    {
        public static readonly String BOMB_NAME = "Bomb";
        public static readonly int BOMB_RANK = int.MaxValue;

        public BombPiece (int teamCode) : base (teamCode)
        {
            this.pieceRank = BOMB_RANK;
            this.pieceName = BOMB_NAME;
            this.attackBehavior = new DiestoMinerandBomb();
            this.defendBehavior = new DiestoMinerandBomb();
            this.limitToMovement = 0;
            this.movable = false;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueBomb : Properties.Resources.RedBomb;
        }
    }
}
