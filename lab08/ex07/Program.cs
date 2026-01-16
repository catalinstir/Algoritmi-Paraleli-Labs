namespace ex07
{
    internal class Program
    {
        static int sharedVariable = 0;
        static object lockSharedVariable = new object();

        public static void Main(string[] args)
        {
            try
            {
                Thread incrementThread = new Thread(IncrementSharedVariable);
                Thread decrementThread = new Thread(DecrementSharedVariable);

                incrementThread.Start();
                decrementThread.Start();

                incrementThread.Join();
                decrementThread.Join();

                Console.WriteLine("Shared Variable: " + sharedVariable);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong.");
                throw;
            }
        }

        static void IncrementSharedVariable()
        {
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    Monitor.Enter(lockSharedVariable);
                    try
                    {
                        sharedVariable++;
                        DoSomeStuff(); // This method can throw an exception
                    }
                    finally
                    {
                        Monitor.Exit(lockSharedVariable);
                    }
                }
                catch { /* Just a classic TODO (later). !!!NEVER EVER DO THIS!!! AT LEAST, LOG THE EXCEPTION. */ }
            }
        }

        static void DecrementSharedVariable()
        {
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    Monitor.Enter(lockSharedVariable);
                    try
                    {
                        sharedVariable--;
                        DoSomeStuff(); // This method can throw an exception
                    }
                    finally
                    {
                        Monitor.Exit(lockSharedVariable);
                    }
                }
                catch { /* Just a classic TODO (later). !!!NEVER EVER DO THIS!!! AT LEAST, LOG THE EXCEPTION. */ }
            }
        }

        static void DoSomeStuff()
        {
            int a = Random.Shared.Next();
            int b = Random.Shared.Next() % 2;
            Console.WriteLine($"Let's try do a division: {a} / {b}");
            int result = a / b;
            Console.WriteLine($"{a} / {b} = {result}");
        }
    }
}
