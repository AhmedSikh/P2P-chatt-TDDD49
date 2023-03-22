using Chat.Core;
using Chat.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System;

namespace Chat.ViewModel
{
    /*
     * 
     * ChatViewModel holds the commands for SEND 
     *      which sends a message to the other instance using Connection and
     *      displays it in our instance.
     * and BUZZ
     *      which also sends a buzz message to the other instance
     *      
     */
    public class ChatViewModel : ObservableObject
    {
        private readonly UserModel _user;
        public ConnectionModel Connection { get; }
        public string Message { get; set; }
        public ObservableCollection<string> Messages { get; set; }


        public ChatViewModel(UserModel user, ConnectionModel connection)
        {
            _user = user;
            Connection = connection;
            Messages = new ObservableCollection<string>();
            ConnectionModel.MessageAdd = (c) => Application.Current.Dispatcher.Invoke(delegate { Messages.Add(c); });
        }


        private ICommand _send;
        public ICommand Send => _send ?? (_send = new RelayCommand(SendMsg, CanExecuteMethod));

        private ICommand _buzzCommand;
        public ICommand BuzzCommand => _buzzCommand ?? (_buzzCommand = new RelayCommand(SendBuzzReq, CanExecuteMethod));

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        // Create message and display it on this instance
        // Send message to the other instance
        private void SendMsg(object obj)
        {
            string msg = $"[{DateTime.Now} {_user.Username}]: {Message}";
            Messages.Add(msg);

            Connection.Send(Message, 1, _user.Username);
            Message = "";
            OnPropertyChanged(nameof(Message));
        }


        private void SendBuzzReq(object obj)
        {
            Connection.Send("", 4, _user.Username);
        }
    }
}
