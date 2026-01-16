namespace ex05
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            string arg_1 = "1";
            Task<string> task_1 = DoSomeWorkAsync(arg_1, cts.Token);
            string arg_2 = "2";
            Task<string> task_2 = DoSomeWorkAsync(arg_2, cts.Token);
            Task<string> task_3 = DoSomeWorkAsync("3", cts.Token);

            Task<string> result = await Task.WhenAny(task_1, task_2, task_3);

            
            cts.Cancel();

            Console.WriteLine($"Final result: {await result}");

            await Task.Delay(4000);

            Console.WriteLine("\n=== Task Status ===");
            Console.WriteLine($"Task 1: {task_1.Status}");
            Console.WriteLine($"Task 2: {task_2.Status}");
            Console.WriteLine($"Task 3: {task_3.Status}");
        }

        static async Task<string> DoSomeWorkAsync(string id, CancellationToken token = default)
        {
            Random random = new Random();
            string result = $"[{id}] => ";

            for (int i = 1; i <= 10; i++)
            {
               
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"[{id}] cancelled!");
                    token.ThrowIfCancellationRequested();
                }

                await Task.Delay(random.Next(1, 10) * 100, token);
                Console.WriteLine($"[{id}] working {i}...");
                result += i;
            }

            return result;
        }
    }
}
