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
        public static void displaySaveDialog(SaveData saveData)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
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

            if (dialog.ShowDialog() == DialogResult.OK)
                saveGame(dialog.FileName, saveData);
        }

        private static void saveGame(string fileName, SaveData saveData)
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
    }
}
