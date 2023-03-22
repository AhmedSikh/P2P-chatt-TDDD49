using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Chat.Model;


namespace Chat.ViewModel
{
    /*
     * 
     * HistoryViewModel uses HistorySearch to find all users MyUsername has chatted with.
     * It will display them in UserList after GetUsersIConversed command is executed.
     * 
     * Use HistoryModel.GetMessages once a user is selected from UserList
     * in order to display the conversation.
     * 
     */
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private readonly UserModel _user;
        private readonly HistorySearch _search;
        private readonly HistoryModel _history;

        public ObservableCollection<string> UsersList { get; set; }
        public ObservableCollection<string> UserMessages { get; set; }
        public string Search { get; set; }

        public HistoryViewModel(UserModel user)
        {
            _user = user;
            _history = new HistoryModel();
            _search = new HistorySearch(_user.Username);
            UsersList = new ObservableCollection<string>();
            UserMessages = new ObservableCollection<string>();

            HistoryModel.MessageAdd = (c) =>
                Application.Current.Dispatcher.Invoke(delegate { UserMessages.Add(c); });

            HistorySearch.UsersListAdd = (c) =>
                Application.Current.Dispatcher.Invoke(delegate { UsersList.Add(c); });
        }


        private ICommand _getUsersIConversed;
        public ICommand GetUsersIConversed =>
                _getUsersIConversed ?? (_getUsersIConversed = new RelayCommand(ExecGetUsersIConversed, CanExecuteMethod));

        private ICommand _searchUsers;
        public ICommand SearchUsers => _searchUsers ?? (_searchUsers = new RelayCommand(ExecSearchUsers, CanExecuteMethod));


        private void ExecGetUsersIConversed(object obj)
        {
            UsersList.Clear();
            _search.GetUsersIConversed(MyUsername);
        }


        private void ExecSearchUsers(object obj)
        {
            // we need to call ExecGetUsersIConversed() after every search
            // because search changes the UsersList with every search so the
            // list gets smaller and smaller.
            ExecGetUsersIConversed(obj);
            _search.Search(MySearch, UsersList);
        }


        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        private string _username;
        public string MyUsername
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }


        public string _mySearch;
        public string MySearch
        {
            get => _mySearch;
            set
            {
                _mySearch = value;
                OnPropertyChanged();
            }
        }


        // Called when an user is selected from the UserList
        private string _selectedItem;
        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem)
                {
                    return;
                }
                _selectedItem = value;

                OnPropertyChanged();
                UserMessages.Clear();
                // if an item is selected in UserLists and the user changes the USERNAME field
                // and presses GO the program will crash, the catch takes care of that.
                try
                {
                    _history.GetMessages(_search.usersAndHost[_selectedItem], _selectedItem);
                }
                catch (ArgumentNullException) { }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
