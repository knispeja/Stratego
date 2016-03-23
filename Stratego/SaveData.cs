using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    struct SaveData
    {
        public int[,] boardState { get; private set; }
        public int difficulty { get; private set; }
        public int turn { get; private set; }
        public bool isSinglePlayer { get; private set; }

        public SaveData(int[,] boardState, int difficulty, int turn, bool isSinglePlayer)
        {
            this.boardState = boardState;
            this.difficulty = difficulty;
            this.isSinglePlayer = isSinglePlayer;
        }
    }
}
