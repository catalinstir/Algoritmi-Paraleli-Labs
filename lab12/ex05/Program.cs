using System.Threading.Tasks.Dataflow;

namespace ex05
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== 1. ActionBlock<TInput> ===\n");

            ActionBlock<int> actionBlock = new ActionBlock<int>(
                nr => Console.WriteLine($"ActionBlock processing: {nr}")
            );

            for (int i = 0; i < 10; i++)
            {
                await actionBlock.SendAsync(i);
            }
            actionBlock.Complete();
            await actionBlock.Completion;

            Console.WriteLine("\n=== 2. TransformBlock<TInput, TOutput> ===\n");

            TransformBlock<string, string> transformBlock = new TransformBlock<string, string>(
                input => input.ToUpper()
            );

            string[] strings = { "one", "two", "three", "four", "five" };
            foreach (string s in strings)
            {
                await transformBlock.SendAsync(s);
            }

            for (int i = 0; i < strings.Length; i++)
            {
                Console.WriteLine($"TransformBlock: {transformBlock.Receive()}");
            }

            Console.WriteLine("\n=== 3. TransformManyBlock<TInput, TOutput> ===\n");

            TransformManyBlock<int, int> transformManyBlock = new TransformManyBlock<int, int>(
                input =>
                {
                    List<int> digits = new List<int>();
                    if (input == 0)
                        return new int[] { 0 };
                    while (input > 0)
                    {
                        digits.Add(input % 10);
                        input /= 10;
                    }
                    digits.Reverse();
                    return digits;
                }
            );

            int[] numbers = { 123456, 0, 10020, 123, -1234 };
            int expectedOutputCount = 0;

            foreach (int num in numbers)
            {
                if (num >= 0)
                {
                    await transformManyBlock.SendAsync(num);

                    if (num == 0)
                        expectedOutputCount += 1;
                    else
                        expectedOutputCount += num.ToString().Length;

                    Console.WriteLine($"Posted: {num}");
                }
                else
                {
                    Console.WriteLine($"Skipped negative number: {num}");
                }
            }

            Console.WriteLine($"\nExpected output count: {expectedOutputCount}");
            Console.WriteLine("\nReceiving digits:");

            for (int i = 0; i < expectedOutputCount; i++)
            {
                int digit = await transformManyBlock.ReceiveAsync();
                Console.Write($"{digit} ");
            }
            Console.WriteLine();
        }
    }
}
