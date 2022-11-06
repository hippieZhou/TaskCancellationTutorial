public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var cancellationTokenSource = new CancellationTokenSource();
        await ExampleWithLoop(cancellationTokenSource);
    }

    static async Task ExampleWithLoop(CancellationTokenSource cancellationTokenSource)
    {
        var _ = Task.Run(() =>
        {
            Console.WriteLine($"Thread ID:{Environment.CurrentManagedThreadId}");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.C)
            {
                cancellationTokenSource.Cancel();
                Console.WriteLine($"Cancelled the task with :{key.Key}");
            }
        });

        try
        {
            while (true)
            {

                Console.WriteLine($"{DateTime.Now} = {Environment.CurrentManagedThreadId}: Doing some work for 1 seconds");
                await Task.Delay(1000, cancellationTokenSource.Token);
            }
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine("Token was cancelled and we exited the loop");
        cancellationTokenSource.Dispose();
    }
}