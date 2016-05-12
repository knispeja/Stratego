namespace Stratego
{
    public class AIv2
    {
        private Gameboard board;
        private int difficulty;
        private Move moveToMake;

        public AIv2(Gameboard initBoard, int difficulty)
        {
            this.board = initBoard;
            this.difficulty = difficulty;
        }

        public void takeTurn()
        {
            this.findMove();
            this.board.move(this.getMoveToMake());
        }

        public Move getMoveToMake()
        {
            return this.moveToMake;
        }

        private void findMove()
        {
            BoardPosition start;
            BoardPosition end;
            this.moveToMake = Move.NULL_MOVE;
            for (int i = 0; i < board.getWidth(); i++)
            {
                for (int j = 0; j < board.getHeight(); j++)
                {
                    start = new BoardPosition(i, j);
                    GamePiece piece = board.getPiece(start);
                    if (piece != null && piece.getTeamCode() == -1)
                    {
                        int[,] moves = board.getPieceMoves(i, j);
                        for (int k = 0; k < board.getWidth(); k++)
                        {
                            for (int l = 0; l < board.getHeight(); l++)
                            {
                                end = new BoardPosition(k, l);
                                if (moves[k, l] == 1)
                                    this.moveToMake = new Move(start, end);
                            }
                        }
                    }
                }
            }            
        }
    }
}