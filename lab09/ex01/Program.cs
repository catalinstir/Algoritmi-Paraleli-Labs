namespace ex01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Thread thread = new Thread(() => DoWork(cancellationTokenSource.Token));
            thread.Start();

            Console.WriteLine("Press ENTER to cancel the operation.");
            Console.ReadLine();
            cancellationTokenSource.Cancel();

            thread.Join();

            Console.WriteLine("Main thread existing...");
        }

        static void DoWork(CancellationToken cancellationToken)
        {
            try
            {
                // Listen for cancellation request by POLLING
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: working phase 1...");
                    // Simulate work
                    Thread.Sleep(1000);

                    // cancellationToken.ThrowIfCancellationRequested();

                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: working phase 2...");
                    // Simulate work
                    Thread.Sleep(1000);
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: working operation cancelled!");
            }
            finally
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: cleanup");
            }
        }
    }
}
