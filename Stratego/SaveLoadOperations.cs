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
        private static string SAVE_FILE_EXTENSION = "strat";
        private static string SETUP_FILE_EXTENSION = "stgostup";

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
                    return dialog.FileName; //TODO: some stuff is still not extracted into this file
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

        private static void storeData(string fileName, Object data)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static SaveData loadSaveData(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            SaveData data = (SaveData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
    }
}
