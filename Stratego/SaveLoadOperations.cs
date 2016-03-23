using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Stratego
{
    class SaveLoadOperations
    {
        public static bool saveGame(SaveData saveData)
        {
            FileDialog dialog = new SaveFileDialog();

            if (displayFileDialog(dialog) == DialogResult.OK)
            {
                storeSaveData(dialog.FileName, saveData);
                return true;
            }

            return false;
        }

        public static SaveData? loadGame()
        {
            FileDialog dialog = new OpenFileDialog();

            // Only return normally if the user didn't cancel out of the menu
            if (displayFileDialog(dialog) == DialogResult.OK)
            {
                // Check if the file is present already
                Stream file;
                if ((file = ((OpenFileDialog)dialog).OpenFile()) != null)
                {
                    file.Close();
                    return loadSaveData(dialog.FileName);
                }
            }

            return null;
        }

        private static DialogResult displayFileDialog(FileDialog dialog)
        {
            dialog.Filter = "strat files (*.strat)|*.strat|All files (*.*)|*.*";
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            if (path.EndsWith("\\bin\\Debug") || path.EndsWith("\\bin\\Release"))
            {
                for (int i = 0; i < path.Length - 3; i++)
                {
                    if ((path[i] == '\\') && (path[i + 1] == 'b') && (path[i + 2] == 'i') && (path[i + 3] == 'n'))
                    {
                        path = path.Substring(0, i);
                        break;
                    }
                }
            }
            dialog.InitialDirectory = System.IO.Path.Combine(path, @"Resources\SaveGames"); // TODO: lots of hardcoded file path crap in here, fix it
            dialog.RestoreDirectory = true;

            return dialog.ShowDialog();
        }

        private static void storeSaveData(string fileName, SaveData saveData)
        {
            string buffer = saveData.isSinglePlayer ? "1 " + saveData.difficulty : "0";

            StreamWriter writer = new StreamWriter(fileName);
            writer.WriteLine(saveData.turn + " " + buffer);
            for (int i = 0; i < saveData.boardState.GetLength(1); i++)
            {
                buffer = "";
                for (int j = 0; j < saveData.boardState.GetLength(0) - 1; j++)
                    buffer += saveData.boardState[j, i] + " ";

                buffer += saveData.boardState[saveData.boardState.GetLength(0) - 1, i];
                writer.WriteLine(buffer);
            }

            writer.Close();
            return;
        }

        public static SaveData loadSaveData(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            Console.WriteLine(fileName);
            string[] lines = new string[100]; // TODO: Make a Standard max size later
            string line = reader.ReadLine();
            lines[0] = line;
            int i = 0;
            while (line != null) 
            {
                lines[i] = line;
                line = reader.ReadLine();
                i++;
            }

            int difficulty = 5;
            string[] numbers = lines[0].Split(' ');
            int turn = -2*Convert.ToInt32(numbers[0]);
            bool isSinglePlayer;
            if (numbers[1] == "0")
                isSinglePlayer = false;
            else
            {
                isSinglePlayer = true;
                difficulty = Convert.ToInt32(numbers[2]);
            }

            numbers = lines[1].Split(' ');
            int[,] newBoard = new int[numbers.Length, i - 1];
            for (int k = 1; k< i; k++ )
            {
                numbers = lines[k].Split(' ');
                for(int j =0; j< numbers.Length; j++)
                {
                    newBoard[j, k-1] = Convert.ToInt32(numbers[j]);
                }
            }
            
            reader.Close();

            return new SaveData(
                    newBoard,
                    difficulty,
                    turn,
                    isSinglePlayer
                );
        }
    }
}
