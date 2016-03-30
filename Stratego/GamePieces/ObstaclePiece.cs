using Stratego.BattleBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    class ObstaclePiece : GamePiece
    {
        public static readonly String OBSTACLE_NAME = "Obstacle";
        public static readonly int OBSTACLE_RANK = 3;

        public ObstaclePiece(int teamCode) : base(teamCode)
        {
            this.teamCode = 0;
            this.pieceRank = OBSTACLE_RANK;
            this.pieceName = OBSTACLE_NAME;
            this.attackBehavior = new Impassible();
            this.defendBehavior = new Impassible();
        }
    }
}
