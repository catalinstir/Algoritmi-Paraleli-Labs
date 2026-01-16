namespace ex04
{
    internal class Program
    {
        static readonly int ARRAY_SIZE = 100;
        static void Main(string[] args)
        {
            int[] numbers = new int[ARRAY_SIZE];
            for (int i = 0; i < ARRAY_SIZE; i++)
            {
                numbers[i] = i + 1;
            }
            Parallel.ForEach(numbers, (number) =>
            {
                if (number % 2 == 0)
                {
                    Console.WriteLine($"{number} is EVEN");
                }
            });
        }
    }
}
