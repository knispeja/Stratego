using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Stratego
{
    public class SaveLoadOperations
    {
        private static string SAVE_FILE_EXTENSION = "strat";
        private static string SETUP_FILE_EXTENSION = "stgostup";

        public static bool saveSetup(SetupData saveData)
        {
            FileDialog dialog = new SaveFileDialog();

            if (displayFileDialog(dialog, SETUP_FILE_EXTENSION) == DialogResult.OK)
            {
                storeSetupData(dialog.FileName, saveData);
                return true;
            }

            return false;
        }

        public static bool saveGame(SaveData saveData)
        {
            FileDialog dialog = new SaveFileDialog();

            if (displayFileDialog(dialog, SAVE_FILE_EXTENSION) == DialogResult.OK)
            {
                storeSaveData(dialog.FileName, saveData);
                return true;
            }

            return false;
        }

        public static string loadSetup()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (displayFileDialog(dialog, SETUP_FILE_EXTENSION) == DialogResult.OK)
            {
                // Check if the file is present already
                Stream file;
                if ((file = dialog.OpenFile()) != null)
                {
                    file.Close();
                    return dialog.FileName;
                }
            }

            return null;
        }

        public static SaveData? loadGame()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            // Only return normally if the user didn't cancel out of the menu
            if (displayFileDialog(dialog, SAVE_FILE_EXTENSION) == DialogResult.OK)
            {
                // Check if the file is present already
                Stream file;
                if ((file = dialog.OpenFile()) != null)
                {
                    file.Close();
                    return loadSaveData(dialog.FileName);
                }
            }

            return null;
        }

        private static DialogResult displayFileDialog(FileDialog dialog, string extension)
        {
            dialog.Filter = extension + " files (*." + extension + ")|*." + extension + "|All files (*.*)|*.*";
            dialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
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

        /// <summary>
        /// Saves the set up of the current teams pieces into a file
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public static void storeSetupData(string fileName, SetupData data)
        {
            StreamWriter writer = new StreamWriter(fileName);

            string buffer = "";

            if (data.turn > 0)
            {
                for (int i = 6; i < 10; i++)
                {
                    for (int j = 0; j < 9; j++)
                        buffer += data.boardState[j, i] + " ";

                    buffer += data.boardState[9, i];
                    writer.WriteLine(buffer);
                    buffer = "";
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 9; j++)
                        buffer += Math.Abs(data.boardState[9 - j, 3 - i]) + " ";

                    buffer += Math.Abs(data.boardState[0, 3 - i]);
                    writer.WriteLine(buffer);
                    buffer = "";
                }
            }

            writer.Close();

            return;
        }
    }
}
