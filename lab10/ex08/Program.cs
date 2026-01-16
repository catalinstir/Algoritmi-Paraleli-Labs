using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ex08
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Stock Price Racing ===\n");

            string symbol = "AAPL";
            CancellationTokenSource cts = new CancellationTokenSource();

            var alphaVantageClient = new AlphaVantageClient();
            var finnhubClient = new FinnhubClient();

            Task<StockResult> task1 = alphaVantageClient.GetStockPriceAsync(symbol, cts.Token);
            Task<StockResult> task2 = finnhubClient.GetStockPriceAsync(symbol, cts.Token);

            try
            {
                Task<StockResult>[] tasks = { task1, task2 };
                List<Task<StockResult>> remainingTasks = tasks.ToList();

                StockResult? winnerResult = null;

                while (remainingTasks.Count > 0 && winnerResult == null)
                {
                    Task<StockResult> completedTask = await Task.WhenAny(remainingTasks);
                    remainingTasks.Remove(completedTask);

                    try
                    {
                        StockResult result = await completedTask;
                        if (result.Success)
                        {
                            winnerResult = result;
                            cts.Cancel();
                        }
                        else
                        {
                            Console.WriteLine($"[{result.Source}] Failed: {result.ErrorMessage}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Task failed: {ex.Message}");
                    }
                }

                if (winnerResult != null)
                {
                    Console.WriteLine($"\n=== WINNER: {winnerResult.Source} ===");
                    Console.WriteLine($"Symbol: {winnerResult.Symbol}");
                    Console.WriteLine($"Price: ${winnerResult.Price:F2}");
                    Console.WriteLine($"Time: {winnerResult.Timestamp}");
                }
                else
                {
                    Console.WriteLine("\nNo API returned a successful response.");
                }

                await Task.Delay(500);
                Console.WriteLine($"\n=== Task Status ===");
                Console.WriteLine($"AlphaVantage: {task1.Status}");
                Console.WriteLine($"Finnhub: {task2.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public class StockResult
    {
        public bool Success { get; set; }
        public string Source { get; set; } = "";
        public string Symbol { get; set; } = "";
        public decimal Price { get; set; }
        public string Timestamp { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
    }

    public class AlphaVantageClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "0XQ77ENJ5QWHXEYO";
        public AlphaVantageClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<StockResult> GetStockPriceAsync(string symbol, CancellationToken token)
        {
            Console.WriteLine($"[AlphaVantage] Starting request for {symbol}...");

            try
            {
                string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";

                var response = await _httpClient.GetStringAsync(url, token);

                Console.WriteLine($"[AlphaVantage] Response received");

                using JsonDocument doc = JsonDocument.Parse(response);

                if (doc.RootElement.TryGetProperty("Global Quote", out JsonElement quote))
                {
                    string priceStr = quote.GetProperty("05. price").GetString() ?? "0";
                    decimal price = decimal.Parse(priceStr, System.Globalization.CultureInfo.InvariantCulture);

                    return new StockResult
                    {
                        Success = true,
                        Source = "AlphaVantage",
                        Symbol = symbol,
                        Price = price,
                        Timestamp = DateTime.Now.ToString("HH:mm:ss")
                    };
                }

                return new StockResult
                {
                    Success = false,
                    Source = "AlphaVantage",
                    ErrorMessage = "Invalid response format"
                };
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"[AlphaVantage] Cancelled");
                throw;
            }
            catch (Exception ex)
            {
                return new StockResult
                {
                    Success = false,
                    Source = "AlphaVantage",
                    ErrorMessage = ex.Message
                };
            }
        }
    }

    public class FinnhubClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "d57a1gpr01qrcrna8ivgd57a1gpr01qrcrna8j00";

        public FinnhubClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<StockResult> GetStockPriceAsync(string symbol, CancellationToken token)
        {
            Console.WriteLine($"[Finnhub] Starting request for {symbol}...");

            try
            {
                string url = $"https://finnhub.io/api/v1/quote?symbol={symbol}&token={_apiKey}";

                var response = await _httpClient.GetStringAsync(url, token);

                Console.WriteLine($"[Finnhub] Response received");

                using JsonDocument doc = JsonDocument.Parse(response);

                if (doc.RootElement.TryGetProperty("c", out JsonElement currentPrice))
                {
                    decimal price = currentPrice.GetDecimal();

                    if (price > 0)
                    {
                        return new StockResult
                        {
                            Success = true,
                            Source = "Finnhub",
                            Symbol = symbol,
                            Price = price,
                            Timestamp = DateTime.Now.ToString("HH:mm:ss")
                        };
                    }
                }

                return new StockResult
                {
                    Success = false,
                    Source = "Finnhub",
                    ErrorMessage = "Invalid response or zero price"
                };
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"[Finnhub] Cancelled");
                throw;
            }
            catch (Exception ex)
            {
                return new StockResult
                {
                    Success = false,
                    Source = "Finnhub",
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
