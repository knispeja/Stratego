//TODO: Run VS's auto fixer because this was all written manually

namespace Stratego
{
    public class AIv2
    {
        private Gameboard board;
        private AIGameboard internalBoard;
        private int difficulty;
        private GameTree gameTree;
        
        public AIv2(Gameboard board, int difficulty){
            this.board = board;
            this.difficulty = difficulty;
        }
        
        public void takeTurn(){
            this.board.move(this.chooseMove());
        }
        
        public Move chooseMove()
        {
            // TODO: thread this so length depends on difficulty
            Move move;
            for(int i = 0; i < 999999; i++){
                runMTCS(internalBoard, this.gameTree.getRoot());
                move = getMostPromisingMove();
            }
            return move;
        }
        
        public bool runMTCS(AIGameBoard internalBoard, GameNode node){
            if(internalBoard.isGameOver()){
                node.incrementOutcomes();
                if(internalBoard.isWin())
                {
                    node.incrementWins();
                    return true;
                }
                return false;
            }
            else{
                // Move move = selectMove();
                // Node newNode = node.getChildFromMove(move);
                // bool won = runMTCS(internalBoard.move(move), )
            }
            
        }
        
        public Move getMostPromisingMove(){
            return GameTree.getMostPromisingChild();
        }
    }
}