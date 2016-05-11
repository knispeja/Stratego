using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.AI
{
    public class GameNode {
        private int wins;
        private int outcomes;
        public void incrementWins()
        {
            this.wins++;
        }
        public void incrementOutcomes()
        {
            this.outcomes++;
        }

        public GameNode getChildFromMove(Move move)
        {
            throw new NotImplementedException();
        }
    }
    public class GameTree
    {
        private GameNode root;
        public GameNode getRoot()
        {
            return this.root;
        }

        public GameNode getMostPromisingChild()
        {
            return null;
        }

        public void setRoot(GameNode newRoot)
        {
            this.root = newRoot;
        }

        public static Move generateMove(GameNode gameNode1, GameNode gameNode2)
        {
            throw new NotImplementedException();
        }
    }
}
