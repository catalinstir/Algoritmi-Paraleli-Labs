namespace ex02
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Product Client with IAsyncEnumerable ===\n");

            var client = new ProductClient("http://localhost:5000/api/products", pageSize: 5);

            int count = 0;

            await foreach (Product product in client.GetProductsAsync())
            {
                Console.WriteLine($"Processing product [{product.Id}]: Name={product.Name}, Category={product.Category}, Description={product.Description}, Price={product.Price}");
                count++;
            }

            Console.WriteLine($"\n=== Total products processed: {count} ===");
        }
    }
}
