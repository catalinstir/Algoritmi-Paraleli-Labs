namespace ex01
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int value = 13;
            Console.WriteLine($"TID[{Thread.CurrentThread.ManagedThreadId}]");

            Task task_1 = Task.Run(async () =>
            {
                for (int i = 0; i < 50; i++)
                {
                    await Task.Delay(500);
                    Console.WriteLine($"TID[{Thread.CurrentThread.ManagedThreadId}] => {i}");
                }
            });

            // Asynchronously wait 1 second
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine($"TID[{Thread.CurrentThread.ManagedThreadId}]");

            await Task.Run(async () =>
            {
                // Asynchronously wait 2 seconds
                await Task.Delay(TimeSpan.FromSeconds(2));
                Console.WriteLine($"TID[{Thread.CurrentThread.ManagedThreadId}]");
            });

            Console.WriteLine($"TID[{Thread.CurrentThread.ManagedThreadId}]");

            await task_1;
        }
    }
}
