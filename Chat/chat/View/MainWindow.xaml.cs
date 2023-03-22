using System.Windows;
using Chat.ViewModel;

namespace Chat
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

       public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();

            //The DataContext serves as the starting point of Binding Paths
            DataContext = _viewModel;

            Closing += _viewModel.OnWindowClosing;
        }
    }
}
