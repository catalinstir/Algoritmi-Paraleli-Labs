namespace ex04
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            
            var progress1 = new Progress<float>();
            progress1.ProgressChanged += (sender, percent) =>
            {
                Console.WriteLine($"TID {Thread.CurrentThread.ManagedThreadId}: [Task1 - Int] ProgressChanged => {percent}%");
            };

            var progress2 = new Progress<float>();
            progress2.ProgressChanged += (sender, percent) =>
            {
                Console.WriteLine($"TID {Thread.CurrentThread.ManagedThreadId}: [Task2 - String] ProgressChanged => {percent}%");
            };

            Task<int> task_1 = DoSomeWorkAsync(progress1);
            Task<string> task_2 = DoSomeStringWorkAsync(progress2);
            Task task_3 = ThrowNotImplementedExceptionAsync();

            try
            {
                await Task.WhenAll(task_3, task_1, task_2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nSomething went wrong! => {ex.Message}");
            }

            
            Console.WriteLine("\n=== Results ===");

            
            if (task_1.IsCompletedSuccessfully)
            {
                Console.WriteLine($"Task 1 (Int) result: {task_1.Result}");
            }
            else if (task_1.IsFaulted)
            {
                Console.WriteLine($"Task 1 failed: {task_1.Exception?.InnerException?.Message}");
            }

            
            if (task_2.IsCompletedSuccessfully)
            {
                Console.WriteLine($"Task 2 (String) result: {task_2.Result}");
            }
            else if (task_2.IsFaulted)
            {
                Console.WriteLine($"Task 2 failed: {task_2.Exception?.InnerException?.Message}");
            }

            
            if (task_3.IsCompletedSuccessfully)
            {
                Console.WriteLine($"Task 3 completed successfully");
            }
            else if (task_3.IsFaulted)
            {
                Console.WriteLine($"Task 3 failed: {task_3.Exception?.InnerException?.Message}");
            }
        }

        static async Task<int> DoSomeWorkAsync(IProgress<float> progress = null)
        {
            int result = 0;
            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(i * 200);
                result += Random.Shared.Next(1, 10);
                progress?.Report(i * 10.0f);
            }
            return result;
        }

        static async Task<string> DoSomeStringWorkAsync(IProgress<float> progress = null)
        {
            string result = string.Empty;
            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(i * 100);
                result += i;
                progress?.Report(i * 10.0f);
            }
            return result;
        }

        static async Task ThrowNotImplementedExceptionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
