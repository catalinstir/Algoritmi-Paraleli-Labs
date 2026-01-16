using System.Threading.Tasks.Dataflow;

namespace ex06
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            /// 3. Grouping blocks
            /// 3.1. BatchBlock<T>
            Console.WriteLine("=== 1. BatchBlock<T> ===\n");

            BatchBlock<string> batchBlock = new BatchBlock<string>(3);

            List<string> persons = new List<string>()
            {
                "Emma Johnson",
                "William Thompson",
                "Mia Taylor",
                "Liam Adams",
                "Charlotte Lewis",
                "Noah Brooks",
                "Ethan Mitchell",
                "Emily Powell",
                "Lucas Reed",
                "Lily Simmons",
                "Daniel Hayes",
                "Isabella Wright",
                "Amelia Davis"
            };

            foreach (string person in persons)
            {
                await batchBlock.SendAsync(person);
            }

            batchBlock.Complete();

            string[] group;
            int groupNumber = 1;
            while (batchBlock.TryReceive(out group))
            {
                Console.WriteLine($"Group {groupNumber}: ");
                foreach (string person in group)
                {
                    Console.WriteLine($"\t{person}");
                }
                groupNumber++;
            }

            Console.WriteLine("\n=== 2. JoinBlock<T1, T2> ===\n");

            /// 3.2. JoinBlock<T1, T2>
            JoinBlock<string, int> joinBlock = new JoinBlock<string, int>();

            string[] cars = new string[]
            {
                "Toyota Camry",
                "Ford Mustang",
                "Honda Accord",
                "Chevrolet Silverado",
                "BMW 3 Series",
                "Nissan Rogue",
                "Tesla Model 3",
                "Volkswagen Golf"
            };

            int[] prices = new int[]
            {
                25000,
                35000,
                27000,
                40000,
                45000,
                30000,
                50000,
                22000
            };

            for (int i = 0; i < cars.Length; i++)
            {
                await joinBlock.Target1.SendAsync(cars[i]);
                await joinBlock.Target2.SendAsync(prices[i]);
            }

            for (int i = 0; i < cars.Length; i++)
            {
                Tuple<string, int> pair = joinBlock.Receive();
                Console.WriteLine($"{pair.Item1}: {pair.Item2}$");
            }

            Console.WriteLine("\n=== 3. BatchedJoinBlock<T1, T2> ===\n");
            BatchedJoinBlock<string, int> batchedJoinBlock = new BatchedJoinBlock<string, int>(3);

            for (int i = 0; i < cars.Length; i++)
            {
                await batchedJoinBlock.Target1.SendAsync(cars[i]);
                await batchedJoinBlock.Target2.SendAsync(prices[i]);
            }
            batchedJoinBlock.Complete();
            int batchNumber = 1;

            while (batchedJoinBlock.TryReceive(out Tuple<IList<string>, IList<int>> batch))
            {
                IList<string> carNames = batch.Item1;
                IList<int> carPrices = batch.Item2;

                Console.WriteLine($"Batch {batchNumber}:");
                Console.WriteLine($"\tCars count: {carNames.Count}, Prices count: {carPrices.Count}");

                int count = Math.Min(carNames.Count, carPrices.Count);

                if (count == 0)
                {
                    Console.WriteLine("\tEmpty batch");
                    batchNumber++;
                    continue;
                }

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"\t{carNames[i]}: {carPrices[i]}$");
                }

                int minIndex = 0;
                int minPrice = carPrices[0];

                for (int i = 1; i < count; i++)
                {
                    if (carPrices[i] < minPrice)
                    {
                        minPrice = carPrices[i];
                        minIndex = i;
                    }
                }

                Console.WriteLine($"\t>>> Cheapest: {carNames[minIndex]} at {minPrice}$\n");

                batchNumber++;
            }
        }
    }
}
