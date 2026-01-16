using System.Diagnostics;
using System.Net;
using System.Collections.Concurrent;
using System.Text;

namespace ex07
{
    internal class Program
    {
        static int parallelRequests = 8;
        static bool saveResponse = false;
        static bool statCountSuccess = false;
        static bool statCountFail = false;
        static bool statMeanTime = false;
        static string url = "";
        static string method = "GET";
        static string body = "";
        static int totalRequests = 100;

        static int successCount = 0;
        static int failCount = 0;
        static ConcurrentBag<long> responseTimes = new ConcurrentBag<long>();

        static void Main(string[] args)
        {
            ParseArguments(args);

            if (string.IsNullOrEmpty(url))
            {
                PrintUsage();
                return;
            }

            Console.WriteLine($"Starting parallel HTTP test tool");
            Console.WriteLine($"  URL: {url}");
            Console.WriteLine($"  Method: {method}");
            Console.WriteLine($"  Parallel requests: {parallelRequests}");
            Console.WriteLine($"  Total requests: {totalRequests}");
            Console.WriteLine();

            RunTests();

            PrintStatistics();
        }

        static void ParseArguments(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.StartsWith("-P="))
                {
                    parallelRequests = int.Parse(arg.Substring(3));
                }
                else if (arg == "--save-response")
                {
                    saveResponse = true;
                }
                else if (arg == "--stat-countsuccess")
                {
                    statCountSuccess = true;
                }
                else if (arg == "--stat-countfail")
                {
                    statCountFail = true;
                }
                else if (arg == "--stat-meantime")
                {
                    statMeanTime = true;
                }
                else if (arg.StartsWith("--url="))
                {
                    url = arg.Substring(6);
                }
                else if (arg.StartsWith("--method="))
                {
                    method = arg.Substring(9).ToUpper();
                }
                else if (arg.StartsWith("--body="))
                {
                    body = arg.Substring(7);
                }
                else if (arg.StartsWith("--total="))
                {
                    totalRequests = int.Parse(arg.Substring(8));
                }
            }

            // Enable all stats by default if none specified
            if (!statCountSuccess && !statCountFail && !statMeanTime)
            {
                statCountSuccess = true;
                statCountFail = true;
                statMeanTime = true;
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: ex07 --url=<URL> [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -P=<N>              Number of parallel requests (default: 8)");
            Console.WriteLine("  --total=<N>         Total number of requests to send (default: 100)");
            Console.WriteLine("  --method=<METHOD>   HTTP method: GET or POST (default: GET)");
            Console.WriteLine("  --body=<JSON>       Request body for POST requests");
            Console.WriteLine("  --save-response     Save responses to files");
            Console.WriteLine("  --stat-countsuccess Count successful requests");
            Console.WriteLine("  --stat-countfail    Count failed requests");
            Console.WriteLine("  --stat-meantime     Calculate mean response time");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("  ex07 --url=http://localhost:5007/product -P=8 --stat-countsuccess");
        }

        static void RunTests()
        {
            int requestsSent = 0;
            object lockObj = new object();
            Thread[] threads = new Thread[parallelRequests];

            Stopwatch totalStopwatch = Stopwatch.StartNew();

            for (int i = 0; i < parallelRequests; i++)
            {
                int threadId = i;
                threads[i] = new Thread(() =>
                {
                    using HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromSeconds(30);

                    while (true)
                    {
                        int currentRequest;
                        lock (lockObj)
                        {
                            if (requestsSent >= totalRequests)
                                break;
                            currentRequest = requestsSent++;
                        }

                        Stopwatch sw = Stopwatch.StartNew();
                        bool success = false;
                        string responseContent = "";

                        try
                        {
                            HttpResponseMessage response;
                            if (method == "POST")
                            {
                                var content = new StringContent(body, Encoding.UTF8, "application/json");
                                response = client.PostAsync(url, content).Result;
                            }
                            else
                            {
                                response = client.GetAsync(url).Result;
                            }

                            sw.Stop();
                            responseTimes.Add(sw.ElapsedMilliseconds);
                            responseContent = response.Content.ReadAsStringAsync().Result;

                            if (response.IsSuccessStatusCode)
                            {
                                Interlocked.Increment(ref successCount);
                                success = true;
                                Console.WriteLine($"[Thread {threadId}] Request {currentRequest}: SUCCESS ({sw.ElapsedMilliseconds}ms)");
                            }
                            else
                            {
                                Interlocked.Increment(ref failCount);
                                Console.WriteLine($"[Thread {threadId}] Request {currentRequest}: FAIL - Status {(int)response.StatusCode} ({sw.ElapsedMilliseconds}ms)");
                            }
                        }
                        catch (Exception ex)
                        {
                            sw.Stop();
                            responseTimes.Add(sw.ElapsedMilliseconds);
                            Interlocked.Increment(ref failCount);
                            Console.WriteLine($"[Thread {threadId}] Request {currentRequest}: ERROR - {ex.Message}");
                        }

                        if (saveResponse)
                        {
                            string fileName = $"response_{currentRequest}_{(success ? "success" : "fail")}.txt";
                            File.WriteAllText(fileName, responseContent);
                        }
                    }
                });
                threads[i].Start();
            }

            // Wait for all threads
            for (int i = 0; i < parallelRequests; i++)
            {
                threads[i].Join();
            }

            totalStopwatch.Stop();
            Console.WriteLine($"\nTotal execution time: {totalStopwatch.ElapsedMilliseconds}ms");
        }

        static void PrintStatistics()
        {
            Console.WriteLine("\n=== Statistics ===");

            if (statCountSuccess)
            {
                Console.WriteLine($"Successful requests: {successCount}");
            }

            if (statCountFail)
            {
                Console.WriteLine($"Failed requests: {failCount}");
            }

            if (statMeanTime && responseTimes.Count > 0)
            {
                double meanTime = responseTimes.Average();
                Console.WriteLine($"Mean response time: {meanTime:F2}ms");
            }

            double errorRate = (double)failCount / (successCount + failCount) * 100;
            Console.WriteLine($"Error rate: {errorRate:F2}%");
        }
    }
}
