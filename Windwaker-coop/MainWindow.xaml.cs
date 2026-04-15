using System.Windows;
using Windwaker_coop.ViewModels;

namespace Windwaker_coop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Closing += (_, _) => Program.Shutdown();
        }
    }
}
