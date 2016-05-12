using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    [Serializable]
    public class SetupData
    {
        [NonSerialized]
        private Dictionary<String, int> placements;

        private string placementsString;

        public Gameboard boardState { get; private set; }
        public int minPieces { get; private set; }
        public int turn { get; private set; }

        public SetupData(Gameboard boardState, Dictionary<String, int> placements, int minPieces, int turn)
        {
            this.boardState = boardState;
            if (turn == StrategoGame.RED_TEAM_CODE)
                this.boardState.flipBoard();

            this.minPieces = minPieces;
            this.turn = turn;
            this.placements = placements;
            this.placementsString = convertDictionaryToString(this.placements);
        }

        public Dictionary<string, int> getPlacementsDictionary()
        {
            if (this.placements == null && !String.IsNullOrEmpty(this.placementsString))
                this.placements = convertStringToDictionary(this.placementsString);

            return this.placements;
        }

        private static Dictionary<string, int> convertStringToDictionary(string s)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            // Divide all pairs (remove empty strings)
            string[] tokens = s.Split(new char[] { ':', ',' },
                StringSplitOptions.RemoveEmptyEntries);

            // Walk through each item
            for (int i = 0; i < tokens.Length; i += 2)
            {
                string name = tokens[i];
                string freq = tokens[i + 1];

                // Parse the int (this can throw)
                int count = int.Parse(freq);

                // Fill the value in the sorted dictionary
                if (dict.ContainsKey(name))
                    dict[name] += count;
                else
                    dict.Add(name, count);
            }

            return dict;
        }

        private static string convertDictionaryToString(Dictionary<string, int> d)
        {
            // Build up each line one-by-one and then trim the end
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, int> pair in d)
            {
                builder.Append(pair.Key).Append(":").Append(pair.Value).Append(',');
            }
            string s = builder.ToString();
            // Remove the final delimiter
            s = s.TrimEnd(',');
            return s;
        }
    }
}
