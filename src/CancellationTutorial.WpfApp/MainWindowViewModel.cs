using CancellationTutorial.CoreLib;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CancellationTutorial.WpfApp
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly IRepository _repository;
        private readonly ILogger<MainWindowViewModel> _logger;

        private CancellationTokenSource? cts;


        public MainWindowViewModel(
            HttpClient httpClient,
            IRepository repository,
            ILogger<MainWindowViewModel> logger)
        {
            _httpClient = httpClient;
            _repository = repository;
            _logger = logger;
        }

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string? json;

        [ObservableProperty]
        private SolidColorBrush brash = new(Colors.Transparent);

        [RelayCommand]
        private void Loaded()
        {
        }

        [RelayCommand]
        private async void Send()
        {
            cts = new CancellationTokenSource();

            IsBusy = true;
            Brash = new SolidColorBrush(Colors.White);

            Json = await CallWebApiAsync(cts.Token);

            #region Other cases
            //await DoworkAsync(cts.Token).ConfigureAwait(true);
            //DoworkAsync(cts.Token).Wait();    
            #endregion

            IsBusy = false;
            Brash = new SolidColorBrush(Colors.Red);
        }

        [RelayCommand]
        private void Cancel()
        {
            cts?.Cancel();
        }

        private async Task<string> CallWebApiAsync(CancellationToken cancellation)
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5041/weatherforecastwithCancel", cancellation);
                return await response.Content.ReadAsStringAsync(cancellation);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<string> CallUnitOfWorkAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAsync(cancellationToken);
            return JsonSerializer.Serialize(entities, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        private async Task DoworkAsync(CancellationToken cancellation)
        {
            _logger.LogInformation("before task.delay");

            await Task.Delay(5000, cancellation).ConfigureAwait(false);

            _logger.LogInformation("after task.delay");
        }
    }
}
