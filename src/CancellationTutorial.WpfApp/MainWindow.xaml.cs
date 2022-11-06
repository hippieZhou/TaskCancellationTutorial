using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;

namespace CancellationTutorial.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<MainWindowViewModel>();
        }
    }
}
