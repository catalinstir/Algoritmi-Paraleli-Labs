using System.Threading.Tasks.Dataflow;

namespace ex07
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Dataflow Pipeline Demo ===\n");

            BufferBlock<Wood> sourceWood = new BufferBlock<Wood>();
            BufferBlock<Stone> sourceStone = new BufferBlock<Stone>();
            BufferBlock<Iron> sourceIron = new BufferBlock<Iron>();

            BroadcastBlock<Wood> broadcastWood = new BroadcastBlock<Wood>(wood => wood);

            JoinBlock<Wood, Stone> joinWoodStoneBlock = new JoinBlock<Wood, Stone>(
                new GroupingDataflowBlockOptions { Greedy = true }
            );

            JoinBlock<Wood, Iron> joinWoodIronBlock = new JoinBlock<Wood, Iron>(
                new GroupingDataflowBlockOptions { Greedy = true }
            );

            int woodStoneCount = 0;
            int woodIronCount = 0;

            ActionBlock<Tuple<Wood, Stone>> actionWoodStone = new ActionBlock<Tuple<Wood, Stone>>(
                async resource =>
                {
                    int current = Interlocked.Increment(ref woodStoneCount);
                    Console.WriteLine($"[Wood+Stone #{current}] Starting...");

                    for (int i = 0; i < 5; i++)
                    {
                        await Task.Delay(100);
                        Console.WriteLine($"[Wood+Stone #{current}] Processing {i + 1}/5");
                    }

                    Console.WriteLine($"[Wood+Stone #{current}] Complete!");
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 4
                }
            );

            ActionBlock<Tuple<Wood, Iron>> actionWoodIron = new ActionBlock<Tuple<Wood, Iron>>(
                async resource =>
                {
                    int current = Interlocked.Increment(ref woodIronCount);
                    Console.WriteLine($"[Wood+Iron #{current}] Starting...");

                    for (int i = 0; i < 5; i++)
                    {
                        await Task.Delay(100);
                        Console.WriteLine($"[Wood+Iron #{current}] Processing {i + 1}/5");
                    }

                    Console.WriteLine($"[Wood+Iron #{current}] Complete!");
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 4
                }
            );

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            sourceWood.LinkTo(broadcastWood, linkOptions);

            broadcastWood.LinkTo(joinWoodStoneBlock.Target1);
            broadcastWood.LinkTo(joinWoodIronBlock.Target1);

            sourceStone.LinkTo(joinWoodStoneBlock.Target2, linkOptions);
            sourceIron.LinkTo(joinWoodIronBlock.Target2, linkOptions);

            joinWoodStoneBlock.LinkTo(actionWoodStone, linkOptions);
            joinWoodIronBlock.LinkTo(actionWoodIron, linkOptions);

            Console.WriteLine("Posting resources...\n");

            var tasks = new List<Task>();
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                int index = i;

                tasks.Add(Task.Run(async () =>
                {
                    await Task.Delay(random.Next(1, 10) * 100);
                    sourceStone.Post(new Stone());
                    Console.WriteLine($"Posted Stone #{index + 1}");
                }));

                tasks.Add(Task.Run(async () =>
                {
                    await Task.Delay(random.Next(1, 10) * 100);
                    sourceIron.Post(new Iron());
                    Console.WriteLine($"Posted Iron #{index + 1}");
                }));

                tasks.Add(Task.Run(async () =>
                {
                    await Task.Delay(random.Next(1, 10) * 100);
                    sourceWood.Post(new Wood());
                    Console.WriteLine($"Posted Wood #{index + 1}");
                }));
            }
            await Task.WhenAll(tasks);

            sourceWood.Complete();
            sourceStone.Complete();
            sourceIron.Complete();

            await Task.WhenAny(
                Task.WhenAll(actionWoodStone.Completion, actionWoodIron.Completion),
                Task.Delay(10000)
            );

            Console.WriteLine($"\n=== Summary ===");
            Console.WriteLine($"Wood+Stone combinations: {woodStoneCount}");
            Console.WriteLine($"Wood+Iron combinations: {woodIronCount}");
        }
    }
}
