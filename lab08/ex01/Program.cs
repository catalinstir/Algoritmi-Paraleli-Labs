namespace ex01
{
    internal class Program
    {
        private static int NO_ITERATIONS = 100;

        static void Main(string[] args)
        {
            Thread thread_1 = new Thread(() => ThreadFunction("Thread 1"));
            Thread thread_2 = new Thread(() => ThreadFunction("Thread 2"));
            Thread thread_3 = new Thread(ThreadFunction_2);
            thread_1.Priority = ThreadPriority.Highest;
            thread_2.Priority = ThreadPriority.Normal;
            thread_3.Priority = ThreadPriority.Lowest;

            thread_1.Start();
            thread_2.Start();
            thread_3.Start("Thread 3");

            thread_1.Join();
            thread_2.Join();
            thread_3.Join();

            Console.WriteLine("All threads have completed the work!");
        }

        static void ThreadFunction(string message)
        {
            for (int i = 1; i <= NO_ITERATIONS; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] => {message} ({i})");

                // Simulate some work
                Thread.Sleep(500);
            }
        }

        static void ThreadFunction_2(object data)
        {
            string message = (string)data;
            for (int i = 1; i <= NO_ITERATIONS; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] => {message} ({i})");

                // Simulate some work
                Thread.Sleep(500);
            }
        }
    }
}
