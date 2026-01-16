namespace ex02
{
    internal class Program
    {
        private static int NO_ITERATIONS = 100000;

        static int sharedVariable = 0;
        static object lockObject = new object();

        static void Main()
        {
            Thread incrementThread = new Thread(IncrementSharedVariable);
            Thread decrementThread = new Thread(DecrementSharedVariable);

            incrementThread.Start();
            decrementThread.Start();

            incrementThread.Join();
            decrementThread.Join();

            Console.WriteLine("Shared Variable: " + sharedVariable);
        }

        static void IncrementSharedVariable()
        {
            for (int i = 0; i < NO_ITERATIONS; i++)
            {
                lock (lockObject)
                {
                    sharedVariable++;
                }
            }
        }

        static void DecrementSharedVariable()
        {
            for (int i = 0; i < NO_ITERATIONS; i++)
            {
                lock (lockObject)
                {
                    sharedVariable--;
                }
            }
        }
    }
}
