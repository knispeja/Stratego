using System;

namespace Stratego
{
    public class BombPiece : GamePiece
    {
        public static readonly String BOMB_NAME = "Bomb";
        public static readonly int BOMB_RANK = int.MaxValue;

        public BombPiece (int teamCode) : base (teamCode)
        {
            this.imageDict.Add(0, Properties.Resources.BlueBomb);
            this.imageDict.Add(1, Properties.Resources.RedBomb);
            this.pieceRank = BOMB_RANK;
            this.pieceName = BOMB_NAME;
            this.pieceImage = this.imageDict[teamCode];
            this.attackBehavior = new DiestoMinerandBomb();
            this.defendBehavior = new DiestoMinerandBomb();
            this.limitToMovement = 0;
        }
    }
}
