using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CoreLib;
using Microsoft.Extensions.Logging;

namespace FrondendApp
{
    public partial class MainWindow : Window
    {
        private readonly MyHttpClient _client;
        private readonly ILogger<MainWindow> _logger;
        public MainWindow()
        {
            _client = Ioc.Default.GetRequiredService<MyHttpClient>();
            _logger = Ioc.Default.GetRequiredService<ILogger<MainWindow>>();
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                htmlTb.Text = _client.GetHtmlString();
                _logger.LogInformation("Down");
            });
        }

        private void ButtonBase_OnClickAsync(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(async () =>
            {
                htmlTb.Text = await _client.GetHtmlStringAsync();
                _logger.LogInformation("Async Down");
            });
        }
    }
}
