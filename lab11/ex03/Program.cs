namespace ex03
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Product Search with CancellationToken ===\n");

            double targetPrice = 79.99;
            int maxProducts = 3;
            Console.WriteLine($"Searching for {maxProducts} products with price ${targetPrice}\n");

            var client = new ProductClient("http://localhost:5000/api/products", pageSize: 5);
            CancellationTokenSource cts = new CancellationTokenSource();

            List<Product> foundProducts = new List<Product>();

            try
            {
                await foreach (Product product in client.GetProductsAsync(cts.Token))
                {
                    if (product.Price == targetPrice)
                    {
                        foundProducts.Add(product);
                        Console.WriteLine($"[FOUND {foundProducts.Count}/{maxProducts}] Product [{product.Id}]: {product.Name}, Price=${product.Price}");

                        if (foundProducts.Count >= maxProducts)
                        {
                            Console.WriteLine($"\n[STOP] Found {maxProducts} products. Cancelling search...");
                            cts.Cancel();
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[SKIP] Product [{product.Id}]: {product.Name}, Price=${product.Price}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }

            Console.WriteLine($"\n=== Search Results ===");
            Console.WriteLine($"Products found: {foundProducts.Count}");

            foreach (var product in foundProducts)
            {
                Console.WriteLine($"  - [{product.Id}] {product.Name}: ${product.Price}");
            }
        }
    }
}
