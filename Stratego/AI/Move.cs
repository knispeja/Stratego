using System;

namespace Stratego
{
    public class Move
    {
        public static Move NullMove = new Move();
        private BoardPosition start;
        private BoardPosition end;

        public Move()
        {
        }

        public Move(int x1, int y1, int x2, int y2)
        {
            this.start = new BoardPosition(x1, y1);
            this.end = new BoardPosition(x2, y2);
        }

        public Move(BoardPosition start, BoardPosition end)
        {
            this.start = start;
            this.end = end;
        }

        public BoardPosition getStart()
        {
            return this.start;
        }

        public BoardPosition getEnd()
        {
            return this.end;
        }
    }
}