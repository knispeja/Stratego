namespace Stratego
{
    using System;
    using Stratego.AI;
    public class AIv2
    {
        private Gameboard board;
        private AIGameboard internalBoard;
        private int difficulty;
        private GameTree gameTree;

        public AIv2(Gameboard initBoard, int difficulty)
        {
            this.board = initBoard;
            this.difficulty = difficulty;
        }

        public void takeTurn()
        {
            this.board.move(this.chooseMove());
        }

        public Move chooseMove()
        {
            BoardPosition start;
            BoardPosition end;
            for (int i = 0; i < board.getWidth(); i++)
            {
                for (int j = 0; j < board.getHeight(); j++)
                {
                    start = new BoardPosition(i, j);
                    GamePiece piece = board.getPiece(start);
                    if (piece != null)
                    {
                        int[,] moves = board.getPieceMoves(i, j);
                        for (int k = 0; k < board.getWidth(); k++)
                        {
                            for (int l = 0; l < board.getHeight(); l++)
                            {
                                end = new BoardPosition(k, l);
                                if (moves[k, l] == 1)
                                    return new Move(start, end);
                            }
                        }
                    }
                }
            }
            return Move.NULL_MOVE;
            /*
            // TODO: thread this so length depends on difficulty
            Move move = Move.NULL_MOVE;
            for (int i = 0; i < 999999; i++)
            {
                runMTCS(internalBoard, this.gameTree.getRoot());
                move = getMostPromisingMove();
            }
            this.gameTree.setRoot(this.gameTree.getRoot().getChildFromMove(move));
            return move;*/
        }

        public bool runMTCS(AIGameboard internalBoard, GameNode node)
        {
            if (internalBoard.isGameOver())
            {
                node.incrementOutcomes();
                if (internalBoard.isWin())
                {
                    node.incrementWins();
                    return true;
                }
                return false;
            }
            Move move = selectMove();
            //bool won = runMTCS(internalBoard.move(move), node.getChildFromMove(move));

            return false;//return won;
        }

        private Move selectMove()
        {
            throw new NotImplementedException();
        }

        public Move getMostPromisingMove()
        {
            return GameTree.generateMove(this.gameTree.getRoot(), this.gameTree.getMostPromisingChild());
        }
    }
}