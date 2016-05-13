using Stratego.BattleBehaviors;
using System;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public class ObstaclePiece : GamePiece
    {
        public static readonly String OBSTACLE_NAME = "Obstacle";
        public static readonly int OBSTACLE_RANK = -1;

        public ObstaclePiece(int teamCode) : base(teamCode)
        {
            this.teamCode = 0;
            this.pieceRank = OBSTACLE_RANK;
            this.pieceName = OBSTACLE_NAME;
            this.attackBehavior = new Impassible();
            this.defendBehavior = new Impassible();
            this.movable = false;
        }

        public override Image getPieceImage()
        {
            return null;
        }
    }
}
