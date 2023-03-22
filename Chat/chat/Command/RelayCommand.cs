using System;
using System.IO;
using System.Windows.Input;

namespace Chat
{
    public class RelayCommand : ICommand
    {
        // Action deligate, function to be executed
        Action<object> executeMethod;

        // Func deligate, condition for the function to be executed
        Func<object, bool> canExecuteMethod;
        private Action enableHistoryView;
        private Func<FileInfo[]> getUsersIConversed;

        public RelayCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public RelayCommand(Action enableHistoryView, Func<object, bool> canExecuteMethod)
        {
            this.enableHistoryView = enableHistoryView;
            this.canExecuteMethod = canExecuteMethod;
        }

        public Action<string> Send { get; }

        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }

    }
}
