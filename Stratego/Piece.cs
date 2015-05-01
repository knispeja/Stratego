using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 0 - Blank
 * 1-9 Normal Pieces
 * 10 - Spy
 * 11 - Bomb
 * 12 - Flag
 * 42 = place you cant move (lakes)
 */
namespace Stratego
{
    public static class Piece
    {
        private static readonly string[] names= new string[] {"Empty", "Marshall", "General", "Colonel", "Major", "Captain", "Lieutenant",
                                    "Sergeant", "Miner", "Scout", "Spy", "Bomb", "Flag"};
        public static int? attack(int first, int second)
        {
            if((Math.Sign(first) == Math.Sign(second) || first == 42 || second == 42) && first != 0 && second != 0) return null;
            

            if (Math.Abs(first) == 11)
            {
            if (Math.Abs(second) != 8)
                return first;
            else
                return second;
            }
            else if (Math.Abs(second) == 11)
            {
                if (Math.Abs(first) != 8)
                    return second;
                else
                    return first;
            }
            if (first == 0)
                return second;
            if (second == 0)
                return first;
            if ((Math.Abs(first) == 1 || Math.Abs(second) == 1) && (Math.Abs(first) == 10 || Math.Abs(second) == 10))
                return first;
            if (Math.Abs(first) < Math.Abs(second))
                return first;
            else if (Math.Abs(first) > Math.Abs(second))
                return second;
            else return 0;
        }

        public static string toString(int pieceNumber)
        {
            if (pieceNumber > 12 || pieceNumber < -12||(pieceNumber>-1&&pieceNumber<1))
            {
                throw new ArgumentException();
            }
            return names[Math.Abs(pieceNumber)];
        }
    }
}
