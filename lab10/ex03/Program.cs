namespace ex03
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var progress = new Progress<float>();
            progress.ProgressChanged += (sender, percent) =>
            {
                Console.WriteLine($"TID {Thread.CurrentThread.ManagedThreadId}: ProgressChanged => {percent}%");
            };

            await DoSomeWorkAsync(progress);
        }

        static async Task DoSomeWorkAsync(IProgress<float> progress = null)
        {
            for (int i = 1; i <= 10; i++)
            {
                // Simulate processing item i
                await Task.Delay(i * 100);

                Console.WriteLine($"BEFORE ProgressChanged => {i * 10.0f}%");

                // Report progress
                progress?.Report(i * 10.0f);

                await Task.Delay((i - 1) * 100);

                Console.WriteLine($"AFTER ProgressChanged => {i * 10.0f}%");
            }
        }
    }
}
