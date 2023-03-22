using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chat.Model
{
    /*
     * 
     * UserModel class is used by MainViewModel to hold user data.
     * 
     */
    public class UserModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int _port;
        private string _addres;

        public UserModel() { }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        public string Addres
        {
            get => _addres;
            set
            {
                _addres = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
