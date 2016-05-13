using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Stratego
{
    public class SaveLoadOperations
    {
        public static readonly string SAVE_FILE_EXTENSION = "strat";
        public static readonly string SETUP_FILE_EXTENSION = "stgostup";

        public static bool saveSetup(SetupData setupData)
        {
            return saveSomething(setupData, SETUP_FILE_EXTENSION);
        }

        public static bool saveGame(SaveData saveData)
        {
            return saveSomething(saveData, SAVE_FILE_EXTENSION);
        }

        private static bool saveSomething(Object data, string fileExtension)
        {
            FileDialog dialog = new SaveFileDialog();

            if (displayFileDialog(dialog, fileExtension) == DialogResult.OK)
            {
                storeData(dialog.FileName, data);
                return true;
            }

            return false;
        }

        public static SetupData loadSetup()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (displayFileDialog(dialog, SETUP_FILE_EXTENSION) == DialogResult.OK)
            {
                // Check if the file is present already
                Stream file;
                if ((file = dialog.OpenFile()) != null)
                {
                    file.Close();
                    return loadSetupData(dialog.FileName);
                }
            }

            return null;
        }

        public static SaveData loadGame()
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

        public static void storeData(string fileName, Object data)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static SaveData loadSaveData(string fileName)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                SaveData data = (SaveData)formatter.Deserialize(stream);
                stream.Close();
                return data;
            }
            catch
            {
                MessageBox.Show("WARNING: Save file at '" + fileName + "' is corrupt or missing");
                return null;
            }
        }

        public static SetupData loadSetupData(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            SetupData data = (SetupData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }

        public static void updateOldSavefile()
        {
            FileDialog dialog = new SaveFileDialog();
            GamePieces.GamePieceFactory fact = new GamePieces.GamePieceFactory();

            if (displayFileDialog(dialog, SAVE_FILE_EXTENSION) == DialogResult.OK)
            {
                System.IO.StreamReader file = new System.IO.StreamReader(dialog.FileName);

                string line = file.ReadLine();
                //string[] firstLine = line.Split(new char[] { ' ' });
                Gameboard board = null;

                int row = 0;
                while ((line = file.ReadLine()) != null)
                {
                    string[] splitLine = line.Split(new char[] { ' ' });
                    if (board == null)
                        board = new Gameboard(splitLine.Length, splitLine.Length);

                    int col = 0;
                    foreach (string s in splitLine)
                    {
                        int pieceNum = Int32.Parse(s);
                        board.setPiece(col, row, fact.getPiece(Math.Abs(pieceNum), Math.Sign(pieceNum)));
                        col++;
                    }
                    row++;
                }

                file.Close();

                throw new NotImplementedException(); 
                /* Hi there! Before using this method, replace 0 in the below method call with
                 * the level number of the file that you are converting. If it is just a normal
                 * preset level file, make the level -1 (I think, unless we change that...) */
                storeData(dialog.FileName + "_new", new SaveData(board, 5, 1, true, 0));
            }
        }

    }
}
