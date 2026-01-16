using System.Collections.Concurrent;

namespace ex11
{
    internal class Program
    {
        static readonly int ARRAY_SIZE = 1000000;
        static readonly int NUM_SECTIONS = 8;
        static void Main(string[] args)
        {
            int[] v = new int[ARRAY_SIZE];
            init(v);

            Console.WriteLine($"Checking primality for {ARRAY_SIZE} numbers...");
            Console.WriteLine($"Using {NUM_SECTIONS} parallel sections\n");

            ConcurrentBag<int> primes = new ConcurrentBag<int>();

            int totalPrimeCount = 0;
            object lockObj = new object();

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            Action[] actions = new Action[NUM_SECTIONS];
            int sectionSize = ARRAY_SIZE / NUM_SECTIONS;

            for (int i = 0; i < NUM_SECTIONS; i++)
            {
                int sectionIndex = i;
                int startIndex = sectionIndex * sectionSize;
                int endIndex = (sectionIndex == NUM_SECTIONS - 1)
                    ? ARRAY_SIZE
                    : startIndex + sectionSize;

                actions[i] = () =>
                {
                    int localCount = 0;
                    List<int> localPrimes = new List<int>();

                    Console.WriteLine($"[Section {sectionIndex}] Processing indices {startIndex} to {endIndex - 1} on Thread {Thread.CurrentThread.ManagedThreadId}");

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        if (IsPrime(v[j]))
                        {
                            localPrimes.Add(v[j]);
                            localCount++;
                        }
                    }

                    foreach (var prime in localPrimes)
                    {
                        primes.Add(prime);
                    }

                    lock (lockObj)
                    {
                        totalPrimeCount += localCount;
                    }

                    Console.WriteLine($"[Section {sectionIndex}] Found {localCount} primes");
                };
            }

            Parallel.Invoke(actions);

            stopwatch.Stop();

            Console.WriteLine($"\nFound {totalPrimeCount} prime numbers in {stopwatch.ElapsedMilliseconds}ms");

            int[] sortedPrimes = primes.OrderBy(x => x).ToArray();

            write(sortedPrimes, "primes_out.txt");
            Console.WriteLine($"Prime numbers written to primes_out.txt");

            File.WriteAllText("primes_out_count.txt", totalPrimeCount.ToString());
            Console.WriteLine($"Prime count written to primes_out_count.txt");
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
