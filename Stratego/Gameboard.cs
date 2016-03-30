using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class Gameboard
    {
        private int width;
        private int height;

        public Gameboard(int v1, int v2)
        {
            this.width = v1;
            this.height = v2;
        }

        public void move(Move move)
        {
            throw new NotImplementedException();
        }

        public int getHeight()
        {
            return this.height;
        }

        public int getWidth()
        {
            return this.width;
        }

        internal GamePiece getPiece(int x, int y)
        {
            throw new NotImplementedException();
        }

        internal void setPiece(int x, int row, GamePiece value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given row in the board state with the given value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        public void fillRow(GamePiece value, int row)
        {
            for (int x = 0; x < this.width; x++) setPiece(x, row, value);
        }


    }
}
