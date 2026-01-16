namespace ex10
{
    internal class Program
    {
        static readonly int ARRAY_SIZE = 10;

        static void Main(string[] args)
        {
            int[] v = new int[ARRAY_SIZE];
            init(v);

            Parallel.Invoke(
                () => ProcessPartialArray(v, 0, v.Length / 2),
                () => ProcessPartialArray(v, v.Length / 2, v.Length)
            );
        }

        static void ProcessPartialArray(int[] array, int begin, int end)
        {
            Random random = new Random();
            for (int i = begin; i < end; i++)
            {
                Thread.Sleep(random.Next(1, 5) * 100);
                Console.WriteLine($"[{begin}, {end}] => {array[i]}");
            }
        }

        static void init(int[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = i;
            }
        }
    }
}
