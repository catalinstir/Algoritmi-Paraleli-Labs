using Newtonsoft.Json;
using System.Net;
using System.Collections.Concurrent;

namespace ex05
{
    internal class Program
    {
        static readonly ConcurrentQueue<string> urlQueue = new ConcurrentQueue<string>();
        static readonly ConcurrentDictionary<string, bool> downloadedUrls = new ConcurrentDictionary<string, bool>();
        static readonly ConcurrentQueue<string> processQueue = new ConcurrentQueue<string>();
        static bool running = true;
        static object lockObj = new object();

        static void Main(string[] args)
        {
            // Start the three threads
            Thread requesterThread = new Thread(RequesterWork);
            Thread downloaderThread = new Thread(DownloaderWork);
            Thread processerThread = new Thread(ProcesserWork);

            requesterThread.Start();
            downloaderThread.Start();
            processerThread.Start();

            // Run for a limited time or until user presses a key
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            // Stop all threads
            running = false;

            requesterThread.Join();
            downloaderThread.Join();
            processerThread.Join();

            Console.WriteLine("\nAll threads stopped.");
        }

        // Thread 1: Requester - fetches URLs with exponential backoff
        static void RequesterWork()
        {
            int backoffSeconds = 1;
            int maxBackoff = 64;

            using (WebClient webClient = new WebClient())
            {
                while (running)
                {
                    try
                    {
                        string apiResponseJsonString = webClient.DownloadString("http://localhost:5000/api/image");
                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponseJsonString);

                        if (apiResponse.Status == "SUCCESS")
                        {
                            Console.WriteLine($"[Requester] Got URL: {apiResponse.Url}");
                            urlQueue.Enqueue(apiResponse.Url);
                            backoffSeconds = 1; // Reset backoff on success
                        }
                        else if (apiResponse.Status == "RETRY-LATER")
                        {
                            Console.WriteLine($"[Requester] Server busy, waiting {backoffSeconds}s (exponential backoff)");
                            Thread.Sleep(backoffSeconds * 1000);
                            backoffSeconds = Math.Min(backoffSeconds * 2, maxBackoff);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Requester] Error: {ex.Message}");
                        Thread.Sleep(1000);
                    }
                }
            }
            Console.WriteLine("[Requester] Thread stopped.");
        }

        // Thread 2: Downloader - downloads images from URLs
        static void DownloaderWork()
        {
            using (WebClient webClient = new WebClient())
            {
                while (running)
                {
                    if (urlQueue.TryDequeue(out string url))
                    {
                        // Check if already downloaded
                        if (downloadedUrls.TryAdd(url, true))
                        {
                            try
                            {
                                byte[] imageData = webClient.DownloadData(url);
                                string fileName = Path.GetFileName(new Uri(url).LocalPath);
                                File.WriteAllBytes(fileName, imageData);
                                Console.WriteLine($"[Downloader] Downloaded: {fileName}");
                                
                                // Add to process queue
                                processQueue.Enqueue(fileName);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[Downloader] Error downloading {url}: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[Downloader] Already downloaded: {url}");
                        }
                    }
                    else
                    {
                        Thread.Sleep(100); // Wait for new URLs
                    }
                }
            }
            Console.WriteLine("[Downloader] Thread stopped.");
        }

        // Thread 3: Processer - adds watermark to downloaded images
        static void ProcesserWork()
        {
            while (running)
            {
                if (processQueue.TryDequeue(out string fileName))
                {
                    try
                    {
                        // Simple watermark: add ".watermarked" to filename
                        string watermarkedFileName = Path.GetFileNameWithoutExtension(fileName) + ".watermarked" + Path.GetExtension(fileName);
                        
                        // For a simple demo, just copy the file with new name
                        // In production, you would use ImageSharp or similar to add actual watermark
                        if (File.Exists(fileName))
                        {
                            File.Copy(fileName, watermarkedFileName, true);
                            Console.WriteLine($"[Processer] Watermarked: {watermarkedFileName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Processer] Error processing {fileName}: {ex.Message}");
                    }
                }
                else
                {
                    Thread.Sleep(100); // Wait for new files
                }
            }
            Console.WriteLine("[Processer] Thread stopped.");
        }
    }
}
