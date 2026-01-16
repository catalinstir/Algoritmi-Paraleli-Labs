namespace ex09
{
    internal class Program
    {
        static readonly int ARRAY_SIZE = 1000000;

        static void Main(string[] args)
        {
            int[] v = new int[ARRAY_SIZE];
            init(v);

            Console.WriteLine($"Checking primality for {ARRAY_SIZE} numbers using aggregation...\n");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            int totalPrimeCount = 0;
            object lockObj = new object();

            Parallel.ForEach(
                v,
                () => 0,
                (number, state, localCount) =>
                {
                    if (IsPrime(number))
                    {
                        return localCount + 1;
                    }
                    return localCount;
                },

                (localCount) =>
                {
                    lock (lockObj)
                    {
                        totalPrimeCount += localCount;
                    }
                }
            );

            stopwatch.Stop();

            Console.WriteLine($"Found {totalPrimeCount} prime numbers in {stopwatch.ElapsedMilliseconds}ms");

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
