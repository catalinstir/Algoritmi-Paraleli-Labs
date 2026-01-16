using System.Threading.Tasks.Dataflow;

namespace ex04
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== 1. BufferBlock<T> ===\n");

            BufferBlock<int> bufferBlock = new BufferBlock<int>();

            for (int i = 0; i < 10; i++)
            {
                await bufferBlock.SendAsync(i);
                Console.WriteLine($"Posted: {i}");
            }

            Console.WriteLine();
            for (int i = 0; i < 15; i++)
            {
                if (bufferBlock.TryReceive(out int nr))
                {
                    Console.WriteLine($"Received: {nr}");
                }
                else
                {
                    Console.WriteLine($"Buffer empty at iteration {i}");
                }
            }

            Console.WriteLine("\n=== 2. BroadcastBlock<T> ===\n");

            BroadcastBlock<string> broadcastBlock = new BroadcastBlock<string>(null);

            broadcastBlock.Post("Low battery!");
            broadcastBlock.Post("Battery sufficiently charged!");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Receive {i}: {broadcastBlock.Receive()}");
            }

            Console.WriteLine("\n=== 3. WriteOnceBlock<T> ===\n");

            WriteOnceBlock<int> writeOnceBlock = new WriteOnceBlock<int>(null);

            bool first = await writeOnceBlock.SendAsync(5);
            Console.WriteLine($"SendAsync(5) accepted: {first}");

            bool second = await writeOnceBlock.SendAsync(6);
            Console.WriteLine($"SendAsync(6) accepted: {second}");

            Console.WriteLine($"Receive 1: {await writeOnceBlock.ReceiveAsync()}");
            Console.WriteLine($"Receive 2: {await writeOnceBlock.ReceiveAsync()}");
            Console.WriteLine($"Receive 3: {await writeOnceBlock.ReceiveAsync()}");
        }
    }
}
