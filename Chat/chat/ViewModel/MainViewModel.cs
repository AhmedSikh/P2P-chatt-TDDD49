using System.ComponentModel;
using Chat.Model;
using Chat.Core;
using System.Windows.Input;
using System;
using System.Windows;

namespace Chat.ViewModel
{

    /*
     *  
     * MainViewModel holds ChatViewModel and HistoryViewModel
     * If a connection is established show ChatView.
     * If History Tab is pressed show HistoryView.
     * 
     */
    public class MainViewModel : ObservableObject
    {
        private ConnectionModel _connection;
        private UserModel _user;
        public ChatViewModel ChatVM { get; set; }
        public HistoryViewModel HistoryVM { get; set; }
        private readonly Window window = Application.Current.MainWindow;


        public MainViewModel()
        {
            _user = new UserModel
            {
                Username = "john",
                Port = 3004,
                Addres = "127.0.0.1"
            };

            _connection = new ConnectionModel(_user);

            ConnectionModel.ShowMessageBox = (a, b) =>
                Application.Current.Dispatcher.Invoke(delegate { _ = MessageBox.Show(a, b); });

            ConnectionModel.Buzz = () => 
                Application.Current.Dispatcher.Invoke(delegate { Buzz(); });

            ConnectionModel.SwitchToChatView = () => 
                Application.Current.Dispatcher.Invoke(delegate { SwitchToChatView(); });

            ConnectionModel.RequestResponse = (a, b, c) => 
                Application.Current.Dispatcher.Invoke(delegate { RequestResponse(a, b, c); });
        }


        public UserModel User => _user;

        public ConnectionModel Connection => _connection;

        private ICommand _connect;
        public ICommand Connect => _connect ?? (_connect =
                new RelayCommand(Connection.ExecuteConnect, CanExecuteMethod));

        private ICommand _listen;
        public ICommand Listen => _listen ?? (_listen =
                new RelayCommand(Connection.ExecuteListen, CanExecuteMethod));

        private ICommand _history;
        public ICommand History => _history ?? (_history =
                new RelayCommand(EnableHistoryView, CanExecuteMethod));


        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        internal void OnWindowClosing(object sender, CancelEventArgs e)
        {
            _connection.AnnounceDisconnect();
        }


        public void ShowMessage(string text, string tabtext = "")
        {
            MessageBox.Show(text, tabtext);
        }


        // used in _connection.ProcessPacket
        // When a host gets a connection request from a user
        // "Accept Connection?" Yes/No
        public void RequestResponse(string text, string from, string tabtext = "")
        {
            MessageBoxResult result = MessageBox.Show(text, tabtext, MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Connection.AcceptConnection(_user.Username);
                    SwitchToChatView();
                    _connection._history.Init(_user.Username, from);
                    break;
                case MessageBoxResult.No:
                    Connection.DenyConnection();
                    break;
            }
        }


        private object _chatView;
        public object ChatView
        {
            get => _chatView;
            set
            {
                _chatView = value;
                OnPropertyChanged();
            }
        }

        // When a connection is established create the ChatView
        public void SwitchToChatView()
        {
            ChatVM = new ChatViewModel(_user, _connection);
            ChatView = ChatVM;
        }


        private object _historyView;
        public object HistoryView
        {
            get => _historyView;
            set
            {
                _historyView = value;
                OnPropertyChanged();
            }
        }


        /*
         * When the user presses the History Tab
         * The Visibility variable in the MainWindow for
         * HistoryView will be set to visible.
         * When we press another Tab, for example Join
         * Visibility will be set to collapsed
         */
        private void EnableHistoryView(object param)
        {
            HistoryVM = new HistoryViewModel(_user);
            HistoryView = HistoryVM;
        }


        public void Buzz()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                int defaultLeft = (int)window.Left;
                int shake = 75;

                Random Random = new Random();

                for (int t = 150; t >= 0; t--)
                {
                    window.Left = Random.Next(defaultLeft, defaultLeft + shake);
                }
                window.Left = defaultLeft;
            });
        }
    }
}
