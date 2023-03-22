using System;
using System.ComponentModel;

namespace Chat.Model
{
    /*
     * 
     * This class is used by Connection.Send for 
     * communication between Listen and Connect
     * 
     */
    public class MessageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
      
        public MessageModel(string content, int reqType, string fromUser)
        {
            _content = content;
            _reqType = reqType;
            _fromUser = fromUser;
            _date = DateTime.Now;
        }

        private int _reqType;
        public int ReqType
        {
            get => _reqType;
            set
            {
                _reqType = value;
                OnPropertyChanged("ReqType");
            }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }


        private string _fromUser;
        public string FromUser
        {
            get => _fromUser;
            set
            {
                _fromUser = value;
                OnPropertyChanged("Content");
            }
        }


        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Content");
            }
        }


        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
