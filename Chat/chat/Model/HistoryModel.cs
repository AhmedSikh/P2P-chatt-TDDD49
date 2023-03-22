using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chat.Model
{
    /*
     * 
     * This class takes care of Loading previous conversation into program memory (history)
     * Creating those conversation files if they don't already exist
     * And saving the new messages upon exit.

     * Each instance will create it's own file this will result in two files
     * with the same Messages but slightly different file names.

     * For example if John was to talk with Bobby
     * The John instance will create a John_Bobby.json
     * And the Bobby instance will create a Bobby_John.json
     * This is done by design as in reality this program would not be ran
     * on the same computer with two instance but on two different computers.
     *
     */
    public class HistoryModel
    {
        private JsonFile history = new JsonFile();
        private string currFileLocation;
        public static Action<string> MessageAdd { get; set; }

        public HistoryModel() { }

        /*
         * 
         * Get the path of History File
         * If a conversation between hostUser and username (john_bobby.json) dosen't exist
         * create the file and initialize a JsonFile object.
         * Else if the converstation exists, (john_bobby.json)
         * read the file in the history variable
         * 
         */
        public void Init(string hostUser, string username)
        {
            currFileLocation = GetPath(hostUser, username);
            if (!File.Exists(currFileLocation))
            {
                _ = File.Create(currFileLocation);
                history = new JsonFile
                {
                    Messages = new List<Msg>()
                };
            }
            else
            {
                using (StreamReader file = File.OpenText(currFileLocation))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    history = (JsonFile)serializer.Deserialize(file, typeof(JsonFile));
                }
            }
        }


        // Used only in HistoryViewModel SelectedItem
        public void GetMessages(string hostUser, string username)
        {
            Init(hostUser, username);

            if (history.Messages.Count != 0)
            {
                for (int i = 0; i < history.Messages.Count; i++)
                {
                    string msg = $"[{history.Messages[i].Date} " +
                                 $"{history.Messages[i].FromUser}]: " +
                                 $"{history.Messages[i].Content}";

                    MessageAdd?.Invoke(msg);
                }
            }
        }


        // Used only by Connection.Send. Save message in history
        public void SaveMsg(MessageModel packet)
        {
            Msg new_msg = new Msg
            {
                Content = packet.Content,
                Date = packet.Date.ToString(),
                FromUser = packet.FromUser
            };

            history.Messages.Add(new_msg);
        }

        /*
         * 
         * Triggered by MainViewModel.OnWindowClose and
         * executed by ConnectionModel.ProcessPacket case 3
         * Saves all mesages to disk
         * 
         */
        public void SaveToFile()
        {
            using (StreamWriter file = File.CreateText(currFileLocation))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, history);
            }
        }


        /*
         * 
         * Gets the path of the conversation
         * assuming the program in started from 
         * tddd49\Chat\Chat\bin\Debug
         * it will look for a History folder that
         * should be created ahead of time in Bin
         * 
         */
        private string GetPath(string hostUser, string username)
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = Path.Combine(sCurrentDirectory, @"..\History\");
            string sFilePath = Path.GetFullPath(sFile);
            string fileLocation = $@"{sFilePath}{hostUser}_{username}.json";
            return fileLocation;
        }
    }

    // Used by Json sterilizer
    public class JsonFile
    {
        public List<Msg> Messages { get; set; }
    }


    public class Msg
    {
        public string Date { get; set; }
        public string Content { get; set; }
        public string FromUser { get; set; }
    }
}
