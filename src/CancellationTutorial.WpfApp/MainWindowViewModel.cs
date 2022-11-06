using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTutorial.WpfApp
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MainWindowViewModel> _logger;

        private CancellationTokenSource? cts;


        public MainWindowViewModel(HttpClient httpClient, ILogger<MainWindowViewModel> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string? json;

        [RelayCommand]
        private void Loaded()
        {
        }

        [RelayCommand]
        private async void Send()
        {
            IsBusy = true;
            cts = new CancellationTokenSource();
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5041/weatherforecastwithCancel", cts.Token);
                response.EnsureSuccessStatusCode();
                Json = await response.Content.ReadAsStringAsync(cts.Token);
            }
            catch (TaskCanceledException ex)
            {
                Json = ex.Message;
                _logger.LogError(ex, "Cancel request");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            cts?.Cancel();
        }
    }
}
