using System;

namespace Stratego
{
    public class Move
    {
        public static readonly Move NULL_MOVE = new Move();

        private BoardPosition start;
        private BoardPosition end;

        public Move()
        {
            this.start = BoardPosition.NULL_BOARD_POSITION;
            this.end = BoardPosition.NULL_BOARD_POSITION;
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

        public bool isNull()
        {
            return this.Equals(Move.NULL_MOVE);
        }

        public override bool Equals(Object other)
        {
            if (other == null)
                return false;

            if (other.GetType() != this.GetType())
                return false;

            Move otherMove = (Move)other;
            return this.start.Equals(otherMove.getStart()) && this.end.Equals(otherMove.getEnd());
        }
    }
}