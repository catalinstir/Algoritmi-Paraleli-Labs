namespace ex07
{
    internal class Program
    {
        private static int NUM_OF_ITERATIONS = 50;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any key to terminate the program...\n");

            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var progress_1 = new Progress<float>();
                progress_1.ProgressChanged += (sender, percent) =>
                {
                    Console.WriteLine($"DoWork_1_Async: {percent}%");
                };

                var progress_2 = new Progress<float>();
                progress_2.ProgressChanged += (sender, percent) =>
                {
                    Console.WriteLine($"DoWork_2_Async: {percent}%");
                };

                Task<int> task_1 = DoWork_1_Async(cts.Token, progress_1);
                Task<int> task_2 = DoWork_2_Async(cts.Token, progress_2);

                Task<int> winnerTask = await Task.WhenAny(task_1, task_2);

                cts.Cancel();

                string winnerName = winnerTask == task_1 ? "DoWork_1_Async" : "DoWork_2_Async";
                int result = await winnerTask;

                Console.WriteLine($"\n=== WINNER ===");
                Console.WriteLine($"Task: {winnerName}");
                Console.WriteLine($"Result: {result}");

                await Task.Delay(500);

                Console.WriteLine($"\n=== Task Status ===");
                Console.WriteLine($"Task 1: {task_1.Status}");
                Console.WriteLine($"Task 2: {task_2.Status}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static async Task<int> DoWork_1_Async(CancellationToken cancellationToken, IProgress<float> progress = null)
        {
            int result = 0;
            Random random = new Random();

            for (int i = 1; i <= NUM_OF_ITERATIONS; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                progress?.Report((i * 100.0f) / NUM_OF_ITERATIONS);
                await Task.Delay(random.Next(1, 20) * 100, cancellationToken);
                result += i;
            }

            return result;
        }

        static async Task<int> DoWork_2_Async(CancellationToken cancellationToken, IProgress<float> progress = null)
        {
            int result = 0;
            Random random = new Random();

            for (int i = 1; i <= NUM_OF_ITERATIONS; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                progress?.Report((i * 100.0f) / NUM_OF_ITERATIONS);
                await Task.Delay(random.Next(1, 20) * 100, cancellationToken);
                result += i;
            }

            return result;
        }
    }
}
