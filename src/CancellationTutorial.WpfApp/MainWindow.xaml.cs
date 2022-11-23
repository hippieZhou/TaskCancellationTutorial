using CommunityToolkit.Mvvm.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CancellationTutorial.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => { });

            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<MainWindowViewModel>();
        }
    }
}
