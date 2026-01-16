namespace ex08
{
    internal class Program
    {
        static readonly int ARRAY_SIZE = 5;

        static void Main(string[] args)
        {
            int[] v = new int[ARRAY_SIZE];

            init(v);

            int result = ParallelProduct(v);

            Console.WriteLine(result);
        }

        static int ParallelProduct(int[] numbers)
        {
            object mutex = new object();
            int result = 1;

            Parallel.ForEach
            (
                source: numbers,
                localInit: () => 1,
                body: (item, state, localValue) =>
                {
                    return localValue * item;
                },
                localFinally: localValue =>
                {
                    lock (mutex)
                        result *= localValue;
                }
            );

            return result;
        }

        static void init(int[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = i + 1;
            }
        }
    }
}
