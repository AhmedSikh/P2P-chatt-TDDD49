using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chat.Model
{
    /*
     * 
     * This class handles the search functionality for HistoryViewModel
     * 
     */
    public class HistorySearch
    {
        private string _username;
        private readonly string _path;
        public Dictionary<string, string> usersAndHost;
        public static Action<string> UsersListAdd { get; set; }

        public HistorySearch(string username)
        {
            _username = username;
            _path = GetPath();
        }

        // Gets the path to History FOLDER
        private string GetPath()
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = Path.Combine(sCurrentDirectory, @"..\History\");
            string sFilePath = Path.GetFullPath(sFile);
            return sFilePath;
        }


        /*
         *
         * If my username is John this function will find all files 
         * that start with "John_*" the * is the other user John chatted with.
         * 
         * Sort the files by creation time descending order
         * 
         * save the names of users john chatted with in a dictonary
         *     "john" = "someuser"
         *     
         */
        public void GetUsersIConversed(string username)
        {
            _username = username;
            usersAndHost = new Dictionary<string, string>();
            DirectoryInfo dir = new DirectoryInfo(_path);
            FileInfo[] files = dir.GetFiles($"{_username}_*.json");

            IOrderedEnumerable<FileInfo> orderedList = from f in dir.EnumerateFiles($"{_username}_*.json")
                                                       orderby f.LastWriteTime descending
                                                       select f;

            string pattern = @"(\w+)_(\w+)";
            Regex rg = new Regex(pattern);
            foreach (FileInfo element in orderedList)
            {
                string filename = element.ToString(); //john_bobi.json
                Match usermatch = rg.Match(filename);
                string host = usermatch.Groups[1].Value; //john
                string user = usermatch.Groups[2].Value; //bobi

                usersAndHost[user] = host;
                UsersListAdd?.Invoke(user);
            }
        }


        /*
         * 
         * Create a copy of the original ObservableCollection UserList
         * That way we can clear the list while working on the copy
         * The sorted result will be displayd in UserList.
         * 
         */
        public void Search(string mySearch, ObservableCollection<string> usersList)
        {
            List<string> copy = new List<string>();
            foreach (string element in usersList)
            {
                copy.Add(element);
            }

            usersList.Clear();

            string[] match = copy.Where(x => x.Contains(mySearch)).ToArray();
            foreach (string element in match)
            {
                usersList.Add(element);
            }
        }
    }
}
