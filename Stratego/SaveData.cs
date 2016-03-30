using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Simple data structure to hold any save data passed to and from save/load operations
    /// </summary>
    public struct SaveData
    {
        public GamePiece[,] boardState { get; private set; }
        public int difficulty { get; private set; }
        public int turn { get; private set; }
        public bool isSinglePlayer { get; private set; }

        public SaveData(GamePiece[,] boardState, int difficulty, int turn, bool isSinglePlayer) : this()
        {
            this.boardState = boardState;
            this.difficulty = difficulty;
            this.turn = turn;
            this.isSinglePlayer = isSinglePlayer;
        }
    }
}
