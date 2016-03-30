using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class BoardPosition
    {
        public static readonly BoardPosition NULL_BOARD_POSITION = new BoardPosition(-1, -1);

        private int x;
        private int y;
        public BoardPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public bool isNull()
        {
            return this.Equals(BoardPosition.NULL_BOARD_POSITION);
        }

        public override bool Equals(Object other)
        {
            if (other == null)
                return false;

            if (other.GetType() != this.GetType())
                return false;

            BoardPosition otherBP = (BoardPosition)other;
            return this.x == otherBP.getX() && this.y == otherBP.getY();
        }
    }
}
