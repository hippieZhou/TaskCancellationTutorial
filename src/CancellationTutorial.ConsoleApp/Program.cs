namespace CancellationTutorial.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Start:{DateTime.Now}");
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync("http://localhost:5041/weatherforecastwithCancel", cts.Token);
                var json = response.Content.ReadAsStringAsync(cts.Token);
                Console.WriteLine(json);
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"Stop:{DateTime.Now}");
            Console.ReadKey();
        }
    }
}