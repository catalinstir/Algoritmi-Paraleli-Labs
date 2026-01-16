namespace ex07
{
    internal class Program
    {
        static readonly int ARRAY_SIZE = 1000000;

        static int firstPrimeFound = -1;
        static object lockObj = new object();

        static void Main(string[] args)
        {
            int[] v = new int[ARRAY_SIZE];
            init(v);

            Console.WriteLine($"Searching for first prime in {ARRAY_SIZE} numbers...\n");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            Parallel.ForEach(v, (number, state) =>
            {
                if (state.IsStopped || state.ShouldExitCurrentIteration)
                {
                    return;
                }

                if (IsPrime(number))
                {
                    lock (lockObj)
                    {
                        if (firstPrimeFound == -1 || number < firstPrimeFound)
                        {
                            firstPrimeFound = number;
                            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Found prime: {number}");

                            state.Stop();
                        }
                    }
                }
            });

            stopwatch.Stop();

            Console.WriteLine($"\n=== Result ===");
            Console.WriteLine($"First prime found: {firstPrimeFound}");
            Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds}ms");
        }

        static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n <= 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;

            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
            }

            return true;
        }

        static void init(int[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = i;
            }
        }

        static void print(int[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                Console.Write(v[i]);
                Console.Write(' ');
            }
            Console.WriteLine();
        }

        static void write(int[] v, string filename)
        {
            File.WriteAllText(filename, string.Join(" ", v));
        }
    }
}
