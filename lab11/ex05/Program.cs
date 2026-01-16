using System.Collections.Concurrent;

namespace ex05
{
    internal class Program
    {
        static readonly int ARRAY_SIZE = 1000000;

        static void Main(string[] args)
        {
            int[] v = new int[ARRAY_SIZE];
            init(v);

            Console.WriteLine($"Checking primality for {ARRAY_SIZE} numbers...");

            ConcurrentBag<int> primes = new ConcurrentBag<int>();

            int primeCount = 0;

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            Parallel.ForEach(v, (number) =>
            {
                if (IsPrime(number))
                {
                    primes.Add(number);
                    Interlocked.Increment(ref primeCount);
                }
            });

            stopwatch.Stop();

            Console.WriteLine($"Found {primeCount} prime numbers in {stopwatch.ElapsedMilliseconds}ms");

            int[] sortedPrimes = primes.OrderBy(x => x).ToArray();

            write(sortedPrimes, "primes_out.txt");
            Console.WriteLine($"Prime numbers written to primes_out.txt");

            File.WriteAllText("primes_out_count.txt", primeCount.ToString());
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
