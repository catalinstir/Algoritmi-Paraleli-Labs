namespace ex06
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Task<string> task_1 = DoSomeWorkAsync(10);
            Task<string> task_2 = DoSomeWorkAsync(2);
            Task<string> task_3 = DoSomeWorkAsync(5);

            Task<string>[] tasks = new[] { task_1, task_2, task_3 };

            List<Task<stirng>> remainingTasks = tasks.ToList();
            while (remainingTasks.Count > 0)
            {
                Task<string> completedTask = await Task.WhenAny(remainingTasks);
                remainingTasks.Remove(completedTask);
                string result = await completedTask;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Result: {result}");
            }
        }

        static async Task<string> DoSomeWorkAsync(int times)
        {
            string result = string.Empty;

            for (int i = 0; i < times; i++)
            {
                await Task.Delay(100);
                result += i;
            }

            return result;
        }
    }
}
