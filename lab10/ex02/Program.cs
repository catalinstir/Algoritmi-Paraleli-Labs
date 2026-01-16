using System.Collections.Concurrent;

namespace ex02
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ConcurrentQueue<string> urlQueue = new ConcurrentQueue<string>();
            CancellationTokenSource cts = new CancellationTokenSource();

            int totalRequests = 20;

            Console.WriteLine("Starting async Requester and Downloader...\n");
            Directory.CreateDirectory("downloads");

            Requester requester = new Requester("http://localhost:5000/api/image");
            Downloader downloader = new Downloader();

            Task requesterTask = requester.StartAsync(urlQueue, totalRequests);
            Task downloaderTask = downloader.StartAsync(urlQueue, cts.Token);

            await requesterTask;
            Console.WriteLine("\n[Main] Requester finished.");
            await Task.Delay(2000);
            cts.Cancel();

            Console.WriteLine("[Main] Downloader finished.");
            Console.WriteLine($"\nFiles downloaded: {Directory.GetFiles("downloads").Length}");
        }
    }
}
