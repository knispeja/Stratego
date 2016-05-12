using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    [Serializable]
    public class SaveData
    {
        public Gameboard boardState { get; private set; }
        public int difficulty { get; private set; }
        public int turn { get; private set; }
        public bool isSinglePlayer { get; private set; }
        public int level { get; private set; }

        public SaveData(Gameboard boardState, int difficulty, int turn, bool isSinglePlayer, int level)
        {
            this.boardState = boardState;
            this.difficulty = difficulty;
            this.turn = turn;
            this.isSinglePlayer = isSinglePlayer;
            this.level = level;
        }
    }
}
